namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    /// <summary>
    /// 表示一个时间的语音和文字
    /// </summary>
    internal class KanGirlVoiceItem
    {
        /// <summary>
        /// 语音Url
        /// </summary>
        public string Mp3Url { get; }
        /// <summary>
        /// 日文文本
        /// </summary>
        public string? JapaneseText { get; }
        /// <summary>
        /// 中文文本
        /// </summary>
        public string? ChineseText { get; }

        internal KanGirlVoiceItem(string mp3Url, string japanText, string chineseText)
        {
            Mp3Url = mp3Url;
            JapaneseText = japanText;
            ChineseText = chineseText;
        }
    }
}
