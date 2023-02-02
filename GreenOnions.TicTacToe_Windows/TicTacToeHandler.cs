using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace GreenOnions.TicTacToe_Windows
{
    public class TicTacToeHandler : IMessagePlugin, IPluginSetting, IPluginHelp
    {
        private string? _pluginPath;
        private Regex? _regexTicTacToeStart;
        private Regex? _regexTicTacToeStop;
        private TicTacToeConfig? _config;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;

        public IDictionary<long, TicTacToeSession> PlayingTicTacToeSessions { get; } = new Dictionary<long, TicTacToeSession>();
        public ConcurrentDictionary<long, DateTime> PlayingTicTacToeUsers { get; } = new ConcurrentDictionary<long, DateTime>();

        public string Name => "井字棋";

        public string Description => "葱葱井字棋游戏插件";

        public GreenOnionsMessages HelpMessage
        {
            get
            {
                if (!Directory.Exists("Icon"))
                    Directory.CreateDirectory("Icon");

                List<GreenOnionsBaseMessage> messages = new()
                {
                    $"发送\"{_api!.ReplaceGreenOnionsStringTags(_config!.StartTicTacToeCmd)}\"来开启一局井字棋游戏。\r\n",
                    $"{_botConfig!.BotName}会发送一个空棋盘图片，\r\n"
                };

                if ((_config.TicTacToeMoveMode & TicTacToeMoveMode.OpenCV) != 0)
                {
                    messages.Add("您可以对棋盘进行表情涂鸦来进行下子。\r\n");
                    messages.Add("手机端操作方式：\r\n");

                    string mobieGraffitiFile = Path.Combine(_pluginPath!, "Icon", "MobieGraffiti.jpg");
                    Resource.MobieGraffiti.Save(mobieGraffitiFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(mobieGraffitiFile));

                    messages.Add("电脑端操作方式：\r\n");

                    string pcGraffitiFile = Path.Combine(_pluginPath!, "Icon", "PcGraffiti.jpg");
                    Resource.PcGraffiti.Save(pcGraffitiFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(pcGraffitiFile));
                }
                if (_config.TicTacToeMoveMode == (TicTacToeMoveMode.OpenCV | TicTacToeMoveMode.Nomenclature))
                {
                    messages.Add("另外，");
                }
                if ((_config.TicTacToeMoveMode & TicTacToeMoveMode.Nomenclature) != 0)
                {
                    messages.Add("您可以通过输入格号来下子，如\"C2\"。\r\n");
                    messages.Add("棋盘编号命名示例为：\r\n");

                    string chessboardFile = Path.Combine(_pluginPath!, "Icon", "Chessboard.jpg");
                    Resource.Chessboard.Save(chessboardFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(chessboardFile));
                }
                return messages.ToArray();
            }
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            _regexTicTacToeStart = new Regex(_api!.ReplaceGreenOnionsStringTags(_config!.StartTicTacToeCmd));
            _regexTicTacToeStop = new Regex(_api!.ReplaceGreenOnionsStringTags(_config!.StopTicTacToeCmd));
        }

        /// <summary>
        /// 开始棋局
        /// </summary>
        /// <param name="qqId">玩家QQ</param>
        public void StartTicTacToeSession(long qqId, Action<GreenOnionsMessages> SendMessage)
        {
            if (PlayingTicTacToeUsers.ContainsKey(qqId))
            {
                PlayingTicTacToeUsers[qqId] = DateTime.Now.AddMinutes(2);
                SendMessage(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeAlreadyStartReply));
            }
            else
            {
                PlayingTicTacToeUsers.TryAdd(qqId, DateTime.Now.AddMinutes(2));

                _api!.SetWorkingTimeout(qqId, () =>  //启动棋局计时
                {
                    if (PlayingTicTacToeSessions.ContainsKey(qqId))
                        PlayingTicTacToeSessions.Remove(qqId);
                    if (PlayingTicTacToeUsers.ContainsKey(qqId))
                        PlayingTicTacToeUsers.TryRemove(qqId, out _);
                    SendMessage(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeTimeoutReply));  //超时退出棋局
                }, PlayingTicTacToeUsers);

                TicTacToeSession session = new();
                Bitmap chessboard = session.StartNewSession();  //不能using, 要保留在棋局对象里
                using MemoryStream tempMs = new();
                chessboard.Save(tempMs, ImageFormat.Jpeg);
                using MemoryStream ms = new(tempMs.ToArray());
                PlayingTicTacToeSessions.Add(qqId, session);
                SendMessage(new GreenOnionsBaseMessage[] { _api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeStartedReply), ms });
            }
        }

        /// <summary>
        /// 中止棋局
        /// </summary>
        /// <param name="qqId">玩家QQ</param>
        public void StopTicTacToeSession(long qqId, Action<GreenOnionsMessages> SendMessage)
        {
            if (PlayingTicTacToeUsers.ContainsKey(qqId))
            {
                PlayingTicTacToeUsers.TryRemove(qqId, out _);
                SendMessage(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeStoppedReply));
            }
            else
            {
                SendMessage(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeAlreadStopReply));
            }
            if (PlayingTicTacToeSessions.ContainsKey(qqId))
                PlayingTicTacToeSessions.Remove(qqId);
        }

        /// <summary>
        /// 使用坐标下子
        /// </summary>
        /// <param name="moveMsg">坐标命令</param>
        /// <param name="qqId">玩家QQ</param>
        public void PlayerMoveByNomenclature(string moveMsg, long qqId, Action<GreenOnionsMessages> SendMessage)
        {
            if (PlayingTicTacToeSessions.ContainsKey(qqId))
            {
                PlayingTicTacToeUsers[qqId] = DateTime.Now.AddMinutes(2);

                int x = -1;
                int y = -1;

                if (char.IsDigit(moveMsg[1]))
                    x = moveMsg[1] - '1';

                if (char.IsUpper(moveMsg[0]))
                    y = moveMsg[0] - 'A';
                else if (char.IsLower(moveMsg[0]))
                    y = moveMsg[0] - 'a';
                else if (char.IsDigit(moveMsg[0]))
                    y = moveMsg[0] - '1';

                if (x > -1 && x < 3 && y > -1 && y < 3)
                {
                    Bitmap? nowStepBmp = PlayingTicTacToeSessions[qqId].PlayerMove(x, y, out int? winOrLostType);
                    if (nowStepBmp == null)  //下子失败
                        SendMessage(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeMoveFailReply));
                    else
                        SendMessage(SendBitmapAfterMove(qqId, nowStepBmp, winOrLostType));
                }
            }
            else
                SendMessageToAdmin($"葱葱井字棋插件错误：数据异常, 时间表中存在QQ:{qqId}, 但对局表中不存在, 可能是刚刚超时了(坐标下子操作)");
        }

        /// <summary>
        /// 使用涂鸦下子
        /// </summary>
        /// <param name="qqId">玩家QQ</param>
        /// <param name="playerMoveStream">玩家下子图片</param>
        public GreenOnionsMessages? PlayerMoveByBitmap(long qqId, Stream playerMoveStream)
        {
            if (PlayingTicTacToeSessions.ContainsKey(qqId))
            {
                PlayingTicTacToeUsers[qqId] = DateTime.Now.AddMinutes(2);

                Bitmap bmpTemp = new(playerMoveStream);
                Bitmap playerMoveBmp = new(bmpTemp);
                bmpTemp.Dispose();
                if (playerMoveBmp != null)
                {
                    int isameSize = PlayingTicTacToeSessions[qqId].IsBitmapSizeSame(playerMoveBmp.Width, playerMoveBmp.Height); //图片尺寸相同才进行识别, 有时沙雕群友都喜欢在棋局中间穿插表情包
                    if (isameSize != -1)
                    {
                        Dictionary<Point, int>? weight = PlayingTicTacToeSessions[qqId].PlayerMoveByBitmap(playerMoveBmp);

                        if (weight.Keys.Count == 0)  //没有修改
                            return _api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeNoMoveReply);
                        else if (weight.Keys.Count > 1)  //多个格子被修改
                            return _api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeIllegalMoveReply);
                        else
                        {
                            var maxWeight = weight.Where(kv => kv.Value > 11).OrderByDescending(kv => kv.Value);
                            if (maxWeight != null && maxWeight.Any())
                            {
                                Point hit = maxWeight.First().Key;
                                Bitmap? nowStepBmp = PlayingTicTacToeSessions[qqId].PlayerMove(hit.X, hit.Y, out int? winOrLostType);
                                if (nowStepBmp == null)
                                    return _api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeMoveFailReply);
                                else
                                    return SendBitmapAfterMove(qqId, nowStepBmp, winOrLostType);
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    SendMessageToAdmin("葱葱井字棋插件错误：图片转换失败");
                    return _api!.ReplaceGreenOnionsStringTags("图裂了o(╥﹏╥)o");
                }
            }
            else
            {
                SendMessageToAdmin($"葱葱井字棋插件错误：数据异常, 时间表中存在QQ:{qqId}, 但对局表中不存在, 可能是刚刚超时了(涂鸦下子操作)");
                return _api!.ReplaceGreenOnionsStringTags("<机器人名称>把图弄丢了, 这局就当您赢了吧, 请向<机器人名称>反馈Bug o(╥﹏╥)o");
            }
        }

        public GreenOnionsMessages SendBitmapAfterMove(long qqId, Bitmap nowStepBmp, int? winOrLostMsg)
        {
            using MemoryStream tempMs = new();
            nowStepBmp.Save(tempMs, ImageFormat.Jpeg);
            using MemoryStream ms = new(tempMs.ToArray());
            GreenOnionsMessages outMsg = new() { ms };

            if (winOrLostMsg != null)
            {
                switch (winOrLostMsg)
                {
                    case -1: //bot获胜
                        outMsg.Add(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeBotWinReply));
                        break;
                    case 0:  //平局
                        outMsg.Add(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToeDrawReply));
                        break;
                    case 1:  //玩家获胜
                        outMsg.Add(_api!.ReplaceGreenOnionsStringTags(_config!.TicTacToePlayerWinReply));
                        break;
                }
                PlayingTicTacToeSessions.Remove(qqId);
                PlayingTicTacToeUsers.TryRemove(qqId, out _);
            }
            return outMsg;
        }

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            _botConfig = botConfig;
            _pluginPath = pluginPath;
            string configFileName = Path.Combine(_pluginPath, "config.json");
            if (File.Exists(configFileName))
            {
                string strConfig = File.ReadAllText(configFileName);
                _config = JsonSerializer.Deserialize<TicTacToeConfig>(strConfig);
            }
            else
            {
                _config = new TicTacToeConfig();
            }
        }

        public void OnDisconnected()
        {
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (PlayingTicTacToeUsers.ContainsKey(msgs.SenderId))  //井字棋
            {
                if (msgs.Count == 1 && msgs?.First() is GreenOnionsImageMessage imgMsg)
                {
                    DownloadImageAsMemoryStreamAsync(imgMsg.Url!).ContinueWith(t =>
                    {
                        using MemoryStream? playerMoveStream = t.Result;
                        if (playerMoveStream == null)
                            return;  //图片下载失败, 暂时没想好怎么处理

                        GreenOnionsMessages? outMsgs = PlayerMoveByBitmap(msgs.SenderId, playerMoveStream);
                        if (outMsgs != null)
                            Response(outMsgs);
                    });
                    return true;
                }
            }
            if (msgs?.First() is GreenOnionsTextMessage txtMsg)
            {
                if (_regexTicTacToeStart!.IsMatch(txtMsg.Text))
                {
                    StartTicTacToeSession(msgs.SenderId, Response);
                    return true;
                }
                else if (_regexTicTacToeStop!.IsMatch(txtMsg.Text))
                {
                    StopTicTacToeSession(msgs.SenderId, Response);
                    return true;
                }
                else if ((_config!.TicTacToeMoveMode & TicTacToeMoveMode.Nomenclature) != 0 && PlayingTicTacToeUsers.ContainsKey(msgs.SenderId) && txtMsg.Text.Length == 2)
                {
                    PlayerMoveByNomenclature(txtMsg.Text, msgs.SenderId, Response);
                    return true;
                }
            }
            return false;
        }

        public void Setting()
        {
            new FrmSettings(_config!, _pluginPath!).ShowDialog();
            if (_api != null)
            {
                _regexTicTacToeStart = new Regex(_api.ReplaceGreenOnionsStringTags(_config!.StartTicTacToeCmd));
                _regexTicTacToeStop = new Regex(_api.ReplaceGreenOnionsStringTags(_config!.StopTicTacToeCmd));
            }
        }

        private async void SendMessageToAdmin(string msg)
        {
            foreach (long item in _botConfig!.AdminQQ)
            {
                await _api!.SendFriendMessageAsync(item, msg);
            }
        }

        public static async Task<MemoryStream?> DownloadImageAsMemoryStreamAsync(string url)
        {
            bool retry = true;
        IL_Retry:;
            using (HttpClient httpClient = new())
            {
                try
                {
                    var t = await httpClient.GetAsync(url);
                    byte[] bytes = await t.Content.ReadAsByteArrayAsync();
                    MemoryStream? ms = new(bytes);
                    if (ms.Length == 0)
                    {
                        if (retry)
                        {
                            retry = false;
                            goto IL_Retry;
                        }
                        else
                        {
                            ms.Dispose();
                            ms = null;
                        }
                    }
                    return ms;
                }
                catch (Exception)
                {
                    if (retry)
                    {
                        retry = false;
                        goto IL_Retry;
                    }
                    return null;
                }
            }
        }
    }

    public enum TicTacToeMoveMode : int
    {
        OpenCV = 2,
        Nomenclature = 4,
        All = OpenCV | Nomenclature
    }
}