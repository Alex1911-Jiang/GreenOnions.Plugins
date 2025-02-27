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
                if (!SngletonInstance.Config.AdminQQ.Contains(e.Chain.FriendUin))
                    return;

                if (e.Chain.GetEntity<TextEntity>() is not TextEntity text)
                    return;

                if (text.Text != "导出群成员")
                    return;

                DataSet ds = new DataSet();
                var groups = await context.FetchGroups();
                foreach (var group in groups)
                {
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
                await context.UploadFriendFile(e.Chain.FriendUin, new FileEntity(xmlByte, "groups.xml"));
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }
    }
}
