using GreenOnions.NT.PictureSearcher.Enums;
using YamlDotNet.Serialization;

namespace GreenOnions.NT.PictureSearcher
{
    internal class Config
    {
        /// <summary>
        /// 是否启用搜图功能
        /// </summary>
        [YamlMember(Description = "启用搜图功能")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 是否使用代理
        /// </summary>
        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// 启用的搜图引擎
        /// </summary>
        [YamlMember(Description = "启用的搜图引擎（同时决定搜索顺序，支持 SauceNAO、IqdbAnime、Iqdb3d、TraceMoe、Ascii2d、AnimeTrace、Soutubot）")]
        public SearcherSources[] EnabledSources { get; set; } = [SearcherSources.SauceNAO, SearcherSources.IqdbAnime, SearcherSources.Soutubot, SearcherSources.Ascii2d];

        /// <summary>
        /// AnimeTrace搜图实使用的模型
        /// </summary>
        [YamlMember(Description = "AnimeTrace搜图实使用的模型（目前支持 anime_model_lovelive、pre_stable、anime、game、game_model_kirakira）如果你希望在一次消息中使用不同模型搜索多次，需要同时在enabledSources中添加多个AnimeTrace")]
        public string[] EnabledAnimeTraceModels { get; set; } = ["anime", "game"];

        /// <summary>
        /// 中断搜索相似度阈值
        /// </summary>
        [YamlMember(Description = "中断搜索相似度阈值（当相似度达到此设定值时不再使用下一个搜索引擎搜索，另：Ascii2d没有相似度，总是会使用下一个引擎，建议放到最后）")]
        public int BreakSimilarity { get; set; } = 87;

        /// <summary>
        /// 发送缩略图相似度阈值
        /// </summary>
        [YamlMember(Description = "发送缩略图相似度阈值（当相似度达到此设定值时才会发送缩略图，否则发送<低于发送缩略图相似度阈值回复语>）")]
        public int SendThuImgSimilarity { get; set; } = 60;

        /// <summary>
        /// 低于发送缩略图相似度阈值回复语
        /// </summary>
        [YamlMember(Description = "低于发送缩略图相似度阈值回复语")]
        public string LowSimilarityReply { get; set; } = "相似度低于<SendThuImgSimilarity>%，缩略图不予显示。";

        #region -- SauceNAO --

        /// <summary>
        /// SauceNAO是否发送缩略图
        /// </summary>
        [YamlMember(Description = "SauceNAO发送缩略图")]
        public bool SauceNAOSendThuImg { get; set; } = true;

        /// <summary>
        /// 使用浏览器访问SauceNAO搜索
        /// </summary>
        [YamlMember(Description = "使用浏览器访问SauceNAO搜索（如果遭遇403请启用）")]
        public bool SauceNAOUseChromium { get; set; } = false;

        /// <summary>
        /// SauceNAO Api-Key
        /// </summary>
        [YamlMember(Description = "SauceNAO Api-Key")]
        public HashSet<string> SauceNAOApiKey { get; set; } = new HashSet<string>();

        #endregion -- SauceNAO --

        #region -- Ascii2d --

        /// <summary>
        /// Ascii2d是否发送缩略图
        /// </summary>
        [YamlMember(Description = "发送缩略图")]
        public bool Ascii2dSendThuImg { get; set; } = true;

        /// <summary>
        /// 使用浏览器访问Ascii2d搜索
        /// </summary>
        [YamlMember(Description = "使用浏览器访问Ascii2d搜索（如果遭遇人机验证403请启用）")]
        public bool Ascii2dUseChromium { get; set; } = false;

        /// <summary>
        /// Ascii2d显示结果数量
        /// </summary>
        [YamlMember(Description = "Ascii2d显示结果数量")]
        public int Ascii2dResultNum { get; set; } = 1;

        #endregion -- Ascii2d --

        #region -- Iqdb --

        /// <summary>
        /// Iqdb anime是否发送缩略图
        /// </summary>
        [YamlMember(Description = "是否发送Iqdb搜索结果缩略图")]
        public bool IqdbAnimeSendThuImg { get; set; } = true;

        /// <summary>
        /// Iqdb 3d是否发送缩略图
        /// </summary>
        [YamlMember(Description = "是否发送3d Iqdb搜索结果缩略图")]
        public bool Iqdb3dSendThuImg { get; set; } = true;

        /// <summary>
        /// Iqdb 是否发送标签
        /// </summary>
        [YamlMember(Description = "Iqdb是否发送标签")]
        public bool IqdbSendTags { get; set; } = false;

        /// <summary>
        /// Iqdb 只发送分级为安全的缩略图
        /// </summary>
        [YamlMember(Description = "是否只发送Iqdb搜索结果分级为安全的缩略图")]
        public bool IqdbSendThuImgMustSafe { get; set; } = true;

        #endregion -- Iqdb --

        #region -- TraceMoe --

        /// <summary>
        /// TraceMoe是否发送缩略图
        /// </summary>
        [YamlMember(Description = "是否发送TraceMoe搜索结果的缩略图")]
        public bool TraceMoeSendThuImg { get; set; } = true;

        /// <summary>
        /// TraceMoe搜番结果为里番时是否发送缩略图
        /// </summary>
        [YamlMember(Description = "TraceMoe搜图结果为里番时是否发送缩略图")]
        public bool TraceMoeSendAdultThuImg { get; set; } = false;

        #endregion -- TraceMoe --

        /// <summary>
        /// 开启连续搜图命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "开启连续搜图模式命令")]
        public string SearchModeOnCmd { get; set; } = "<BotName>搜[图圖図]";

        /// <summary>
        /// 开启连续搜图功能返回消息
        /// </summary>
        [YamlMember(Description = "进入连续搜图模式回复语")]
        public string SearchModeOnReply { get; set; } = "了解～请发送图片吧！支持批量噢！\r\n如想退出搜索模式请发送“谢谢<BotName>”";

        /// <summary>
        /// 已在连续搜图模式下返回消息
        /// </summary>
        [YamlMember(Description = "已进入连续搜图模式回复语")]
        public string SearchModeAlreadyOnReply { get; set; } = "您已经在搜图模式下啦！\r\n如想退出搜索模式请发送“谢谢<BotName>”";

        /// <summary>
        /// 发起搜索时的回复语(正在搜索但未搜索完毕)
        /// </summary>
        [YamlMember(Description = "正在搜索回复语")]
        public string? SearchingReply { get; set; } = string.Empty;

        /// <summary>
        /// 关闭连续搜图命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "退出连续搜图模式命令")]
        public string SearchModeOffCmd { get; set; } = "[谢謝][谢謝]<BotName>";

        /// <summary>
        /// 连续搜图超时返回消息
        /// </summary>
        [YamlMember(Description = "连续搜图模式超时回复语")]
        public string SearchModeTimeOutReply { get; set; } = "由于超时，已为您自动退出搜图模式，以后要记得说“谢谢<BotName>”来退出搜图模式噢";

        /// <summary>
        /// 退出连续搜图功能返回消息
        /// </summary>
        [YamlMember(Description = "退出连续搜图模式回复语")]
        public string SearchModeOffReply { get; set; } = "不用谢!(<ゝω・)☆";

        /// <summary>
        /// 已经退出连续搜图功能返回消息
        /// </summary>
        [YamlMember(Description = "已退出连续搜图模式回复语")]
        public string SearchModeAlreadyOffReply { get; set; } = "虽然不知道为什么谢我, 但是还请注意补充营养呢(｀・ω・´)";

        /// <summary>
        /// 没有搜索到结果返回消息
        /// </summary>
        [YamlMember(Description = "没有搜索到地址回复语")]
        public string SearchNoResultReply { get; set; } = "<搜索类型>没有搜到该图片的地址o(╥﹏╥)o";

        /// <summary>
        /// 搜索过程中发生异常返回消息
        /// </summary>
        [YamlMember(Description = "搜索错误回复语")]
        public string SearchErrorReply { get; set; } = "<搜索类型>搜索失败_(:3」∠)_ <错误信息>";

        /// <summary>
        /// 下载缩略图失败时追加回复
        /// </summary>
        [YamlMember(Description = "下载缩略图失败时追加回复")]
        public string DownloadThuImgFailReply { get; set; } = "缩略图下载失败。";

    }
}
