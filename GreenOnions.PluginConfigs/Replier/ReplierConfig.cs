namespace GreenOnions.PluginConfigs.Replier
{
    public struct ReplierConfig
    {
        /// <summary>
        /// 触发消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 匹配模式
        /// </summary>
        public MatchModes MatchMode { get; set; }
        /// <summary>
        /// 触发模式
        /// </summary>
        public TriggerModes TriggerMode { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyValue { get; set; }
        /// <summary>
        /// 以"回复"方式发送(仅限群)
        /// </summary>
        public bool ReplyMode { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
    }

    public enum MatchModes
    {
        完全 = 0,
        包含 = 1,
        前缀 = 2,
        后缀 = 3,
        正则表达式 = 4,
    }

    public enum TriggerModes : int
    {
        私聊 = 1,
        群组 = 2,
    }
}
