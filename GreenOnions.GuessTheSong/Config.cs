namespace GreenOnions.GuessTheSong
{
    public class Config
    {
        /// <summary>
        /// FFmpeg程序路径，填写后会使用FFmpeg将MP3转换为AMR格式，不填写时以MP3原样发送（MP3格式PC可以听但手机听不了）
        /// </summary>
        public string? FFmpegPath { get; set; }
        /// <summary>
        /// 按文件方式发送语音
        /// </summary>
        public bool SendAsFile { get; set; } = false;
        /// <summary>
        /// 启动猜歌游戏的命令
        /// </summary>
        public string StartCmd { get; set; } = "<机器人名称>猜歌";
        /// <summary>
        /// 帮助信息
        /// </summary>
        public string HelpMessage { get; set; } = "输入 \"<机器人名称>猜歌\" 发起一场听歌猜曲名游戏，每场游戏时长为一分钟，任意群友猜出正确曲名或超时后结束。";
        /// <summary>
        /// 裁剪歌曲片段长度（单位：秒）
        /// </summary>
        public int ClipLengthSecond { get; set; } = 5;
        /// <summary>
        /// 自定义歌单歌曲ID和对应的答案列表
        /// </summary>
        public Dictionary<long, string[]> MusicIDAndAnswers { get; set; } = new Dictionary<long, string[]>();
        /// <summary>
        /// 搜索模式的搜索关键词
        /// </summary>
        public HashSet<string> SearchKeywords { get; set; } = new HashSet<string>();
        /// <summary>
        /// 最大搜索范围（再前多少首歌中随机，要确保您的搜索词能搜索到至少这么多首歌）
        /// </summary>
        public int MaximumSearchRange { get; set; } = 300;
        /// <summary>
        /// 答对并结束游戏的回复语
        /// </summary>
        public string RightAnswerReply { get; set; } = "恭喜你，答对了";
        /// <summary>
        /// 超时结束游戏的回复语
        /// </summary>
        public string TimeOutReplyReply { get; set; } = "真遗憾，没有人猜出正确曲名，答案为：\"<歌曲名称>\"，下次加油哦！";
    }
}
