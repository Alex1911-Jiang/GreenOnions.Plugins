namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    internal class AnnounceSetting
    {
        /// <summary>
        /// 是否指定群组
        /// </summary>
        public bool DesignateGroup { get; set; }
        /// <summary>
        /// 指定的群
        /// </summary>
        public List<long> DesignatedGroups { get; set; } = new List<long>();
        /// <summary>
        /// 是否指定舰娘
        /// </summary>
        public bool DesignateKanGirl { get; set; }
        /// <summary>
        /// 指定的舰娘名字
        /// </summary>
        public string? DesignatedKanGirl { get; set; }
        /// <summary>
        /// 指定的报时时间
        /// </summary>
        public List<int> DesignatedTime { get; set; } = new List<int>();
        /// <summary>
        /// 是否发送日文文字
        /// </summary>
        public bool SendJapaneseText { get; set; }
        /// <summary>
        /// 是否发送中文文字
        /// </summary>
        public bool SendChineseText { get; set; }
    }
}
