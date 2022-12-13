namespace GreenOnions.ChatGPTClient
{
    public class Config
    {
        public string? APIkey { get; set; }
        public float Temperature { get; set; }
        public string[]? StartCommands { get; set; }
        public string[]? ExitCommands { get; set; }
        public string? ChatStartMessage { get; set; }
        public string? ExitChatMessage { get; set; }
        public string? TimeOutMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public int TimeOutSecond { get; set; }
    }
}
