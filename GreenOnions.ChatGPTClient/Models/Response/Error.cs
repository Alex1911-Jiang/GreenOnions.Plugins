namespace GreenOnions.ChatGPTClient.Models.Response
{
    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public string? param { get; set; }
        public string? code { get; set; }
    }
}
