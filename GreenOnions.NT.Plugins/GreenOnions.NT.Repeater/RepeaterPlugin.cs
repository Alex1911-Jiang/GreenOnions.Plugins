using System;
using System.Linq;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.Repeater
{
    public class RepeaterPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private BotContext? _bot;
        private ICommonConfig? _commonConfig;
        public string Name => "复读机";
        public string Description => "随机/连续复读插件";
        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            LoadConfig(_pluginPath);
        }

        private void LoadConfig(string pluginPath)
        {
            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            _config ??= new Config();
            File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));
        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;
            _bot = bot;
            _commonConfig = commonConfig;
            LoadConfig(_pluginPath);

            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
        }

        private async void OnGroupMessage(BotContext context, GroupMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;

            if (e.Chain.Count > 1)  //如果一条消息内存在多种类型，例如@人+内容，则不复读
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
                LogHelper.LogWarning($"{Name}插件配置为空");
                return;
            }

            if (_config.RepeatSettings.TryGetValue(e.Chain.GroupUin!.Value, out RepeatSetting? thisGroupSetting))
            {
                if (!thisGroupSetting.Enabled)
                    return;
                await Repeate(context, e, thisGroupSetting);
                return;
            }

            if (!_config.RepeatSettings.TryGetValue(0, out RepeatSetting? allGroupSetting))
                return;

            if (!allGroupSetting.Enabled)
                return;
            await Repeate(context, e, allGroupSetting);
        }

        private async Task Repeate(BotContext context, GroupMessageEvent e, RepeatSetting repeatSetting)
        {
            uint groupUin = e.Chain.GroupUin!.Value;
            var inputMsg = e.Chain.First();

            if (!_lastMessages.ContainsKey(groupUin))
                _lastMessages.Add(groupUin, new LastMessage { Message = "", Repeated = false, Repetitions = 0 });
            LastMessage messageRecord = _lastMessages[groupUin];

            if (inputMsg is TextEntity textEntity)
            {
                if (messageRecord.Message == textEntity.Text)
                {
                    messageRecord.Repetitions++;
                }
                else
                {
                    messageRecord.Message = textEntity.Text;
                    messageRecord.Repetitions = 1;
                }

                if (repeatSetting.IgnoreWords.Any(w => textEntity.Text.Contains(w.ReplaceTags())))  //包含忽略词
                    return;

                if (messageRecord.Repeated)  //已经复读过
                    return;

                if (messageRecord.Repetitions >= repeatSetting.SuccessiveRepeatCount)  //连续复读
                {
                    string outputMsg = textEntity.Text;
                    foreach (var item in repeatSetting.ReplaceWords)
                        outputMsg.Replace(item.Key.ReplaceTags(), item.Value.ReplaceTags(), StringComparison.OrdinalIgnoreCase);

                    LogHelper.LogMessage($"复读机插件在{groupUin}群触发了连续复读，内容为：{outputMsg}");
                    await context.SendMessage(MessageBuilder.Group(groupUin).Text(outputMsg).Build());
                    messageRecord.Repeated = true;
                    return;
                }

                if (repeatSetting.RandomProbability > 0 && Random.Shared.NextDouble() < repeatSetting.RandomProbability)  //随机复读
                {
                    string outputMsg = textEntity.Text;
                    foreach (var item in repeatSetting.ReplaceWords)
                        outputMsg.Replace(item.Key.ReplaceTags(), item.Value.ReplaceTags(), StringComparison.OrdinalIgnoreCase);

                    LogHelper.LogMessage($"复读机插件在{groupUin}群触发了随机复读，内容为：{outputMsg}");
                    messageRecord.Repeated = true;
                    return;
                }
            }
            else if (inputMsg is ImageEntity imageEntity)
            {
                if (messageRecord.Message == imageEntity.ImageUrl)
                {
                    messageRecord.Repetitions++;
                }
                else
                {
                    messageRecord.Message = imageEntity.ImageUrl;
                    messageRecord.Repetitions = 1;
                }

                if (messageRecord.Repeated)  //已经复读过
                    return;

                if (messageRecord.Repetitions >= repeatSetting.SuccessiveRepeatCount)  //连续复读
                {
                    messageRecord.Repeated = true;
                    return;
                }

                if (repeatSetting.RandomProbability > 0 && Random.Shared.NextDouble() < repeatSetting.RandomProbability)  //随机复读
                {
                    messageRecord.Repeated = true;
                    return;
                }
            }
        }

        private Dictionary<uint, LastMessage> _lastMessages = new();
    }

    public class LastMessage
    {
        public string Message { get; set; }
        public int Repetitions { get; set; }
        public bool Repeated { get; set; }
    }
}
