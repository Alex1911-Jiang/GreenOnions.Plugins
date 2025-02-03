using Newtonsoft.Json;
using System.Text;

namespace GreenOnions.NT.OpenAiClient
{
    internal static class ChatHelper
    {
        public static string DomainToUrl(this string domain)
        {
            if (domain.EndsWith("chat/completions") || domain.EndsWith("chat/completions/"))
                return domain;
            if (domain.EndsWith("/"))
                return $"{domain}chat/completions";
            return $"{domain}/chat/completions";
        }

        public static StringContent ToJsonContent<T>(this T json)
        {
            return new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
        }
    }
}
