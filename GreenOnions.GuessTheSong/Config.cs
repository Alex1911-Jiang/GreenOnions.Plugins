namespace GreenOnions.GuessTheSong
{
    public class Config
    {
        public string? FFmpegPath { get; set; }
        public int ClipLengthSecond { get; set; } = 5;
        public Dictionary<long, string[]> MusicIDAndAnswers { get; set; } = new Dictionary<long, string[]>();
        public HashSet<string> SearchKeywords { get; set; } = new HashSet<string>();
        public string RightAnswerReply { get; set; } = "恭喜你，答对了";
        public string TimeOutReplyReply { get; set; } = "真遗憾，没有人猜出正确曲名，答案为：\"<歌曲名称>\"，下次加油哦！";
    }
}
