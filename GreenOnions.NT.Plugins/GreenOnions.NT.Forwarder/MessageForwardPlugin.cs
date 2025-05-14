using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.Forwarder
{
    public class MessageForwardPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;
        private Regex? _replyRegex;

        public string Name => "消息转发插件";
        public string Description => "将消息转发到指定的群或好友";
        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _replyRegex = new Regex(config.ReplyFriendCommand);
        }

        private Config LoadConfig(string pluginPath)
        {
            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            else
            {
                Config exampleConfig = new Config();
                exampleConfig.Friend.Add(new FriendForwardSettings
                {
                    FromFriendUin = 100,
                    ToFriendUin = 101,
                    Remark = "第1个好友"
                });
                exampleConfig.Friend.Add(new FriendForwardSettings
                {
                    FromFriendUin = 200,
                    ToFriendUin = 201,
                    Remark = "第2个好友"
                });
                exampleConfig.Group.Add(new GroupForwardSettings
                {
                    FromGroupUin = 3000,
                    FromMemberUin = 100,
                    ToGroupUin = 4000,
                    Remark = "第1个群的第1个群友"
                });
                exampleConfig.Group.Add(new GroupForwardSettings
                {
                    FromGroupUin = 3000,
                    FromMemberUin = 101,
                    ToGroupUin = 4000,
                    Remark = "第1个群的第2个群友"
                });
                exampleConfig.Group.Add(new GroupForwardSettings
                {
                    FromGroupUin = 4000,
                    FromMemberUin = 103,
                    ToGroupUin = 5000,
                    Remark = "第2个群的第1个群友"
                });
                File.WriteAllText(configPath, YamlConvert.SerializeObject(exampleConfig));
                LogHelper.LogMessage("消息转发插件配置文件不存在，已生成默认配置文件，请修改后执行重读配置文件命令或重启机器人");
            }
            _config ??= new Config();
            return _config;
        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);

            _replyRegex = new Regex(config.ReplyFriendCommand);

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
        }

        private async void OnGroupMessage(BotContext context, GroupMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;

            if (!e.Chain.AllowUseIfDebug())
                return;

            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning("消息转发插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用消息转发
                return;

            try
            {
                GroupForwardSettings? forwarderConfig = _config.Group.FirstOrDefault(g => g.FromGroupUin == e.Chain.GroupUin && g.FromMemberUin == e.Chain.FriendUin);
                if (forwarderConfig is null)
                    return;

                foreach (var entity in e.Chain)
                {
                    MessageBuilder builder = MessageBuilder.Group(forwarderConfig.ToGroupUin);
                    string format = forwarderConfig.Format ?? "";
                    string[] prefixAndSuffix = format.Split("<Message>");
                    if (prefixAndSuffix.Length == 2)
                    {
                        string prefix = prefixAndSuffix.First().Replace("<MemberNickName>", e.Chain.GroupMemberInfo?.MemberName).Replace("<MemberUin>", e.Chain.FriendUin.ToString());
                        string suffix = prefixAndSuffix.Last().Replace("<MemberNickName>", e.Chain.GroupMemberInfo?.MemberName).Replace("<MemberUin>", e.Chain.FriendUin.ToString());
                        builder.Text(prefix);
                        builder.Add(entity);
                        builder.Text(suffix);
                    }
                    else
                    {
                        builder.Add(entity);
                    }
                    await context.SendMessage(builder.Build());
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async void OnFriendMessage(BotContext context, FriendMessageEvent e)
        {
            if (!e.Chain.AllowUseIfDebug())
                return;

            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning("消息转发插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用消息转发
                return;

            try
            {
                await ForwardFriendMessageToAdmin(_config, _commonConfig, context, e);
                await ForwardFriendMessage(_config, context, e);
                await ReplyMessage(_config, _commonConfig, context, e);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async Task ForwardFriendMessageToAdmin(Config config, ICommonConfig commonConfig, BotContext context, FriendMessageEvent e)
        {
            if (!config.ForwardAllPrivateMessageToAdmin)
                return;

            foreach (var entity in e.Chain)
            {
                foreach (var admin in commonConfig.AdminQQ)
                {
                    MessageBuilder builder = MessageBuilder.Friend(admin);
                    string format = config.ForwardToAdminFormat ?? "";
                    string[] prefixAndSuffix = format.Split("<Message>");
                    if (prefixAndSuffix.Length == 2)
                    {
                        string prefix = prefixAndSuffix.First().Replace("<FriendNickName>", e.Chain.FriendInfo?.Nickname).Replace("<FriendUin>", e.Chain.FriendUin.ToString());
                        string suffix = prefixAndSuffix.Last().Replace("<FriendNickName>", e.Chain.FriendInfo?.Nickname).Replace("<FriendUin>", e.Chain.FriendUin.ToString());
                        builder.Text(prefix);
                        builder.Add(entity);
                        builder.Text(suffix);
                    }
                    else
                    {
                        builder.Add(entity);
                    }
                    await context.SendMessage(builder.Build());
                }
            }
        }

        private async Task ForwardFriendMessage(Config config, BotContext context, FriendMessageEvent e)
        {
            FriendForwardSettings? forwarderConfig = config.Friend.FirstOrDefault(f => f.FromFriendUin == e.Chain.FriendUin);
            if (forwarderConfig is null)
                return;

            foreach (var entity in e.Chain)
            {
                MessageBuilder builder = MessageBuilder.Friend(forwarderConfig.ToFriendUin);
                string format = forwarderConfig.Format ?? "";
                string[] prefixAndSuffix = format.Split("<Message>");
                if (prefixAndSuffix.Length == 2)
                {
                    string prefix = prefixAndSuffix.First().Replace("<FriendNickName>", e.Chain.FriendInfo?.Nickname).Replace("<FriendUin>", e.Chain.FriendUin.ToString());
                    string suffix = prefixAndSuffix.Last().Replace("<FriendNickName>", e.Chain.FriendInfo?.Nickname).Replace("<FriendUin>", e.Chain.FriendUin.ToString());
                    builder.Text(prefix);
                    builder.Add(entity);
                    builder.Text(suffix);
                }
                else
                {
                    builder.Add(entity);
                }
                await context.SendMessage(builder.Build());
            }
        }

        private async Task ReplyMessage(Config config, ICommonConfig commonConfig, BotContext context, FriendMessageEvent e)
        {
            if (!commonConfig.AdminQQ.Contains(e.Chain.FriendUin))
                return;
            if (_replyRegex is null)
                return;
            if (e.Chain.First() is not TextEntity textMessage)
                return;
            Match match = _replyRegex.Match(textMessage.Text);
            if (!match.Success)
                return;
            uint replyUin = Convert.ToUInt32(match.Groups["Uin"].Value);
            string text = match.Groups["Message"].Value;
            string replyText = config.AdminReplyFormat.Replace("<FriendNickName>", e.Chain.FriendInfo?.Nickname).Replace("<FriendUin>", e.Chain.FriendUin.ToString()).Replace("<Message>", text);
            await context.SendMessage(MessageBuilder.Friend(replyUin).Text(replyText).Build());
        }
    }
}
