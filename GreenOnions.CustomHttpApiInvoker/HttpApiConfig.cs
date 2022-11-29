using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace GreenOnions.CustomHttpApiInvoker
{
    public class HttpApiConfig
    {
        public string? Url { get; set; }
        public string? Cmd { get; set; }
        public string? Remark { get; set; }
        public string? HelpMessage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpMethodEnum HttpMethod { get; set; } = HttpMethodEnum.GET;
        public EncodingEnum Encoding { get; set; } = EncodingEnum.UTF8;
        public string MediaType { get; set; } = "application/text";
        public Dictionary<string, string>? Headers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ContentTypeEnum ContentType { get; set; }
        public string? RowContent { get; set; }
        public Dictionary<string, string>? FormDataContent { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ParseModeEnum ParseMode { get; set; }
        public string? ParseExpression { get; set; }
        public string? SubTextFrom { get; set; }
        public string? SubTextTo { get; set; }
        public bool SubTextWithPrefix { get; set; }
        public bool SubTextWithSuffix { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SendModeEnum SendMode { get; set; }
    }
    public enum HttpMethodEnum
    {
        GET = 0,
        POST = 1,
    }
    public enum EncodingEnum
    {
        UTF8 = 0,
        Unicode = 1,
        BigEndianUnicode = 2,
        UTF32 = 3,
        ASCII = 4,
        GBK = 5,
    }
    public enum ContentTypeEnum
    {
        raw = 0,
        form_data = 1,
    }
    public enum ParseModeEnum
    {
        Text = 0,
        Json = 1,
        Xml = 2,
        XPath = 3,
        JavaScript = 4,
        Stream = 5,
    }
    public enum SendModeEnum
    {
        Text = 0,
        ImageUrl = 1,
        ImageBase64 = 2,
        ImageStream = 3,
        VoiceUrl = 4,
        VoiceBase64 = 5,
        VoiceStream = 6,
    }
}