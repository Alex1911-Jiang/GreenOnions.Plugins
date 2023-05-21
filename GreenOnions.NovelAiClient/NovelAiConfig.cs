using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.NovelAiClient
{
    public class NovelAiConfig
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string? Cmd { get; set; } = "<机器人名称>画[图画][:：]";
        /// <summary>
        /// 连接的前端（Naifu/WebUI/CustomWebUI）
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FrontEnd ConnectFrontEnd { get; set; } =  FrontEnd.WebUI;
        /// <summary>
        /// 前端地址
        /// </summary>
        public string? URL { get; set; } = "http://127.0.0.1:7860/";
        /// <summary>
        /// WebUI 的 fn_index 参数
        /// </summary>
        public int fn_index { get; set; } = 0;
        /// <summary>
        /// 提示词参数位于参数列表的第几个位置
        /// </summary>
        public int PromptIndex { get; set; } = 0;
        /// <summary>
        /// 屏蔽词参数位于参数列表的第几个位置
        /// </summary>
        public int UndesiredIndex { get; set; } = 1;
        /// <summary>
        /// 开始绘制提示
        /// </summary>
        public string? StartDrawMessage { get; set; } = "开始绘制";
        /// <summary>
        /// 绘制完成提示
        /// </summary>
        public string? DrawEndMessage { get; set; } = "绘制完毕，耗时：<耗时>秒";
        /// <summary>
        /// 绘制错误提示
        /// </summary>
        public string? DrawErrorMessage { get; set; } = "绘制错误(Ｔ▽Ｔ)";
        /// <summary>
        /// 撤回时间（秒）
        /// </summary>
        public int RevokeSecond { get; set; } = 30;
        /// <summary>
        /// 默认添加提示词
        /// </summary>
        public string? DefaultPrompt { get; set; } = "masterpiece, best quality,";
        /// <summary>
        /// 默认屏蔽词
        /// </summary>
        public string? DefaultUndesired { get; set; } = "lowres, bad anatomy, bad hands, text, error, missing fingers, extra digit, fewer digits, cropped, worst quality, low quality, normal quality, jpeg artifacts, signature, watermark, username, blurry";  //, nsfw, r18, r-18, nude, nipple, nipples, breast, breasts, pussy, vaginal, asshole, penis, testicle, testicles, sex,
    }

    public enum FrontEnd
    {
        WebUI,
        Naifu
    }
}
