using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace GreenOnions.ChatGPTClient.Models
{
    public class Message
    {
        public Message()
        {
        }
        public Message(Role _role, string _content)
        {
            role = _role;
            content = _content;
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public Role role { get; set; }
        public string content { get; set; }
    }

    public enum Role
    {
        system,
        user,
        assistant,
    }
}
