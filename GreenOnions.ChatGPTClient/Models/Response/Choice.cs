namespace GreenOnions.ChatGPTClient.Models.Response
{
    public class Choice
    {
        public Message message { get; set; }
        public int index { get; set; }
        public string finish_reason { get; set; }
    }
}
