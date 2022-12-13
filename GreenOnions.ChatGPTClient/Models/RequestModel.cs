namespace GreenOnions.ChatGPTClient.Models
{
    public class RequestModel
    {
        public string model { get; set; } = "text-davinci-003";
        public string prompt { get; set; }
        public int max_tokens { get; set; }
        public float temperature { get; set; }
    }
}
