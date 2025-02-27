using YamlDotNet.Serialization;

namespace GreenOnions.NT.MessageNotifyApi
{
    internal class Config
    {
        [YamlMember(Description = "监听地址，只允许本机就填'127.0.0.1'，允许公网访问就填'*'")]
        public string ListenIp { get; set; } = "*";

        [YamlMember(Description = "监听端口")]
        public int ListenPort { get; set; } = 1919;

        [YamlMember(Description = "是否只允许发送给管理员")]
        public bool AdminOnly { get; set; } = true;
    }
}
