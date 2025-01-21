using YamlDotNet.Serialization;

namespace GreenOnions.NT.PixivDownloader
{
    internal class Config
    {
        /// <summary>
        /// 是否启用下载Pixiv原图功能
        /// </summary>
        [YamlMember(Description = "启用下载Pixiv原图功能")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 是否使用代理
        /// </summary>
        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// Pixiv代理地址
        /// </summary>
        [YamlMember(Description = "Pixiv图片代理地址")]
        public string PixivProxy { get; set; } = "pixiv.re";

        /// <summary>
        /// 开始下载原图回复语
        /// </summary>
        [YamlMember(Description = "开始下载回复语")]
        public string DownloadingReply { get; set; } = "正在下载，请稍候...";

        /// <summary>
        /// 下载失败回复语
        /// </summary>
        [YamlMember(Description = "下载失败回复语")]
        public string DownloadFailReply { get; set; } = "图片下载失败 <错误信息>";

        /// <summary>
        /// 下载原图命令
        /// </summary>
        [YamlMember(Description = "下载原图命令")]
        public string Command { get; set; } = "<BotName>下[載载][Pp]([Ii][Xx][Ii][Vv]|站)原[圖图][:：](?<pid>.+)";
    }
}
