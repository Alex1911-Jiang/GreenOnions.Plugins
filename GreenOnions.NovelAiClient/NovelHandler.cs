using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.PluginConfigs.NovelAiClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.NovelAiClient
{
    public class NovelHandler : IMessagePlugin, IPluginSetting, IPluginHelp
    {
        private string? _pluginPath;
        private NovelAiConfig? _config;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;
        private string? _strCmd;
        private string? _configDirect;
        public string Name => "NovelAi画图";

        public string Description => "NovelAi画图插件";

        public GreenOnionsMessages? HelpMessage => "发送 \"<机器人名称>画图：<关键词>\" 来绘制一张图片，多个关键词请用英文逗号分隔。";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            ReloadConfig();
        }

        public void ReloadConfig()
        {
            string configPath = Path.Combine(_pluginPath!, "config.json");
            if (File.Exists(configPath))
            {
                _config = JsonConvert.DeserializeObject<NovelAiConfig>(File.ReadAllText(configPath));
                if (_config is null)
                {
                    for (int i = 0; i < _botConfig!.AdminQQ.Count; i++)
                        _api.SendFriendMessageAsync(_botConfig.AdminQQ.ToArray()[i], $"葱葱NovelAi画图插件读取配置文件失败，请检查json格式，文件路径为：{configPath}");
                }
            }
            else
            {
                //配置文件例子
                _config = new NovelAiConfig();
                string paramFileDirect = Path.Combine(_pluginPath!, "params.txt");
                if (!File.Exists(paramFileDirect))
                    File.WriteAllText(paramFileDirect, "");
            }
            File.WriteAllText(configPath!, JsonConvert.SerializeObject(_config, Formatting.Indented));
            _strCmd = _config!.Cmd;
        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            _pluginPath = pluginPath;
            _botConfig = botConfig;
            _configDirect = Path.Combine(_pluginPath!, "config.json");
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (_config is not null)
            {
                if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg && _strCmd is not null)
                {
                    Regex regex = new Regex(_api!.ReplaceGreenOnionsStringTags(_strCmd));
                    if (regex.IsMatch(msg.Text))
                    {
                        Match match = regex.Match(msg.Text);
                        string promptStr = msg.Text.Substring(match.Value.Length);
                        string prompts = string.Join(',', promptStr.Split(',', '，').Select(s => s.Trim()).Where(p => !_config!.DefaultUndesired!.Contains(p)));
                        if (!string.IsNullOrWhiteSpace(_config.StartDrawMessage))
                            Response(_config.StartDrawMessage);
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        try
                        {
                            if (_config!.ConnectFrontEnd == FrontEnd.Naifu)
                                GetNaifuImageBytes(prompts).ContinueWith(CallBack);
                            else if (_config.ConnectFrontEnd == FrontEnd.WebUI)
                                GetWebUIImageBytes(prompts).ContinueWith(CallBack);
                            //else if (_config.ConnectFrontEnd.Equals("webui", StringComparison.OrdinalIgnoreCase))
                            //    GetWebUIImageBytes(prompts).ContinueWith(CallBack);

                            void CallBack(Task<byte[]?> task)
                            {
                                sw.Stop();
                                if (task.IsFaulted || task.IsCanceled || task.Result is null)
                                {
                                    if (!string.IsNullOrEmpty(_config!.DrawErrorMessage))
                                        Response(_config!.DrawErrorMessage!.Replace("<耗时>", (sw.ElapsedMilliseconds / 1000f).ToString()));
                                    return;
                                }
                                GreenOnionsMessages msgs = new GreenOnionsImageMessage(new MemoryStream(task.Result));
                                msgs.RevokeTime = _config.RevokeSecond;
                                Response(msgs);
                                if (!string.IsNullOrWhiteSpace(_config!.DrawEndMessage))
                                    Response(_config.DrawEndMessage.Replace("<耗时>", (sw.ElapsedMilliseconds / 1000f).ToString()));
                            }
                        }
                        catch (Exception ex)
                        {
                            sw.Stop();
                            if (!string.IsNullOrEmpty(_config!.DrawErrorMessage))
                                Response(_config!.DrawErrorMessage!.Replace("<耗时>", (sw.ElapsedMilliseconds / 1000f).ToString()));

                            for (int i = 0; i < _botConfig!.AdminQQ.Count; i++)
                                _api.SendFriendMessageAsync(_botConfig.AdminQQ.ToArray()[i], $"葱葱NovelAi画图插件发生异常，{ex.Message}");
                        }
                        return true;
                    }
                }
            }
            return false;
        }



        private Task<byte[]?> GetNaifuImageBytes(string prompts)
        {
            NaifuClient naifuClient = new NaifuClient(_config!.URL!);
            string defalutPrompt = _config!.DefaultPrompt!.Trim();
            if (!defalutPrompt.EndsWith(','))
                defalutPrompt += ',';
            return naifuClient.PostAsync(defalutPrompt + prompts, _config!.DefaultUndesired);
        }

        //private Task<byte[]?> GetWebUIImageBytes(string prompts)
        //{
        //    Obsolete_WebUIClient webuiClient = new NovelAIClient.WebUIClient(_config!.fn_index, _config!.URL!);
        //    string defalutPrompt = _config!.DefaultPrompt!.Trim();
        //    if (!defalutPrompt.EndsWith(','))
        //        defalutPrompt += ',';
        //    return webuiClient.PostAsync(defalutPrompt + prompts, _config!.DefaultUndesired);
        //}

        private Task<byte[]?> GetWebUIImageBytes(string prompts)
        {
            WebUIClient webuiClient = new WebUIClient(_config!.fn_index, _config!.URL!, _config.PromptIndex, _config.UndesiredIndex);
            string defalutPrompt = _config!.DefaultPrompt!.Trim();
            if (!defalutPrompt.EndsWith(','))
                defalutPrompt += ',';

            string paramFileDirect = Path.Combine(_pluginPath!, "params.txt");
            string customData = File.ReadAllText(paramFileDirect);
            return webuiClient.PostAsync(customData, defalutPrompt + prompts, _config!.DefaultUndesired ?? "");
        }

        public void Setting()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new Exception("请进入插件目录修改config.json配置文件");

            string editorDirect = Path.Combine("Plugins", "GreenOnions.PluginConfigEditor", "GreenOnions.PluginConfigEditor.exe");
            if (!File.Exists(editorDirect))
                throw new Exception("配置文件编辑器不存在，请安装 GreenOnions.PluginConfigEditor 插件。");
            string paramFileDirect = Path.Combine(_pluginPath!, "params.txt");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect!, paramFileDirect }).WaitForExit();
            ReloadConfig();
            string jsonConfig = JsonConvert.SerializeObject(_config, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(_configDirect!, jsonConfig);
        }
    }
}