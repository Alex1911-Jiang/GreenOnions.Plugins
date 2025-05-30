using System.Data;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.ExportGroupMembers
{
    public class ExportPlugin : IPlugin
    {
        public string Name => "导出群成员";

        public string Description => "导出群成员信息为Excel的插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {

        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            bot.Invoker.OnFriendMessageReceived += Invoker_OnFriendMessageReceived;
            bot.Invoker.OnBotOnlineEvent += Invoker_OnBotOnlineEvent;
        }

        private async void Invoker_OnBotOnlineEvent(BotContext context, Lagrange.Core.Event.EventArg.BotOnlineEvent e)
        {
            await Export(context);
        }

        private async void Invoker_OnFriendMessageReceived(BotContext context, Lagrange.Core.Event.EventArg.FriendMessageEvent e)
        {
            try
            {
                if (SngletonInstance.Config is null)
                {
                    LogHelper.LogError("配置文件为空");
                    return;
                }
                if (e.Chain.GetEntity<TextEntity>() is not TextEntity text)
                    return;

                if (text.Text != "导出群成员")
                    return;

                if (!SngletonInstance.Config.AdminQQ.Contains(e.Chain.FriendUin))
                {
                    LogHelper.LogWarning($"QQ号：{e.Chain.FriendUin} 触发了命令，但该号码不是管理员，不响应命令");
                    return;
                }

                LogHelper.LogMessage($"开始导出机器人的群员信息给：{e.Chain.FriendUin}");

                byte[] xmlByte = await Export(context);
                await context.UploadFriendFile(e.Chain.FriendUin, new FileEntity(xmlByte, "groups.xml"));
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async Task<byte[]> Export(BotContext context)
        {
            LogHelper.LogMessage($"开始导出群成员");
            DataSet ds = new DataSet();
            var groups = await context.FetchGroups();
            foreach (var group in groups)
            {
                LogHelper.LogMessage($"写入Sheet：{group.GroupName}-{group.GroupUin}");
                DataTable dt = new DataTable($"{group.GroupName}-{group.GroupUin}");
                dt.Columns.Add("QQ号");
                dt.Columns.Add("昵称");
                dt.Columns.Add("身份");
                ds.Tables.Add(dt);
                var members = await context.FetchMembers(group.GroupUin);
                foreach (var member in members)
                    dt.Rows.Add(member.Uin, member.MemberName, member.Permission);
            }

            using MemoryStream ms = new MemoryStream();
            ds.WriteExcelXml(ms);
            string dir = Path.Combine(AppContext.BaseDirectory, "groups.xml");
            byte[] xmlByte = ms.ToArray();
            File.WriteAllBytes(dir, xmlByte);
            LogHelper.LogMessage($"导出完成");
            return xmlByte;
        }
    }
}
