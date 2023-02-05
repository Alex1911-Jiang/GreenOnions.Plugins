using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using GreenOnions.GPT3Client.Models;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.Interface.DispatchCenter;
using GreenOnions.PluginConfigs.GPT3Client;
using Newtonsoft.Json;

namespace GreenOnions.GPT3Client
{
    public class ChatClient : IMessagePlugin, IPluginHelp, IPluginSetting
    {
        private string? _path;
        private string? _configDirect;
        private IBotConfig? _botConfig;
        private GPT3ClientConfig? _config;
        private IEnumerable<string>? _startCmds;
        private IEnumerable<string>? _exitCmds;
        private ConcurrentDictionary<long, TimeOutWorker> ChatingUser = new ConcurrentDictionary<long, TimeOutWorker>();
        private IGreenOnionsApi? _api;
        private Dictionary<long, HttpClient> _clients = new Dictionary<long, HttpClient>();
        private GPT_3_Encoder_Sharp.Encoder? _gptEncoder;
        private CancellationTokenSource? _timeOutWorkerTs;

        public string Name => "AI聊天";

        public string Description => "GPT3聊天插件";

        public GreenOnionsMessages? HelpMessage
        {
            get
            {
                if (_config is null)
                    return null;

                string? startCmd = _config?.StartCommands?.FirstOrDefault();
                string? exitCmd =_config?.ExitCommands?.FirstOrDefault();

                StringBuilder sbHelp = new StringBuilder();

                if (startCmd is not null)
                {
                    sbHelp.Append($"发送 \"{startCmd}\" 进入聊天模式，之后您发送的所有消息都将被视为与<机器人名称>对话");
                }
                if (_config!.TimeOutSecond > 0) 
                {
                    sbHelp.Append($"{_config.TimeOutSecond / 60}分钟不发言");
                }
                if (_config!.TimeOutSecond > 0 && exitCmd is not null)
                {
                    sbHelp.Append('或');
                }
                if (exitCmd is not null)
                {
                    sbHelp.Append($"发送 \"{exitCmd}\"");
                }
                if (_config!.TimeOutSecond > 0 || exitCmd is not null)
                {
                    sbHelp.Append($"结束聊天模式。");
                }
                if (sbHelp.Length > 0)
                {
                    sbHelp.AppendLine($"(请注意：<机器人名称>只会记QQ号，不会记群号，在您退出聊天模式之前，您在其他群的发言也会被视为与<机器人名称>聊天)");
                }
                return _api!.ReplaceGreenOnionsStringTags(sbHelp.ToString());
            }
        }

