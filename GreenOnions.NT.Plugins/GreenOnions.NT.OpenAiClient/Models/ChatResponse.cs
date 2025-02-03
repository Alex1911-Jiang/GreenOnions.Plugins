namespace GreenOnions.NT.OpenAiClient.Models
{
    internal class ChatResponse
    {
        public ChatError? error { get; set; }
        public Choices[]? choices { get; set; }
    }
}
