using System.Diagnostics;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using Newtonsoft.Json;
using NovelAIClient;

namespace GreenOnions.NovelAiClient
{
    public class NovelHandler : IPlugin
    {
        private Config? _config;
        private IBotConfig? _botConfig;
        private IGreenOnionsApi? _api;
        private string? _strCmd;
        private string _errorMsg = "";
        public string Name => "NovelAi画图";

        public string Description => "NovelAi画图插件";

        public GreenOnionsMessages? HelpMessage => "发送 \"<机器人名称>画图：<关键词>\" 来绘制一张图片，多个关键词请用英文逗号分隔。";

        public bool DisplayedInTheHelp => true;

        public void ConsoleSetting()
        {

        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _api = api;
            if (string.IsNullOrWhiteSpace(_errorMsg))
            {
                _strCmd = _config!.Cmd;
            }
            else
            {
                for (int i = 0; i < _botConfig!.AdminQQ.Count; i++)
                    _api.SendFriendMessageAsync(_botConfig.AdminQQ.ToArray()[i], _errorMsg);
            }
        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            _botConfig = botConfig;
            string configPath = Path.Combine(pluginPath, "config.json");
            if (File.Exists(configPath))
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
                if (_config == null)
                {
                    _errorMsg = $"葱葱NovelAi画图插件读取配置文件失败，请检查json格式，文件路径为：{configPath}";
                }
            }
            else
            {
                //配置文件例子
                _config = new Config()
                {
                    Cmd = "<机器人名称>画[图画][:：]",
                    ConnectFrontEnd = "Naifu",
                    URL = "http://127.0.0.1:6969/",
                    ImageWidth = 512,
                    ImageHeight = 768,
                    DefaultPrompt = "masterpiece, best quality,",
                    DefaultUndesired = "lowres, bad anatomy, bad hands, text, error, missing fingers, extra digit, fewer digits, cropped, worst quality, low quality, normal quality, jpeg artifacts, signature, watermark, username, blurry, nsfw, r18, r-18, nude, nipple, nipples, breast, breasts, pussy, vaginal, asshole, penis, testicle, testicles, sex,",
                    StartDrawMessage = "开始绘制",
                    DrawEndMessage = "绘制完毕，耗时：<耗时>秒",
                    DrawErrorMessage = "绘制错误(Ｔ▽Ｔ)",
                    RevokeSecond = 30,
                };
                File.WriteAllText(configPath, JsonConvert.SerializeObject(_config));
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (_config != null)
            {
                if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg && _strCmd != null)
                {
                    Regex regex = new Regex(_strCmd);
                    if (regex.IsMatch(msg.Text))
                    {
                        Match match = regex.Match(msg.Text);
                        string promptStr = msg.Text.Substring(match.Value.Length);
                        string prompts = string.Join(',', promptStr.Split(',', '，').Select(s => s.Trim()).Where(p => !_config!.DefaultUndesired!.Contains(p)));
                        if (!string.IsNullOrWhiteSpace(_config.StartDrawMessage))
                            Response(_config.StartDrawMessage);
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        if (_config!.ConnectFrontEnd!.Equals("naifu", StringComparison.OrdinalIgnoreCase))
                            GetNaifuImageBytes(prompts).ContinueWith(CallBack);
                        else if (_config.ConnectFrontEnd.Equals("webui", StringComparison.OrdinalIgnoreCase))
                            GetWebUIImageBytes(prompts).ContinueWith(CallBack);

                        void CallBack(Task<byte[]?> task)
                        {
                            sw.Stop();
                            if (task.IsFaulted || task.IsCanceled || task.Result == null)
                            {
                                if (string.IsNullOrEmpty(_config!.DrawErrorMessage))
                                    Response(_config!.DrawErrorMessage!.Replace("<耗时>", (sw.ElapsedMilliseconds / 1000f).ToString()));
                                return;
                            }
                            GreenOnionsMessages msgs = new GreenOnionsImageMessage(new MemoryStream(task.Result));
                            msgs.RevokeTime = _config.RevokeSecond;
                            Response(msgs);
                            if (!string.IsNullOrWhiteSpace(_config!.DrawEndMessage))
                                Response(_config.DrawEndMessage.Replace("<耗时>", (sw.ElapsedMilliseconds / 1000f).ToString()));
                        }
                        return true;
                    }

                }
            }
            return false;
        }



        private Task<byte[]?> GetNaifuImageBytes(string prompts)
        {
            NaifuClient naifuClient = new NaifuClient(_config!.URL!);
            string defalutPrompt = _config!.DefaultPrompt!.Trim();
            if (!defalutPrompt.EndsWith(','))
                defalutPrompt += ',';
            return naifuClient.PostAsync(defalutPrompt + prompts, _config!.DefaultUndesired, _config.ImageWidth, _config.ImageHeight);
        }

        private Task<byte[]?> GetWebUIImageBytes(string prompts)
        {
            WebUIClient webuiClient = new WebUIClient(_config!.URL!);
            string defalutPrompt = _config!.DefaultPrompt!.Trim();
            if (!defalutPrompt.EndsWith(','))
                defalutPrompt += ',';
            return webuiClient.PostAsync(defalutPrompt + prompts, _config!.DefaultUndesired, _config.ImageWidth, _config.ImageHeight);
        }

        public bool WindowSetting()
        {
            return false;
        }
    }

    public class Config
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string? Cmd { get; set; }
        /// <summary>
        /// 连接的前端（Naifu/WebUI）
        /// </summary>
        public string? ConnectFrontEnd { get; set; }
        /// <summary>
        /// 前端地址
        /// </summary>
        public string? URL { get; set; }
        /// <summary>
        /// 图片宽
        /// </summary>
        public int ImageWidth { get; set; }
        /// <summary>
        /// 图片高
        /// </summary>
        public int ImageHeight { get; set; }
        /// <summary>
        /// 默认添加提示词
        /// </summary>
        public string? DefaultPrompt { get; set; }
        /// <summary>
        /// 默认屏蔽词
        /// </summary>
        public string? DefaultUndesired { get; set; }
        /// <summary>
        /// 开始绘制提示
        /// </summary>
        public string? StartDrawMessage{ get; set; }
        /// <summary>
        /// 绘制完成提示
        /// </summary>
        public string? DrawEndMessage { get; set; }
        /// <summary>
        /// 绘制错误提示
        /// </summary>
        public string? DrawErrorMessage { get; set; }
        /// <summary>
        /// 撤回时间（秒）
        /// </summary>
        public int RevokeSecond { get; set; }
    }
}