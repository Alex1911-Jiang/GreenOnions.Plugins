using YamlDotNet.Serialization;

namespace GreenOnions.NT.Forwarder
{
    internal class Config
    {
        /// <summary>
        /// 是否启用消息转发器
        /// </summary>
        [YamlMember(Description = "启用消息转发器")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 回复私聊消息命令
        /// </summary>
        [YamlMember(Description = "回复私聊消息命令")]
        public string ReplyFriendCommand { get; set; } = "回复(?<Uin>\\d{5,19})[;：](?<Message>.+)";

        /// <summary>
        /// 是否转发所有私聊消息到管理员
        /// </summary>
        [YamlMember(Description = "是否转发所有私聊消息到管理员（由于Lagrange不存在中断消息链机制，所以收到其他插件的命令也会被转发）")]
        public bool ForwardAllPrivateMessageToAdmin { get; set; } = false;

        /// <summary>
        /// 转发消息给管理员时的排版格式
        /// </summary>
        [YamlMember(Description = "转发消息给管理员时的排版格式")]
        public string ForwardToAdminFormat { get; set; } = "<FriendNickName>(<FriendUin>)说：<Message>";

        /// <summary>
        /// 管理员回复消息的排版格式
        /// </summary>
        [YamlMember(Description = "管理员回复消息的排版格式")]
        public string AdminReplyFormat { get; set; } = "管理员回复：<Message>";

        /// <summary>
        /// 好友消息转发配置
        /// </summary>
        [YamlMember(Description = "好友消息转发配置")]
        public List<FriendForwardSettings> Friend = [];

        /// <summary>
        /// 群消息转发配置
        /// </summary>
        [YamlMember(Description = "群消息转发配置")]
        public List<GroupForwardSettings> Group = [];
    }

    internal class FriendForwardSettings
    {
        [YamlMember(Description = "转发来源的好友QQ号")]
        public uint FromFriendUin { get;set; }

        [YamlMember(Description = "转发去向的好友QQ号")]
        public uint ToFriendUin { get; set; }

        [YamlMember(Description = "排版格式，留空则原样转发")]
        public string Format { get; set; } = "<FriendNickName>(<FriendUin>)说：<Message>";

        [YamlMember(Description = "备注")]
        public string Remark { get; set; } = string.Empty;
    }

    internal class GroupForwardSettings
    {
        [YamlMember(Description = "转发来源的群号")]
        public uint FromGroupUin { get; set; }

        [YamlMember(Description = "转发来源的群友QQ号")]
        public uint FromMemberUin { get; set; }

        [YamlMember(Description = "转发去向群号")]
        public uint ToGroupUin { get; set; }

        [YamlMember(Description = "排版格式，留空则原样转发")]
        public string Format { get; set; } = "<GroupUin>的<MemoryNickName>说：<Message>";

        [YamlMember(Description = "备注")]
        public string Remark { get; set; } = string.Empty;
    }
}
