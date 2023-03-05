namespace GreenOnions.ChatGPTClient.Models.Request
{
    public class RequestModel
    {
        public RequestModel(string _model,float _temperature)
        {
            model = _model;
            temperature = _temperature;
        }
        public string model { get; set; }
        public float temperature { get; set; }
        public List<Message> messages { get; set; } = new List<Message>();
    }
}
