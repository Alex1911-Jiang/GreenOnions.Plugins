using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using GreenOnions.NT.OpenAiClient.Models;
using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Newtonsoft.Json;

namespace GreenOnions.NT.OpenAiClient
{
    public class OpenAiClientPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;
        private List<ChatingUser> _chatingUsers = new();
        private Timer? timeOutChecker = null;

        public string Name => "OpenAi客户端";

        public string Description => "访问所有兼容OpenAi接口协议的大语言模型插件";

        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);

            if (timeOutChecker is not null)
                timeOutChecker.Dispose();
            if (config.Enabled)
                timeOutChecker = new Timer(CheckTimeOut, null, 1000, 1000);
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

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;

            if (timeOutChecker is not null)
                timeOutChecker.Dispose();
            if (config.Enabled)
                timeOutChecker = new Timer(CheckTimeOut, null, 1000, 1000);
        }

        private async void OnGroupMessage(BotContext context, GroupMessageEvent e)
        {
            if (e.Chain.FriendUin == context.BotUin)  //自己的消息
                return;
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async void OnFriendMessage(BotContext context, FriendMessageEvent e)
        {
            try
            {
                await OnMessage(context, e.Chain);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{Name}插件发生了不在预见范围内的异常，错误信息：{ex.Message}");
            }
        }

        private async Task OnMessage(BotContext context, MessageChain chain)
        {
            if (!chain.AllowUseIfDebug())
                return;

            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning("OpenAi客户端插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用OpenAi客户端
                return;

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var entity in chain)
            {
                if (entity is not TextEntity text)
                    continue;

                string msg = text.Text;

                ChatingUser? chatingUser = _chatingUsers.FirstOrDefault(u => u.Chain.FriendUin == chain.FriendUin && u.Chain.GroupUin == chain.GroupUin);
                if (chatingUser is null)  //此用户并非在聊天模式中
                {
                    string msgCmd = msg;
                    if (firstAt)
                        msgCmd = _commonConfig.BotName + msg.TrimStart();

                    foreach (var config in _config.ChatConfigs)
                    {
                        if (string.IsNullOrWhiteSpace(config.ApiKey))
                            continue;
                        if (string.IsNullOrWhiteSpace(config.ModelId))
                            continue;
                        if (string.IsNullOrWhiteSpace(config.Url))
                            continue;

                        Regex regexStart = new Regex(config.StartCommand.ReplaceConfigTags(config), RegexOptions.IgnoreCase);
                        Match matchStart = regexStart.Match(msgCmd);
                        if (!matchStart.Success)
                            continue;

                        LogHelper.LogMessage($"开始{config.Remark} AI聊天");

                        ChatingUser user = new ChatingUser(chain, DateTime.Now.AddSeconds(config.TimeOutSeconds), config);
                        lock (_chatingUsers)
                        {
                            _chatingUsers.Add(user);
                            if (!string.IsNullOrWhiteSpace(config.SystemChatMessage))
                                user.ChatMessages.Add(ChatMessage.FromSystem(config.SystemChatMessage));
                        }
                        await chain.ReplyAsync(config.StartedChatReply.ReplaceConfigTags(config));
                        return;
                    }
                }
                else  //此用户在聊天模式中
                {
                    string msgCmd = msg;
                    if (firstAt)
                        msgCmd = _commonConfig.BotName + msg.TrimStart();

                    Regex regexCleanContext = new Regex(chatingUser.Config.ClearContextCommand.ReplaceConfigTags(chatingUser.Config));
                    Match matchCleanContext = regexCleanContext.Match(msgCmd);
                    if (matchCleanContext.Success)  //清除上下文
                    {
                        LogHelper.LogMessage($"清除{chatingUser.Config} AI聊天的上下文");

                        chatingUser.ChatMessages.Clear();
                        if (!string.IsNullOrWhiteSpace(chatingUser.Config.SystemChatMessage))
                            chatingUser.ChatMessages.Add(ChatMessage.FromSystem(chatingUser.Config.SystemChatMessage));
                        await chain.ReplyAsync(chatingUser.Config.CleanContextReply.ReplaceConfigTags(chatingUser.Config));
                        return;
                    }

                    Regex regexExit = new Regex(chatingUser.Config.ExitCommand.ReplaceConfigTags(chatingUser.Config));
                    Match matchExit = regexExit.Match(msgCmd);
                    if (matchExit.Success)  //退出聊天
                    {
                        LogHelper.LogMessage($"退出{chatingUser.Config} AI聊天");

                        lock (_chatingUsers)
                        {
                            _chatingUsers.Remove(chatingUser);
                        }
                        await chain.ReplyAsync(chatingUser.Config.ExitedChatReply.ReplaceConfigTags(chatingUser.Config));
                        return;
                    }

                    //重置超时时间
                    chatingUser.TimeOut = DateTime.Now.AddSeconds(chatingUser.Config.TimeOutSeconds);

                    if (!chatingUser.Config.MaintainContext)  //如果设定为不保持上下文，则清空上下文并重新添加System
                    {
                        chatingUser.ChatMessages.Clear();
                        if (!string.IsNullOrWhiteSpace(chatingUser.Config.SystemChatMessage))
                            chatingUser.ChatMessages.Add(ChatMessage.FromSystem(chatingUser.Config.SystemChatMessage));
                    }

                    //聊天
                    using HttpClientHandler handler = new HttpClientHandler { UseProxy = chatingUser.Config.UseProxy };
                    using HttpClient client = new HttpClient(handler);
                    try
                    {
                        List<ChatMessage> nowChatMessages = chatingUser.ChatMessages.ToList();  //创建一个副本添加本次对话内容，避免在聊天失败时将本次的对话添加到上下文
                        nowChatMessages.Add(ChatMessage.FromUser(msg));

                        using StringContent content = new ChatRequest
                        {
                            model = chatingUser.Config.ModelId!,
                            messages = nowChatMessages,
                            temperature = chatingUser.Config.Temperature,
                            stream = chatingUser.Config.Stream,

                        }.ToJsonContent();

                        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, chatingUser.Config.Url!.DomainToUrl()) { Content = content };
                        request.Headers.Accept.TryParseAdd("application/json");
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", chatingUser.Config.ApiKey);
                        HttpResponseMessage responseMessage = await client.SendAsync(request);
                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            try
                            {
                                string errorMessage = await responseMessage.Content.ReadAsStringAsync();
                                var errorObject = JsonConvert.DeserializeObject<ChatResponse>(errorMessage);
                                LogHelper.LogError($"{chatingUser.Config.Remark}聊天失败，{errorObject?.error?.code}");
                                await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config, new Exception($"{errorObject?.error?.message}")));
                            }
                            catch
                            {
                                LogHelper.LogError($"{chatingUser.Config.Remark}聊天失败，{(int)responseMessage.StatusCode} {responseMessage.StatusCode}");
                                await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config, new Exception($"{(int)responseMessage.StatusCode} {responseMessage.StatusCode}")));
                            }
                            return;
                        }

                        StringBuilder? multiOutput = new StringBuilder();
                        if (chatingUser.Config.Stream)
                        {
                            Console.WriteLine($"{chatingUser.Config.Remark}流式响应回复：");
                            await using var stream = await responseMessage.Content.ReadAsStreamAsync();
                            using StreamReader reader = new(stream);
                            StringBuilder contentBuilder = new StringBuilder();
                            StringBuilder reasoningBuilder = new StringBuilder();
                            while (true)
                            {
                                string? line = await reader.ReadLineAsync();
                                if (line is null)
                                    break;
                                if (line == ": keep-alive")
                                    continue;
                                if (!line.StartsWith("data: ")) 
                                    continue;
                                string jsonData = line["data: ".Length..];
                                if (jsonData == "[DONE]")
                                    break;
                                var responseObject = JsonConvert.DeserializeObject<ChatResponse>(jsonData);
                                if (!string.IsNullOrWhiteSpace(responseObject?.choices?[0].delta?.reasoning_content))
                                {
                                    Console.Write(responseObject?.choices?.FirstOrDefault()?.delta?.reasoning_content);
                                    reasoningBuilder.Append(responseObject?.choices?.FirstOrDefault()?.delta?.reasoning_content);
                                }
                                if (!string.IsNullOrWhiteSpace(responseObject?.choices?[0].delta?.content))
                                {
                                    Console.WriteLine(responseObject?.choices?.FirstOrDefault()?.delta?.content);
                                    contentBuilder.Append(responseObject?.choices?.FirstOrDefault()?.delta?.content);
                                }
                            }
                            if (reasoningBuilder.Length > 0)
                                multiOutput.AppendLine($"<think>\r\n{reasoningBuilder}\r\n</think>");
                            if (contentBuilder.Length > 0)
                                multiOutput.AppendLine(contentBuilder.ToString());
                        }
                        else
                        {
                            string responseJson = await responseMessage.Content.ReadAsStringAsync();
                            if (string.IsNullOrWhiteSpace(responseJson))
                                goto IL_Empty;

                            ChatResponse? response = JsonConvert.DeserializeObject<ChatResponse>(responseJson);
                            if (response is null)
                            {
                                LogHelper.LogError($"{chatingUser.Config.Remark}聊天错误，解析Json失败 {responseJson}");
                                await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config));
                                return;
                            }

                            if (response.error is not null)
                            {
                                LogHelper.LogError($"{chatingUser.Config.Remark}聊天失败，{response.error.code} {response.error.message}");
                                await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config, new Exception($"{response.error.code} {response.error.message}")));
                                return;
                            }

                            string? responseReasoningContent = response.choices?.FirstOrDefault()?.message?.reasoning_content;
                            string? responseContent = response.choices?.FirstOrDefault()?.message?.content;

                            if (!string.IsNullOrWhiteSpace(responseReasoningContent))
                                multiOutput.AppendLine($"<think>\r\n{responseReasoningContent}\r\n</think>");
                            if (!string.IsNullOrWhiteSpace(responseContent))
                                multiOutput.AppendLine(responseContent);
                        }

                    IL_Empty:
                        string outputMsg = multiOutput.ToString();
                        if (string.IsNullOrWhiteSpace(outputMsg))
                        {
                            LogHelper.LogError($"{chatingUser.Config.Remark}聊天错误，接口返回内容为空");
                            await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config, new Exception($"服务器繁忙，请稍后再试。")));
                            return;
                        }

                        string result = outputMsg;
                        LogHelper.LogMessage($"{chatingUser.Config.Remark} AI聊天返回内容：{multiOutput}");
                        if (chatingUser.Config.RemoveThink && outputMsg.StartsWith("<think>"))  //移除DeepSeek思考过程
                            result = outputMsg.Substring(outputMsg.IndexOf("</think>") + "</think>".Length).TrimStart();
                        await chain.ReplyAsync(result);

                        nowChatMessages.Add(ChatMessage.FromAssistant(multiOutput.ToString()));
                        chatingUser.ChatMessages = nowChatMessages;
                    }
                    catch (Exception ex)
                    {
                        await chain.ReplyAsync(_config.ErrorReply.ReplaceConfigTags(chatingUser.Config, ex));
                    }
                }
            }
        }

        private async void CheckTimeOut(object? state)
        {
            ChatingUser[] timeOutUsers = _chatingUsers.Where(u => DateTime.Now > u.TimeOut).ToArray();
            foreach (var item in timeOutUsers)
            {
                lock (_chatingUsers)
                {
                    _chatingUsers.Remove(item);
                }
                if (_config is null)
                    continue;
                await item.Chain.ReplyAsync(item.Config.TimeOutReply);
            }
        }
    }
}
