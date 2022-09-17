using GreenOnions.Interface;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    public class Announcer : IPlugin
    {
        private AnnounceSetting _settings;
        private string _pluginPath;
        private Func<Task<List<GreenOnionsGroupInfo>>> GetGroupListAsync;
        private Func<long, GreenOnionsMessages, Task<int>> SendGroupMessageAsync;
        private Task _worker;
        private CancellationTokenSource _source;
        private KanGirlVoiceItem _nextHourVoiceItem = null;
        private bool _connected = false;

        public string Name => "舰C报时";

        public string Description => "舰队Collection整点语音报时插件";

        public string? HelpMessage => null;

        public void ConsoleSetting()
        {
            Console.WriteLine("本插件没有设计Console设置功能，请使用Windows端加载。");
        }

        public void OnConnected(long selfId,
            Func<long, GreenOnionsMessages, Task<int>> sendFriendMessageAsync,
            Func<long, GreenOnionsMessages, Task<int>> sendGroupMessageAsync,
            Func<long, long, GreenOnionsMessages, Task<int>> sendTempMessageAsync,
            Func<Task<List<GreenOnionsFriendInfo>>> getFriendListAsync,
            Func<Task<List<GreenOnionsGroupInfo>>> getGroupListAsync,
            Func<long, Task<List<long>>> getMemberListAsync,
            Func<long, long, Task<GreenOnionsMemberInfo>> getMemberInfoAsync)
        {
            GetGroupListAsync = getGroupListAsync;
            SendGroupMessageAsync = sendGroupMessageAsync;
            _connected = true;
            RestartWorker();
        }


        /// <summary>
        /// 报时
        /// </summary>
        private async void Announce()
        {
            while (!_source.IsCancellationRequested)
            {
                int nextHour = DateTime.Now.Hour + 1;
                if (nextHour >= 24)
                    nextHour = 0;
                //下个小时要报时且没有获取报时语音地址
                if (_settings.DesignatedTime.Contains(nextHour) && _nextHourVoiceItem == null)
                {
                    if (_settings.DesignateKanGirl)  //指定舰娘
                    {
                        _nextHourVoiceItem = await GetNextHourVoiceUrl(_settings.DesignatedKanGirl, nextHour);
                        if (_nextHourVoiceItem == null)
                        {
                            File.AppendAllText(Path.Combine(_pluginPath, "错误.log"), "获取音频失败，所选舰娘没有报时语音或地址需要人机验证，请重新选择和检查人机验证( https://zh.moegirl.org.cn/舰队Collection )后重连葱葱。\r\n");
                            _source.Cancel();
                            return;
                        }
                    }
                    else  //随机舰娘
                    {
                        //获取舰娘列表
                        List<string> kanGirlsName = await MoeGirlHelper.GetKanGrilNameList();
                        if (kanGirlsName == null)  //获取失败, 需要人机验证
                        {
                            File.AppendAllText(Path.Combine(_pluginPath, "错误.log"), "获取舰娘列表失败，需要人机验证，请手动打开 https://zh.moegirl.org.cn/舰队Collection 通过验证并重启葱葱。\r\n");
                            _source.Cancel();
                            return;
                        }
                    IL_Retry:;
                        Random rdm = new Random(Guid.NewGuid().GetHashCode());
                        int randomIndex = rdm.Next(0, kanGirlsName.Count);
                        string kanGirlName = kanGirlsName[randomIndex];
                        _nextHourVoiceItem = await GetNextHourVoiceUrl(kanGirlName, nextHour);
                        if (_nextHourVoiceItem == null)
                        {
                            kanGirlsName.Remove(kanGirlName);
                            goto IL_Retry;  //目标舰娘没有报时语音, 重新随机一个
                        }
                    }
                }

                GreenOnionsTextMessage japanTextMsg = null;  //日语文本消息
                GreenOnionsTextMessage chineseTextMsg = null;  //中文文本消息
                if (_settings.SendJapaneseText)
                    japanTextMsg = new GreenOnionsTextMessage(_nextHourVoiceItem.JapaneseText + "\r\n");
                if (_settings.SendChineseText)
                    chineseTextMsg = new GreenOnionsTextMessage(_nextHourVoiceItem.ChineseText + "\r\n");
                GreenOnionsVoiceMessage voiceMsg = new GreenOnionsVoiceMessage(_nextHourVoiceItem.Mp3Url, null);  //音频消息

                //报时的时间
                for (int i = 0; i < _settings.DesignatedTime.Count; i++)
                {
                    TimeOnly t = TimeOnly.FromDateTime(DateTime.Now);
                    int iHour = _settings.DesignatedTime[i];
                    if (TimeConsistent(t, iHour))  //这个小时需要报时且现在是0分0秒
                    {
                        //发送报时消息
                        for (int j = 0; j < _settings.DesignatedGroups.Count; j++)
                        {
                            if (_settings.SendJapaneseText)
                                await SendGroupMessageAsync(_settings.DesignatedGroups[j], japanTextMsg);
                            if (_settings.SendChineseText)
                                await SendGroupMessageAsync(_settings.DesignatedGroups[j], chineseTextMsg);
                            await SendGroupMessageAsync(_settings.DesignatedGroups[j], voiceMsg);
                        }
                        _nextHourVoiceItem = null;
                        await Task.Delay(10 * 1000, _source.Token);
                        break;
                    }
                }

                await Task.Delay(600);
            }
        }

        /// <summary>
        /// 获取下个小时的音频对象
        /// </summary>
        /// <param name="kanGirlName">舰娘名称</param>
        /// <param name="hour">小时数</param>
        /// <returns></returns>
        private async Task<KanGirlVoiceItem> GetNextHourVoiceUrl(string kanGirlName, int hour)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($@"https://zh.moegirl.org.cn/舰队Collection:{kanGirlName}");
                string html = await response.Content.ReadAsStringAsync() + "</body></html>";

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
                            if (timeToPley.ContainsKey(timeAndVoice!.Value.time))
                                timeToPley[timeAndVoice!.Value.time] = timeAndVoice!.Value.voiceItem;
                            else
                                timeToPley.Add(timeAndVoice!.Value.time, timeAndVoice!.Value.voiceItem);
                        }
                        else
                        {
                            if (tds[0].InnerText.Contains("报时")) //从包含"报时"的格子开始往下找
                            {
                                findedTimeTd = true;
                                var timeAndVoice = GetTimeAndVoiceUrl(tds[1], tds[2]);
                                if (timeToPley.ContainsKey(timeAndVoice!.Value.time))
                                    timeToPley[timeAndVoice!.Value.time] = timeAndVoice!.Value.voiceItem;
                                else
                                    timeToPley.Add(timeAndVoice!.Value.time, timeAndVoice!.Value.voiceItem);
                            }
                        }
                    }
                }
                if (!timeToPley.ContainsKey(hour))
                    return null;
                return timeToPley[hour];
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
            string voiceUrl = jData["component"]["params"]["playlist"][0]["audioFileUrl"].ToString();
            string br = "<br>";
            int indexBr = font.InnerHtml.IndexOf(br);
            string japaneseText = font.InnerHtml.Substring(5, indexBr - 5).Replace("<span lang=\"ja\">", "").Replace("</span>", "");
            string chineseText = font.InnerHtml.Substring(indexBr + br.Length).Replace(strTime, "").Replace("\n", "");
            KanGirlVoiceItem voiceItem = new KanGirlVoiceItem(voiceUrl, japaneseText, chineseText);
            return (time, voiceItem);
        }

        private bool TimeConsistent(TimeOnly timeNow, int targetHour)
        {
            //这个小时需要报时且现在是0分0秒
            if (timeNow.Hour == targetHour &&
                timeNow.Minute == 0 &&
                timeNow.Second == 0)
                return true;
            return false;
        }

        private async void RestartWorker()
        {
            StopWorker();
            if (_connected)
            {
                if (_worker != null)
                    await _worker;
                _source = new CancellationTokenSource();

                //没有指定报时时间
                if (_settings.DesignatedTime == null || _settings.DesignatedTime.Count == 0)
                    return;
                //仅发部分群但没有指定
                if (_settings.DesignateGroup && (_settings.DesignatedGroups == null || _settings.DesignatedGroups.Count == 0))
                    return;

                _worker = Task.Run(Announce, _source!.Token);
            }
        }

        private void StopWorker()
        {
            if (_source != null)
                _source.Cancel();
        }

        public void OnDisconnected()
        {
            _connected = false;
            StopWorker();
        }

        public void OnLoad(string pluginPath)
        {
            _pluginPath = pluginPath;
            string configFileName = Path.Combine(_pluginPath, "config.json");
            if (File.Exists(configFileName))
                _settings = JsonConvert.DeserializeObject<AnnounceSetting>(File.ReadAllText(configFileName))!;
            if (_settings == null)
                _settings = new AnnounceSetting();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            return false;
        }

        public bool WindowSetting()
        {
            FrmSettings frmSettings = new FrmSettings(_settings);
            frmSettings.ShowDialog();
            string configFileName = Path.Combine(_pluginPath, "config.json");
            File.WriteAllText(configFileName, JsonConvert.SerializeObject(_settings));
            RestartWorker();
            return true;
        }
    }
}