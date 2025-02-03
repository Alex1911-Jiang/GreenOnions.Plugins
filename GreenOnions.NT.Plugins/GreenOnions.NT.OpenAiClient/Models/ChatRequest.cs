namespace GreenOnions.NT.OpenAiClient.Models
{
    internal class ChatRequest
    {
        public string model { get; set; }
        public IEnumerable<ChatMessage> messages { get; set; }
        public float temperature { get; set; } = 1.0f;
        public bool stream { get; set; } = false;
    }
}
