using YamlDotNet.Serialization;

namespace GreenOnions.NT.WakeOnLan
{
    public class Config
    {
        /// <summary>
        /// 目标设备网卡mac
        /// </summary>
        [YamlMember(Description = "目标设备网卡mac")]
        public string Mac { get; set; } = "";

        /// <summary>
        /// 你的设备在局域网内的ip
        /// </summary>
        [YamlMember(Description = "你的设备在局域网内的ip")]
        public string Ip { get; set; } = "192.168.1.1";
        
    }
}
