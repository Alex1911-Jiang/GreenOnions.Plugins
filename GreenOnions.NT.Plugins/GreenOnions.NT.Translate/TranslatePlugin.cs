using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.Translate.Clients;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.Translate
{
    public class TranslatePlugin : IPlugin
    {
        private Regex? _translateToChineseCommandRegex;
        private Regex? _translateToLanguageCommandRegex;
        private Regex? _translateFromLanguageToLanguageChineseCommandRegex;
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;

        public string Name => "翻译";
        public string Description => "翻译插件";
        public void OnConfigUpdate(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _translateToChineseCommandRegex = new Regex(config.TranslateToChineseCommand.ReplaceTags());
            _translateToLanguageCommandRegex = new Regex(config.TranslateToLanguageCommand.ReplaceTags());
            _translateFromLanguageToLanguageChineseCommandRegex = new Regex(config.TranslateFromLanguageToLanguageCommand.ReplaceTags());
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

        public void OnLoad(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);
            _translateToChineseCommandRegex = new Regex(config.TranslateToChineseCommand.ReplaceTags());
            _translateToLanguageCommandRegex = new Regex(config.TranslateToLanguageCommand.ReplaceTags());
            _translateFromLanguageToLanguageChineseCommandRegex = new Regex(config.TranslateFromLanguageToLanguageCommand.ReplaceTags());

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
        }

        private async void OnGroupMessage(BotContext context, Lagrange.Core.Event.EventArg.GroupMessageEvent e)
        {
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在遇见范围内的异常");
            }
        }

        private async void OnFriendMessage(BotContext context, Lagrange.Core.Event.EventArg.FriendMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在遇见范围内的异常");
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
                LogHelper.LogWarning("翻译插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用翻译
                return;


            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var item in chain)
            {
                if (item is not TextEntity text)
                    continue;

                string msg = text.Text;

                if (firstAt)
                    msg = _commonConfig.BotName + msg.TrimStart();
                Match match;
                if (_translateToChineseCommandRegex is null)
                    goto IL_Next1;
                LogHelper.LogMessage($"{chain.FriendUin} 的消息：'{msg}'消息触发自动识别语言并翻译为中文");
                match = _translateToChineseCommandRegex.Match(msg);
                if (match.Success)
                {
                    try
                    {
                        string src = match.Groups["Value"].Value;
                        string dst = await TranslateToChinese(src);
                        await chain.ReplyAsync(dst, false);
                    }
                    catch (Exception ex)
                    {
                        await chain.ReplyAsync(_config.TranslateFailReply.Replace("<错误信息>", ex.Message));
                    }
                }
            IL_Next1:
                if (_translateToLanguageCommandRegex is null)
                    goto IL_Next2;
                LogHelper.LogMessage($"{chain.FriendUin} 的消息：'{msg}'消息触发自动识别语言并翻译为指定语言");
                match = _translateToLanguageCommandRegex.Match(msg);
                if (match.Success)
                {
                    try
                    {
                        string toLanguage = match.Groups["ToLanguage"].Value;
                        string src = match.Groups["Value"].Value;
                        string dst = await TranslateTo(src, toLanguage);
                        await chain.ReplyAsync(dst, false);
                    }
                    catch (Exception ex)
                    {
                        await chain.ReplyAsync(_config.TranslateFailReply.Replace("<错误信息>", ex.Message));
                    }
                }
            IL_Next2:
                if (_translateFromLanguageToLanguageChineseCommandRegex is null)
                    return;
                LogHelper.LogMessage($"{chain.FriendUin} 的消息：'{msg}'消息触发从指定语言翻译为指定语言");
                match = _translateFromLanguageToLanguageChineseCommandRegex.Match(msg);
                if (match.Success)
                {
                    try
                    {
                        string fromLanguage = match.Groups["FromLanguage"].Value;
                        string toLanguage = match.Groups["ToLanguage"].Value;
                        string src = match.Groups["Value"].Value;
                        string dst = await TranslateFromTo(src, fromLanguage, toLanguage);
                        await chain.ReplyAsync(dst, false);
                    }
                    catch (Exception ex)
                    {
                        await chain.ReplyAsync(_config.TranslateFailReply.Replace("<错误信息>", ex.Message));
                    }
                }
            }
        }

        private BaseTranslateClient CreateClient()
        {
            if (_config is null)
                throw new Exception("未配置翻译插件");
            if (!_config.Enabled)
                throw new Exception("管理员关闭了翻译功能");

            return _config.Engine switch
            {
                TranslateEngines.YouDaoApi => new YouDaoTranslateApiClient(_config),
                TranslateEngines.BaiduApi => new BaiduTranslateApiClient(_config),
                TranslateEngines.AliyunApi => new AliyunTranslateApiClient(_config),
                TranslateEngines.TencentApi => new TencentTranslateApiClient(_config),
                TranslateEngines.GoogleApi => new GoogleTranslateApiClient(_config),
                _ => throw new NotImplementedException("无效的翻译引擎"),
            };
        }

        public Task<string> TranslateToChinese(string text)
        {
            return CreateClient().TranslateToChinese(text);
        }

        public Task<string> TranslateTo(string text, string toLanguageChineseName)
        {
            return CreateClient().TranslateTo(text, toLanguageChineseName);
        }

        public Task<string> TranslateFromTo(string text, string fromLanguageChineseName, string toLanguageChineseName)
        {
            return CreateClient().TranslateFromTo(text, fromLanguageChineseName, toLanguageChineseName);
        }
    }
}
