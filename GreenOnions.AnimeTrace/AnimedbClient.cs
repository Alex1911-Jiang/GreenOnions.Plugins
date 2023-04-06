using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace GreenOnions.AnimeTrace
{
    public class AnimedbClient : IMessagePlugin
    {
        private string? _pluginPath;
        private AnimeTraceConfig? _config;
        private IGreenOnionsApi? _api;
        private Dictionary<long, models> _searingUser = new Dictionary<long, models>();
        private string? _startSearchCommand;
        private string? _startSearchInAnimeCommand;
        private string? _startSearchInGameCommand;

        public string Name => "搜动漫角色";

        public string Description => "以图片搜索动漫角色信息插件(animedb)";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            ReloadConfig();
        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages?> Response)
        {
            var firstMsg = msgs.First();
            if (_searingUser.ContainsKey(msgs.SenderId) && msgs.All(m => m is GreenOnionsImageMessage))
            {
                Search(msgs, senderGroup, Response, _searingUser[msgs.SenderId]);  //在搜索模式中
            }
            if (firstMsg is not GreenOnionsTextMessage txtMsg)
            {
                return false;
            }
            string cmd = txtMsg.Text.TrimEnd('\n');
            if (cmd != _startSearchCommand &&
                cmd != _startSearchInAnimeCommand &&
                cmd != _startSearchInGameCommand)
            {
                return false;
            }

            models model = models.pre_stable;
            if (cmd == _startSearchInAnimeCommand)
                model = models.anime;
            else if (cmd == _startSearchInGameCommand)
                model = models.game;

            if (msgs.Count > 1)
            {
                Search(msgs, senderGroup, Response, model);  //一条消息中包含了命令和图片，直接发起搜索
            }
            else
            {
                _searingUser.Add(msgs.SenderId, model);
                Response(_config.SearchStartReply);  //进入搜索模式
            }
            return true;
        }

        private void Search(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response, models model)
        {
            _searingUser.Remove(msgs.SenderId);
            foreach (var imgMsg in msgs.OfType<GreenOnionsImageMessage>())
            {
                try
                {
                    SearchOneImage(imgMsg, msgs.SenderId, senderGroup, model);
                }
                catch (Exception ex)
                {
                    Response(_api!.ReplaceGreenOnionsStringTags(_config!.SearchErrortReply, ("<错误信息>", ex.Message))!);
                }
            }
        }

        private async void SearchOneImage(GreenOnionsImageMessage imgMsg, long targetId, long? targetGroup, models model)
        {
            using MultipartFormDataContent formData = new ($"--------------------------{DateTime.Now.Ticks}");
            using HttpClient client = new();
            Stream imgStream = await client.GetStreamAsync(imgMsg.Url);
            StreamContent streamContent = new (imgStream);
            streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"image\"; filename=\"img.png\"");
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/png");
            formData.Add(streamContent);
            var resp = await client.PostAsync($"https://aiapiv2.animedb.cn/ai/api/detect?model={model}&force_one={_config!.force_one}", formData);
            string responseContent = await resp.Content.ReadAsStringAsync();
            AnimedbResult? result = JsonSerializer.Deserialize<AnimedbResult>(responseContent);
            if (result is null)
            {
                throw new Exception("返回结果解析失败");
            }
            GreenOnionsMessages outMsg = new GreenOnionsMessages();
            if (_config.force_one == 1)  //多个结果
            {
                for (int i = 0; i < result.data[0]!.@char!.Length; i++)
                {
                    outMsg.Add($"第{i + 1}个结果：\r\n");
                    outMsg.Add($"角色：{result.data[0].@char[i].name}\r\n");
                    outMsg.Add($"来自作品：{result.data[0].@char[i].cartoonname}\r\n");
                    outMsg.Add($"\r\n");
                }
            }
            else
            {
                outMsg.Add($"角色：{result.data[0].name}\r\n");
                outMsg.Add($"来自作品：{result.data[0].cartoonname}\r\n");
            }
            if (targetGroup is null)
                await _api!.SendFriendMessageAsync(targetId, outMsg);
            else
                await _api!.SendGroupMessageAsync(targetGroup.Value, outMsg);
        }

        public void ReloadConfig()
        {
            JsonSerializerOptions ops = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, };
            string configDirect = Path.Combine(_pluginPath!, "config.json");
            if (!File.Exists(configDirect))
            {
                _config = new AnimeTraceConfig();
                File.WriteAllText(configDirect, JsonSerializer.Serialize(_config, ops));
            }
            else
            {
                string json = File.ReadAllText(configDirect);
                _config = JsonSerializer.Deserialize<AnimeTraceConfig>(json, ops);
            }
            _startSearchCommand = _api?.ReplaceGreenOnionsStringTags(_config!.StartSearchCommand);
            _startSearchInAnimeCommand = _api?.ReplaceGreenOnionsStringTags(_config!.StartSearchInAnimeCommand);
            _startSearchInGameCommand= _api?.ReplaceGreenOnionsStringTags(_config!.StartSearchInGameCommand);
        }
    }
}