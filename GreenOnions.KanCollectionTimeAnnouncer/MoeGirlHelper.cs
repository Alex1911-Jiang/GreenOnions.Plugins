using System.Runtime;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.PluginConfigs.KanCollectionTimeAnnouncer;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace GreenOnions.KanCollectionTimeAnnouncer
{
    internal class MoeGirlHelper
    {
        private string _pluginPath;
        private CancellationToken _token;
        private IGreenOnionsApi _api;
        private IBotConfig _botConfig;

        internal MoeGirlHelper(string pluginPath, IGreenOnionsApi api, IBotConfig botConfig, CancellationToken token)
        {
            _pluginPath = pluginPath;
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
        internal async Task<List<string>?> GetKanGrilNameListAsync()
        {
            List<string>? kanGirlList;
            string jsonCache;

            string kanGirlListFileName = Path.Combine(_pluginPath, "KanGirlList.json");
            if (!File.Exists(kanGirlListFileName) || new FileInfo(kanGirlListFileName).Length == 0)
                goto IL_GetMoeGirl;

            jsonCache = File.ReadAllText(kanGirlListFileName);
            if (string.IsNullOrEmpty(jsonCache))
                goto IL_GetMoeGirl;

            kanGirlList = JsonConvert.DeserializeObject<List<string>>(jsonCache);
            if (kanGirlList is null || kanGirlList.Count == 0)
                goto IL_GetMoeGirl;

            return kanGirlList;

        IL_GetMoeGirl:;
            using HttpClient client = new();
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
                File.WriteAllText(Path.Combine(_pluginPath, "获取舰娘失败.html"), html);
                SendMessageToAdmin("葱葱舰C报时插件获取舰娘列表失败，可能需要滑动验证，请手动打开 https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘 ，如果无需滑动验证，请打开插件目录下\"获取舰娘失败.html\"查看错误信息");
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

            kanGirlList = collectionNames.ToList();
            jsonCache = JsonConvert.SerializeObject(kanGirlList, Formatting.Indented);
            File.WriteAllText(kanGirlListFileName, jsonCache);
            return kanGirlList;
        }

        internal void CoverSaveKanGirlList(List<string> kanGirlList)
        {
            string kanGirlListFileName = Path.Combine(_pluginPath, "KanGirlList.json");
            string jsonCache = JsonConvert.SerializeObject(kanGirlList, Formatting.Indented);
            File.WriteAllText(kanGirlListFileName, jsonCache);
        }

        internal async Task<KanGirlVoiceItem?> GetNextHourVoiceUrlAsync(KanCollectionConfig config, int hour)
        {
            KanGirlVoiceItem? item;
            if (config.DesignateKanGirl)  //指定舰娘
            {
                item = await GetNextHourVoiceUrlInnerAsync(config.DesignatedKanGirl!, hour);
                if (item is null)
                {
                    SendMessageToAdmin($"葱葱舰C报时插件错误：获取音频失败，所选舰娘没有报时语音或地址需要人机验证，请重新选择和检查人机验证( https://zh.moegirl.org.cn/舰队Collection )后重连葱葱。");
                    return null;
                }
            }
            else  //随机舰娘
            {
                //获取舰娘列表
                List<string>? kanGirlsName = await GetKanGrilNameListAsync();
                if (kanGirlsName is null)  //获取失败, 需要人机验证
                {
                    SendMessageToAdmin("葱葱舰C报时插件错误：获取舰娘列表失败，需要人机验证，请手动打开 https://zh.moegirl.org.cn/舰队Collection 通过验证并重启葱葱。");
                    return null;
                }
            IL_Retry:;
                Random rdm = new Random(Guid.NewGuid().GetHashCode());
                int randomIndex = rdm.Next(0, kanGirlsName.Count);
                string kanGirlName = kanGirlsName[randomIndex];
                item = await GetNextHourVoiceUrlInnerAsync(kanGirlName, hour);
                if (item is null)
                {
                    kanGirlsName.Remove(kanGirlName);
                    CoverSaveKanGirlList(kanGirlsName);
                    goto IL_Retry;  //目标舰娘没有报时语音, 重新随机一个
                }
            }
            return item;
        }

        /// <summary>
        /// 获取下个小时的音频对象
        /// </summary>
        /// <param name="kanGirlName">舰娘名称</param>
        /// <param name="hour">小时数</param>
        /// <returns></returns>
        private async Task<KanGirlVoiceItem?> GetNextHourVoiceUrlInnerAsync(string kanGirlName, int hour)
        {
            string mp3Path = Path.Combine(_pluginPath, "MP3", kanGirlName);
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
                    var scrs = doc.DocumentNode.SelectNodes("script").Where(scr => scr.Attributes["src"] is not null && scr.Attributes["src"].Value.Contains("ssl.captcha")).Count() > 0;
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
                                if (timeAndVoice is null)
                                    return null;

                                if (timeToPley.ContainsKey(timeAndVoice.Value.time))
                                    timeToPley[timeAndVoice.Value.time] = timeAndVoice.Value.voiceItem;
                                else
                                    timeToPley.Add(timeAndVoice.Value.time, timeAndVoice.Value.voiceItem);
                                await SaveMp3ChacheFile(timeAndVoice.Value.voiceItem, kanGirlName, hour);
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
                string mp3Path = Path.Combine(_pluginPath, "MP3", kanGirlName);
                if (!Directory.Exists(mp3Path))
                    Directory.CreateDirectory(mp3Path);
                using (HttpClient client = new HttpClient())
                {
                    var res = await client.GetAsync(item.Mp3UrlOrFileName, _token);
                    byte[] mp3Bytes = await res.Content.ReadAsByteArrayAsync(_token);
                    await File.WriteAllBytesAsync(Path.Combine(mp3Path, $"{hour}.mp3"), mp3Bytes, _token);
                    await File.WriteAllTextAsync(Path.Combine(mp3Path, $"{hour}_Japanese.txt"), item.JapaneseText, _token);
                    await File.WriteAllTextAsync(Path.Combine(mp3Path, $"{hour}_Chinese.txt"), item.ChineseText, _token);
                }
            }
            catch (Exception ex)
            {
                SendMessageToAdmin($"葱葱舰C报时插件错误：下载音频到本地失败。{ex}\r\n 舰娘名为：{kanGirlName}，报时时间为：{hour}，音频地址为：{item.Mp3UrlOrFileName}");
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
            HtmlNode? voiceNode = back.SelectSingleNode("div[2]");
            if (voiceNode is null)
                return null;
            var data = voiceNode.Attributes["data-bind"].Value.Replace("&quot;", "\"");
            var jData = JsonConvert.DeserializeObject<JToken>(data);
            string? voiceUrl = jData?["component"]?["params"]?["playlist"]?[0]?["audioFileUrl"]?.ToString();
            if (voiceUrl == null)
                return null;

            string strTimeStart = font.ChildNodes[0].InnerText;
            string japaneseText = font.ChildNodes[1].InnerText;
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < font.ChildNodes.Count; i++)
                sb.Append(font.ChildNodes[i].InnerText);
            string chineseText = sb.ToString().Replace(strTimeStart, "").Replace("\n","");
            KanGirlVoiceItem voiceItem = new KanGirlVoiceItem(voiceUrl, chineseText, japaneseText);
            int time = Convert.ToInt32(font.InnerText[0..2]);
            return (time, voiceItem);
        }
    }
}
