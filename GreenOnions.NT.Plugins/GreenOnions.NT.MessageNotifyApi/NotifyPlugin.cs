using System.Net;
using System.Text;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.MessageNotifyApi
{
    public class NotifyPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private HttpListener _listener = new HttpListener();

        public string Name => "消息通知";
        public string Description => "消息通知接口插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            _listener.Stop();
            StartListen(config, commonConfig);
        }

        private Config LoadConfig(string pluginPath)
        {
            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            _config ??= new Config();
            File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));
            return _config;
        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;

            Config config = LoadConfig(pluginPath);
            _listener.Stop();
            StartListen(config, commonConfig);
        }

        internal async void StartListen(Config config, ICommonConfig commonConfig)
        {
            string url = $"http://{config.ListenIp}:{config.ListenPort}/";
            _listener.Prefixes.Clear();
            _listener.Prefixes.Add(url);
            LogHelper.LogMessage($"已启动消息通知服务在 {url}");
            _listener.Start();
            while (_listener.IsListening)
            {
                HttpListenerContext context;
                try
                {
                    context = await _listener.GetContextAsync();
                }
                catch (HttpListenerException ex)
                {
                    if (ex.ErrorCode == 995)  //取消监听
                    {
                        LogHelper.LogWarning("已主动停止监听");
                        return;
                    }
                    LogHelper.LogException(ex, $"监听发生异常，{ex.Message}");
                    continue;
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"监听发生异常，{ex.Message}");
                    continue;
                }
                Console.WriteLine("收到Http请求");
                if (SngletonInstance.Bot is null)
                {
                    LogHelper.LogError("机器人实例未启动，无法发送消息，请重启机器人");
                    context.Response.Close();
                    continue;
                }
                try
                {
                    if (context.Request.HttpMethod != "POST")
                    {
                        LogHelper.LogWarning($"收到非Post请求");
                        context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        context.Response.Close();
                        continue;
                    }

                    using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
                    string body = await reader.ReadToEndAsync();

                    RequestModel? requestModel = JsonConvert.DeserializeObject<RequestModel>(body);
                    if (requestModel is null)
                    {
                        LogHelper.LogWarning($"收到的请求内容无法解析为转发对象");
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.Close();
                        continue;
                    }

                    if (config.AdminOnly && !commonConfig.AdminQQ.Contains(requestModel.Target))
                    {
                        LogHelper.LogWarning($"收到的消息请求转发到的QQ号：{requestModel.Target} 不在管理员列表中：{string.Join(',', commonConfig.AdminQQ)}，内容：{requestModel.Message}");
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.Close();
                        continue;
                    }

                    Console.WriteLine($"向：{requestModel.Target}，发送消息：{requestModel.Message}");
                    MessageBuilder msg = MessageBuilder.Friend(requestModel.Target);
                    msg.Text(requestModel.Message);
                    await SngletonInstance.Bot.SendMessage(msg.Build());

                    // 设置响应
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentLength64 = 0;
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"处理消息通知请求时发生异常，{ex.Message}");
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }
    }
}
