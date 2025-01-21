using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Clients;
using GreenOnions.NT.PictureSearcher.Enums;
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
        private Timer? timeOutChecker = null;

        public string Name => "搜图";

        public string Description => "多平台聚合搜图插件";

        public void OnConfigUpdate(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _searchOnCommandRegex = new Regex(config.SearchModeOnCmd.ReplaceTags());
            _searchOffCommandRegex = new Regex(config.SearchModeOffCmd.ReplaceTags());
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
            if (timeOutChecker is not null)
                timeOutChecker.Dispose();

            _pluginPath = pluginPath;
            _bot = bot;
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);

            _searchOnCommandRegex = new Regex(config.SearchModeOnCmd.ReplaceTags());
            _searchOffCommandRegex = new Regex(config.SearchModeOffCmd.ReplaceTags());

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
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
                await item.Chain.ReplyAsync(_config.SearchModeTimeOutReply);
            }
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
                LogHelper.LogException(ex, $"{Name}插件发生了不在遇见范围内的异常");
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
                await chain.ReplyAsync(config.SearchingReply);

            int animeTracModelIndex = 0;
            foreach (var item in config.EnabledSources)
            {
                double similarity = item switch
                {
                    SearcherSources.SauceNAO => await SauceNAOSearcher.Search(commonConfig, config, context, chain, imageUrl),
                    SearcherSources.Ascii2d => await Ascii2dSearcher.Search(commonConfig, config, context, chain, imageUrl),
                    SearcherSources.TraceMoe => await TraceMoeSearcher.Search(commonConfig, config, context, chain, imageUrl),
                    SearcherSources.IqdbAnime => await IqdbSearcher.SearchAnime(commonConfig, config, context, chain, imageUrl),
                    SearcherSources.Iqdb3d => await IqdbSearcher.Search3d(commonConfig, config, context, chain, imageUrl),
                    SearcherSources.AnimeTrace => await AnimeTraceSearcher.Search(commonConfig, config, context, chain, imageUrl, animeTracModelIndex++),
                    _ => throw new NotImplementedException("不支持的搜索引擎"),
                };
                
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
                await chain.ReplyAsync(config.SearchModeAlreadyOnReply);  //已经在连续搜图模式中回复
                return;
            }
            lock (_searchingUsers)
            {
                _searchingUsers.Add(new SearchingUser(chain, DateTime.Now.AddMinutes(2)));
            }
            await chain.ReplyAsync(config.SearchModeOnReply);  //进入连续搜图模式回复
        }

        private async Task SearchOff(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain)
        {
            SearchingUser? user = GetSearchingUser(chain);
            if (user is null)
            {
                await chain.ReplyAsync(config.SearchModeAlreadyOffReply);  //没有正在搜图回复
                return;
            }
            lock (_searchingUsers)
            {
                _searchingUsers.Remove(user);
            }
            await user.Chain.ReplyAsync(config.SearchModeOffReply);  //退出搜图模式回复
        }

        private SearchingUser? GetSearchingUser(MessageChain chain)
        {
            return _searchingUsers.FirstOrDefault(u => u.Chain.FriendUin == chain.FriendUin && u.Chain.GroupUin == chain.GroupUin);
        }
    }
}
