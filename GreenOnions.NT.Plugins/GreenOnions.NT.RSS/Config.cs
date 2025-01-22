using YamlDotNet.Serialization;

namespace GreenOnions.NT.RSS
{
    internal class Config
    {
        /// <summary>
        /// 是否启用RSS订阅转发
        /// </summary>
        [YamlMember(Description = "启用RSS订阅转发")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 是否使用代理
        /// </summary>
        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// 抓取RSS间隔时间(秒)
        /// </summary>
        [YamlMember(Description = "获取内容时间间隔（单位：秒，可以填写小数）")]
        public double ReadInterval { get; set; } = 600.0;

        /// <summary>
        /// 订阅的地址和需要转发到的QQ或群列表
        /// </summary>
        [YamlMember(Description = "RSS订阅项")]
        public HashSet<SubscriptionItem>? RssSubscription { get; set; } = new()
        {
            new SubscriptionItem
            {
                Url = "https://rsshub.app/twitter/user/cfm_miku",
                Remark = "初音ミク 公式",
            }
        };
    }
}
