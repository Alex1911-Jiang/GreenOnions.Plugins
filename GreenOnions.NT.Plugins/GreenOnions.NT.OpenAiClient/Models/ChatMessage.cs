namespace GreenOnions.NT.OpenAiClient.Models
{
    internal class ChatMessage(Role role, string content)
    {
        public static ChatMessage FromSystem(string content) => new ChatMessage(Role.system, content);
        public static ChatMessage FromDeveloper(string content) => new ChatMessage(Role.developer, content);
        public static ChatMessage FromUser(string content) => new ChatMessage(Role.user, content);
        public static ChatMessage FromAssistant(string content) => new ChatMessage(Role.assistant, content);
        public static ChatMessage FromTool(string content) => new ChatMessage(Role.tool, content);

        public Role role { get; } = role;
        public string content { get; } = content;
        public string? reasoning_content { get; set; }
    }
}
