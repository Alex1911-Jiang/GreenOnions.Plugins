using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    public class Announcer : IPlugin
    {
        private AnnounceSetting? _settings;
        private MoeGirlHelper? _moeGirlHelper;
        private string? _pluginPath;
        private Task? _worker;
        private CancellationTokenSource? _source;
        private KanGirlVoiceItem? _nextHourVoiceItem = null;
        private bool _connected = false;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;

        public string Name => "舰C报时";

        public string Description => "舰队Collection整点语音报时插件";

        public GreenOnionsMessages? HelpMessage => null;

        public bool DisplayedInTheHelp => false;

        public void ConsoleSetting()
        {
            Console.WriteLine("本插件没有设计Console设置功能，请使用Windows端加载。");
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            _connected = true;
            RestartWorker();
        }

        /// <summary>
        /// 报时
        /// </summary>
        private async void Announce()
        {
            while (!_source!.IsCancellationRequested)
            {
                int nextHour = DateTime.Now.Hour + 1;
                if (nextHour >= 24)
                    nextHour = 0;
                //下个小时要报时且没有获取报时语音地址
                if (_settings!.DesignatedTime.Contains(nextHour) && _nextHourVoiceItem == null)
                {
                    if (_settings.DesignateKanGirl)  //指定舰娘
                    {
                        _nextHourVoiceItem = await _moeGirlHelper!.GetNextHourVoiceUrlAsync(_settings.DesignatedKanGirl!, nextHour);
                        if (_nextHourVoiceItem == null)
                        {
                            _source.Cancel();
                            SendMessageToAdmin($"葱葱舰C报时插件错误：获取音频失败，所选舰娘没有报时语音或地址需要人机验证，请重新选择和检查人机验证( https://zh.moegirl.org.cn/舰队Collection )后重连葱葱。");
                            return;
                        }
                    }
                    else  //随机舰娘
                    {
                        //获取舰娘列表
                        List<string>? kanGirlsName = await _moeGirlHelper!.GetKanGrilNameListAsync();
                        if (kanGirlsName == null)  //获取失败, 需要人机验证
                        {
                            _source.Cancel();
                            SendMessageToAdmin("葱葱舰C报时插件错误：获取舰娘列表失败，需要人机验证，请手动打开 https://zh.moegirl.org.cn/舰队Collection 通过验证并重启葱葱。");
                            return;
                        }
                    IL_Retry:;
                        Random rdm = new Random(Guid.NewGuid().GetHashCode());
                        int randomIndex = rdm.Next(0, kanGirlsName.Count);
                        string kanGirlName = kanGirlsName[randomIndex];
                        _nextHourVoiceItem = await _moeGirlHelper.GetNextHourVoiceUrlAsync(kanGirlName, nextHour);
                        if (_nextHourVoiceItem == null)
                        {
                            kanGirlsName.Remove(kanGirlName);
                            _moeGirlHelper!.CoverSaveKanGirlList(kanGirlsName);
                            goto IL_Retry;  //目标舰娘没有报时语音, 重新随机一个
                        }
                    }
                }

                GreenOnionsTextMessage? japanTextMsg = null;  //日语文本消息
                GreenOnionsTextMessage? chineseTextMsg = null;  //中文文本消息
                if (_settings.SendJapaneseText)
                    japanTextMsg = new GreenOnionsTextMessage(_nextHourVoiceItem!.JapaneseText + "\r\n");
                if (_settings.SendChineseText)
                    chineseTextMsg = new GreenOnionsTextMessage(_nextHourVoiceItem!.ChineseText + "\r\n");

                GreenOnionsVoiceMessage voiceMsg = new GreenOnionsVoiceMessage(_nextHourVoiceItem!.Mp3UrlOrFileName);  //音频消息

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
                                await _api!.SendGroupMessageAsync(_settings.DesignatedGroups[j], japanTextMsg!);
                            if (_settings.SendChineseText)
                                await _api!.SendGroupMessageAsync(_settings.DesignatedGroups[j], chineseTextMsg!);
                            await _api!.SendGroupMessageAsync(_settings.DesignatedGroups[j], voiceMsg);
                        }
                        _nextHourVoiceItem = null;
                        try
                        {
                            await Task.Delay(10 * 1000, _source.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        break;
                    }
                }

                await Task.Delay(600);
            }
        }

        private async void SendMessageToAdmin(string msg)
        {
            foreach (long item in _botConfig!.AdminQQ)
            {
                await _api!.SendFriendMessageAsync(item, msg);
            }
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

        private void CreateHelper(string pluginPath)
        {
            _source = new CancellationTokenSource();
            _moeGirlHelper = new MoeGirlHelper(pluginPath, _api!, _botConfig!, _source.Token);
        }

        private async void RestartWorker()
        {
            StopWorker();
            if (_connected)
            {
                if (_worker != null)
                    await _worker;

                CreateHelper(_pluginPath!);

                //没有指定报时时间
                if (_settings!.DesignatedTime == null || _settings.DesignatedTime.Count == 0)
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

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            _botConfig = botConfig;
            _pluginPath = pluginPath;
            string configFileName = Path.Combine(_pluginPath!, "config.json");
            if (File.Exists(configFileName))
                _settings = JsonSerializer.Deserialize<AnnounceSetting>(File.ReadAllText(configFileName))!;
            if (_settings == null)
                _settings = new AnnounceSetting();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            return false;
        }

        public bool WindowSetting()
        {
            FrmSettings frmSettings = new FrmSettings(_settings!, _moeGirlHelper!);
            frmSettings.ShowDialog();
            string configFileName = Path.Combine(_pluginPath!, "config.json");
            File.WriteAllText(configFileName, JsonSerializer.Serialize(_settings, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            RestartWorker();
            return true;
        }
    }
}