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
        private ICommonConfig? _commonConfig;
        private HttpListener _listener = new HttpListener();
        private Task? _listenerTask;

        public string Name => "消息通知";

        public string Description => "消息通知接口插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            StopListen();
            _listenerTask = StartListen(config, _commonConfig);

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
            _commonConfig = commonConfig;

            Config config = LoadConfig(pluginPath);
            StopListen();
            _listenerTask = StartListen(config, commonConfig);
        }

        private async void StopListen()
        {
            if (!_listener.IsListening)
                return;
            _listener.Stop();
            _listener.Close();
            if (_listenerTask is not null)
                await _listenerTask;
        }   

        internal async Task StartListen(Config config, ICommonConfig commonConfig)
        {
            LogHelper.LogMessage($"准备启动消息通知服务");
            string url = $"http://{config.ListenIp}:{config.ListenPort}/";
            _listener.Prefixes.Clear();
            _listener.Prefixes.Add(url);
            _listener.Start();
            LogHelper.LogMessage($"已启动消息通知服务在 {url}");
            while (_listener.IsListening)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                if (SngletonInstance.Bot is null)
                {
                    LogHelper.LogError("机器人实例未启动，无法发送消息，请重启机器人");
                    context.Response.Close();
                    continue;
                }
                try
                {
                    var request = context.Request;
                    var response = context.Response;

                    if (request.HttpMethod != "POST")
                    {
                        response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        response.Close();
                        return;
                    }

                    using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
                    string body = await reader.ReadToEndAsync();

                    RequestModel? requestModel = JsonConvert.DeserializeObject<RequestModel>(body);
                    if (requestModel is null)
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.Close();
                        return;
                    }

                    if (config.AdminOnly && !commonConfig.AdminQQ.Contains(requestModel.Target))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        response.Close();
                        return;
                    }

                    MessageBuilder msg = MessageBuilder.Friend(requestModel.Target);
                    msg.Text(requestModel.Message);
                    await SngletonInstance.Bot.SendMessage(msg.Build());

                    // 设置响应
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ContentLength64 = 0;
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
