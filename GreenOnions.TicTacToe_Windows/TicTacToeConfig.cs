using GreenOnions.Interface;

namespace GreenOnions.TicTacToe_Windows
{
    public static class TicTacToeConfig
    {
        /// <summary>
        /// 开启一局新的井字棋对局命令
        /// </summary>
        public static string StartTicTacToeCmd
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(StartTicTacToeCmd)) ?? "<机器人名称>井字棋";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(StartTicTacToeCmd), value);
        }

        /// <summary>
        /// 开启井字棋对局成功回复语
        /// </summary>
        public static string TicTacToeStartedReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeStartedReply)) ?? "成功开启棋局，玩家为×，<机器人名称>为○，请对棋盘图片用QQ表情涂鸦或输入格号进行下子，您先下。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeStartedReply), value);
        }

        /// <summary>
        /// 已经开始后再次尝试开启对局的回复语
        /// </summary>
        public static string TicTacToeAlreadyStartReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeAlreadyStartReply)) ?? "您已经在棋局中啦，请不要重复开启棋局。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeAlreadyStartReply), value);
        }

        /// <summary>
        /// 中止一场对局命令
        /// </summary>
        public static string StopTicTacToeCmd
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(StopTicTacToeCmd)) ?? "<机器人名称>不玩啦";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(StopTicTacToeCmd), value);
        }

        /// <summary>
        /// 中止对局成功回复语
        /// </summary>
        public static string TicTacToeStoppedReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeStoppedReply)) ?? "下次再玩哦~";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeStoppedReply), value);
        }

        /// <summary>
        /// 未在对局中时尝试中止对局回复语
        /// </summary>
        public static string TicTacToeAlreadStopReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeAlreadStopReply)) ?? "您现在什么也没有和我玩耶QAQ";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeAlreadStopReply), value);
        }

        /// <summary>
        /// 对局超时回复语
        /// </summary>
        public static string TicTacToeTimeoutReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeTimeoutReply)) ?? "由于超时，已为您自动退出棋局，下次请说：\"<机器人名称>不玩啦\"来临阵脱逃哦。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeTimeoutReply), value);
        }

        /// <summary>
        /// 玩家获胜回复语
        /// </summary>
        public static string TicTacToePlayerWinReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToePlayerWinReply)) ?? "您赢了，这个<机器人名称>就是逊啦";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToePlayerWinReply), value);
        }

        /// <summary>
        /// 机器人获胜回复语
        /// </summary>
        public static string TicTacToeBotWinReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeBotWinReply)) ?? "<机器人名称>赢了，现在知道谁是老大了ho~";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeBotWinReply), value);
        }

        /// <summary>
        /// 平局回复语
        /// </summary>
        public static string TicTacToeDrawReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeDrawReply)) ?? "平局了，再来一局吧~";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeDrawReply), value);
        }

        /// <summary>
        /// 没有识别到玩家下子回复语
        /// </summary>
        public static string TicTacToeNoMoveReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeNoMoveReply)) ?? "<机器人名称>没看到您下子，请把想要下子的格子涂黑下子。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeNoMoveReply), value);
        }

        /// <summary>
        /// 玩家下子在已有棋子的格子上回复语
        /// </summary>
        public static string TicTacToeMoveFailReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeMoveFailReply)) ?? "您不能在已经有棋子的地方下子啦，重新下一次吧。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeMoveFailReply), value);
        }

        /// <summary>
        /// 玩家下子不止一格回复语
        /// </summary>
        public static string TicTacToeIllegalMoveReply
        {
            get => JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeIllegalMoveReply)) ?? "您把整个棋盘都下满了这让<机器人名称>也很难办啊，重新下一次吧。";
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeIllegalMoveReply), value);
        }

        /// <summary>
        /// 启用的下子模式
        /// </summary>
        public static int TicTacToeMoveMode
        {
            get
            {
                string strValue = JsonHelper.GetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeMoveMode));
                if (int.TryParse(strValue, out int iValue)) return iValue;
                return 6;
            }
            set => JsonHelper.SetSerializationValue(JsonHelper.JsonConfigFileName, JsonHelper.JsonNodeNameTicTacToe, nameof(TicTacToeMoveMode), value.ToString());
        }
    }
}
