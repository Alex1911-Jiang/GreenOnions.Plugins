namespace GreenOnions.PluginConfigs.CustomHttpApiInvoker
{
    public class MainConfig
    {
        public string HelpCmd { get; set; } = "<机器人名称>API";
        public List<HttpApiConfig> ApiConfig { get; set; } = new List<HttpApiConfig>();
    }
}
