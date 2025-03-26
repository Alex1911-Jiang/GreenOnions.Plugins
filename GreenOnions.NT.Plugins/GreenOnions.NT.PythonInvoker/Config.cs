using YamlDotNet.Serialization;

namespace GreenOnions.NT.PythonInvoker
{
    public class Config
    {
        [YamlMember(Description = "是否启用Python调用器")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 命令配置
        /// </summary>
        [YamlMember(Description = "命令配置")]
        public List<ConfigItem> Inovkers { get; set; } = new()
        {
            new ()
            {
                Command = "<BotName>jm (?<参数>.+)",
                Script = @"import jmcomic
option = jmcomic.create_option_by_file('jm.yml')
jmcomic.download_album(<参数>, option)",
                ReadFileMode = ReadFileModes.Raw,
                ReadFileName = "<参数>.pdf",
                Remark = "禁漫天堂",
                InvokingReply = "开始下载<参数>",
                InvokedReply = "<参数>下载完成",
                ErrorReply = "下载失败。<错误信息>"
            }
};
    }

    public class ConfigItem
    {
        [YamlMember(Description = "发起调用命令")]
        public string Command { get; set; }

        [YamlMember(Description = "执行的Python脚本内容")]
        public string Script { get; set; }

        [YamlMember(Description = "备注信息（只用于自己记忆）")]
        public string Remark { get; set; }

        [YamlMember(Description = "执行完成后读取文件并发送时将文件视为：None=不读取文件（同时忽略ReadFileName参数），Text=文本，Image=图片，Video=视频，Aduio=音频，Raw=不解析文件直接上传")]
        public ReadFileModes ReadFileMode { get; set; }

        [YamlMember(Description = "执行完成后读取文件路径")]
        public string ReadFileName { get; set; }

        [YamlMember(Description = "开始调用时的回复语")]
        public string InvokingReply { get; set; }

        [YamlMember(Description = "调用完成后的回复语（如果有完成后要读取的文件，此消息会在发送文件之前发送）")]
        public string InvokedReply { get; set; }

        [YamlMember(Description = "发生错误的回复语")]
        public string ErrorReply { get; set; }

        [YamlMember(Description = "白名单群")]
        public HashSet<uint> WhiteGroup { get; set; } = new HashSet<uint>();

        [YamlMember(Description = "仅限白名单群使用")]
        public bool WhiteGroupOnly { get; set; } = false;

        [YamlMember(Description = "允许私聊使用")]
        public bool AllowPrivateMessage { get; set; } = true;
    }

    public enum ReadFileModes
    {
        None,
        Text,
        Image,
        Video,
        Audio,
        Raw,
    }
}
