namespace GreenOnions.NT.MessageNotifyApi
{
    public class RequestModel
    {
        public uint Target { get; set; }
        public string? Message { get; set; }
        public byte[]? Image { get; set; }
    }
}
