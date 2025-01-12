using GreenOnions.NT.HPictures.Enums;
using YamlDotNet.Serialization;

namespace GreenOnions.NT.HPictures
{
    internal class Config
    {
        /// <summary>
        /// 色图完整命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "是否启用色图插件")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 色图完整命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "色图命令(正则表达式)")]
        public string Command { get; set; } = "(?<前缀><机器人名称>[我再]?[要来來发發给給])(?<数量>[0-9零一壹二两贰兩三叁四肆五伍六陆陸七柒八捌九玖十拾百佰千仟万萬亿億]+)?(?<单位>[张張个個幅份])(?<r18>[Rr]-?18的?)?(?<关键词>.+?)?(?<后缀>[的得地滴の]?[色瑟涩铯啬渋][图圖図])";

        /// <summary>
        /// 使用代理
        /// </summary>
        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// Pixiv代理地址
        /// </summary>
        [YamlMember(Description = "Pixiv代理地址")]
        public string PrixvProxy { get; set; } = "i.pixiv.re";

        /// <summary>
        /// 启用的色图图库
        /// </summary>
        [YamlMember(Description = "启用的图库（支持Lolicon,Yande_re,Lolisuki,Yuban10703,Konachan_net）")]
        public HashSet<HPictureSource> EnabledSource { get; set; } = new HashSet<HPictureSource>() { HPictureSource.Lolicon };

        /// <summary>
        /// 使用浏览器请求Lolicon Api
        /// </summary>
        [YamlMember(Description = "使用浏览器请求接口（解决 Windows Server 2012 R2 没有钥匙串导致无法建立安全连接的问题）")]
        public bool RequestApiByChromium { get; set; } = false;

        /// <summary>
        /// 反和谐
        /// </summary>
        [YamlMember(Description = "反和谐")]
        public bool AntiShielding { get; set; } = false;

        /// <summary>
        /// 自定义色图命令
        /// </summary>
        [YamlMember(Description = "自定义色图命令")]
        public HashSet<string> UserCmd { get; set; } = new HashSet<string>() { "--setu" };

        /// <summary>
        /// 屏蔽关键词
        /// </summary>
        [YamlMember(Description = "屏蔽关键词")]
        public HashSet<string> ShieldingWords { get; set; } = new HashSet<string>();

        /// <summary>
        /// 是否启用R-18
        /// </summary>
        [YamlMember(Description = "允许R-18")]
        public bool AllowR18 { get; set; } = true;

        /// <summary>
        /// 白名单群
        /// </summary>
        [YamlMember(Description = "白名单群")]
        public HashSet<uint> WhiteGroup { get; set; } = new HashSet<uint>();

        /// <summary>
        /// 是否仅限白名单使用色图
        /// </summary>
        [YamlMember(Description = "仅限白名单群使用色图")]
        public bool WhiteGroupOnly { get; set; } = false;

        /// <summary>
        /// 是否仅限白名单使用R-18
        /// </summary>
        [YamlMember(Description = "仅限白名单群使用R-18")]
        public bool R18WhiteGroupOnly { get; set; } = true;

        /// <summary>
        /// 冷却时间
        /// </summary>
        [YamlMember(Description = "群冷却时间（非白名单群，单位：秒）")]
        public int GroupCoolDownSecond { get; set; } = 0;

        /// <summary>
        /// 次数限制
        /// </summary>
        [YamlMember(Description = "群次数限制（非白名单群）")]
        public int Limit { get; set; } = 0;

        /// <summary>
        /// 撤回时间
        /// </summary>
        [YamlMember(Description = "群撤回时间（非白名单群），单位：秒")]
        public int GroupRecallSecond { get; set; } = 0;

        /// <summary>
        /// 白名单群冷却时间
        /// </summary>
        [YamlMember(Description = "白名单群冷却时间，单位：秒")]
        public int WhiteGroupCoolDownSecond { get; set; } = 0;

