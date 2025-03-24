using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Clients;
using GreenOnions.NT.HPictures.Enums;
using GreenOnions.NT.HPictures.Helpers;
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
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;
        private Regex? _commandRegex;

        public string Name => "随机色图";

        public string Description => "随机色图插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _commandRegex = new Regex(config.Command.ReplaceTags());
        }

        private Config LoadConfig(string pluginPath)
        {
            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            _config ??= new Config();
            File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));
            return _config;
        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);

            _commandRegex = new Regex(config.Command.ReplaceTags());

            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
        }

        private async void OnGroupMessage(BotContext context, GroupMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async void OnFriendMessage(BotContext context, FriendMessageEvent e)
        {
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async Task OnMessage(BotContext context, MessageChain chain)
        {
            if (!chain.AllowUseIfDebug())
                return;

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

            if (!_config.Enabled)  //没有启用色图
                return;

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var item in chain)
            {
                if (item is not TextEntity text)
                    continue;

                string msg = text.Text;

                if (firstAt)
                    msg = _commonConfig.BotName + msg.TrimStart();

                //自定义色图命令
                if (_config.UserCmd.Contains(msg))  //命中自定义色图命令
                {
                    LogHelper.LogMessage($"{chain.FriendUin}的消息'{msg}'命中了自定义色图命令");
                    if (!await CheckPermissions(_commonConfig, _config, chain))  //检查权限
                    {
                        LogHelper.LogMessage($"{chain.FriendUin}无权使用色图");
                        return;
                    }

                    await SendHPictures("", 1, false, _commonConfig, _config, context, chain);
                    return;
                }

                if (_commandRegex is null)
                {
                    LogHelper.LogWarning("没有色图命令匹配式");
                    return;
                }

                //常规色图命令
                Match matchHPcitureCmd = _commandRegex.Match(msg);
                if (!matchHPcitureCmd.Success)
                    return;
                
                //命中常规色图命令
                LogHelper.LogMessage($"{chain.FriendUin}的消息'{msg}'命中了常规色图命令");
                if (!await CheckPermissions(_commonConfig, _config, chain))  //检查权限
                {
                    LogHelper.LogMessage($"{chain.FriendUin}无权使用色图");
                    return;
                }

                (string keyword, int num, bool r18) = RegexMatchHelper.ExtractParameter(matchHPcitureCmd);

                if (num < 1)
                {
                    LogHelper.LogMessage($"{chain.FriendUin}请求了少于1张色图，不响应命令");
                    return;
                }

                if (num > _config.OnceMessageMaxImageCount)
                    num = _config.OnceMessageMaxImageCount;

                num = GetHPictureQuota(num, _commonConfig, _config, chain);  //剩余次数

                if (num < 1) //请求大于等于1张，但次数已耗尽
                {
                    LogHelper.LogMessage($"{chain.FriendUin}色图次数耗尽");
                    await chain.ReplyAsync(_config.OutOfLimitReply);  //次数用尽
                    return;
                }

                if (_config.ShieldingWords.Contains(keyword))  //屏蔽词
                {
                    LogHelper.LogMessage($"{chain.FriendUin}的色图命令'{msg}'中包含屏蔽词，不响应该命令");
                    return;
                }

                if (r18 && !_config.AllowR18)  //不允许R18
                    return;
                if (r18 && chain.GroupUin is uint groupUin && _config.R18WhiteGroupOnly && _config.WhiteGroup.Contains(groupUin))  // 仅限白名单但此群不在白名单中, 不响应R18命令
                    return;

                await SendHPictures(keyword, num, r18, _commonConfig, _config, context, chain);
            }
        }

        private async Task SendHPictures(string keyword, int num, bool r18, ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain)
        {
            HPictureSource pictureSource = RandomHPictureSource(config);

            LogHelper.LogMessage($"开始为{chain.FriendUin}在{pictureSource}查找色图");

            await chain.ReplyAsync(config.DownloadingReply);  //开始查找回复

            IAsyncEnumerable<MessageBuilder> builders = pictureSource switch
            {
                HPictureSource.Lolicon => LoliconClient.CreateMessage(keyword, num, r18, config, commonConfig, context, chain),
                HPictureSource.Yande_re => Yande_re_Client.CreateMessage(keyword, num, r18, config, commonConfig, context, chain),
                HPictureSource.Lolisuki => LolisukiClient.CreateMessage(keyword, num, r18, config, commonConfig, context, chain),
                HPictureSource.Yuban10703 => Yuban10703Client.CreateMessage(keyword, num, r18, config, commonConfig, context, chain),
                HPictureSource.Konachan_net => Konachan_net_Client.CreateMessage(keyword, num, r18, config, commonConfig, context, chain),
                _ => throw new NotImplementedException("图库设置有误或指定图库已失效，请联系机器人管理员"),
            };

            bool anySuccess = false;

            try
            {
                await foreach (var builder in builders)
                {
                    if (chain.GroupUin is null)
                    {
                        LogHelper.LogMessage($"向好友{chain.FriendUin}发送色图");
                        MessageResult messageResult = await context.SendMessage(builder.Build());
                        RecordLimit(commonConfig, config, chain, LimitType.Number);  //记录张数限制
                        anySuccess = true;
                        if (config.PrivateMessageRecallSecond > 0)
                            _ = Task.Delay(1000 * config.PrivateMessageRecallSecond).ContinueWith(_ => context.RecallFriendMessage(chain.FriendUin, messageResult));
                    }
                    else
                    {
                        LogHelper.LogMessage($"向群{chain.GroupUin}发送色图");
                        MessageResult messageResult = await context.SendMessage(builder.Build());
                        RecordLimit(commonConfig, config, chain, LimitType.Number);  //记录张数限制
                        anySuccess = true;
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
            }
            catch (Exception ex)
            {
                await chain.ReplyAsync(config.ErrorReply.Replace("<错误信息>", ex.Message));
            }
            if (!anySuccess)
                return;
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
            RecordHPictureCoolDown(commonConfig, config, chain);  //色图记录CD
        }

        /// <summary>
        /// 检查色图权限
        /// </summary>
        private async Task<bool> CheckPermissions(ICommonConfig commonConfig, Config config, MessageChain chain)
        {
            if (GetHPictureQuota(1, commonConfig, config, chain) <= 0)
            {
                LogHelper.LogMessage($"{chain.FriendUin}色图次数耗尽");
                await chain.ReplyAsync( config.OutOfLimitReply);
                return false;
            }

            if (CheckHPictureCoolDown(commonConfig, config, chain))
            {
                LogHelper.LogMessage($"{chain.FriendUin}色图冷却中");
                await chain.ReplyAsync(config.CoolDownUnreadyReply);
                return false;
            }

            if (chain.GroupUin is null && !config.AllowPrivateMessage)//私聊 并且不允许私聊
            {
                LogHelper.LogMessage($"没有启用私聊色图，不响应私聊色图命令");
                return false;
            }

            if (chain.GroupUin is not null && config.WhiteGroupOnly && !config.WhiteGroup.Contains(chain.GroupUin.Value))  //群聊 且仅限白名单，但当前群不在白名单中
            {
                LogHelper.LogMessage($"启用了仅限白名单但{chain.GroupUin.Value}不在白名单中");
                return false;
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
        internal bool CheckHPictureCoolDown(ICommonConfig commonConfig, Config config, MessageChain chain)
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
        internal void RecordHPictureCoolDown(ICommonConfig commonConfig, Config config, MessageChain chain)
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
