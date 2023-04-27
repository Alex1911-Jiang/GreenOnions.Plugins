using System.Collections;
using System.Net.Http.Json;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.ChatGLMClient
{
    public class WebUIClient : IMessagePlugin
    {
        private string? _path;
        private string? _configDirect;
        private IBotConfig? _botConfig;
        private bool _chating = false;
        private long _chatingQQId = 0;
        private ChatGLMClientConfig? _config;
        private IGreenOnionsApi? _api;
        private IEnumerable<string>? _startCmds;
        private IEnumerable<string>? _exitCmds;
        private IEnumerable<string>? _clearContextCmds;
        private DateTime _timeout = DateTime.MaxValue;

        public string Name => "ChatGLM";

        public string Description => "ChatGLM本地AI聊天插件";

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

        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _path = pluginPath;
            _botConfig = config;
            _configDirect = Path.Combine(_path!, "config.json");
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages?> Response)
        {
            if (msgs.First() is not GreenOnionsTextMessage txm)
            {
                return false;
            }
            bool hitStartCmd = _startCmds!.Contains(txm.Text);
            bool hitClearCmd = _clearContextCmds!.Contains( txm.Text);
            bool hitExitCmd = _exitCmds!.Contains(txm.Text);

            if (_chating && _chatingQQId == msgs.Id && hitClearCmd)  //清除上下文
            {
                Response(_api!.ReplaceGreenOnionsStringTags(_config!.CleanContextMessage));
            }
            if (_chating && _chatingQQId == msgs.Id && hitExitCmd)  //退出聊天
            {
                _chatingQQId = 0;
                _chating = false;
                Response(_api!.ReplaceGreenOnionsStringTags(_config!.ExitChatMessage));
            }
            if (_chating && _chatingQQId == msgs.Id && !hitStartCmd && !hitExitCmd)  //聊天
            {
                using HttpClient client = new HttpClient();
                var json = new
                {
                    fn_index = 0,
                    data = new ArrayList()
                    {
                        txm.Text,
                        _config!.MaxLength,
                        _config.TopP,
                        _config.Temperature,
                    }
                };
                string url = _config.Address.EndsWith('/') ? $"{_config.Address}api/predict" : $"{_config.Address}/api/predict";
                client.PostAsJsonAsync(url, new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json")).ContinueWith(async callback =>
                {
                    JToken? jt = JsonConvert.DeserializeObject<JToken>(await callback.Result.Content.ReadAsStringAsync());
                    if (jt is null)
                    {
                        Response(_api!.ReplaceGreenOnionsStringTags(_config!.ErrorMessage, ("<错误信息>", $"AI模型超时未返回")));
                        return;
                    }
                    if (jt["error"] is not null)
                    {
                        Response(_api!.ReplaceGreenOnionsStringTags(_config!.ErrorMessage, ("<错误信息>", jt["error"]!.ToString())));
                        return;
                    }
                    if (jt["data"] is not JArray ja)
                    {
                        Response(_api!.ReplaceGreenOnionsStringTags(_config!.ErrorMessage, ("<错误信息>", "AI模型返回内容为空")));
                        return;
                    }
                    Response(ja.First().Last().Last().ToString());  //回复内容
                });
            }

            if (_chating && _chatingQQId != msgs.Id && hitStartCmd)
            {
                Response(_config!.AnotherUsingMessage);  //其他人正在用
                return true;
            }

            if (!_chating && hitStartCmd)  //进入聊天
            {
                _chatingQQId = msgs.SenderId;
                _chating = true;
                Response(_api!.ReplaceGreenOnionsStringTags(_config!.ChatStartMessage));
                _timeout = DateTime.Now.AddSeconds(_config!.TimeOutSecond);

                Task.Run(async () =>
                {
                    while (DateTime.Now < _timeout)
                    {
                        _timeout = DateTime.MaxValue;
                        _chating = false;
                        _chatingQQId = 0;
                        if (!string.IsNullOrEmpty(_config!.TimeOutMessage))
                            Response(_api!.ReplaceGreenOnionsStringTags(_config.TimeOutMessage));
                        await Task.Delay(1000);
                    }
                });
                return true;
            }
            return false;
        }

        public void ReloadConfig()
        {
            if (!File.Exists(_configDirect))
                return;

            string strConfig = File.ReadAllText(_configDirect!);
            _config = JsonConvert.DeserializeObject<ChatGLMClientConfig>(strConfig);
            if (_config is null)
                throw new ArgumentNullException("葱葱ChatGLM聊天插件配置文件不合法，请检查json格式");

            if (_config.StartCommands is not null)
                _startCmds = _config.StartCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            if (_config.ExitCommands is not null)
                _exitCmds = _config.ExitCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
            if (_config.ClearContextCommands is not null)
                _clearContextCmds = _config.ClearContextCommands.Select(s => _api!.ReplaceGreenOnionsStringTags(s)!);
        }
    }
}