namespace GreenOnions.GuessTheSong
{
    public class Config
    {
        public string? FFmpegPath { get; set; }
        public string StartCmd { get; set; } = "<机器人名称>猜歌";
        public string HelpMessage { get; set; } = "输入 \"<机器人名称>猜歌\" 发起一场听歌猜曲名游戏，每场游戏时长为一分钟，任意群友猜出正确曲名或超时后结束。";
        public int ClipLengthSecond { get; set; } = 5;
        public Dictionary<long, string[]> MusicIDAndAnswers { get; set; } = new Dictionary<long, string[]>();
        public HashSet<string> SearchKeywords { get; set; } = new HashSet<string>();
        public string RightAnswerReply { get; set; } = "恭喜你，答对了";
        public string TimeOutReplyReply { get; set; } = "真遗憾，没有人猜出正确曲名，答案为：\"<歌曲名称>\"，下次加油哦！";
    }
}
