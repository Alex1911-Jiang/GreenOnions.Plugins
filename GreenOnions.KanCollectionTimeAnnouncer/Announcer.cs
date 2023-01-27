using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.PluginConfigs.KanCollectionTimeAnnouncer;
using Newtonsoft.Json;

namespace GreenOnions.KanCollectionTimeAnnouncer
{
    public class Announcer : IPlugin
    {
        private KanCollectionSetting? _settings;
        private MoeGirlHelper? _moeGirlHelper;
        private string? _pluginPath;
        private string? _configDirect;
        private Task? _worker;
        private CancellationTokenSource? _source;
        private KanGirlVoiceItem? _nextHourVoiceItem = null;
        private bool _connected = false;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _botApi;

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
            _botApi = api;
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
                                await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], japanTextMsg!);
                            if (_settings.SendChineseText)
                                await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], chineseTextMsg!);
                            await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], voiceMsg);
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
                await _botApi!.SendFriendMessageAsync(item, msg);
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
            _moeGirlHelper = new MoeGirlHelper(pluginPath, _botApi!, _botConfig!, _source.Token);
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
            _configDirect = Path.Combine(_pluginPath!, "config.json");
        }

        private void ReloadConfig()
        {
            if (File.Exists(_configDirect))
                _settings = JsonConvert.DeserializeObject<KanCollectionSetting>(File.ReadAllText(_configDirect))!;
            if (_settings == null)
                _settings = new KanCollectionSetting();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            return false;
        }

        public bool WindowSetting()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;

            string editorDirect = Path.Combine("Plugins", "GreenOnions.PluginConfigEditor", "GreenOnions.PluginConfigEditor.exe");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect! }).WaitForExit();
            ReloadConfig();
            RestartWorker();
            return true;
        }
    }
}