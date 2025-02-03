using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.NT.OpenAiClient.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        system,
        developer,
        user,
        assistant,
        tool,
    }
}
