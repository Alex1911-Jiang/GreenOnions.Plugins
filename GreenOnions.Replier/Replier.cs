using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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
        private string? _audioPath;
        private ReplierConfig[]? _commandTable;
        private Dictionary<string, string> _imageNameAndPaths;
        private Dictionary<string, string> _audioNameAndPaths;

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
            _audioPath = Path.Combine(_pluginPath, "Audios");
            _configDirect = Path.Combine(_pluginPath, "config.json");
            ReloadConfig();
        }

        private void ReloadConfig()
        {
            if (File.Exists(_configDirect))
                _commandTable = JsonConvert.DeserializeObject<ReplierConfig[]>(File.ReadAllText(_configDirect))!;

            _imageNameAndPaths = new Dictionary<string, string>();
            _audioNameAndPaths = new Dictionary<string, string>();

            string[] imgs = Directory.GetFiles(_imagePath!);
            for (int i = 0; i < imgs.Length; i++)
            {
                string imgTag = $"<{Path.GetFileName(imgs[i])}>";
                _imageNameAndPaths.Add(imgTag, imgs[i]);
            }
            string[] ados = Directory.GetFiles(_audioPath!);
            for (int i = 0; i < ados.Length; i++)
            {
                string imgTag = $"<{Path.GetFileName(ados[i])}>";
                _audioNameAndPaths.Add(imgTag, ados[i]);
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (_commandTable is null)
                return false;
            if (msgs.First() is not GreenOnionsTextMessage textMsg)
                return false;
            var comms = _commandTable.OrderBy(c => c.Priority);
            foreach (var comm in comms)
            {
                if (senderGroup is not null && (comm.TriggerMode & TriggerModes.群组) == 0)
                    continue;
                if (senderGroup is null && (comm.TriggerMode & TriggerModes.私聊) == 0)
                    continue;
                GreenOnionsMessages? reply = CaeateReply(textMsg, comm);
                if (reply is null)
                    continue;
                reply.Reply = comm.ReplyMode;
                Response(reply);
                return true;
            }
            return false;
        }

        private GreenOnionsMessages? CaeateReply(GreenOnionsTextMessage textMsg, ReplierConfig comm)
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
            var sameCmdAndPriorityGroup = _commandTable!.Where(r => r.Message == msgCmd && r.Priority == priority).ToArray();
            if (sameCmdAndPriorityGroup.Length > 1)
                return ReplaceImages(sameCmdAndPriorityGroup[new Random(Guid.NewGuid().GetHashCode()).Next(0, sameCmdAndPriorityGroup.Length)].ReplyValue);
            return ReplaceImages(sameCmdAndPriorityGroup.First().ReplyValue);
        }

        private GreenOnionsMessages ReplaceImages(string textMessage)
        {
            List<string> splitedText = new();
            bool tagStart = false;
            StringBuilder itemText = new StringBuilder();
            for (int i = 0; i < textMessage.Length; i++)
            {
                if (!tagStart)
                {
                    if (textMessage[i] == '<')
                    {
                        splitedText.Add(itemText.ToString());
                        itemText.Clear();
                        tagStart = true;
                    }
                }
                else
                {
                    if (textMessage[i] == '>')
                    {
                        tagStart = false;
                        itemText.Append(textMessage[i]);
                        splitedText.Add(itemText.ToString());
                        itemText.Clear();
                        continue;
                    }
                }
                itemText.Append(textMessage[i]);
            }
            splitedText.Add(itemText.ToString());
            splitedText = splitedText.Where(t => !string.IsNullOrEmpty(t)).ToList();

            GreenOnionsMessages messages = new GreenOnionsMessages();
            for (int i = 0; i < splitedText.Count; i++)
            {
                if (_imageNameAndPaths.ContainsKey(splitedText[i]))
                    messages.Add(new GreenOnionsImageMessage(_imageNameAndPaths[splitedText[i]]));
                else if (_audioNameAndPaths.ContainsKey(splitedText[i]))
                    messages.Add(new GreenOnionsVoiceMessage(_audioNameAndPaths[splitedText[i]]));
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
            if (!File.Exists(editorDirect))
                throw new Exception("配置文件编辑器不存在，请安装 GreenOnions.PluginConfigEditor 插件。");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect! }).WaitForExit();
            ReloadConfig();
        }
    }
}