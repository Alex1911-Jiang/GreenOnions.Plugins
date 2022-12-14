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
        public string Mp3UrlOrFileName { get; }
        /// <summary>
        /// 日文文本
        /// </summary>
        public string? JapaneseText { get; }
        /// <summary>
        /// 中文文本
        /// </summary>
        public string? ChineseText { get; }

        internal KanGirlVoiceItem(string mp3UrlOrFileName, string chineseText, string japanText)
        {
            Mp3UrlOrFileName = mp3UrlOrFileName;
            JapaneseText = japanText;
            ChineseText = chineseText;
        }
    }
}
