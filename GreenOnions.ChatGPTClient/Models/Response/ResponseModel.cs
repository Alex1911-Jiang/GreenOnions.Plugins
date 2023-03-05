namespace GreenOnions.ChatGPTClient.Models.Response
{
    public class ResponseModel
    {
        public Error? error { get; set; }
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
    }
}
