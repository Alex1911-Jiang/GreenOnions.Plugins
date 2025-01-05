using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Clients;
using GreenOnions.NT.HPictures.Enums;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.HPictures
{
    public class HPicturesPlugin : IPlugin
    {
        private Config? _config;
        private ICommonConfig? _commonConfig;
        public Regex? ModuleRegex { get; private set; }

        public string Name => "随机色图";

        public string Description => "随机色图插件";

        public void OnConfigUpdate(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            ModuleRegex = new Regex(_config!.Command.Replace("<机器人名称>", commonConfig.BotName));
        }

        public void OnLoad(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            Console.WriteLine("色图插件加载成功");
            _commonConfig = commonConfig;

            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            _config ??= new Config();

            File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));

            ModuleRegex = new Regex(_config.Command.Replace("<机器人名称>", commonConfig.BotName));

            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
        }

        private async void OnGroupMessage(BotContext context, GroupMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;
            await OnMessage(context, e.Chain);
        }

        private async void OnFriendMessage(BotContext context, FriendMessageEvent e)
        {
            await OnMessage(context, e.Chain);
        }

        private async Task OnMessage(BotContext context, MessageChain chain)
        {
            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning("色图插件配置为空");
                return;
            }

            if (_commonConfig.DebugMode && chain.GroupUin is not null && !_commonConfig.DebugGroups.Contains(chain.GroupUin.Value))
                return;
            if (_commonConfig.DebugMode && chain.GroupUin is null && !_commonConfig.AdminQQ.Contains(chain.FriendUin))
                return;

            string? msg = null;
            if (chain.GetEntity<TextEntity>() is TextEntity text)
                msg = text.Text;
            else if (chain.GetEntity<ImageEntity>() is ImageEntity image)
                msg = image.FilePath;
            else
                return;


            //自定义色图命令
            if (_config.UserCmd.Contains(msg))  //命中自定义色图命令
            {
                LogHelper.LogMessage($"{chain.FriendUin}的消息 '{msg}' 命中了自定义色图命令");
                if (!await CheckPermissions(context, _commonConfig, _config, chain))  //检查权限
                {
                    LogHelper.LogMessage($"{chain.FriendUin}无权使用色图");
                    return;
                }

                LogHelper.LogMessage($"开始为 {chain.FriendUin} 查找色图");
                await context.ReplyAsync(chain, _config.DownloadingReply);  //开始查找回复

                await SendHPictures(_commonConfig, _config, context, chain);
                return;
            }
        }

        private async Task SendHPictures(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain)
        {
            HPictureSource pictureSource = RandomHPictureSource(config);

            IAsyncEnumerable<MessageBuilder> builders = pictureSource switch
            {
                HPictureSource.Lolicon => LoliconClient.CreateMessage("", 1, false, config, commonConfig, context, chain),
                HPictureSource.Yande_re => throw new NotImplementedException(),
                HPictureSource.Lolisuki3 => throw new NotImplementedException(),
                HPictureSource.Yuban10703 => throw new NotImplementedException(),
                HPictureSource.Lolibooru => throw new NotImplementedException(),
                HPictureSource.Konachan_net => throw new NotImplementedException(),
                _ => throw new NotImplementedException("图库设置有误或指定图库已失效，请联系机器人管理员"),
            };

            await foreach (var builder in builders)
            {
                try
                {
                    if (chain.GroupUin is null)
                    {
                        MessageResult messageResult = await context.SendMessage(builder.Build());
                        RecordLimit(commonConfig, config, chain, LimitType.Number);  //记录张数限制
                        if (config.PrivateMessageRecallSecond > 0)
                            _ = Task.Delay(1000 * config.PrivateMessageRecallSecond).ContinueWith(_ => context.RecallFriendMessage(chain.FriendUin, messageResult));
                    }
                    else
                    {
                        MessageResult messageResult = await context.SendMessage(builder.Build());
                        RecordLimit(commonConfig, config, chain, LimitType.Number);  //记录张数限制
                        Task delay;
                        if (config.WhiteGroup.Contains(chain.GroupUin.Value) && config.WhiteGroupRecallSecond > 0)
                            delay = Task.Delay(1000 * config.WhiteGroupRecallSecond);
                        else if (config.GroupRecallSecond > 0)
                            delay = Task.Delay(1000 * config.GroupRecallSecond);
                        else
                            continue;
                        _ = delay.ContinueWith(_ => context.RecallGroupMessage(chain.GroupUin.Value, messageResult));
                    }
                }
                catch (Exception ex)
                {
                    await context.ReplyAsync(chain, config.ErrorReply.Replace("<错误信息>", ex.Message));
                }
            }
            RecordLimit(commonConfig, config, chain, LimitType.Frequency);  //记录次数限制
        }

        private HPictureSource RandomHPictureSource(Config config)
        {
            if (config.EnabledSource.Count == 0)
            {
                LogHelper.LogWarning($"没有启用任何色图图库");
                throw new Exception("没有连接到任何图库，请联系机器人管理员");
            }

            if (config.EnabledSource.Count == 1)
                return config.EnabledSource.First();

            Random rdm = new(Guid.NewGuid().GetHashCode());
            int rdmIndex = rdm.Next(0, config.EnabledSource.Count);
            return config.EnabledSource.ToArray()[rdmIndex];
        }

        /// <summary>
        /// 设置次数限制和冷却时间
        /// </summary>
        private void RecordLimit(ICommonConfig commonConfig, Config config, MessageChain chain, LimitType limitType)
        {
            if (limitType == config.LimitType)
            {
                if (LimitCache.LimitNumber.ContainsKey(chain.FriendUin))
                    LimitCache.LimitNumber[chain.FriendUin]++;
                else
                    LimitCache.LimitNumber.TryAdd(chain.FriendUin, 1);
            }
            RecordHPictureCD(commonConfig, config, chain);  //色图记录CD
        }

        /// <summary>
        /// 检查色图权限
        /// </summary>
        private async Task<bool> CheckPermissions(BotContext bot, ICommonConfig commonConfig, Config config, MessageChain chain)
        {
            if (!config.Enabled)  //没有启用色图
            {
                LogHelper.LogMessage($"没有启用色图插件，不响应命令");
                return false;
            }

            if (GetHPictureQuota(1, commonConfig, config, chain) <= 0)
            {
                LogHelper.LogMessage($"{chain.FriendUin}色图次数耗尽");
                await bot.ReplyAsync(chain, config.OutOfLimitReply);
                return false;
            }

            if (CheckHPictureCD(commonConfig, config, chain))
            {
                LogHelper.LogMessage($"{chain.FriendUin}色图冷却中");
                await bot.ReplyAsync(chain, config.CoolDownUnreadyReply);
                return false;
            }

            if (chain.GroupUin is null)
            {
                if (!config.AllowPrivateMessage) //不允许私聊
                {
                    LogHelper.LogMessage($"没有启动私聊色图，不响应私聊色图命令");
                    return false;
                }
            }
            else
            {
                if (config.WhiteGroupOnly && !config.WhiteGroup.Contains(chain.GroupUin.Value))  //仅限白名单但当前群不在白名单中
                {
                    LogHelper.LogMessage($"启用了仅限白名单但{chain.GroupUin.Value}不在白名单中");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取色图剩余配额
        /// </summary>
        internal int GetHPictureQuota(int num, ICommonConfig commonConfig, Config config, MessageChain chain)
        {
            if (config.Limit == 0)  //无限制
                return num;
            if (commonConfig.AdminQQ.Contains(chain.FriendUin) && config.AdminNoLimit)  //机器人管理员无限制
                return num;
            if (chain.GroupUin is null && config.PrivateMessageNoLimit)  //私聊且开了私聊无限制
                return num;
            if(chain.GroupUin is not null && config.WhiteGroup.Contains(chain.GroupUin.Value) && config.WhiteGroupNoLimit)  //群，且是白名单群，且开了白名单群无限制
                return num;

            if (config.LimitType == LimitType.Frequency && LimitCache.LimitNumber.ContainsKey(chain.FriendUin) && LimitCache.LimitNumber[chain.FriendUin] >= config.Limit)
                return 0;
            else if(LimitCache.LimitNumber.ContainsKey(chain.FriendUin))
                return Math.Min(num, config.Limit - LimitCache.LimitNumber[chain.FriendUin]);
            return Math.Min(num, config.Limit);
        }

        /// <summary>
        /// 检查冷却时间，true为冷却中
        /// </summary>
        internal bool CheckHPictureCD(ICommonConfig commonConfig, Config config, MessageChain chain)
        {
            //机器人管理员且启用了管理员无限制
            if (commonConfig.AdminQQ.Contains(chain.FriendUin) && config.AdminNoLimit)
                return false;
            //私聊 并且启用了私聊无限制
            if (chain.GroupUin is null && config.PrivateMessageNoLimit)
                return false;
            //私聊 但没启用私聊无限制
            if (chain.GroupUin is null)
                return LimitCache.PrivateMessageCoolDown.TryGetValue(chain.FriendUin, out DateTime coolDownTime) ? DateTime.Now < coolDownTime : false;
            //白名单群 且开了白名单群无限制
            if (config.WhiteGroup.Contains(chain.GroupUin.Value) && config.WhiteGroupNoLimit)
                return false;
            //白名单群 但没开白名单群无限制
            if (config.WhiteGroup.Contains(chain.GroupUin.Value))
                return LimitCache.WhiteGroupCoolDown.TryGetValue(chain.FriendUin, out DateTime coolDownTime) ? DateTime.Now < coolDownTime : false;

            //非白名单群
            return LimitCache.GroupCoolDown.TryGetValue(chain.FriendUin, out DateTime groupCoolDownTime) ? DateTime.Now < groupCoolDownTime : false;
        }

        /// <summary>
        /// 记录色图冷却时间
        /// </summary>
        internal void RecordHPictureCD(ICommonConfig commonConfig, Config config, MessageChain chain)
        {
            //机器人管理员且启用了管理员无限制，不记录
            if (commonConfig.AdminQQ.Contains(chain.FriendUin) && config.AdminNoLimit)
                return;

            //私聊
            if (chain.GroupUin is null && config.PrivateMessageCoolDownSecond > 0)
            {
                LimitCache.PrivateMessageCoolDown[chain.FriendUin] = DateTime.Now.AddSeconds(config.PrivateMessageCoolDownSecond);
                return;
            }
            //白名单群
            if (chain.GroupUin is not null && config.WhiteGroup.Contains(chain.GroupUin.Value) && config.WhiteGroupCoolDownSecond > 0)
            {
                LimitCache.WhiteGroupCoolDown[chain.FriendUin] = DateTime.Now.AddSeconds(config.WhiteGroupCoolDownSecond);
                return;
            }
            //非白名单群
            if (chain.GroupUin is not null && !config.WhiteGroup.Contains(chain.GroupUin.Value) && config.GroupCoolDownSecond > 0)
            {
                LimitCache.GroupCoolDown[chain.FriendUin] = DateTime.Now.AddSeconds(config.GroupCoolDownSecond);
                return;
            }
        }
    }
}
