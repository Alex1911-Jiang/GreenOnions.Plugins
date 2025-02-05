using YamlDotNet.Serialization;

namespace GreenOnions.NT.TicTacToe
{
    public class Config
    {
        /// <summary>
        /// 启用井字棋
        /// </summary>
        [YamlMember(Description = "是否启用井字棋功能")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 开启一局新的井字棋对局命令
        /// </summary>
        [YamlMember(Description = "开启一局新的井字棋对局命令")]
        public string StartCommand { get; set; } = "<BotName>井字棋";

        /// <summary>
        /// 开启井字棋对局成功回复语
        /// </summary>
        [YamlMember(Description = "开启井字棋对局成功回复语")]
        public string StartedReply { get; set; } = "成功开启棋局，玩家为×，<BotName>为○，请对棋盘图片用QQ表情涂鸦下子，您先下。";

        /// <summary>
        /// 已经开始后再次尝试开启对局的回复语
        /// </summary>
        [YamlMember(Description = "已经开始后再次尝试开启对局的回复语")]
        public string AlreadyStartReply { get; set; } = "您已经在棋局中啦，请不要重复开启棋局。";

        /// <summary>
        /// 退出一场对局命令
        /// </summary>
        [YamlMember(Description = "退出一场对局命令")]
        public string ExitCommand { get; set; } = "<BotName>不玩[了瞭啦力]";

        /// <summary>
        /// 退出对局成功回复语
        /// </summary>
        [YamlMember(Description = "退出对局成功回复语")]
        public string ExitedReply { get; set; } = "下次再玩哦~";

        /// <summary>
        /// 未在对局中时尝试退出对局回复语
        /// </summary>
        [YamlMember(Description = "未在对局中时尝试退出对局回复语")]
        public string AlreadExitReply { get; set; } = "您现在什么也没有和我玩耶QAQ";

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        [YamlMember(Description = "超时时间（单位：秒）")]
        public int TimeoutSecond { get; set; } = 120;

        /// <summary>
        /// 对局超时回复语
        /// </summary>
        [YamlMember(Description = "对局超时回复语")]
        public string TimeoutReply { get; set; } = "由于超时，已为您自动退出棋局，下次请说：\"<BotName>不玩啦\"来临阵脱逃哦。🙄";

        /// <summary>
        /// 玩家获胜回复语
        /// </summary>
        [YamlMember(Description = "对局超时回复语")]
        public string PlayerWonReply { get; set; } = "您赢了，这个<BotName>就是逊啦";

        /// <summary>
        /// 机器人获胜回复语
        /// </summary>
        [YamlMember(Description = "机器人获胜回复语")]
        public string BotWonReply { get; set; } = "<BotName>赢了，现在知道谁是老大了ho~";

        /// <summary>
        /// 平局回复语
        /// </summary>
        [YamlMember(Description = "平局回复语")]
        public string ScoreDrawReply { get; set; } = "平局了，再来一局吧~";

        /// <summary>
        /// 没有识别到玩家下子回复语
        /// </summary>
        [YamlMember(Description = "没有识别到玩家下子回复语")]
        public string NoMoveReply { get; set; } = "<BotName>没看到您下子，请在您要下子的格子上画上×。";

        /// <summary>
        /// 玩家下子在已有棋子的格子上回复语
        /// </summary>
        [YamlMember(Description = "玩家下子在已有棋子的格子上回复语")]
        public string MoveFailReply { get; set; } = "您不能在已经有棋子的地方下子啦，重新下一次吧。";

        /// <summary>
        /// 玩家下子不止一格回复语
        /// </summary>
        [YamlMember(Description = "玩家下子不止一格回复语")]
        public string IllegalMoveReply { get; set; } = "您把整个棋盘都下满了这让<BotName>也很难办啊，重新下一次吧。";

        /// <summary>
        /// 下载图片失败回复语
        /// </summary>
        [YamlMember(Description = "下载图片失败回复语")]
        public string DownloadImageFailReply { get; set; } = "<BotName>下载图片失败了，请再发一次 o(╥﹏╥)o";

        /// <summary>
        /// 发生错误回复语
        /// </summary>
        [YamlMember(Description = "发生错误回复语")]
        public string ErrorReply { get; set; } = "<BotName>把图弄丢了, 这局就当您赢了吧, 请向<BotName>反馈Bug o(╥﹏╥)o";
    }
}
