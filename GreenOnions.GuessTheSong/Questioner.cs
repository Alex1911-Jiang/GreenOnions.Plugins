using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.GuessTheSong
{
    public class Questioner : IPlugin
    {
        private Config? _config;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;
        private CancellationTokenSource? _ts = null;
        private List<string>? _answers = new List<string>();

        public string Name => "听歌猜曲名";

        public string Description => "葱葱听歌猜曲名插件";

        public GreenOnionsMessages? HelpMessage => "输入 \"<机器人名称>猜歌\" 发起一场听歌猜曲名游戏，每场游戏时长为一分钟，任意群友猜出正确曲名或超时后结束。";

        public bool DisplayedInTheHelp => true;

        public void ConsoleSetting()
        {
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
        }

        public void OnDisconnected()
        {
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            string configFileName = Path.Combine(pluginPath, "config.json");
            if (File.Exists(configFileName))
            {
                string strConfig = File.ReadAllText(configFileName);
                try
                {
                    _config = JsonConvert.DeserializeObject<Config>(strConfig);
                }
                catch (Exception ex)
                {
                    _config = new Config();
                    File.AppendAllText(Path.Combine(pluginPath, "error.log"), $"\r\n解析配置文件失败，请检查config.json格式  --------{DateTime.Now}\r\n{ex.Message}\r\n");
                }

                if (_config!.MusicIDAndAnswers.Count == 0 && _config.SearchKeywords.Count == 0)
                {
                    SendMessageToAdmin("葱葱听歌猜曲名插件配置错误：<搜索关键词>或<自定义歌曲ID和答案>至少要存在一项");
                }
            }
            else
            {
                _config = new Config();
                _config.SearchKeywords.Add("初音ミク");
                _config.MusicIDAndAnswers.Add(29023577, new[] { "深海少女" });
                _config.MusicIDAndAnswers.Add(26096272, new[] { "千本樱", "千本桜", "千本櫻" });
                _config.MusicIDAndAnswers.Add(22709632, new[] { "世界第一公主殿下", "ワールドイズマイン", "世界で一番おひめさま", "世界第一的公主殿下", "World Is Mine" });
                _config.MusicIDAndAnswers.Add(4888328, new[] { "炉心融解" });
                string strConfig = JsonConvert.SerializeObject(_config);
                File.WriteAllText(configFileName, strConfig);
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg)
            {
                if (_ts == null)  //开始请求歌曲
                {
                    if (msg.Text == _api!.ReplaceGreenOnionsStringTags("<机器人名称>猜歌"))
                    {
                        Random rdm = new Random(Guid.NewGuid().GetHashCode());
                        _ts = new CancellationTokenSource();
                        try
                        {
                            if (_config!.MusicIDAndAnswers.Count > 0)  //自定义模式
                            {
                                int musicIndex = rdm.Next(0, _config.MusicIDAndAnswers.Count);
                                long musicID = _config.MusicIDAndAnswers.Keys.ToArray()[musicIndex];
                                _answers = _config.MusicIDAndAnswers[musicID].ToList();
                                try
                                {
                                    DownloadMusic(musicID).ContinueWith(originalMusic =>
                                    {
                                        try
                                        {
                                            SendSongAndReceiveAnswers(originalMusic.Result);
                                        }
                                        catch (Exception ex)
                                        {
                                            Response($"剪歌失败，请重试");
                                            SendMessageToAdmin($"葱葱听歌猜曲名插件剪歌失败，歌曲ID为：{musicID}，错误信息为：{ex.Message}");
                                        }
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Response($"下载歌曲失败，请联系机器人管理员");
                                    SendMessageToAdmin($"葱葱听歌猜曲名插件下载歌曲失败，歌曲ID为：{musicID}，错误信息为：{ex.Message}");
                                    throw;
                                }
                            }
                            else  //搜索模式
                            {
                                int keywordIndex = rdm.Next(0, _config.SearchKeywords.Count);
                                string searchKey = _config.SearchKeywords.ToArray()[keywordIndex];
                                Search(searchKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            Response($"发生错误，{ex.Message}");
                            _ts?.Cancel();
                            _ts = null;
                            _answers?.Clear();
                            _answers = null;
                        }
                    }
                    return true;
                }
                else  //已经请求过获取歌曲
                {
                    if (_answers != null)
                    {
                        if (_answers.Contains(msg.Text))  //回答了正确的答案
                        {
                            _ts?.Cancel();
                            _ts = null;
                            _answers?.Clear();
                            _answers = null;
                            Response(_config!.RightAnswerReply);
                        }
                    }
                }

                void Search(string searchKey)
                {
                    SearchMusic(searchKey).ContinueWith(async r =>
                    {
                        if (r.Result == -1)  //搜索失败
                        {
                            _ts?.Cancel();
                            _ts = null;
                            _answers?.Clear();
                            _answers = null;
                            Response("搜索歌曲失败，请联系机器人管理员");
                            SendMessageToAdmin($"葱葱听歌猜曲名插件搜索失败，用户搜索词为：{searchKey}");
                        }
                        double duration = await GetSongDurationSeconds(r.Result);
                        if (duration < _config.ClipLengthSecond * 2 + 2)  //歌曲总时长低于裁剪片段时长
                        {
                            Search(searchKey);
                            return;
                        }

                        Stream originalMusic;
                        try
                        {
                            originalMusic = await DownloadMusic(r.Result);
                        }
                        catch (Exception ex)
                        {
                            Response($"下载歌曲失败，请联系机器人管理员");
                            SendMessageToAdmin($"葱葱听歌猜曲名插件下载歌曲失败，歌曲ID为：{r.Result}，错误信息为：{ex.Message}");
                            return;
                        }
                        try
                        {
                            SendSongAndReceiveAnswers(originalMusic);
                        }
                        catch (Exception ex)
                        {
                            Response($"剪歌失败，请重试");
                            SendMessageToAdmin($"葱葱听歌猜曲名插件剪歌失败，歌曲ID为：{r.Result}，错误信息为：{ex.Message}");
                        }
                    });
                }

                async void SendSongAndReceiveAnswers(Stream originalMusic)
                {
                    using MemoryStream? ms = await CutMp3(originalMusic);
                    if (ms != null)
                    {
                        GreenOnionsMessages msgVoice = new GreenOnionsVoiceMessage(ms);
                        msgVoice.Reply = false;
                        Response(msgVoice);
                        try
                        {
                            await Task.Delay(60 * 1000, _ts!.Token);
                        }
                        catch (TaskCanceledException)
                        {
                        }
                        if (_ts != null)
                        {
                            if (!_ts.IsCancellationRequested)
                            {
                                GreenOnionsMessages msgEnd = new GreenOnionsMessages(_config!.TimeOutReplyReply);
                                msgEnd.Reply = false;
                                Response(msgEnd);
                            }
                            _ts.Dispose();
                        }
                        _ts = null;
                    }
                }
            }
            return false;
        }

        private void SendMessageToAdmin(string msg)
        {
            foreach (long item in _botConfig!.AdminQQ)
            {
                _api!.SendFriendMessageAsync(item, msg);
            }
        }

        private async Task<double> GetSongDurationSeconds(long songID)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://music.163.com/api/song/detail/?id={songID}&ids=%5B{songID}%5D");
            string strSongInfo = await response.Content.ReadAsStringAsync();
            JToken? songInfo = JsonConvert.DeserializeObject<JToken>(strSongInfo);
            if (songInfo != null)
            {
                JToken? duration = songInfo["songs"]?[0]?["duration"];
                if (duration != null)
                {
                    long lDuration = Convert.ToInt64(duration.ToString());
                    TimeSpan span = TimeSpan.FromMilliseconds(lDuration);
                    return span.TotalSeconds;
                }
            }
            return -1;
        }

        private async Task<long> SearchMusic(string musicName)
        {
            using HttpClient httpClient = new HttpClient();
            Random rdmOffset = new Random(Guid.NewGuid().GetHashCode());
            int iOffset = rdmOffset.Next(0, 1500);
            List<string> answers = new List<string>();
            HttpResponseMessage songListResponse = await httpClient.GetAsync($"http://music.163.com/api/search/get/web?csrf_token=hlpretag=&hlposttag=&s={musicName}&type=1&offset={iOffset}&total=true&limit=100");
            string songListStr = await songListResponse.Content.ReadAsStringAsync();
            JToken? jtSongs = JsonConvert.DeserializeObject<JToken>(songListStr)?["result"]?["songs"];
            if (jtSongs != null)
            {
                int songCount = jtSongs.Count();
                if (songCount > 0)
                {
                    Random rdmSongIndex = new Random(Guid.NewGuid().GetHashCode());
                    int iSongIndex = rdmSongIndex.Next(0, songCount);
                    JToken song = jtSongs[iSongIndex]!;
                    long id = Convert.ToInt64(song!["id"]);
                    string name = song!["name"]!.ToString();
                    answers.Add(name);
                    JToken? transNamesJt = song["album"]?["transNames"];
                    if (transNamesJt != null)
                    {
                        string[]? transNames = JsonConvert.DeserializeObject<string[]>(transNamesJt.ToString());  //中文名
                        if (transNames != null)
                            answers.AddRange(transNames);
                    }
                    _answers = answers;
                    return id;
                }
            }
            return -1;
        }


        private async Task<Stream> DownloadMusic(long musicID)
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage mp3Response = await httpClient.GetAsync($"http://music.163.com/song/media/outer/url?id={musicID}.mp3");
            return await mp3Response.Content.ReadAsStreamAsync();
        }

        public async Task<MemoryStream?> CutMp3(Stream inputStream)
        {
            using Mp3FileReader reader = new Mp3FileReader(inputStream);
            if (reader.TotalTime.TotalSeconds < 12)
                return null;

            Random rdm = new Random();
            int startSecond = rdm.Next(6, (int)reader.TotalTime.TotalSeconds - 5 - _config!.ClipLengthSecond);

            var startTime = TimeSpan.FromSeconds(startSecond);
            var endTime = TimeSpan.FromSeconds(startSecond + _config!.ClipLengthSecond);

            MemoryStream outputStream = new MemoryStream();
            Mp3Frame frame;
            do
            {
                frame = reader.ReadNextFrame();
                if (reader.CurrentTime >= startTime)
                {
                    if (reader.CurrentTime > endTime)
                        break;
                    await outputStream.WriteAsync(frame.RawData, 0, frame.RawData.Length);
                }
            } while (frame != null);
            return outputStream;
        }

        public bool WindowSetting()
        {
            return false;
        }
    }
}