        private void StartTimeoutWorker()
        {
            if (!string.IsNullOrWhiteSpace(_config!.TimeOutMessage))
            {
                _timeOutWorkerTs?.Cancel();
                _timeOutWorkerTs?.Dispose();
                _timeOutWorkerTs = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    while (true)
                    {
                        foreach (var item in ChatingUser)
                        {
                            if (DateTime.Now > item.Value.TimeOutAt)
                            {
                                item.Value.TimeOutDo?.Invoke(_config.TimeOutMessage);
                                ChatingUser.TryRemove(item.Key, out _);
                            }
                        }
                        await Task.Delay(1000);
                    }
                }, _timeOutWorkerTs.Token);
            }
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            foreach (KeyValuePair<long, HttpClient> item in _clients)
                item.Value.Dispose();
            _clients.Clear();
            try
            {
                ReloadConfig();
            }
            catch (Exception ex)
            {
                foreach (var item in _botConfig!.AdminQQ)
                    _api!.SendFriendMessageAsync(item, ex.Message);
                return;
            }
        }

        public void OnDisconnected()
        {
            _timeOutWorkerTs?.Cancel();
            _timeOutWorkerTs?.Dispose();
            _timeOutWorkerTs = null;
            foreach (KeyValuePair<long, HttpClient> item in _clients)
                item.Value.Dispose();
            _clients.Clear();
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _path = pluginPath;
            _botConfig = config;
            _configDirect = Path.Combine(_path!, "config.json");
            _gptEncoder = GPT_3_Encoder_Sharp.Encoder.Get_Encoder();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (_config is null || string.IsNullOrWhiteSpace(_config.APIkey))
                return false;
            if (msgs.FirstOrDefault() is not GreenOnionsTextMessage msg)
                return false;
            if (ChatingUser.ContainsKey(msgs.SenderId))
            {
                if (_exitCmds != null && _exitCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //聊天结束
                {
                    ChatingUser.TryRemove(msgs.SenderId, out _);
                    if (!string.IsNullOrEmpty(_config.ExitChatMessage))
                        Response(_api!.ReplaceGreenOnionsStringTags(_config.ExitChatMessage)!);
                }
                else  //聊天中
                {
                    ChatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);//先重置一下超时时间，避免请求过程中超时
                    SendMessageToGPT(msgs.SenderId, msg.Text, _config.APIkey)?.ContinueWith(callback =>
                    {
                        GreenOnionsMessages? outMsg = callback.Result;
                        outMsg!.Reply = _config.SendMessageByReply;
                        Response(outMsg);
                        ChatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);  //消息发出去后再重置一下超时时间
                    });
                }
                return true;
            }
            else
            {
                if (_startCmds != null)
                {
                    if (_startCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //开始聊天
                    {
                        if (!string.IsNullOrEmpty(_config.ChatStartMessage))
                            Response(_api!.ReplaceGreenOnionsStringTags(_config.ChatStartMessage)!);
                        ChatingUser.TryAdd(msgs.SenderId, new TimeOutWorker { TimeOutAt = DateTime.Now.AddSeconds(_config.TimeOutSecond), TimeOutDo = Response });
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<string> SendMessageToGPT(long qqId, string msg, string apiKey)
        {
            int maxToken = 4096 - _gptEncoder!.Encode(msg).Count;
            RequestModel requestModel = new RequestModel()
            {
                prompt = msg,
                max_tokens = maxToken,
                temperature = _config!.Temperature,
            };
            string strBody = JsonConvert.SerializeObject(requestModel, Formatting.Indented);
            
            string? respStr;
            try
            {
                if (_config.RequestByPlugin && HttpClientSubstitutes.PostStringAsync is not null)  //请求
                    respStr = await PostByPlugin(strBody, apiKey);
                else
                    respStr = await PostBySharp(qqId, strBody, apiKey);
            }
            catch (Exception ex)
            {
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", ex.Message))!;
            }

            if (string.IsNullOrEmpty(respStr))
            {
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "GPT3没有返回任何结果"))!;
            }
            ResponseModel? response = JsonConvert.DeserializeObject<ResponseModel>(respStr);
            if (response is null)
            {
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "响应信息解析失败"))!;
            }

            if (response.error is null)
            {
                choice? choice = response.choices.FirstOrDefault();
                if (choice is null)
                    return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "GPT返回内容中不含任何文本"))!;
                else
                    return choice.text.TrimStart();  //正常回答
            }
            return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", response.error.message))!;
        }

        private async Task<string> PostByPlugin(string strBody, string apiKey)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {apiKey}" }
            };
            return await HttpClientSubstitutes.PostStringAsync!("https://api.openai.com/v1/completions", strBody, headers);
        }

        private async Task<string> PostBySharp(long qqId, string strBody, string apiKey)
        {
            HttpClient client;
            if (_clients.ContainsKey(qqId))
            {
                client = _clients[qqId];
            }
            else
            {
                HttpClientHandler handler = new HttpClientHandler();
                if (_config!.UseProxy)
                {
                    handler.UseProxy = true;
                    handler.Proxy = new WebProxy(_botConfig!.ProxyUrl);
                }
                client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.46");
            }

            using HttpRequestMessage request = new(HttpMethod.Post, "https://api.openai.com/v1/completions");
            using HttpContent content = new StringContent(strBody, Encoding.UTF8, "application/json");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            HttpResponseMessage resp = await client!.SendAsync(request);
            return await resp.Content.ReadAsStringAsync();
        }

        private void ReloadConfig()
        {
            if (!File.Exists(_configDirect))
                return;

            string strConfig = File.ReadAllText(_configDirect!);
            _config = JsonConvert.DeserializeObject<GPT3ClientConfig>(strConfig);
            if (_config is null)
            {
                throw new ArgumentNullException("葱葱GPT3聊天插件配置文件不合法，请检查json格式");
            }

            if (_config.StartCommands != null)
                _startCmds = _config.StartCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            if (_config.ExitCommands != null)
                _exitCmds = _config.ExitCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            StartTimeoutWorker();
        }

        public void Setting()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new Exception("请进入插件目录修改config.json配置文件");

            string editorDirect = Path.Combine("Plugins", "GreenOnions.PluginConfigEditor", "GreenOnions.PluginConfigEditor.exe");
            if (!File.Exists(editorDirect))
                throw new Exception("配置文件编辑器不存在，请安装 GreenOnions.PluginConfigEditor 插件。");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect! }).WaitForExit();
            ReloadConfig();
            string jsonConfig = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_configDirect!, jsonConfig);
            
        }
    }
}