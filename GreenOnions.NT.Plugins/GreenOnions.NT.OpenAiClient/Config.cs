using YamlDotNet.Serialization;

namespace GreenOnions.NT.OpenAiClient
{
    internal class Config
    {
        /// <summary>
        /// 是否启用AI聊天插件
        /// </summary>
        [YamlMember(Description = "是否启用AI聊天插件")]
        public bool Enabled { get; set; } = false;

        [YamlMember(Description = "聊天配置")]
        public List<ChatConfig> ChatConfigs { get; set; } = new List<ChatConfig>
        {
            new ChatConfig(),
        };

        [YamlMember(Description = "发生错误回复")]
        public string ErrorReply { get; set; } = "发生错误，请联系机器人管理员。<错误信息>";
    }

    internal class ChatConfig
    {
        [YamlMember(Description = "地址 例：https://api.openai.com/ 注意结尾要带/")]
        public string? Domain { get; set; }

        [YamlMember(Description = "API-Key")]
        public string? ApiKey { get; set; }

        [YamlMember(Description = "模型代号")]
        public string? ModelId { get; set; }

        [YamlMember(Description = "备注，可以以<Remark>标签作为替换项放到回复内容或命令的配置中")]
        public string Remark { get; set; } = "";

        [YamlMember(Description = "采样温度，取值范围0-1，越大回复越具有创造性")]
        public float Temperature { get; set; } = 0.8f;

        [YamlMember(Description = "使用代理（机器人主配置中的地址）")]
        public bool UseProxy { get; set; } = false;

        [YamlMember(Description = "是否保持上下文")]
        public bool MaintainContext { get; set; } = true;

        [YamlMember(Description = "AI设定内容（在开始聊天时会设置到大模型的System字段中，例：你是一个猫娘。）")]
        public string? SystemChatMessage { get; set; } = null;

        [YamlMember(Description = "开始聊天命令")]
        public string StartCommand { get; set; } = "<BotName>(尬聊|[Cc][Hh][Aa][Tt]|<Remark>)";

        [YamlMember(Description = "成功开始聊天回复")]
        public string StartedChatReply { get; set; } = "您已进入<Remark> AI聊天模式，如需退出请发送\"<BotName>闭嘴\"";

        [YamlMember(Description = "结束聊天命令")]
        public string ExitCommand { get; set; } = "<BotName>([闭閉]嘴|住口|不聊了)";

        [YamlMember(Description = "成功结束聊天回复")]
        public string ExitedChatReply { get; set; } = "下次再聊哦";

        [YamlMember(Description = "清除上下文命令")]
        public string ClearContextCommand { get; set; } = "<BotName>洗[脑腦]";

        [YamlMember(Description = "成功清除上下文回复")]
        public string CleanContextReply { get; set; } = "已清除<Remark>上下文";

        [YamlMember(Description = "超时自动退出回复")]
        public string TimeOutReply { get; set; } = "由于超时，已自动为您退出聊天模式。";

        [YamlMember(Description = "超时自动退出聊天的时间，单位：秒")]
        public int TimeOutSeconds { get; set; } = 600;

        [YamlMember(Description = "移除思考过程（<think>中的内容）")]
        public bool RemoveThink { get; set; } = false;
    }
}
