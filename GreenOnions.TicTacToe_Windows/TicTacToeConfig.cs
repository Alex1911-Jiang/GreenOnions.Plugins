namespace GreenOnions.TicTacToe_Windows
{
    public class TicTacToeConfig
    {
        /// <summary>
        /// 开启一局新的井字棋对局命令
        /// </summary>
        public string StartTicTacToeCmd { get; set; } = "<机器人名称>井字棋";

        /// <summary>
        /// 开启井字棋对局成功回复语
        /// </summary>
        public string TicTacToeStartedReply { get; set; } = "成功开启棋局，玩家为×，<机器人名称>为○，请对棋盘图片用QQ表情涂鸦或输入格号进行下子，您先下。";

        /// <summary>
        /// 已经开始后再次尝试开启对局的回复语
        /// </summary>
        public string TicTacToeAlreadyStartReply { get; set; } = "您已经在棋局中啦，请不要重复开启棋局。";

        /// <summary>
        /// 中止一场对局命令
        /// </summary>
        public string StopTicTacToeCmd { get; set; } = "<机器人名称>不玩啦";

        /// <summary>
        /// 中止对局成功回复语
        /// </summary>
        public string TicTacToeStoppedReply { get; set; } = "下次再玩哦~";

        /// <summary>
        /// 未在对局中时尝试中止对局回复语
        /// </summary>
        public string TicTacToeAlreadStopReply { get; set; } = "您现在什么也没有和我玩耶QAQ";

        /// <summary>
        /// 对局超时回复语
        /// </summary>
        public string TicTacToeTimeoutReply { get; set; } = "由于超时，已为您自动退出棋局，下次请说：\"<机器人名称>不玩啦\"来临阵脱逃哦。";

        /// <summary>
        /// 玩家获胜回复语
        /// </summary>
        public string TicTacToePlayerWinReply { get; set; } = "您赢了，这个<机器人名称>就是逊啦";

        /// <summary>
        /// 机器人获胜回复语
        /// </summary>
        public string TicTacToeBotWinReply { get; set; } = "<机器人名称>赢了，现在知道谁是老大了ho~";

        /// <summary>
        /// 平局回复语
        /// </summary>
        public string TicTacToeDrawReply { get; set; } = "平局了，再来一局吧~";

        /// <summary>
        /// 没有识别到玩家下子回复语
        /// </summary>
        public string TicTacToeNoMoveReply { get; set; } = "<机器人名称>没看到您下子，请把想要下子的格子涂黑下子。";

        /// <summary>
        /// 玩家下子在已有棋子的格子上回复语
        /// </summary>
        public string TicTacToeMoveFailReply { get; set; } = "您不能在已经有棋子的地方下子啦，重新下一次吧。";

        /// <summary>
        /// 玩家下子不止一格回复语
        /// </summary>
        public string TicTacToeIllegalMoveReply { get; set; } = "您把整个棋盘都下满了这让<机器人名称>也很难办啊，重新下一次吧。";

        /// <summary>
        /// 启用的下子模式
        /// </summary>
        public TicTacToeMoveMode TicTacToeMoveMode { get; set; } = TicTacToeMoveMode.All;
    }
}
