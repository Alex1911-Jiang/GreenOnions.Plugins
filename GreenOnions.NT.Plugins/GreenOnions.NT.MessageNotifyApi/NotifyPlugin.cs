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

        public string Name => "消息通知";

        public string Description => "消息通知接口插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
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
            Task.Delay(5000).ContinueWith(_ => StartListen(config, commonConfig));
        }

        internal async void StartListen(Config config, ICommonConfig commonConfig)
        {
            while (true)
            {
                using HttpListener listener = new HttpListener();
                try
                {
                    string url = $"http://{config.ListenIp}:{config.ListenPort}/";
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(url);
                    listener.Start();
                    LogHelper.LogMessage($"已启动消息通知服务在 {url}");
                    HttpListenerContext context = await listener.GetContextAsync();
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
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"启动通知监听接口失败：{ex.Message}");
                }
                listener.Stop();
                listener.Close();
            }
        }
    }
}
