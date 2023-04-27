namespace GreenOnions.ChatGLMClient
{
    public class ChatGLMClientConfig
    {
        public string Address { get; set; } = "http://127.0.0.1:17860/";
        public float MaxLength { get; set; } = 2048;
        public float TopP { get; set; } = 0.7f;
        public float Temperature { get; set; } = 1;
        public bool SendMessageByReply { get; set; } = true;
        public string[]? StartCommands { get; set; } = new[] { "<机器人名称>尬聊", "<机器人名称>ChatGLM", "<机器人名称>AI聊天" };
        public string[]? ExitCommands { get; set; } = new[] { "<机器人名称>闭嘴", "<机器人名称>住口", "<机器人名称>不聊了" };
        public string[]? ClearContextCommands { get; set; } = new[] { "<机器人名称>洗脑", "<机器人名称>我们讨论一个新的问题吧" };
        public string? ChatStartMessage { get; set; } = "您已进入ChatGLM聊天模式，如需退出请发送\"<机器人名称>闭嘴\"";
        public string? ExitChatMessage { get; set; } = "下次再聊哦";
        public string? CleanContextMessage { get; set; } = "已清除ChatGLM记忆。";
        public string? TimeOutMessage { get; set; } = "由于超时，已自动为您退出聊天模式。";
        public string? ErrorMessage { get; set; } = "发生错误，请联系机器人管理员。\r\n（<错误信息>）";
        public string? AnotherUsingMessage { get; set; } = "当前AI模型为独占模式，其他用户正在使用中，请稍后再聊。";
        public uint TimeOutSecond { get; set; } = 120;
    }
}
