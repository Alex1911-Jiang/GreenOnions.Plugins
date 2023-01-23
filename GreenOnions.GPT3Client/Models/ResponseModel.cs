namespace GreenOnions.GPT3Client.Models
{
    public class ResponseModel
    {
        public error? error { get; set; }
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public choice[] choices { get; set; }
        public usage usage { get; set; }
    }
}
