using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.PluginConfigEditor
{
    internal static class ConfigLoader
    {
        public static T LoadConfig<T>(string configDirect) where T : new()
        {
            T outConfig;
            string strConfigJson;
            if (!File.Exists(configDirect) || string.IsNullOrWhiteSpace(strConfigJson = File.ReadAllText(configDirect)))
            {
                MessageBox.Show($"配置文件 {configDirect} 不存在，即将重新生成");
                outConfig = new T();
                strConfigJson = JsonConvert.SerializeObject(outConfig, Formatting.Indented, new StringEnumConverter());
                File.WriteAllText(configDirect, strConfigJson);
            }
            else
            {
                T? config = JsonConvert.DeserializeObject<T>(strConfigJson);
                if (config is null)
                {
                    MessageBox.Show("配置文件读取失败，重新生成");
                    config = new T();
                }
                outConfig = config;
            }
            return outConfig;
        }
    }
}
