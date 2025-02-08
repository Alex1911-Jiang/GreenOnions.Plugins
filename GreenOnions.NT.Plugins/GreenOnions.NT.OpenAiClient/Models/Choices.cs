namespace GreenOnions.NT.OpenAiClient.Models
{
    internal class Choices
    {
        public ChatMessage? message { get; set; }
        public ChatDelta? delta { get; set; }
    }
}
