using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using GreenOnions.GPT3Client.Models;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace GreenOnions.GPT3Client
{
    public class ChatClient : IMessagePlugin, IPluginHelp
    {
        private string? _pluginPath;
        private Config? _config;
        private IEnumerable<string>? _startCmds;
        private IEnumerable<string>? _exitCmds;
        private ConcurrentDictionary<long, TimeOutWorker> ChatingUser = new ConcurrentDictionary<long, TimeOutWorker>();
        private IGreenOnionsApi? _api;
        private HttpClient? _client;
        private GPT_3_Encoder_Sharp.Encoder? _gptEncoder;
        private CancellationTokenSource? _timeOutWorkerTs;

        public string Name => "AI聊天";

        public string Description => "GPT3聊天插件";

        public GreenOnionsMessages? HelpMessage
        {
            get
            {
                string? startCmd = _config?.StartCommands?.FirstOrDefault();
                string? exitCmd = _config?.ExitCommands?.FirstOrDefault();
                if (_config != null && startCmd != null && exitCmd != null)
                    return $"发送 \"{startCmd}\" 进入聊天模式，之后您发送的所有消息都将被视为与<机器人名称>对话，{_config.TimeOutSecond / 60}分钟不发言或发送 \"{exitCmd}\" 结束聊天模式。\r\n(<机器人名称>只会记QQ号，不会记群号，在您退出聊天模式之前，您在其他群的发言也会被视为与<机器人名称>聊天)";
                return null;
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
            _client?.Dispose();
            _client = null;
            string configFileName = Path.Combine(_pluginPath!, "config.json");
            if (File.Exists(configFileName))
            {
                string strConfig = File.ReadAllText(configFileName);
                _config = JsonSerializer.Deserialize<Config>(strConfig);
                if (_config != null)
                {
                    if (_config.StartCommands != null)
                        _startCmds = _config.StartCommands.Select(s => api.ReplaceGreenOnionsStringTags(s));
                    if (_config.ExitCommands != null)
                        _exitCmds = _config.ExitCommands.Select(s => api.ReplaceGreenOnionsStringTags(s));

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.46");
                }
                StartTimeoutWorker();
            }
            else
            {
                Config newConfig = new Config()
                {
                    APIkey = "",
                    StartCommands = new[] { "<机器人名称>尬聊", "<机器人名称>AI聊天" },
                    ExitCommands = new[] { "<机器人名称>不聊了", "<机器人名称>住口", "<机器人名称>闭嘴" },
                    ChatStartMessage = "您已进入AI聊天模式，如需退出请发送\"<机器人名称>闭嘴\"",
                    ExitChatMessage = "下次再聊哦",
                    TimeOutMessage = "由于超时，已为您自动退出聊天模式。",
                    ErrorMessage = "发生错误，请联系机器人管理员（<错误信息>）",
                    Temperature = 0,
                    TimeOutSecond = 120,
                };
                File.WriteAllText(configFileName, JsonSerializer.Serialize(newConfig, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
                File.WriteAllText(Path.Combine(_pluginPath!, "说明.txt"), "使用前，用户需要自行魔法上网，注册OpenAI账号，并在 https://beta.openai.com/account/api-keys 处申请一个API-Key，申请完API后，无需魔法上网也可使用\r\n" +
                    "必须填写APIkey才可以使用，修改配置文件后请重启葱葱\r\n\r\n" +
                    "配置文件说明：\r\n" +
                    "StartCommands:进入聊天模式命令，支持多条命令\r\n" +
                    "ExitChatMessage:主动退出聊天模式命令，支持多条命令\r\n" +
                    "ChatStartMessage:进入聊天模式后的固定回复语\r\n" +
                    "TimeOutMessage:超时自动退出聊天模式的回复语\r\n" +
                    "ExitChatMessage:退出聊天模式后的回复语\r\n" +
                    "ErrorMessage:请求GPT3失败的回复语\r\n" +
                    "Temperature:允许范围0-1，小数点后一位，数值越大，GPT会给出越具有创造性的回答，越小则越保守\r\n" +
                    "TimeOutSecond:超时时间，单位(秒)");
            }
        }

        public void OnDisconnected()
        {
            _timeOutWorkerTs?.Cancel();
            _timeOutWorkerTs?.Dispose();
            _timeOutWorkerTs = null;
            _client?.Dispose();
            _client = null;
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (_config != null && !string.IsNullOrWhiteSpace(_config.APIkey))
            {
                if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg)
                {
                    if (ChatingUser.ContainsKey(msgs.SenderId))
                    {
                        if (_exitCmds != null && _exitCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //聊天结束
                        {
                            ChatingUser.TryRemove(msgs.SenderId, out _);
                            if (!string.IsNullOrEmpty(_config.ExitChatMessage))
                                Response(_api!.ReplaceGreenOnionsStringTags(_config.ExitChatMessage));
                        }
                        else  //聊天中
                        {
                            if (ChatingUser.ContainsKey(msgs.SenderId))
                            {
                                ChatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);//先重置一下超时时间，避免请求过程中超时
                                SendMessageToGPT(msg.Text, _config.APIkey)?.ContinueWith(callback =>
                                {
                                    Response(callback.Result);
                                    ChatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);  //消息发出去后再重置一下超时时间
                                });
                            }
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
                                    Response(_api!.ReplaceGreenOnionsStringTags(_config.ChatStartMessage));
                                ChatingUser.TryAdd(msgs.SenderId, new TimeOutWorker { TimeOutAt = DateTime.Now.AddSeconds(_config.TimeOutSecond), TimeOutDo = Response });
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private async Task<string> SendMessageToGPT(string msg, string apiKey)
        {
            if (_gptEncoder is null)
                _gptEncoder = GPT_3_Encoder_Sharp.Encoder.Get_Encoder();

            int maxToken = 4096 - _gptEncoder.Encode(msg).Count;
            RequestModel requestModel = new RequestModel()
            {
                prompt = msg,
                max_tokens = maxToken,
                temperature = _config!.Temperature,
            };

            try
            {
                using HttpRequestMessage request = new(HttpMethod.Post, "https://api.openai.com/v1/completions");
                using HttpContent content = new StringContent(JsonSerializer.Serialize(requestModel, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
                request.Content = content;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                HttpResponseMessage resp = await _client!.SendAsync(request);
                string respStr = await resp.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(respStr))
                {
                    return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "GPT3没有返回任何结果"));
                }
                ResponseModel? response = JsonSerializer.Deserialize<ResponseModel>(respStr);
                if (response is null)
                {
                    return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "响应信息解析失败"));
                }

                if (response.error is null)
                {
                    choice? choice = response.choices.FirstOrDefault();
                    if (choice is null)
                        return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "GPT返回内容中不含任何文本"));
                    else
                        return choice.text;  //正常回答
                }
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", response.error.message));
            }
            catch (Exception ex)
            {
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", ex.Message));
            }
        }

        public void WindowSetting()
        {
            throw new Exception("请进入插件目录修改config.json配置文件");
        }
    }
}