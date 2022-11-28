namespace GreenOnions.CustomHttpApiInvoker
{
    public class MainConfig
    {
        public string HelpCmd { get; set; } = "<机器人名称>API";
        public Dictionary<string, HttpApiConfig> ApiConfig { get; set; } = new Dictionary<string, HttpApiConfig>();
    }
}
