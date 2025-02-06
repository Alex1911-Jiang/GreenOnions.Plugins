using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.TicTacToe
{
    public class TicTacToePlugin : IPlugin
    {
        private Config? _config;
        private Regex? _startGameCommandRegex;
        private Regex? _exitGameCommandRegex;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;
        private List<Session> _playingUsers = new();
        private Timer? timeOutChecker = null;

        public string Name => "井字棋";

        public string Description => "井字棋游戏插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);

            _startGameCommandRegex = new Regex(config.StartCommand.ReplaceTags());
            _exitGameCommandRegex = new Regex(config.ExitCommand.ReplaceTags());

            if (timeOutChecker is not null)
                timeOutChecker.Dispose();
            if (config.Enabled)
                timeOutChecker = new Timer(CheckTimeOut, null, 1000, 1000);
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

            _startGameCommandRegex = new Regex(config.StartCommand.ReplaceTags());
            _exitGameCommandRegex = new Regex(config.ExitCommand.ReplaceTags());

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;

            if (timeOutChecker is not null)
                timeOutChecker.Dispose();
            if (config.Enabled)
                timeOutChecker = new Timer(CheckTimeOut, null, 1000, 1000);
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
                LogHelper.LogWarning("井字棋插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用井字棋
                return;

            if (_exitGameCommandRegex is null || _startGameCommandRegex is null)
            {
                LogHelper.LogWarning("没有井字棋命令匹配式");
                return;
            }

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var entity in chain)
            {
                if (entity is TextEntity text)
                {
                    string msg = text.Text;

                    if (firstAt)
                        msg = _commonConfig.BotName + msg.TrimStart();

                    bool matchOff = _exitGameCommandRegex.IsMatch(msg);
                    bool matchOn = _startGameCommandRegex.IsMatch(msg);

                    if (matchOff)
                    {
                        await ExitGame(chain, _config);
                        return;
                    }

                    if (matchOn)
                    {
                        await StartGame(context, chain, _config);
                        return;
                    }
                }
                else if (entity is ImageEntity image)
                {
                    Session? session = GetPlayingUser(chain);
                    if (session is null)  //不是下棋的的图片消息
                        return;

                    if (image.PictureSize.X != image.PictureSize.Y)  //图片长宽不一样，可能是穿插的表情包
                        return;

                    session.TimeOut = DateTime.Now.AddSeconds(_config.TimeoutSecond);  //重置棋局超时时间

                    using HttpClientHandler handle = new HttpClientHandler { UseProxy = false };
                    using HttpClient client = new HttpClient(handle);
                    byte[] img;
                    try
                    {
                        img = await client.GetByteArrayAsync(image.ImageUrl.Replace("https://multimedia.nt.qq.com.cn", "http://multimedia.nt.qq.com.cn"));
                        if (img.Length == 0 || img.All(b => b == 0))
                            throw new Exception("图片为空");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogException(ex, $"井字棋插件下载图片 {image.ImageUrl} 失败");
                        await chain.ReplyAsync(_config.DownloadImageFailReply);
                        return;
                    }
                    await PlayerMove(context, chain, session, _config, img);
                    return;
                }
            }
        }

        internal async Task PlayerMove(BotContext bot, MessageChain chain, Session session, Config config, byte[] playMoveImg)
        {
            MoveValidities playerMoveValidity = session.PlayerMove(playMoveImg);
            if (playerMoveValidity != MoveValidities.Valid)
            {
                switch (playerMoveValidity)
                {
                    case MoveValidities.Invalid:
                        await chain.ReplyAsync(config.NoMoveReply);
                        return;
                    case MoveValidities.MultiSelection:
                        await chain.ReplyAsync(config.IllegalMoveReply);
                        return;
                    case MoveValidities.Occupied:
                        await chain.ReplyAsync(config.MoveFailReply);
                        return;
                }
            }
            ScoreTypes score = session.GetScore();
            if (score == ScoreTypes.NoResult)
                session.ComputerMove();
            score = session.GetScore();

            byte[] result = await session.GetImageBytes();

            MessageBuilder msg;
            if (chain.GroupUin is not null)
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Image(result);
            else
                msg = MessageBuilder.Friend(chain.FriendUin).Image(result);

            if (score == ScoreTypes.NoResult)
            {
                await bot.SendMessage(msg.Build());
                return;
            }

            lock (_playingUsers)
            {
                _playingUsers.Remove(session);
                session.Dispose();
            }

            switch (score)
            {
                case ScoreTypes.Draw:
                    msg.Text(config.ScoreDrawReply.ReplaceTags());
                    break;
                case ScoreTypes.PlayerWin:
                    msg.Text(config.PlayerWonReply.ReplaceTags());
                    break;
                case ScoreTypes.ComputeWin:
                    msg.Text(config.BotWonReply.ReplaceTags());
                    break;
            }
            await bot.SendMessage(msg.Build());
        }

        /// <summary>
        /// 开始棋局
        /// </summary>
        public async Task StartGame(BotContext bot, MessageChain chain, Config config)
        {
            Session? session = GetPlayingUser(chain);
            if (session is not null)
            {
                session.TimeOut = DateTime.Now.AddSeconds(config.TimeoutSecond);
                await chain.ReplyAsync(config.AlreadyStartReply);
                return;
            }
            session = new Session(chain, config);
            _playingUsers.Add(session);
            byte[] chessboard = await session.GetImageBytes();

            MessageChain msg;
            if (chain.GroupUin is not null)
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(config.StartedReply.ReplaceTags()).Image(chessboard).Build();
            else
                msg = MessageBuilder.Friend(chain.FriendUin).Image(chessboard).Text(config.StartedReply.ReplaceTags()).Build();
            await bot.SendMessage(msg);
        }

        /// <summary>
        /// 退出棋局
        /// </summary>
        private async Task ExitGame(MessageChain chain, Config config)
        {
            Session? session = GetPlayingUser(chain);
            if (session is null)
            {
                if (string.IsNullOrWhiteSpace(config.AlreadExitReply))
                    return;
                await chain.ReplyAsync(config.AlreadExitReply);
                return;
            }

            lock (_playingUsers)
            {
                _playingUsers.Remove(session);
                session.Dispose();
            }
            await chain.ReplyAsync(config.ExitedReply);
        }

        private async void CheckTimeOut(object? state)
        {
            Session[] timeOutUsers = _playingUsers.Where(u => DateTime.Now > u.TimeOut).ToArray();
            foreach (var item in timeOutUsers)
            {
                if (_config is null)
                    continue;
                await item.Chain.ReplyAsync(_config.TimeoutReply);
                lock (_playingUsers)
                {
                    _playingUsers.Remove(item);
                    item.Dispose();
                }
            }
        }

        private Session? GetPlayingUser(MessageChain chain)
        {
            return _playingUsers.FirstOrDefault(u => u.Chain.FriendUin == chain.FriendUin && u.Chain.GroupUin == chain.GroupUin);
        }
    }
}
