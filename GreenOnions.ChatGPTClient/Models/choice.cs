namespace GreenOnions.ChatGPTClient.Models
{
    public class choice
    {
        public string text { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public string finish_reason { get; set; }
    }
}
