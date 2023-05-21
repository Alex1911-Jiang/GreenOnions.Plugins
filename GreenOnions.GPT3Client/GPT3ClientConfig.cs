namespace GreenOnions.GPT3Client
{
    public class GPT3ClientConfig
    {
        public string? APIkey { get; set; }
        public float Temperature { get; set; } = 1;
        public bool UseProxy { get; set; } = false;
        public bool SendMessageByReply { get; set; } = true;
        public bool RequestByPlugin { get; set; } = false;
        public string[]? StartCommands { get; set; } = new[] { "<机器人名称>尬聊", "<机器人名称>AI聊天" };
        public string[]? ExitCommands { get; set; } = new[] { "<机器人名称>不聊了", "<机器人名称>住口", "<机器人名称>闭嘴" };
        public string? ChatStartMessage { get; set; } = "您已进入AI聊天模式，如需退出请发送\"<机器人名称>闭嘴\"";
        public string? ExitChatMessage { get; set; } = "下次再聊哦";
        public string? TimeOutMessage { get; set; } = "由于超时，已为您自动退出聊天模式。";
        public string? ErrorMessage { get; set; } = "发生错误，请联系机器人管理员（<错误信息>）";
        public uint TimeOutSecond { get; set; } = 120;
    }
}
