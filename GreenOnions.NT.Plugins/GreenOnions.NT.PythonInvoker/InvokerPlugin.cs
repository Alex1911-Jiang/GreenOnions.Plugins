using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.PythonInvoker
{
    public class InvokerPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;

        public string Name =>  "Python调用器";

        public string Description => "执行Python代码或调用第三方Python程序插件";

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

            bot.Invoker.OnFriendMessageReceived -= OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived -= OnGroupMessage;
            bot.Invoker.OnFriendMessageReceived += OnFriendMessage;
            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
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
                LogHelper.LogWarning("Python调用器插件配置为空");
                return;
            }

            if (!_config.Enabled)  //没有启用Python调用器
                return;

            bool firstAt = chain.FirstOrDefault() is MentionEntity at && at.Uin == context.BotUin;
            foreach (var entity in chain)
            {
                if (entity is not TextEntity inputText)
                    continue;

                string msg = inputText.Text;

                if (firstAt)
                    msg = _commonConfig.BotName + msg.TrimStart();

                foreach (var config in _config.Inovkers)
                {
                    Regex regex = new Regex(config.Command.ReplaceConfigTags(config), RegexOptions.IgnoreCase);
                    Match match = regex.Match(msg);
                    if (!match.Success)
                        continue;

                    if (chain.GroupUin is null && !config.AllowPrivateMessage)//私聊 并且不允许私聊
                    {
                        LogHelper.LogMessage($"没有启用私聊调用{config.Remark}，不响应调用Python程序命令");
                        return;
                    }

                    if (chain.GroupUin is not null && config.WhiteGroupOnly && !config.WhiteGroup.Contains(chain.GroupUin.Value))  //群聊 且仅限白名单，但当前群不在白名单中
                    {
                        LogHelper.LogMessage($"启用了仅限白名单调用{config.Remark}，但{chain.GroupUin.Value}不在白名单中，不响应调用Python程序命令");
                        return;
                    }

                    LogHelper.LogMessage($"命中Python调用器{config.Remark}命令，开始调用");

                    string param = string.Empty;
                    if (match.Groups["参数"].Success)
                        param = match.Groups["参数"].Value;

                    if (!string.IsNullOrWhiteSpace(config.InvokingReply))
                    {
                        string reply = config.InvokingReply;
                        if (!string.IsNullOrWhiteSpace(param))
                            reply = reply.Replace("<参数>", param);
                        await chain.ReplyAsync(reply.ReplaceConfigTags(config));
                    }

                    try
                    {
                        string script = config.Script;
                        if (!string.IsNullOrWhiteSpace(param))
                            script = script.Replace("<参数>", param);
                        script = script.ReplaceConfigTags(config);

                        await ExecutePythonAsync(script);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogException(ex, $"调用Python发生异常，{ex.Message}");
                        await chain.ReplyAsync(config.ErrorReply.ReplaceConfigTags(config, ex));
                    }

                    LogHelper.LogMessage($"调用Python{config.Remark}完成");

                    if (!string.IsNullOrWhiteSpace(config.InvokedReply))
                    {
                        string reply = config.InvokedReply;
                        if (!string.IsNullOrWhiteSpace(param))
                            reply = reply.Replace("<参数>", param);
                        await chain.ReplyAsync(reply.ReplaceConfigTags(config));
                    }

                    if (config.ReadFileMode == ReadFileModes.None)
                        return;

                    if (string.IsNullOrWhiteSpace(config.ReadFileName))
                    {
                        LogHelper.LogError($"{config.Remark}未配置调用Python完成后读取的文件路径");
                        await chain.ReplyAsync(config.ErrorReply.ReplaceConfigTags(config, new Exception("未配置文件路径")));
                    }
                    string fileName = Path.Combine(_pluginPath!, config.ReadFileName);

                    if (!string.IsNullOrWhiteSpace(param))
                        fileName = fileName.Replace("<参数>", param);

                    if (!File.Exists(fileName))
                    {
                        LogHelper.LogWarning($"文件{fileName}不存在");
                        await chain.ReplyAsync(config.ErrorReply.ReplaceConfigTags(config));
                        return;
                    }

                    LogHelper.LogMessage($"{config.Remark}调用Python完毕，开始上传文件");

                    if (config.ReadFileMode == ReadFileModes.Raw)
                    {
                        bool uploaded;
                        uint target;
                        if (chain.GroupUin is not null)
                        {
                            target = chain.GroupUin.Value;
                            uploaded = await context.GroupFSUpload(chain.GroupUin.Value, new FileEntity(fileName));
                        }
                        else
                        {
                            target = chain.FriendUin;
                            uploaded = await context.UploadFriendFile(chain.FriendUin, new FileEntity(fileName));
                        }
                        if (uploaded)
                        {
                            LogHelper.LogMessage($"上传{config.ReadFileName.Replace("<参数>", param)}至{target}成功");
                        }
                        else
                        {
                            LogHelper.LogWarning($"上传{config.ReadFileName.Replace("<参数>", param)}至{target}失败");
                            await chain.ReplyAsync(config.ErrorReply.ReplaceConfigTags(config, new Exception($"上传文件{config.ReadFileName.Replace("<参数>", param)}被拒绝")));
                        }
                        return;
                    }

                    MessageBuilder builder;
                    if (chain.GroupUin is not null)
                        builder = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
                    else
                        builder = MessageBuilder.Friend(chain.FriendUin);

                    switch (config.ReadFileMode)
                    {
                        case ReadFileModes.Text:
                            string text = File.ReadAllText(fileName);
                            builder.Text(text);
                            break;
                        case ReadFileModes.Image:
                            builder.Image(fileName);
                            break;
                        case ReadFileModes.Video:
                            builder.Video(fileName);
                            break;
                        case ReadFileModes.Audio:
                            builder.Record(fileName);
                            break;
                    }
                    await context.SendMessage(builder.Build());
                    return;
                }
            }
        }

        private async Task<string> ExecutePythonAsync(string script)
        {
            var process = new Process();
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var pythonExe = isWindows ? "python" : "python3";

            process.StartInfo = new ProcessStartInfo
            {
                FileName = pythonExe,
                WorkingDirectory = _pluginPath,
                Arguments = $"-c \"{script}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(error))
            {
                LogHelper.LogError(error);
                throw new Exception(error);
            }
            LogHelper.LogMessage(output);

            return output;
        }
    }
}
