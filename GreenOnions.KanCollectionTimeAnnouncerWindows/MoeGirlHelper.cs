using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    internal class MoeGirlHelper
    {
        private string _cachePath;
        private CancellationToken _token;
        private IGreenOnionsApi _api;
        private IBotConfig _botConfig;

        internal MoeGirlHelper(string cachePath, IGreenOnionsApi api, IBotConfig botConfig, CancellationToken token)
        {
            _cachePath = cachePath;
            _api = api;
            _botConfig = botConfig;
            _token = token;
        }

        private async void SendMessageToAdmin(string msg)
        {
            foreach (long item in _botConfig!.AdminQQ)
            {
                await _api!.SendFriendMessageAsync(item, msg);
            }
        }

        /// <summary>
        /// 请求萌娘百科获取舰娘名称列表
        /// </summary>
        /// <returns></returns>
        internal async Task<List<string>?> GetKanGrilNameListAsync(bool reget = false)
        {
            string kanGirlListFileName = Path.Combine(_cachePath, "KanGirlList.json");
            if (File.Exists(kanGirlListFileName))
            {
                if (reget)
                {
                    File.Delete(kanGirlListFileName);
                }
                else if (new FileInfo(kanGirlListFileName).Length > 0)
                {
                    string jsonCache = File.ReadAllText(kanGirlListFileName);
                    if (!string.IsNullOrEmpty(jsonCache))
                    {
                        List<string>? kanGirlList = JsonConvert.DeserializeObject<List<string>>(jsonCache);
                        if (kanGirlList != null && kanGirlList.Count > 0)
                        {
                            return kanGirlList;
                        }
                    }
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
                string html;
                try
                {
                    HttpResponseMessage response = await client.GetAsync(@"https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘", _token);
                    html = await response.Content.ReadAsStringAsync(_token) + "</body></html>";
                }
                catch (OperationCanceledException)
                {
                    return new List<string>();
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //按图鉴编号排列的Table
                var tables = doc.DocumentNode.SelectNodes(@"/html/body/template[@id='MOE_SKIN_TEMPLATE_BODYCONTENT']/div[@id='mw-content-text']/div[@class='mw-parser-output']/table[@class='wikitable']");

                if (tables == null)
                {
                    File.WriteAllText(Path.Combine(_cachePath, "滑动验证html检查.html"), html);
                    return null;  //滑动验证
                }

                HashSet<string> collectionNames = new HashSet<string>();
                foreach (HtmlNode table in tables)
                {
                    foreach (HtmlNode tr in table.SelectNodes("tbody/tr"))
                    {
                        foreach (string title in tr.SelectNodes("td/a").Select(a => a.Attributes["title"].Value))
                        {
                            if (title.Contains("舰队Collection:"))  //排除联动的舰娘
                                collectionNames.Add(title.Substring("舰队Collection:".Length));
                        }
                    }
                }

                List<string> kanGirlList = collectionNames.ToList();
                string jsonCache = JsonConvert.SerializeObject(kanGirlList, Formatting.Indented);
                File.WriteAllText(kanGirlListFileName, jsonCache);
                return kanGirlList;
            }
        }

        internal void CoverSaveKanGirlList(List<string> kanGirlList)
        {
            string kanGirlListFileName = Path.Combine(_cachePath, "KanGirlList.json");
            string jsonCache = JsonConvert.SerializeObject(kanGirlList, Formatting.Indented);
            File.WriteAllText(kanGirlListFileName, jsonCache);
        }

        /// <summary>
        /// 获取下个小时的音频对象
        /// </summary>
        /// <param name="kanGirlName">舰娘名称</param>
        /// <param name="hour">小时数</param>
        /// <returns></returns>
        internal async Task<KanGirlVoiceItem?> GetNextHourVoiceUrlAsync(string kanGirlName, int hour)
        {
            string mp3Path = Path.Combine(_cachePath, "MP3", kanGirlName);
            if (!Directory.Exists(mp3Path))
                Directory.CreateDirectory(mp3Path);

            string mp3FileName = Path.Combine(mp3Path, $"{hour}.mp3");
            string jpnFileName = Path.Combine(mp3Path, $"{hour}_Japanese.txt");
            string chsFileName = Path.Combine(mp3Path, $"{hour}_Chinese.txt");
            if (File.Exists(mp3FileName) && new FileInfo(mp3FileName).Length > 0 && File.Exists(chsFileName) && new FileInfo(chsFileName).Length > 0 && File.Exists(jpnFileName) && new FileInfo(jpnFileName).Length > 0)
            {
                return new KanGirlVoiceItem(mp3FileName, File.ReadAllText(jpnFileName), File.ReadAllText(chsFileName));
            }

            using (HttpClient client = new HttpClient())
            {
                string html;
                try
                {
                    HttpResponseMessage response = await client.GetAsync($@"https://zh.moegirl.org.cn/舰队Collection:{kanGirlName}", _token);
                    html = await response.Content.ReadAsStringAsync(_token) + "</body></html>";
                }
                catch (OperationCanceledException)
                {
                    return null;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var trs = doc.DocumentNode.SelectNodes(@"/html/body/template[@id='MOE_SKIN_TEMPLATE_BODYCONTENT']/div[@id='mw-content-text']/div[@class='mw-parser-output']/table[@class='wikitable'][2]/tbody/tr");
                if (trs == null)
                {
                    var scrs = doc.DocumentNode.SelectNodes("script").Where(scr => scr.Attributes["src"] != null && scr.Attributes["src"].Value.Contains("ssl.captcha")).Count() > 0;
                    //滑动验证
                    return null;
                }

                Dictionary<int, KanGirlVoiceItem> timeToPley = new Dictionary<int, KanGirlVoiceItem>();
                bool findedTimeTd = false;
                for (int i = 0; i < trs.Count; i++)
                {
                    var tds = trs[i].SelectNodes("td");
                    if (tds is not null && tds.Count > 0)
                    {
                        if (findedTimeTd)
                        {
                            var timeAndVoice = GetTimeAndVoiceUrl(tds[0], tds[1]);
                            if (timeAndVoice == null)
                                break;
                            if (timeToPley.ContainsKey(timeAndVoice.Value.time))
                                timeToPley[timeAndVoice.Value.time] = timeAndVoice.Value.voiceItem;
                            else
                                timeToPley.Add(timeAndVoice.Value.time, timeAndVoice.Value.voiceItem);
                            await SaveMp3ChacheFile(timeAndVoice.Value.voiceItem, kanGirlName, hour);
                        }
                        else
                        {
                            if (tds[0].InnerText.Contains("报时")) //从包含"报时"的格子开始往下找
                            {
                                findedTimeTd = true;
                                var timeAndVoice = GetTimeAndVoiceUrl(tds[1], tds[2]);
                                if (timeAndVoice != null)
                                {
                                    if (timeToPley.ContainsKey(timeAndVoice.Value.time))
                                        timeToPley[timeAndVoice.Value.time] = timeAndVoice.Value.voiceItem;
                                    else
                                        timeToPley.Add(timeAndVoice.Value.time, timeAndVoice.Value.voiceItem);
                                    await SaveMp3ChacheFile(timeAndVoice.Value.voiceItem, kanGirlName, hour);
                                }
                            }
                        }
                    }
                }
                if (!timeToPley.ContainsKey(hour))
                    return null;
                return timeToPley[hour];
            }
        }

        private async Task SaveMp3ChacheFile(KanGirlVoiceItem item, string kanGirlName, int hour)
        {
            try
            {
                string mp3Path = Path.Combine(_cachePath, "MP3", kanGirlName);
                if (!Directory.Exists(mp3Path))
                    Directory.CreateDirectory(mp3Path);
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var res = await client.GetAsync(item.Mp3UrlOrFileName, _token);
                        byte[] mp3Bytes = await res.Content.ReadAsByteArrayAsync(_token);
                        await File.WriteAllBytesAsync(Path.Combine(mp3Path, $"{hour}.mp3"), mp3Bytes, _token);
                        await File.WriteAllTextAsync(Path.Combine(mp3Path, $"{hour}_Japanese.txt"), item.JapaneseText, _token);
                        await File.WriteAllTextAsync(Path.Combine(mp3Path, $"{hour}_Chinese.txt"), item.ChineseText, _token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            }
            catch (Exception ex)
            {
                SendMessageToAdmin($"葱葱舰C报时插件错误：下载音频到本地失败。{ex}");
            }
        }

        /// <summary>
        /// 解析html为时间和音频对象
        /// </summary>
        /// <returns></returns>
        private (int time, KanGirlVoiceItem voiceItem)? GetTimeAndVoiceUrl(HtmlNode font, HtmlNode back)
        {
            int indexTimeEnd = font.InnerText.IndexOf("：");
            if (indexTimeEnd == -1)
                return null;
            int time = Convert.ToInt32(font.InnerText[0..2]);
            string strTime = font.InnerText[0..5];
            var data = back.SelectSingleNode("div[2]").Attributes["data-bind"].Value.Replace("&quot;", "\"");
            var jData = JsonConvert.DeserializeObject<JToken>(data);
            string? voiceUrl = jData?["component"]?["params"]?["playlist"]?[0]?["audioFileUrl"]?.ToString();
            if (voiceUrl == null)
                return null;
            string br = "<br>";
            int indexBr = font.InnerHtml.IndexOf(br);
            string japaneseText = font.InnerHtml.Substring(5, indexBr - 5).Replace("<span lang=\"ja\">", "").Replace("</span>", "");
            string chineseText = font.InnerHtml.Substring(indexBr + br.Length).Replace(strTime, "").Replace("\n", "");
            KanGirlVoiceItem voiceItem = new KanGirlVoiceItem(voiceUrl, japaneseText, chineseText);
            return (time, voiceItem);
        }
    }
}
