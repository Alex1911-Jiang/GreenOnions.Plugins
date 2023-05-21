using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.ChatGPTClient
{
    public class ChatGPTClientConfig
    {
        public string? APIkey { get; set; }
        public string? Model { get; set; } = "gpt-3.5-turbo";
        public float Temperature { get; set; } = 1;
        public bool UseProxy { get; set; } = false;
        public bool RequestByPlugin { get; set; } = false;
        public bool SendMessageByReply { get; set; } = true;
        public bool ClearContextAfterExit { get; set; } = true;
        public bool RemoveMarkdownExpression { get; set; } = true;
        [JsonConverter(typeof(StringEnumConverter))]
        public ContextSetting Context { get; set; } = ContextSetting.仅保持上一条上下文;
        public string[]? StartCommands { get; set; } = new[] { "<机器人名称>尬聊", "<机器人名称>ChatGPT", "<机器人名称>AI聊天" };
        public string[]? ExitCommands { get; set; } = new[] { "<机器人名称>闭嘴", "<机器人名称>住口", "<机器人名称>不聊了" };
        public string[]? ClearContextCommands { get; set; } = new[] { "<机器人名称>洗脑", "<机器人名称>我们讨论一个新的问题吧" };
        public string? ChatStartMessage { get; set; } = "您已进入ChatGPT聊天模式，如需退出请发送\"<机器人名称>闭嘴\"";
        public string? ExitChatMessage { get; set; } = "下次再聊哦";
        public string? CleanContextMessage { get; set; } = "已清除ChatGPT记忆。";
        public string? TimeOutMessage { get; set; } = "由于超时，已自动为您退出聊天模式。";
        public string? ErrorMessage { get; set; } = "发生错误，请联系机器人管理员。\r\n（<错误信息>）";
        public uint TimeOutSecond { get; set; } = 120;
    }
    public enum ContextSetting
    {
        尽量保持上下文,
        仅保持上一条上下文,
        不保持上下文,
    }
}