        /// <summary>
        /// 白名单群撤回时间
        /// </summary>
        [YamlMember(Description = "白名单群撤回时间，单位：秒")]
        public int WhiteGroupRecallSecond { get; set; } = 0;

        /// <summary>
        /// 允许私聊
        /// </summary>
        [YamlMember(Description = "允许私聊使用色图命令")]
        public bool AllowPrivateMessage { get; set; } = true;

        /// <summary>
        /// 私聊冷却时间
        /// </summary>
        [YamlMember(Description = "私聊冷却时间，单位：秒")]
        public int PrivateMessageCoolDownSecond { get; set; } = 0;

        /// <summary>
        /// 私聊撤回时间
        /// </summary>
        [YamlMember(Description = "私聊撤回时间，单位：秒")]
        public int PrivateMessageRecallSecond { get; set; } = 0;

        /// <summary>
        /// 机器人管理员无冷却时间/次数限制
        /// </summary>
        [YamlMember(Description = "机器人管理员无限制")]
        public bool AdminNoLimit { get; set; } = true;

        /// <summary>
        /// 私聊无冷却时间/次数限制
        /// </summary>
        [YamlMember(Description = "私聊无限制")]
        public bool PrivateMessageNoLimit { get; set; } = false;

        /// <summary>
        /// 白名单群无冷却时间/次数限制
        /// </summary>
        [YamlMember(Description = "白名单群无限制")]
        public bool WhiteGroupNoLimit { get; set; } = true;

        /// <summary>
        /// 色图是否发送作品地址
        /// </summary>
        [YamlMember(Description = "发送作品地址")]
        public bool SendUrl { get; set; } = true;

        /// <summary>
        /// 色图是否发送图片代理地址
        /// </summary>
        [YamlMember(Description = "发送图片代理地址")]
        public bool SendProxyUrl { get; set; } = false;

        /// <summary>
        /// 色图是否发送标题和作者
        /// </summary>
        [YamlMember(Description = "发送标题和作者")]
        public bool SendTitle { get; set; } = true;

        /// <summary>
        /// 色图是否发送标签
        /// </summary>
        [YamlMember(Description = "发送标签")]
        public bool SendTags { get; set; } = false;

        /// <summary>
        /// 开始下载色图时回复
        /// </summary>
        [YamlMember(Description = "开始下载色图回复语")]
        public string DownloadingReply { get; set; } = "正在查找色图...";

        /// <summary>
        /// 冷却中回复
        /// </summary>
        [YamlMember(Description = "冷却时间内回复语")]
        public string CoolDownUnreadyReply { get; set; } = "乖，要懂得节制哦，休息一会再冲吧 →_→";

        /// <summary>
        /// 次数用尽回复
        /// </summary>
        [YamlMember(Description = "超过次数回复语")]
        public string OutOfLimitReply { get; set; } = "今天不要再冲了啦(>_<)";

        /// <summary>
        /// 发生错误回复
        /// </summary>
        [YamlMember(Description = "发生错误回复语")]
        public string ErrorReply { get; set; } = "色图服务器爆炸惹_(:3」∠)_ <错误信息>";

        /// <summary>
        /// 没有结果回复
        /// </summary>
        [YamlMember(Description = "没有结果回复语")]
        public string NoResultReply { get; set; } = "没有找到符合条件的图ㄟ( ▔, ▔ )ㄏ";

        /// <summary>
        /// 下载失败回复
        /// </summary>
        [YamlMember(Description = "图片下载失败回复语")]
        public string DownloadFailReply { get; set; } = "图片下载失败o(╥﹏╥)o  <错误信息>";

        /// <summary>
        /// 色图次数限制记录类型
        /// </summary>
        [YamlMember(Description = "次数限制记录类型")]
        public LimitType LimitType { get; set; } = LimitType.Frequency;

        /// <summary>
        /// 一条色图命令最多允许返回多少张色图
        /// </summary>
        [YamlMember(Description = "单次请求最大图片数量(支持1-20, 不建议超过10, 容易无法撤回)")]
        public int OnceMessageMaxImageCount { get; set; } = 10;

    }
}
