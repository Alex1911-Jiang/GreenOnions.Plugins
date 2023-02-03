using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.PluginConfigs.Replier;
using Newtonsoft.Json;

namespace GreenOnions.Replier
{
    public class Replier : IMessagePlugin, IPluginSetting
    {
        private string? _pluginPath;
        private string? _configDirect;
        private string? _imagePath;
        private CommandSetting[]? _commandTable;

        public string Name => "自定义回复";

        public string Description => "自定义回复插件";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            _pluginPath = pluginPath;
            _imagePath = Path.Combine(_pluginPath, "Images");
            _configDirect = Path.Combine(_pluginPath, "config.json");
            ReloadConfig();
        }

        private void ReloadConfig()
        {
            if (File.Exists(_configDirect))
                _commandTable = JsonConvert.DeserializeObject<CommandSetting[]>(File.ReadAllText(_configDirect))!;
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.First() is GreenOnionsTextMessage textMsg)
            {
                var comms = _commandTable.OrderBy(c => c.Priority);
                foreach (var comm in comms)
                {
                    if ((comm.TriggerMode & TriggerModes.群组) != 0 && senderGroup != null)
                    {
                        GreenOnionsMessages? reply = CaeateReply(textMsg, comm);
                        if (reply != null)
                        {
                            reply.Reply = comm.ReplyMode;
                            Response(reply);
                            return true;
                        }
                    }
                    if ((comm.TriggerMode & TriggerModes.私聊) != 0 && senderGroup == null)
                    {
                        GreenOnionsMessages? reply = CaeateReply(textMsg, comm);
                        if (reply != null)
                        {
                            reply.Reply = comm.ReplyMode;
                            Response(reply);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private GreenOnionsMessages? CaeateReply(GreenOnionsTextMessage textMsg, CommandSetting comm)
        {
            if (comm.MatchMode == MatchModes.完全 && textMsg.Text == comm.Message)
                return RandomSamePriority(comm.Message, comm.Priority);
            else if (comm.MatchMode == MatchModes.前缀 && textMsg.Text.StartsWith(comm.Message))
                return RandomSamePriority(comm.Message, comm.Priority);
            else if (comm.MatchMode == MatchModes.后缀 && textMsg.Text.EndsWith(comm.Message))
                return RandomSamePriority(comm.Message, comm.Priority);
            else if (comm.MatchMode == MatchModes.正则表达式)
            {
                Regex regex = new Regex(comm.Message);
                Match match = regex.Match(textMsg.Text);
                if (match.Value == textMsg.Text)
                    return RandomSamePriority(regex.ToString(), comm.Priority);
            }
            else if (comm.MatchMode == MatchModes.包含 && textMsg.Text.Contains(comm.Message))
                return RandomSamePriority(comm.Message, comm.Priority);
            return null;
        }

        private GreenOnionsMessages RandomSamePriority(string msgCmd, int priority)
        {
            var sameCmdAndPriorityGroup = _commandTable.Where(r => r.Message == msgCmd && r.Priority == priority).ToArray();
            if (sameCmdAndPriorityGroup.Length > 1)
                return ReplaceImages(sameCmdAndPriorityGroup[new Random().Next(0, sameCmdAndPriorityGroup.Length)].ReplyValue);
            return ReplaceImages(sameCmdAndPriorityGroup.First().ReplyValue);
        }

        private GreenOnionsMessages ReplaceImages(string textMessage)
        {
            Dictionary<string, string> imageNameAndPaths = new Dictionary<string, string>();
            List<string> splitedText = new List<string>();
            splitedText.Add(textMessage);

            string[] imgs = Directory.GetFiles(_imagePath!);
            for (int i = 0; i < imgs.Length; i++)
            {
                string imgTag = $"<{Path.GetFileName(imgs[i])}>";
                imageNameAndPaths.Add(imgTag, imgs[i]);
            IL_Research:;
                for (int j = 0; j < splitedText.Count; j++)
                {
                    if (splitedText[j] != imgTag && splitedText[j].Contains(imgTag))
                    {
                        string originalMsg = splitedText[j];
                        splitedText.RemoveAt(j);
                        string[] splited = originalMsg.Split(imgTag);
                        for (int k = 0; k < splited.Length; k++)
                        {
                            splitedText.Add(splited[k]);
                            if (k < splited.Length -1)
                                splitedText.Add(imgTag);
                        }
                        goto IL_Research;
                    }
                }
            }
            GreenOnionsMessages messages = new GreenOnionsMessages();
            for (int i = 0; i < splitedText.Count; i++)
            {
                if (imageNameAndPaths.ContainsKey(splitedText[i]))
                    messages.Add(new GreenOnionsImageMessage(imageNameAndPaths[splitedText[i]]));
                else
                    messages.Add(new GreenOnionsTextMessage(splitedText[i]));
            }
            return messages;
        }

        public void Setting()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            string editorDirect = Path.Combine("Plugins", "GreenOnions.PluginConfigEditor", "GreenOnions.PluginConfigEditor.exe");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect! }).WaitForExit();
            ReloadConfig();
        }
    }
}