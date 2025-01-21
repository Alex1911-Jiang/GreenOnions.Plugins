using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.PixivDownloader
{
    public class DownloaderPlugin : IPlugin
    {
        private Regex? _downloadCommandRegex;
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;

        public string Name => "Pixiv原图下载器";
        public string Description => "输入Pixiv作品ID下载原图插件";
        public void OnConfigUpdate(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _downloadCommandRegex = new Regex(config.Command.ReplaceTags());
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
            _downloadCommandRegex = new Regex(config.Command.ReplaceTags());

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
                LogHelper.LogWarning("下载Pixiv原图插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用下载Pixiv原图
                return;

            if (_downloadCommandRegex is null)
            {
                LogHelper.LogWarning("没有下载Pixiv原图命令匹配式");
                return;
            }

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var item in chain)
            {
                if (item is not TextEntity text)
                    continue;

                string msg = text.Text;

                if (firstAt)
                    msg = _commonConfig.BotName + msg.TrimStart();

                Match matchHPcitureCmd = _downloadCommandRegex.Match(msg);
                if (!matchHPcitureCmd.Success)
                    return;

                string pidStr = matchHPcitureCmd.Groups["pid"].Value;
                await ExtractAsync(pidStr, _config, context, chain);
                return;
            }
        }

        private async Task ExtractAsync(string strPixivId, Config config, BotContext context, MessageChain chain)
        {
            string[] idWithIndex = strPixivId.Split("-");
            string[] idWithP = strPixivId.ToLower().Split(['p', 'P'], StringSplitOptions.None);
            if (idWithIndex.Length != 2 && idWithP.Length != 2)
                return;
            if (idWithIndex.Length == 2 && idWithP.Length == 2)
                return;

            await chain.ReplyAsync(config.DownloadingReply);

            long id;
            if (idWithIndex.Length == 2 && long.TryParse(idWithIndex[0], out id) && int.TryParse(idWithIndex[1], out int index))
                await SendImageAsync(id, index - 1, config, context, chain);
            if (idWithP.Length == 2 && long.TryParse(idWithP[0], out id) && int.TryParse(idWithP[1], out int p))
                await SendImageAsync(id, p, config, context, chain);
        }

        private async Task SendImageAsync(long id, int p, Config config, BotContext context, MessageChain chain)
        {
            string index = "";
            if (p != -1)
                index = $"-{p + 1}";

            string url = $"https://{config.PixivProxy}/{id}{index}.png";

            using HttpClientHandler handler = new() { UseProxy = config.UseProxy };
            using HttpClient client = new(handler);

            byte[] img;
            try
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    LogHelper.LogError($"下载Pixiv {url} 原图失败，{(int)response.StatusCode} {response.StatusCode}");
                    await chain.ReplyAsync(config.DownloadFailReply.Replace("<错误信息>", $"{(int)response.StatusCode} {response.StatusCode}"));
                    return;
                }
                img = await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"下载Pixiv {url} 原图失败");
                await chain.ReplyAsync(config.DownloadFailReply.Replace("<错误信息>", ex.Message));
                return;
            }

            MessageChain msg;
            if (chain.GroupUin is not null)
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Image(img).Build();
            else
                msg = MessageBuilder.Friend(chain.FriendUin).Image(img).Build();
            await context.SendMessage(msg);
        }
    }
}
