using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using BingChat;
using GreenOnions.ChatGPTClient.Models;
using GreenOnions.ChatGPTClient.Models.Request;
using GreenOnions.ChatGPTClient.Models.Response;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.Interface.DispatchCenter;
using GreenOnions.PluginConfigs.ChatGPTClient;
using Markdig;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.ChatGPTClient
{
    public class ChatClient : IMessagePlugin, IPluginHelp, IPluginSetting
    {
        private string? _path;
        private string? _configDirect;
        private IBotConfig? _botConfig; 
        private IGreenOnionsApi? _api;

        private ChatGPTClientConfig? _config;
        private IEnumerable<string>? _startCmds;
        private IEnumerable<string>? _exitCmds;
        private IEnumerable<string>? _clearContextCmds;
        private ConcurrentDictionary<long, TimeOutWorker> _chatingUser = new ConcurrentDictionary<long, TimeOutWorker>();
        private ConcurrentDictionary<long, List<Message>> _chatHistory = new ConcurrentDictionary<long, List<Message>>();
        private GPT_3_Encoder_Sharp.Encoder? _gptEncoder;
        private CancellationTokenSource? _timeOutWorkerTs;

        public string Name => "ChatGPT";

        public string Description => "ChatGPT聊天插件";

        public GreenOnionsMessages? HelpMessage
        {
            get
            {
                if (_config is null)
                    return null;

                string? startCmd = _config?.StartCommands?.FirstOrDefault();
                string? exitCmd = _config?.ExitCommands?.FirstOrDefault();
                string? clearContextCmd = _config?.ClearContextCommands?.FirstOrDefault();

                StringBuilder sbHelp = new StringBuilder();

                if (!string.IsNullOrEmpty(startCmd))
                {
                    sbHelp.Append($"发送 \"{startCmd}\" 进入聊天模式，之后您发送的所有消息都将被视为与<机器人名称>对话");
                }
                if (_config!.TimeOutSecond > 0)
                {
                    if (sbHelp.Length > 0)
                        sbHelp.Append("，");
                    sbHelp.Append($"{_config.TimeOutSecond / 60}分钟不发言");
                }
                
                if (!string.IsNullOrEmpty(exitCmd))
                {
                    if (_config!.TimeOutSecond > 0)
                        sbHelp.Append('或');
                    sbHelp.Append($"发送 \"{exitCmd}\"");
                }
                if (_config!.TimeOutSecond > 0 || exitCmd is not null)
                {
                    sbHelp.Append($"结束聊天模式。");
                }
                sbHelp.AppendLine();
                if (sbHelp.Length > 0)
                {
                    sbHelp.AppendLine($"(请注意：<机器人名称>只会记QQ号，不会记群号，在您退出聊天模式之前，您在其他群的发言也会被视为与<机器人名称>聊天)");
                }
                if (!string.IsNullOrEmpty(clearContextCmd))
                {
                    sbHelp.AppendLine($"如果您希望<机器人名称>忘掉前面的聊天内容并开始一轮新的聊天，可发送\"{clearContextCmd}\"来重置语境。");
                }
                return _api!.ReplaceGreenOnionsStringTags(sbHelp.ToString());
            }
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            try
            {
                ReloadConfig();
            }
            catch (Exception ex)
            {
                _api.SendMessageToAdmins(ex.Message!);
            }
        }

        public void OnDisconnected()
        {
            _timeOutWorkerTs?.Cancel();
            _timeOutWorkerTs?.Dispose();
            _timeOutWorkerTs = null;
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _path = pluginPath;
            _botConfig = config;
            _configDirect = Path.Combine(_path!, "config.json");
            _gptEncoder = GPT_3_Encoder_Sharp.Encoder.Get_Encoder();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages?> Response)
        {
            if (_config is null || string.IsNullOrWhiteSpace(_config.APIkey))
                return false;
            if (msgs.FirstOrDefault() is not GreenOnionsTextMessage msg)
                return false;
            if (_chatingUser.ContainsKey(msgs.SenderId))
            {
                if (_exitCmds is not null && _exitCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //聊天结束
                {
                    _chatingUser.TryRemove(msgs.SenderId, out _);
                    if (!string.IsNullOrEmpty(_config.ExitChatMessage))
                        Response(_api!.ReplaceGreenOnionsStringTags(_config.ExitChatMessage)!);
                }
                else if (_clearContextCmds is not null && _clearContextCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //重置上下文
                {
                    _chatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);
                    if (_chatHistory.ContainsKey(msgs.SenderId))
                        _chatHistory[msgs.SenderId].Clear();
                    Response(_api!.ReplaceGreenOnionsStringTags(_config.CleanContextMessage)!);
                }
                else  //聊天中
                {
                    _chatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);//先重置一下超时时间，避免请求过程中超时

                    Func<long, string, Task<string>> SendMessage = _config.Model == "NewBing" ? SendMessageToNewBing : SendMessageToChatGPT;
                    try
                    {
                        SendMessage(msgs.SenderId, msg.Text)?.ContinueWith(callback =>
                        {
                            GreenOnionsMessages? outMsg = _config.RemoveMarkdownExpression ? Markdown.ToPlainText(callback.Result) : callback.Result;
                            if (outMsg is null || outMsg.Count == 0)
                                return;
                            outMsg!.Reply = _config.SendMessageByReply;
                            Response(outMsg);
                            _chatingUser[msgs.SenderId].TimeOutAt = DateTime.Now.AddSeconds(_config!.TimeOutSecond);  //消息发出去后再重置一下超时时间
                        });
                    }
                    catch (Exception ex)
                    {
                        _chatingUser.TryRemove(msgs.SenderId, out _);
                        Response(_api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", ex.Message)));
                        _api.SendMessageToAdmins($"葱葱ChatGPT插件错误：{ex.Message}");
                    }
                }
                return true;
            }
            else
            {
                if (_startCmds is null)
                    return false;
                if (_startCmds.Any(c => string.Equals(c, msg.Text, StringComparison.OrdinalIgnoreCase)))  //开始聊天
                {
                    if (!string.IsNullOrEmpty(_config.ChatStartMessage))
                        Response(_api!.ReplaceGreenOnionsStringTags(_config.ChatStartMessage)!);
                    IBingChattable? bingClient = null;
                    if (_config.Model == "NewBing")
                        bingClient = CreateBingClient().Result;
                    _chatingUser.TryAdd(msgs.SenderId, new TimeOutWorker { TimeOutAt = DateTime.Now.AddSeconds(_config.TimeOutSecond), TimeOutDo = Response, BingClient = bingClient });
                    return true;
                }
            }
            return false;
        }

        private async Task<IBingChattable> CreateBingClient()
        {
            var client = new BingChatClient(new BingChatClientOptions
            {
                // The "_U" cookie's value
                Cookie = _config.APIkey,
                ProxyUrl = _botConfig.ProxyUrl
            });
            return await client.CreateConversation();
        }

        private async Task<string> SendMessageToNewBing(long qqId, string msg)
        {
            if (!_chatingUser.ContainsKey(qqId) || _chatingUser[qqId].BingClient is null)
                throw new Exception("管理员切换了目标模型，请重新开启聊天。");
            return await _chatingUser[qqId].BingClient!.AskAsync(msg);
        }

        private async Task<string> SendMessageToChatGPT(long qqId, string msg)
        {
            RequestModel requestModel = new RequestModel(_config!.Model!, _config.Temperature);
            if (!_chatHistory.ContainsKey(qqId))
            {
                _chatHistory.TryAdd(qqId, new List<Message>());
            }
            Message[] userMsgs = _chatHistory[qqId].ToArray().Reverse().ToArray();
            switch (_config.Context)
            {
                case ContextSetting.尽量保持上下文:
                    int remainingTokens = 4096;
                    remainingTokens -= _gptEncoder!.Encode(msg).Count;
                    //倒序计算可分配的Token数
                    for (int i = 0; i < userMsgs.Length; i++)
                    {
                        remainingTokens -= _gptEncoder!.Encode(userMsgs[i].content).Count;
                        if (remainingTokens <= 0)
                            break;
                        requestModel.messages.Add(userMsgs[i]);
                    }
                    requestModel.messages.Reverse();
                    break;
                case ContextSetting.仅保持上一条上下文:
                    if (userMsgs.Length < 2)
                        break;
                    if (userMsgs[0].role != Role.assistant && userMsgs[1].role != Role.user)
                        break;
                    requestModel.messages.Add(userMsgs[1]);
                    requestModel.messages.Add(userMsgs[0]);
                    break;
            }
            //本次请求消息
            Message userMsg = new Message(Role.user, msg);
            requestModel.messages.Add(userMsg);
            string strBody = JsonConvert.SerializeObject(requestModel, new StringEnumConverter());

            string respStr;
            try
            {
                if (_config.RequestByPlugin && HttpClientSubstitutes.PostStringAsync is not null)  //请求
                    respStr = await PostByPlugin(strBody);
                else
                    respStr = await PostBySharp(strBody);
            }
            catch (Exception ex)
            {

                _api!.SendMessageToAdmins($"葱葱ChatGPT插件发生错误：{ex.Message}");
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", ex.Message))!;
            }

            if (string.IsNullOrEmpty(respStr))
            {
                _api!.SendMessageToAdmins($"葱葱ChatGPT插件发生错误：ChatGPT没有返回任何结果");
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "ChatGPT没有返回任何结果"))!;
            }
            ResponseModel? response = JsonConvert.DeserializeObject<ResponseModel>(respStr);
            if (response is null)
            {
                _api!.SendMessageToAdmins($"葱葱ChatGPT插件发生错误：响应信息解析失败");
                return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "响应信息解析失败"))!;
            }

            if (response.error is null)
            {
                Choice? _choice = response.choices.FirstOrDefault();
                if (_choice is null)
                {
                    _api!.SendMessageToAdmins($"葱葱ChatGPT插件发生错误：ChatGPT返回内容为空");
                    return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", "ChatGPT返回内容为空"))!;
                }
                else
                {
                    //本次响应消息
                    Message assistantMsg = new Message(Role.assistant, _choice.message.content);
                    switch (_config.Context)
                    {
                        case ContextSetting.尽量保持上下文:
                            _chatHistory[qqId].Add(userMsg);
                            _chatHistory[qqId].Add(assistantMsg);
                            break;
                        case ContextSetting.仅保持上一条上下文:
                            _chatHistory[qqId].Clear();
                            _chatHistory[qqId].Add(userMsg);
                            _chatHistory[qqId].Add(assistantMsg);
                            break;
                        case ContextSetting.不保持上下文:
                            _chatHistory[qqId].Clear();
                            break;
                    }
                    return _choice.message.content.TrimStart();  //正常回答
                }
            }
            _api!.SendMessageToAdmins($"葱葱ChatGPT插件发生错误：{response.error.message}");
            return _api!.ReplaceGreenOnionsStringTags(_config.ErrorMessage ?? "", ("<错误信息>", response.error.message))!;
        }

        private async Task<string> PostByPlugin(string strBody)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_config!.APIkey}" }
            };
            return await HttpClientSubstitutes.PostStringAsync!("https://api.openai.com/v1/chat/completions", strBody, headers);
        }

        private async Task<string> PostBySharp(string strBody)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (_config!.UseProxy)
            {
                handler.UseProxy = true;
                handler.Proxy = new WebProxy(_botConfig!.ProxyUrl);
            }
            HttpClient client = new HttpClient(handler);
            using HttpRequestMessage request = new(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            using HttpContent content = new StringContent(strBody, Encoding.UTF8, "application/json");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config!.APIkey);
            HttpResponseMessage resp = await client!.SendAsync(request);
            return await resp.Content.ReadAsStringAsync();
        }

        private void StartTimeoutWorker()
        {
            if (string.IsNullOrWhiteSpace(_config!.TimeOutMessage))
                return;
            _timeOutWorkerTs?.Cancel();
            _timeOutWorkerTs?.Dispose();
            _timeOutWorkerTs = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var item in _chatingUser)
                    {
                        if (DateTime.Now > item.Value.TimeOutAt)
                        {
                            item.Value.TimeOutDo?.Invoke(_config.TimeOutMessage);
                            _chatingUser.TryRemove(item.Key, out _);
                        }
                    }
                    await Task.Delay(1000);
                }
            }, _timeOutWorkerTs.Token);
        }

        public void ReloadConfig()
        {
            if (!File.Exists(_configDirect))
                return;

            string strConfig = File.ReadAllText(_configDirect!);
            _config = JsonConvert.DeserializeObject<ChatGPTClientConfig>(strConfig);
            if (_config is null)
            {
                throw new ArgumentNullException("葱葱ChatGPT聊天插件配置文件不合法，请检查json格式");
            }

            if (_config.StartCommands is not null)
                _startCmds = _config.StartCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            if (_config.ExitCommands is not null)
                _exitCmds = _config.ExitCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            if (_config.ClearContextCommands is not null)
                _clearContextCmds = _config.ClearContextCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
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
            string jsonConfig = JsonConvert.SerializeObject(_config, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(_configDirect!, jsonConfig);
        }
    }
}