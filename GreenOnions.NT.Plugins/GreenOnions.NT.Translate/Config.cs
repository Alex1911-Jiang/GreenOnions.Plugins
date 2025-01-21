using YamlDotNet.Serialization;

namespace GreenOnions.NT.Translate
{
    internal class Config
    {
        /// <summary>
        /// 是否启用翻译功能
        /// </summary>
        [YamlMember(Description = "启用翻译功能（不启用也能为其他插件提供翻译服务）")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 是否使用代理
        /// </summary>
        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// 翻译引擎
        /// </summary>
        [YamlMember(Description = "翻译引擎：支持 YouDaoApi = 有道, BaiduApi = 百度, TencentApi = 腾讯云, AliyunApi = 阿里云, GoogleApi = 谷歌")]
        public TranslateEngines Engine { get; set; } = TranslateEngines.GoogleApi;

        /// <summary>
        /// 云翻译接口的APP ID
        /// </summary>
        [YamlMember(Description = "翻译接口APPID")]
        public string? AppId { get; set; } = string.Empty;

        /// <summary>
        /// 云翻译接口的密钥
        /// </summary>
        [YamlMember(Description = "翻译接口密钥")]
        public string? AppKey { get; set; } = string.Empty;

        /// <summary>
        /// 翻译为中文命令
        /// </summary>
        [YamlMember(Description = "翻译为中文命令")]
        public string TranslateToChineseCommand { get; set; } = "<BotName>翻[译譯][：:](?<Value>.+)";

        /// <summary>
        /// 翻译为指定语言命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "翻译为指定语言命令")]
        public string TranslateToLanguageCommand { get; set; } = "<BotName>翻[译譯][成为為到至](?<ToLanguage>.+[语語文])[:：](?<Value>.+)";

        /// <summary>
        /// 从指定语言翻译为指定语言命令(正则表达式)
        /// </summary>
        [YamlMember(Description = "翻译为指定语言命令")]
        public string TranslateFromLanguageToLanguageCommand { get; set; } = "<BotName>[把从從自从](?<FromLanguage>.+[语語文])翻[译譯][成为為到至](?<ToLanguage>.+[语語文])[:：](?<Value>.+)";

        /// <summary>
        /// 翻译失败回复语
        /// </summary>
        [YamlMember(Description = "翻译失败回复语")]
        public string TranslateFailReply { get; set; } = "翻译失败 <错误信息>";
    }
}
