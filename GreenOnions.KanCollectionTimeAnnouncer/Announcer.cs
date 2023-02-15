using System.Diagnostics;
using System.Runtime.InteropServices;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.PluginConfigs.KanCollectionTimeAnnouncer;
using Newtonsoft.Json;

namespace GreenOnions.KanCollectionTimeAnnouncer
{
    public class Announcer : IMessagePlugin, IPluginSetting
    {
        private KanCollectionConfig? _settings;
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

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _botApi = api;
            _connected = true;
            ReloadConfig();
            RestartWorker();
        }

        /// <summary>
        /// 报时
        /// </summary>
        private async void Announce()
        {
            while (!_source!.IsCancellationRequested)
            {
                try
                {
                    int nextHour = DateTime.Now.Hour + 1;
                    if (nextHour >= 24)
                        nextHour = 0;

                    if (!_settings!.DesignatedTime.Contains(nextHour))
                    {
                        await Task.Delay(1000 * 60 * 30, _source.Token);
                        continue;
                    }

                    //没有获取报时语音地址
                    _nextHourVoiceItem  ??= await _moeGirlHelper!.GetNextHourVoiceUrlAsync(_settings, nextHour);  //预先下载好音频

                    TimeOnly t = TimeOnly.FromDateTime(DateTime.Now);

                    //报时的时间
                    for (int i = 0; i < _settings.DesignatedTime.Count; i++)
                    {
                        int iHour = _settings.DesignatedTime[i];
                        if (!TimeConsistent(t, iHour))  //这个小时需要报时且现在是0分0秒
                            continue;

                        GreenOnionsVoiceMessage voiceMsg = new GreenOnionsVoiceMessage(_nextHourVoiceItem!.Mp3UrlOrFileName);  //音频消息

                        //发送报时消息
                        for (int j = 0; j < _settings.DesignatedGroups.Count; j++)
                        {
                            if (_settings.SendJapaneseText)
                                await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], _nextHourVoiceItem!.JapaneseText);
                            if (_settings.SendChineseText)
                                await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], _nextHourVoiceItem!.ChineseText);
                            await _botApi!.SendGroupMessageAsync(_settings.DesignatedGroups[j], voiceMsg);
                        }
                        _nextHourVoiceItem = null;
                        await Task.Delay(1000 * 60 * 30, _source.Token);
                        break;
                    }
                    await Task.Delay(600, _source.Token);
                }
                catch (TaskCanceledException)
                {
                }
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
            try
            {
                StopWorker();
                if (_connected)
                {
                    if (_worker is not null)
                        await _worker;

                    CreateHelper(_pluginPath!);

                    //没有指定报时时间
                    if (_settings!.DesignatedTime is null || _settings.DesignatedTime.Count == 0)
                        return;
                    //仅发部分群但没有指定
                    if (_settings.DesignateGroup && (_settings.DesignatedGroups is null || _settings.DesignatedGroups.Count == 0))
                        return;

                    _worker = Task.Run(Announce, _source!.Token);
                }
            }
            catch (Exception ex)
            {
                SendMessageToAdmin(@"舰C报时插件启动发生错误，请检查配置文件。");
            }
        }

        private void StopWorker()
        {
            if (_source is not null)
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
                _settings = JsonConvert.DeserializeObject<KanCollectionConfig>(File.ReadAllText(_configDirect))!;
            if (_settings is null)
                _settings = new KanCollectionConfig();
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            return false;
        }

        public void Setting()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            string editorDirect = Path.Combine("Plugins", "GreenOnions.PluginConfigEditor", "GreenOnions.PluginConfigEditor.exe");
            if (!File.Exists(editorDirect))
                throw new Exception("配置文件编辑器不存在，请安装 GreenOnions.PluginConfigEditor 插件。");
            Process.Start(editorDirect, new[] { new StackTrace(true).GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!, _configDirect! }).WaitForExit();
            ReloadConfig();
            RestartWorker();
        }
    }
}