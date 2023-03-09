using System.Diagnostics;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.GuessTheSong
{
    public class Questioner : IMessagePlugin, IPluginSetting, IPluginHelp
    {
        private string? _pluginPath;
        private Config? _config;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;
        private Dictionary<long, List<string>> playerAndAnswers = new Dictionary<long, List<string>>();

        public string Name => "听歌猜曲名";

        public string Description => "葱葱听歌猜曲名插件";

        public GreenOnionsMessages? HelpMessage => _config?.HelpMessage ??  "输入 \"<机器人名称>猜歌\" 发起一场听歌猜曲名游戏，每场游戏时长为一分钟，任意群友猜出正确曲名或超时后结束。";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;

            string configFileName = Path.Combine(_pluginPath!, "config.json");
            if (File.Exists(configFileName))
            {
                string strConfig = File.ReadAllText(configFileName);
                try
                {
                    _config = JsonConvert.DeserializeObject<Config>(strConfig);
                }
                catch (Exception ex)
                {
                    SendMessageToAdmin($"葱葱听歌猜曲名插件解析配置文件失败，请检查config.json格式。{ex.Message}");
                    _config = new Config();
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
            }
            File.WriteAllText(configFileName, JsonConvert.SerializeObject(_config, Formatting.Indented));
        }

        public void OnDisconnected()
        {
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
            _botConfig = config;
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            long playerId = senderGroup is null ? msgs.SenderId : senderGroup.Value;
            try
            {
                if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg)
                {
                    if (playerAndAnswers.ContainsKey(playerId)) //已经请求过获取歌曲, 在答题状态中
                    {
                        if (playerAndAnswers[playerId].Any(an => an.StartsWith(msg.Text, StringComparison.OrdinalIgnoreCase)))  //回答了正确的答案
                        {
                            playerAndAnswers.Remove(playerId);
                            Response(_config!.RightAnswerReply);
                        }
                    }
                    else //这个组或这个人不在正在游玩的状态
                    {
                        if (msg.Text == _api!.ReplaceGreenOnionsStringTags(_config!.StartCmd))  //开始请求歌曲
                        {
                            Random rdm = new Random(Guid.NewGuid().GetHashCode());

                            if (_config!.MusicIDAndAnswers is not null && _config!.MusicIDAndAnswers.Count > 0)  //自定义模式
                            {
                                int musicIndex = rdm.Next(0, _config.MusicIDAndAnswers.Count);
                                long musicID = _config.MusicIDAndAnswers.Keys.ToArray()[musicIndex];
                                playerAndAnswers.Add(playerId, _config.MusicIDAndAnswers[musicID].ToList());
                                try
                                {
                                    DownloadMusic(musicID).ContinueWith(originalMusic =>
                                    {
                                        if (originalMusic.Result is null)
                                        {
                                            Response($"剪歌失败，请重试");
                                            SendMessageToAdmin($"葱葱听歌猜曲名插件剪歌失败，歌曲ID为：{musicID}，音频流为空");
                                        }
                                        else
                                        {
                                            SendSongAndReceiveAnswers(originalMusic.Result, _config.MusicIDAndAnswers[musicID].First(), musicID, Response);
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
                                Search(searchKey, playerId);
                            }
                        }
                    }

                    void Search(string searchKey, long playerId)
                    {
                        SearchMusic(searchKey).ContinueWith(async r =>
                        {
                            if (r.Result.MusicId == -1)  //搜索失败
                            {
                                Response("搜索歌曲失败，请联系机器人管理员");
                                SendMessageToAdmin($"葱葱听歌猜曲名插件搜索失败，用户搜索词为：{searchKey}");
                            }
                            double duration = await GetSongDurationSeconds(r.Result.MusicId);
                            if (duration < _config.ClipLengthSecond + 20)  //歌曲总时长低于裁剪片段时长
                            {
                                Search(searchKey, playerId);
                                return;
                            }

                            playerAndAnswers.Add(playerId, r.Result.Answers!);

                            Stream? originalMusic;
                            try
                            {
                                originalMusic = await DownloadMusic(r.Result.MusicId);
                            }
                            catch (Exception ex)
                            {
                                Response($"下载歌曲失败，请联系机器人管理员");
                                playerAndAnswers.Remove(playerId);
                                SendMessageToAdmin($"葱葱听歌猜曲名插件下载歌曲失败，歌曲ID为：{r.Result.MusicId}，错误信息为：{ex.Message}");
                                return;
                            }

                            if (originalMusic is null)
                            {
                                Search(searchKey, playerId);
                                return;
                            }

                            SendSongAndReceiveAnswers(originalMusic, r.Result.Answers!.First(), r.Result.MusicId, Response);
                        });
                    }

                    async void SendSongAndReceiveAnswers(Stream originalMusic, string answer, long musicId, Action<GreenOnionsMessages> Response)
                    {
                        try
                        {
                            MemoryStream? ms = CutMp3(originalMusic);
                            if (ms is null)
                            {
                                Response($"剪歌失败，请重试");
                                playerAndAnswers.Remove(playerId);
                                SendMessageToAdmin($"葱葱听歌猜曲名插件剪歌失败，歌曲ID为：{musicId}，音频流为空");
                                return;
                            }
                            using (ms)
                            {
                                GreenOnionsMessages msgVoice;
                                if (!string.IsNullOrWhiteSpace(_config.FFmpegPath) && File.Exists(_config.FFmpegPath))  //转码成amr
                                {
                                    string mp3FileName = Path.Combine(_pluginPath!, "original.mp3");
                                    File.WriteAllBytes(mp3FileName, ms.GetBuffer());

                                    string amrFileName = Path.Combine(_pluginPath!, "transcoded.amr");
                                    if (File.Exists(amrFileName))
                                        File.Delete(amrFileName);

                                    Process p = new Process();
                                    ProcessStartInfo startInfo = new ProcessStartInfo(_config.FFmpegPath);
                                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    startInfo.Arguments = $" -i {mp3FileName} -c:a libopencore_amrnb -ac 1 -ar 8000 -b:a 320K -y {amrFileName}";
                                    p.StartInfo = startInfo;
                                    p.StartInfo.UseShellExecute = false;
                                    p.StartInfo.CreateNoWindow = true;
                                    p.Start();
                                    p.WaitForExit();
                                    if (_config.SendAsFile)
                                    {
                                        msgVoice = new GreenOnionsVoiceMessage(amrFileName)!;
                                    }
                                    else
                                    {
                                        MemoryStream msSend = new MemoryStream(File.ReadAllBytes(amrFileName));
                                        msgVoice = new GreenOnionsVoiceMessage(msSend)!;
                                    }
                                    File.Delete(mp3FileName);
                                }
                                else  //原样发送mp3
                                {
                                    if (_config.SendAsFile)
                                    {
                                        string mp3FileName = Path.Combine(_pluginPath!, "original.mp3");
                                        File.WriteAllBytes(mp3FileName, ms.ToArray());
                                        msgVoice = new GreenOnionsVoiceMessage(mp3FileName)!;
                                    }
                                    else
                                    {
                                        msgVoice = new GreenOnionsVoiceMessage(ms)!;
                                    }
                                }
                                msgVoice.Reply = false;
                                Response(msgVoice);
                                await Task.Delay(60 * 1000);
                                if (playerAndAnswers.ContainsKey(playerId))
                                {
                                    GreenOnionsMessages msgEnd = new GreenOnionsMessages(_config!.TimeOutReplyReply.Replace("<歌曲名称>", answer));  //游戏结束，公布答案
                                    msgEnd.Reply = false;
                                    Response(msgEnd);
                                    playerAndAnswers.Remove(playerId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Response($"剪歌失败，请重试");
                            playerAndAnswers.Remove(playerId);
                            SendMessageToAdmin($"葱葱听歌猜曲名插件剪歌失败，歌曲ID为：{musicId}，错误信息为：{ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                playerAndAnswers.Remove(playerId);
                Response($"发生错误，{ex.Message}");
                SendMessageToAdmin($"葱葱听歌猜曲名插件错误：{ex.Message}");
            }
            return false;
        }

        private async void SendMessageToAdmin(string msg)
        {
            foreach (long item in _botConfig!.AdminQQ)
            {
                await _api!.SendFriendMessageAsync(item, msg);
            }
        }

        private async Task<double> GetSongDurationSeconds(long songID)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://music.163.com/api/song/detail/?id={songID}&ids=%5B{songID}%5D");
            string strSongInfo = await response.Content.ReadAsStringAsync();
            JToken? songInfo = JsonConvert.DeserializeObject<JToken>(strSongInfo);
            if (songInfo is not null)
            {
                JToken? duration = songInfo["songs"]?[0]?["duration"];
                if (duration is not null)
                {
                    long lDuration = Convert.ToInt64(duration.ToString());
                    TimeSpan span = TimeSpan.FromMilliseconds(lDuration);
                    return span.TotalSeconds;
                }
            }
            return -1;
        }

        private async Task<(long MusicId, List<string>? Answers)> SearchMusic(string musicName)
        {
            using HttpClient httpClient = new HttpClient();
            Random rdmOffset = new Random(Guid.NewGuid().GetHashCode());
            int iOffset = rdmOffset.Next(0, _config!.MaximumSearchRange);
            List<string> answers = new List<string>();
            HttpResponseMessage songListResponse = await httpClient.GetAsync($"http://music.163.com/api/search/get/web?csrf_token=hlpretag=&hlposttag=&s={musicName}&type=1&offset={iOffset}&limit=1");
            string songListStr = await songListResponse.Content.ReadAsStringAsync();
            JToken? jtSongs = JsonConvert.DeserializeObject<JToken>(songListStr)?["result"]?["songs"];
            if (jtSongs is not null && jtSongs.Count() > 0)
            {
                JToken song = jtSongs.First();
                long id = Convert.ToInt64(song!["id"]);
                string name = song!["name"]!.ToString();
                answers.AddRange(GetSoneNameWithOutBracket(name));
                answers.Add(name);
                JToken? transNamesJt = song["album"]?["transNames"];
                if (transNamesJt is not null)
                {
                    string[]? transNames = JsonConvert.DeserializeObject<string[]>(transNamesJt.ToString());  //中文名
                    if (transNames is not null)
                    {
                        answers.AddRange(transNames);
                        answers.AddRange(GetSoneNameWithOutBracket(transNames));
                    }
                }

                return (id, answers);
            }
            return (-1, null);
        }

        private List<string> GetSoneNameWithOutBracket(string songOriginalName)
        {
            List<string> names = new List<string>();
            if (songOriginalName.Contains('（'))
                names.Add(songOriginalName.Substring(0, songOriginalName.IndexOf('（')).TrimEnd());
            if (songOriginalName.Contains('('))
                names.Add(songOriginalName.Substring(0, songOriginalName.IndexOf('(')).TrimEnd());
            return names;
        }

        private List<string> GetSoneNameWithOutBracket(string[] songOriginalNames)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < songOriginalNames.Length; i++)
                names.AddRange(GetSoneNameWithOutBracket(songOriginalNames[i]));
            return names;
        }

        private async Task<Stream?> DownloadMusic(long musicID)
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage mp3Response = await httpClient.GetAsync($"http://music.163.com/song/media/outer/url?id={musicID}.mp3");
            if (mp3Response.RequestMessage?.RequestUri?.ToString().Contains("music.163.com/404") == true)
                return null;
            return await mp3Response.Content.ReadAsStreamAsync();
        }

        public MemoryStream? CutMp3(Stream inputStream)
        {
            using Mp3FileReader reader = new Mp3FileReader(inputStream);
            if (reader.TotalTime.TotalSeconds < 20 + _config!.ClipLengthSecond)  //至少要有前后10秒+裁剪长度的时长
                return null;

            Random rdm = new Random();
            int startSecond = rdm.Next(11, (int)reader.TotalTime.TotalSeconds - 10 - _config!.ClipLengthSecond);  //跳过前后10秒一定程度上避免裁剪到无声片段

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
                    outputStream.Write(frame.RawData, 0, frame.RawData.Length);
                }
            } while (frame is not null);
            return outputStream;
        }

        public void Setting()
        {
            throw new Exception("请进入插件目录修改config.json配置文件");
        }
    }
}