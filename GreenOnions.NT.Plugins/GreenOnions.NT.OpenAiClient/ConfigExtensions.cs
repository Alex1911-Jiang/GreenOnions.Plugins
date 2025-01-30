using GreenOnions.NT.Base;

namespace GreenOnions.NT.OpenAiClient
{
    internal static class ConfigExtensions
    {
        public static string ReplaceConfigTags(this string text, ChatConfig config, Exception? ex = null)
        {
            foreach (var prop in config.GetType().GetProperties())
            {
                string tag = $"<{prop.Name}>";
                if (!text.Contains(tag))
                    continue;
                object? val = prop.GetValue(config);
                if (val is null)
                    continue;
                text = text.Replace($"<{prop.Name}>", val.ToString());
            }
            text = text.ReplaceTags();
            if (ex is null)
                return text;
            return text.Replace("<错误信息>", ex.Message);
        }
    }
}
