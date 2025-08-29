using YamlDotNet.Serialization;

namespace GreenOnions.NT.RSS
{
    /// <summary>
    /// 表示一个RSS订阅对象
    /// </summary>
    internal class SubscriptionItem
    {
        public static bool operator ==(SubscriptionItem left, SubscriptionItem right) => left.Url == right.Url;
        public static bool operator !=(SubscriptionItem left, SubscriptionItem right) => left.Url != right.Url;

        public override bool Equals(object? obj)
        {
            if (obj is not SubscriptionItem theOther)
                return false;
            return Url == theOther.Url;
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        /// <summary>
        /// 订阅地址
        /// </summary>
        [YamlMember(Description = "订阅地址")]
        public string Url { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [YamlMember(Description = "备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 是否使用代理订阅
        /// </summary>
        [YamlMember(Description = "使用代理订阅（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;
        /// <summary>
        /// 是否使用代理下载媒体资源
        /// </summary>
        [YamlMember(Description = "是否使用代理下载媒体资源（图片、视频等）")]
        public bool UseProxyDownloadMedia { get; set; } = false;
        /// <summary>
        /// 获取内容时间间隔(秒)
        /// </summary>
        [YamlMember(Description = "获取内容时间间隔（单位：秒，可以填写小数）")]
        public double ReadInterval { get; set; } = 600.0;
        /// <summary>
        /// 转发群组
        /// </summary>
        [YamlMember(Description = "期望转发到的群号")]
        public uint[] SendToGroups { get; set; } = [];
        /// <summary>
        /// 转发好友
        /// </summary>
        [YamlMember(Description = "期望转发到的好友QQ号")]
        public uint[] SendToFriends { get; set; } = [];
        /// <summary>
        /// 过滤模式
        /// </summary>
        [YamlMember(Description = "过滤模式 支持 Disabled = 不过滤（总是发送），SendWhenAny = 包含任一时发送，SendWhenAll = 包含所有时发送，NotSendWhenAny = 包含任一不发送，NotSendWhenAll = 包含所有时不发送")]
        public FilterModes FilterMode { get; set; } = FilterModes.Disabled;
        /// <summary>
        /// 过滤词
        /// </summary>
        [YamlMember(Description = "过滤词")]
        public string[] FilterKeyWords { get; set; } = [];
        /// <summary>
        /// 请求头
        /// </summary>
        [YamlMember(Description = "自定义请求头")]
        public Dictionary<string, string> Headers { get; set; } = new();
        /// <summary>
        /// 排版格式
        /// </summary>
        [YamlMember(Description = "排版格式，在行首添加?代表在本行中没有对应标签的内容时整行不发送")]
        public string?[] Format { get; set; } =
        [
            "<标题>更新啦：",
            "<正文>",
            "<正文翻译>",
            "?视频：<视频地址>",
            "?内容：<嵌入页面地址>",
            "<B站直播封面>",
            "<图片>",
            "<视频>",
            "作者：<作者>",
            "发布时间：<发布时间>",
            "原文地址：<原文地址>",
        ];
        /// <summary>
        /// 是否把RSS订阅源返回值作为一个流下载后再解析
        /// </summary>
        [YamlMember(Description = "把RSS订阅源返回值作为一个流下载后再解析（如果你的订阅地址在浏览器打开后看不到内容但会下载一个xml文件则需要启用此选项）")]
        public bool IsStreamSource { get; set; } = false;
    }
}
