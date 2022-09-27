using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace GreenOnions.TicTacToe_Windows
{
    public class TicTacToeHandler : IPlugin
    {
        private string _pluginPath;
        private Regex _regexTicTacToeStart;
        private Regex _regexTicTacToeStop;
        private GreenOnionsApi _api;

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

                List<GreenOnionsBaseMessage> messages = new List<GreenOnionsBaseMessage>();

                messages.Add($"发送\"{TicTacToeConfig.StartTicTacToeCmd.ReplaceGreenOnionsTags(_api.BotProperties)}\"来开启一局井字棋游戏。\r\n");
                messages.Add($"{_api.BotProperties["机器人名称"]}会发送一个空棋盘图片，\r\n");

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if ((TicTacToeConfig.TicTacToeMoveMode & (int)TicTacToeMoveMode.OpenCV) != 0)
                {
                    messages.Add("您可以对棋盘进行表情涂鸦来进行下子。\r\n");
                    messages.Add("手机端操作方式：\r\n");

                    string mobieGraffitiFile = Path.Combine(path, "Icon", "MobieGraffiti.jpg");
                    Resource.MobieGraffiti.Save(mobieGraffitiFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(mobieGraffitiFile));

                    messages.Add("电脑端操作方式：\r\n");

                    string pcGraffitiFile = Path.Combine(path, "Icon", "PcGraffiti.jpg");
                    Resource.PcGraffiti.Save(pcGraffitiFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(pcGraffitiFile));
                }
                if (TicTacToeConfig.TicTacToeMoveMode == (int)(TicTacToeMoveMode.OpenCV | TicTacToeMoveMode.Nomenclature))
                {
                    messages.Add("另外，");
                }
                if ((TicTacToeConfig.TicTacToeMoveMode & (int)TicTacToeMoveMode.Nomenclature) != 0)
                {
                    messages.Add("您可以通过输入格号来下子，如\"C2\"。\r\n");
                    messages.Add("棋盘编号命名示例为：\r\n");

                    string chessboardFile = Path.Combine(path, "Icon", "Chessboard.jpg");
                    Resource.Chessboard.Save(chessboardFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    messages.Add(new GreenOnionsImageMessage(chessboardFile));
                }
                return messages.ToArray();
            }
        }

        public void OnConnected(long selfId, GreenOnionsApi api)
        {
            _api = api;
            _regexTicTacToeStart = new Regex(TicTacToeConfig.StartTicTacToeCmd.ReplaceGreenOnionsTags(_api.BotProperties)!);
            _regexTicTacToeStop = new Regex(TicTacToeConfig.StopTicTacToeCmd.ReplaceGreenOnionsTags(_api.BotProperties)!);
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
                SendMessage(TicTacToeConfig.TicTacToeAlreadyStartReply.ReplaceGreenOnionsTags(_api.BotProperties));
            }
            else
            {
                PlayingTicTacToeUsers.TryAdd(qqId, DateTime.Now.AddMinutes(2));

                _api.SetWorkingTimeout(qqId, () =>  //启动棋局计时
                {
                    if (PlayingTicTacToeSessions.ContainsKey(qqId))
                        PlayingTicTacToeSessions.Remove(qqId);
                    if (PlayingTicTacToeUsers.ContainsKey(qqId))
                        PlayingTicTacToeUsers.TryRemove(qqId, out _);
                    SendMessage(TicTacToeConfig.TicTacToeTimeoutReply.ReplaceGreenOnionsTags(_api.BotProperties));  //超时退出棋局
                }, PlayingTicTacToeUsers);

                TicTacToeSession session = new TicTacToeSession();
                Bitmap chessboard = session.StartNewSession();  //不能using, 要保留在棋局对象里
                using (MemoryStream tempMs = new MemoryStream())
                {
                    chessboard.Save(tempMs, ImageFormat.Jpeg);
                    using (MemoryStream ms = new MemoryStream(tempMs.ToArray()))
                    {
                        PlayingTicTacToeSessions.Add(qqId, session);
                        SendMessage(new GreenOnionsBaseMessage[] { TicTacToeConfig.TicTacToeStartedReply.ReplaceGreenOnionsTags(_api.BotProperties), ms });
                    }
                }
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
                SendMessage(TicTacToeConfig.TicTacToeStoppedReply.ReplaceGreenOnionsTags(_api.BotProperties));
            }
            else
            {
                SendMessage(TicTacToeConfig.TicTacToeAlreadStopReply.ReplaceGreenOnionsTags(_api.BotProperties));
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
                    Bitmap nowStepBmp = PlayingTicTacToeSessions[qqId].PlayerMove(x, y, out int? winOrLostType);
                    if (nowStepBmp == null)  //下子失败
                        SendMessage(TicTacToeConfig.TicTacToeMoveFailReply.ReplaceGreenOnionsTags(_api.BotProperties));
                    else
                        SendMessage(SendBitmapAfterMove(qqId, nowStepBmp, winOrLostType));
                }
            }
            else
                LogError($"数据异常, 时间表中存在QQ:{qqId}, 但对局表中不存在, 可能是刚刚超时了(坐标下子操作)");
        }

        /// <summary>
        /// 使用涂鸦下子
        /// </summary>
        /// <param name="qqId">玩家QQ</param>
        /// <param name="playerMoveStream">玩家下子图片</param>
        public GreenOnionsMessages PlayerMoveByBitmap(long qqId, Stream playerMoveStream)
        {
            if (PlayingTicTacToeSessions.ContainsKey(qqId))
            {
                PlayingTicTacToeUsers[qqId] = DateTime.Now.AddMinutes(2);

                Bitmap bmpTemp = new Bitmap(playerMoveStream);
                Bitmap playerMoveBmp = new Bitmap(bmpTemp);
                bmpTemp.Dispose();
                if (playerMoveBmp != null)
                {
                    int isameSize = PlayingTicTacToeSessions[qqId].IsBitmapSizeSame(playerMoveBmp.Width, playerMoveBmp.Height); //图片尺寸相同才进行识别, 有时沙雕群友都喜欢在棋局中间穿插表情包
                    if (isameSize != -1)
                    {
                        var weight = PlayingTicTacToeSessions[qqId].PlayerMoveByBitmap(playerMoveBmp);

                        if (weight.Keys.Count == 0)  //没有修改
                            return TicTacToeConfig.TicTacToeNoMoveReply.ReplaceGreenOnionsTags(_api.BotProperties);
                        else if (weight.Keys.Count > 1)  //多个格子被修改
                            return TicTacToeConfig.TicTacToeIllegalMoveReply.ReplaceGreenOnionsTags(_api.BotProperties);
                        else
                        {
                            var maxWeight = weight.Where(kv => kv.Value > 11).OrderByDescending(kv => kv.Value);
                            if (maxWeight != null && maxWeight.Count() > 0)
                            {
                                Point hit = maxWeight.First().Key;
                                Bitmap nowStepBmp = PlayingTicTacToeSessions[qqId].PlayerMove(hit.X, hit.Y, out int? winOrLostType);
                                if (nowStepBmp == null)
                                    return TicTacToeConfig.TicTacToeMoveFailReply.ReplaceGreenOnionsTags(_api.BotProperties);
                                else
                                    return SendBitmapAfterMove(qqId, nowStepBmp, winOrLostType);
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    LogError("井字棋图片转换失败");
                    return "图裂了o(╥﹏╥)o".ReplaceGreenOnionsTags(_api.BotProperties);
                }
            }
            else
            {
                LogError($"数据异常, 时间表中存在QQ:{qqId}, 但对局表中不存在, 可能是刚刚超时了(涂鸦下子操作)");
                return "<机器人名称>把图弄丢了, 这局就当您赢了吧, 请向<机器人名称>反馈Bug o(╥﹏╥)o".ReplaceGreenOnionsTags(_api.BotProperties);
            }
        }

        public GreenOnionsMessages SendBitmapAfterMove(long qqId, Bitmap nowStepBmp, int? winOrLostMsg)
        {
            using (MemoryStream tempMs = new MemoryStream())
            {
                nowStepBmp.Save(tempMs, ImageFormat.Jpeg);
                using (MemoryStream ms = new MemoryStream(tempMs.ToArray()))
                {
                    GreenOnionsMessages outMsg = new GreenOnionsMessages();
                    outMsg.Add(ms);

                    if (winOrLostMsg != null)
                    {
                        switch (winOrLostMsg)
                        {
                            case -1: //bot获胜
                                outMsg.Add(TicTacToeConfig.TicTacToeBotWinReply.ReplaceGreenOnionsTags(_api.BotProperties));
                                break;
                            case 0:  //平局
                                outMsg.Add(TicTacToeConfig.TicTacToeDrawReply.ReplaceGreenOnionsTags(_api.BotProperties));
                                break;
                            case 1:  //玩家获胜
                                outMsg.Add(TicTacToeConfig.TicTacToePlayerWinReply.ReplaceGreenOnionsTags(_api.BotProperties));
                                break;
                        }
                        PlayingTicTacToeSessions.Remove(qqId);
                        PlayingTicTacToeUsers.TryRemove(qqId, out _);
                    }
                    return outMsg;
                }
            }
        }

        public void OnLoad(string pluginPath)
        {
            _pluginPath = pluginPath;
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
                    DownloadImageAsMemoryStreamAsync(_api.ReplaceGroupUrl(imgMsg.Url!)).ContinueWith(t =>
                    {
                        using (MemoryStream? playerMoveStream = t.Result)
                        {
                            if (playerMoveStream == null)
                                return;  //图片下载失败, 暂时没想好怎么处理
                            Response(PlayerMoveByBitmap(msgs.SenderId, playerMoveStream));
                        }
                    });
                    return true;
                }
            }
            if (msgs?.First() is GreenOnionsTextMessage txtMsg)
            {
                if (_regexTicTacToeStart.IsMatch(txtMsg.Text))
                {
                    LogMessage($"{msgs.SenderId}消息触发开始井字棋");
                    StartTicTacToeSession(msgs.SenderId, Response);
                    return true;
                }
                else if (_regexTicTacToeStop.IsMatch(txtMsg.Text))
                {
                    LogMessage($"{msgs.SenderId}消息触发结束井字棋");
                    StopTicTacToeSession(msgs.SenderId, Response);
                    return true;
                }
                else if ((TicTacToeConfig.TicTacToeMoveMode & (int)TicTacToeMoveMode.Nomenclature) != 0 && PlayingTicTacToeUsers.ContainsKey(msgs.SenderId) && txtMsg.Text.Length == 2)
                {
                    LogMessage($"{msgs.SenderId}消息触发井字棋移动");
                    PlayerMoveByNomenclature(txtMsg.Text, msgs.SenderId, Response);
                    return true;
                }
            }
            return false;
        }

        public void ConsoleSetting()
        {

        }

        public bool WindowSetting()
        {
            new FrmSettings().ShowDialog();
            if (_api != null)
            {
                _regexTicTacToeStart = new Regex(TicTacToeConfig.StartTicTacToeCmd.ReplaceGreenOnionsTags(_api.BotProperties)!);
                _regexTicTacToeStop = new Regex(TicTacToeConfig.StopTicTacToeCmd.ReplaceGreenOnionsTags(_api.BotProperties)!);
            }
            return true;
        }

        private void LogMessage(string text)
        {
            string logFile = Path.Combine(_pluginPath, "information.log");
            File.AppendAllText(logFile, $"{text}    ----{DateTime.Now}\r\n");
        }

        public void LogError(string text)
        {
            string logFile = Path.Combine(_pluginPath, "error.log");
            File.AppendAllText(logFile, $"{text}    ----{DateTime.Now}\r\n");
        }

        public async Task<MemoryStream?> DownloadImageAsMemoryStreamAsync(string url)
        {
            bool retry = true;
        IL_Retry:;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var t = await httpClient.GetAsync(url);
                    byte[] bytes = await t.Content.ReadAsByteArrayAsync();
                    MemoryStream ms = new MemoryStream(bytes);
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
                catch (Exception ex)
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
    }
}