using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Clients;
using GreenOnions.NT.PictureSearcher.Enums;
using HtmlAgilityPack;
using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.PictureSearcher
{
    public class PictureSearcherPlugin : IPlugin
    {
        private BotContext? _bot;
        private Regex? _searchOnCommandRegex;
        private Regex? _searchOffCommandRegex;
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;
        private List<SearchingUser> _searchingUsers = new();
        private Timer? timeOutChecker;

        public string Name => "搜图";

        public string Description => "多平台聚合搜图插件";

        public void OnConfigUpdate(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            LoadConfig(_pluginPath);
            if (_config is null)
                return;
            _searchOnCommandRegex = new Regex(_config.SearchModeOnCmd.Replace("<机器人名称>", commonConfig.BotName));
            _searchOffCommandRegex = new Regex(_config.SearchModeOffCmd.Replace("<机器人名称>", commonConfig.BotName));
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
            _bot = bot;
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);

            _searchOnCommandRegex = new Regex(config.SearchModeOnCmd.Replace("<机器人名称>", commonConfig.BotName));
            _searchOffCommandRegex = new Regex(config.SearchModeOffCmd.Replace("<机器人名称>", commonConfig.BotName));

            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;

            timeOutChecker = new Timer(CheckTimeOut, null, 1000, 1000);
        }

        private async void CheckTimeOut(object? state)
        {
            if (_bot is null)
                return;
            SearchingUser[] timeOutUsers = _searchingUsers.Where(u => DateTime.Now > u.TimeOut).ToArray();
            foreach (var item in timeOutUsers)
            {
                lock (_searchingUsers)
                {
                    _searchingUsers.Remove(item);
                }
                if (_config is null)
                    continue;
                await _bot.ReplyAsync(item.Chain, _config.SearchModeTimeOutReply.Replace("<机器人名称>", _commonConfig?.BotName));
            }
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
                LogHelper.LogWarning("搜图插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用搜图
                return;

            if (_searchOnCommandRegex is null || _searchOffCommandRegex is null)
            {
                LogHelper.LogWarning("没有搜图命令匹配式");
                return;
            }

            if (_commonConfig.DebugMode && chain.GroupUin is not null && !_commonConfig.DebugGroups.Contains(chain.GroupUin.Value))
                return;
            if (_commonConfig.DebugMode && chain.GroupUin is null && !_commonConfig.AdminQQ.Contains(chain.FriendUin))
                return;

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var item in chain)
            {
                if (item is TextEntity text)
                {
                    string msg = text.Text;

                    if (firstAt)
                        msg = _commonConfig.BotName + msg.TrimStart();

                    bool matchOff = _searchOffCommandRegex.IsMatch(msg);
                    bool matchOn = _searchOnCommandRegex.IsMatch(msg);

                    if (matchOff)
                    {
                        await SearchOff(_commonConfig, _config, context, chain);
                        return;
                    }

                    if (matchOn)
                    {
                        await SearchOn(_commonConfig, _config, context, chain);
                        return;
                    }
                }
                else if (item is ImageEntity image)
                {
                    SearchingUser? user = GetSearchingUser(chain);
                    if (user is null)  //不是搜图的图片消息
                        return;

                    user.TimeOut = DateTime.Now.AddMinutes(2);  //将搜图超时时间延长到2分钟后
                    await Search(_commonConfig, _config, context, chain, image.ImageUrl);
                }
            }
        }

        private async Task Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            LogHelper.LogMessage($"进入搜图处理逻辑，需要搜索的图片地址为：{imageUrl}");

            if (config.EnabledSources.Length == 0)
            {
                LogHelper.LogWarning("启用了搜图功能但没有启用任何搜图引擎");
                return;
            }

            if (!string.IsNullOrWhiteSpace(config.SearchingReply))  //开始搜索回复
                await context.ReplyAsync(chain, config.SearchingReply.Replace("<机器人名称>", commonConfig.BotName));

            foreach (var item in config.EnabledSources)
            {
                double similarity = 0;
                switch (item)
                {
                    case SearcherSources.SauceNAO:
                        similarity = await SauceNAOSearcher.Search(commonConfig, config, context, chain, imageUrl);
                        break;
                    case SearcherSources.Ascii2d:
                        similarity = await Ascii2dSearcher.Search(commonConfig, config, context, chain, imageUrl);
                        break;
                    case SearcherSources.TraceMoe:
                        similarity = await TraceMoeSearcher.Search(commonConfig, config, context, chain, imageUrl);
                        break;
                    case SearcherSources.IqdbAnime:
                        similarity = await IqdbSearcher.SearchAnime(commonConfig, config, context, chain, imageUrl);
                        break;
                    case SearcherSources.Iqdb3d:
                        similarity = await IqdbSearcher.Search3d(commonConfig, config, context, chain, imageUrl);
                        break;
                    case SearcherSources.AnimeTrace:
                        break;
                    default:
                        break;
                }
                if (similarity >= config.BreakSimilarity)
                    break;
            }
        }

        private async Task SearchOn(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain)
        {
            SearchingUser? user = GetSearchingUser(chain);
            if (user is not null)
            {
                user.TimeOut = DateTime.Now.AddMinutes(2);  //将搜图超时时间延长到2分钟后
                await context.ReplyAsync(chain, config.SearchModeAlreadyOnReply.Replace("<机器人名称>", commonConfig.BotName));  //已经在连续搜图模式中回复
                return;
            }
            lock (_searchingUsers)
            {
                _searchingUsers.Add(new SearchingUser(chain, DateTime.Now.AddMinutes(2)));
            }
            await context.ReplyAsync(chain, config.SearchModeOnReply.Replace("<机器人名称>", commonConfig.BotName));  //进入连续搜图模式回复
        }

        private async Task SearchOff(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain)
        {
            SearchingUser? user = GetSearchingUser(chain);
            if (user is null)
            {
                await context.ReplyAsync(chain, config.SearchModeAlreadyOffReply.Replace("<机器人名称>", commonConfig.BotName));  //没有正在搜图回复
                return;
            }
            lock (_searchingUsers)
            {
                _searchingUsers.Remove(user);
            }
            await context.ReplyAsync(user.Chain, config.SearchModeOffReply.Replace("<机器人名称>", commonConfig.BotName));  //退出搜图模式回复
        }

        private SearchingUser? GetSearchingUser(MessageChain chain)
        {
            return _searchingUsers.FirstOrDefault(u => u.Chain.FriendUin == chain.FriendUin && u.Chain.GroupUin == chain.GroupUin);
        }
    }
}
