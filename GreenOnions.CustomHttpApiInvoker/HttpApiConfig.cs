namespace GreenOnions.CustomHttpApiInvoker
{
    public class HttpApiConfig
    {
        public string HelpCmd { get; set; } = "<机器人名称>API";
        public List<HttpApiItemConfig> ApiConfig { get; set; } = new List<HttpApiItemConfig>();
    }
}
