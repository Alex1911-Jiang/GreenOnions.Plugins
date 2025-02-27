using System.Text;
using Binance.Net.Enums;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.CryptocurrencyPrices
{
    public class CryptocurrencyPricesPlugin : IPlugin
    {
        private Config? _config;
        private string? _pluginPath;
        private ICommonConfig? _commonConfig;

        public string Name => "数字货币询价";

        public string Description => "数字货币询价插件";

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

            bot.Invoker.OnGroupMessageReceived += OnGroupMessage;
            bot.Invoker.OnBotOnlineEvent += Invoker_OnBotOnlineEvent;

        }

        private void Invoker_OnBotOnlineEvent(BotContext context, BotOnlineEvent e)
        {
            if (_config is null)
            {
                LogHelper.LogError("插件配置为空");
                return;
            }
            BinanceHelper.AutoGetPrice(context, _config);
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

        private async Task OnMessage(BotContext context, MessageChain chain)
        {
            if (SngletonInstance.Bot is null)
            {
                LogHelper.LogError("未构建机器人实例，请先登录");
                return;
            }

            if (!chain.AllowUseIfDebug())
                return;

            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning($"{Name}插件配置为空");
                return;
            }

            if (chain.GroupUin is null)
                return;

            if (!_config.WhileGroups.Contains(chain.GroupUin.Value))
                return;

            if (chain.GetEntity<TextEntity>() is not TextEntity text)
                return;

            if (text.Text is "帮助" or "help")
            {
                StringBuilder output = new StringBuilder();
                output.AppendLine("可使用以下命令：");
                foreach (var item in BinanceHelper.CommandToData)
                {
                    if (!item.Value.ShowCommand)
                        continue;
                    output.AppendLine($"{item.Key} : {item.Value.Text}");
                }

                await chain.ReplyAsync(output.ToString());
                return;
            }

            string symbol = text.Text.ToUpper();
            if (_config.ReplaceSymbol.TryGetValue(symbol, out string? replace))
                symbol = replace;
            if (BinanceHelper.CommandToData.TryGetValue(text.Text, out TimeData? data))
            {
                try
                {
                    StringBuilder output = new StringBuilder();
                    output.AppendLine($"{data.Text}");
                    output.AppendLine();
                    if (data.Text.StartsWith("24"))
                    {
                        if (data.Side == PositionSide.Both)
                        {
                            string top5Long = await BinanceHelper.Get24hRiseOrFall(5, true);
                            string top5Short = await BinanceHelper.Get24hRiseOrFall(5, false);
                            output.AppendLine(top5Long);
                            output.AppendLine(top5Short);
                        }
                        else
                        {
                            string top10 = await BinanceHelper.Get24hRiseOrFall(10, data.Side == PositionSide.Long);
                            output.AppendLine(top10);
                        }
                    }
                    else
                    {
                        if (data.Side == PositionSide.Both)
                        {
                            string top5Long = BinanceHelper.GetAnyTimeRiseOrFall(5, true, data.Second);
                            string top5Short = BinanceHelper.GetAnyTimeRiseOrFall(5, false, data.Second);
                            output.AppendLine(top5Long);
                            output.AppendLine(top5Short);
                        }
                        else
                        {
                            string top10 = BinanceHelper.GetAnyTimeRiseOrFall(10, data.Side == PositionSide.Long, data.Second);
                            output.AppendLine(top10);
                        }
                    }

                    MessageChain msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(output.ToString()).Build();
                    MessageResult result = await SngletonInstance.Bot.SendMessage(msg);
                    if (_config.RecallSecond > 0)
                    {
                        LogHelper.LogMessage($"{_config.RecallSecond} 秒后撤回消息");
                        _ = Task.Delay(_config.RecallSecond * 1000).ContinueWith(async _ => await SngletonInstance.Bot.RecallGroupMessage(chain.GroupUin.Value, result));
                    }
                }
                catch (Exception ex)
                {
                    await context.SendMessageToAdmin($"查询 {text.Text} 涨跌榜失败，{ex.Message}");
                    await chain.ReplyAsync("查询失败，请稍后再试");
                }
                return;
            }
            else if (BinanceHelper.SpotPrices.Last().Value.ContainsKey(symbol))  //查现货实价
            {
                try
                {
                    var binance24Hr = await BinanceHelper.GetTickerAsync(symbol);
                    MessageChain msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(binance24Hr).Build();
                    MessageResult result = await SngletonInstance.Bot.SendMessage(msg);
                    if (_config.RecallSecond > 0)
                    {
                        LogHelper.LogMessage($"{_config.RecallSecond} 秒后撤回消息");
                        _ = Task.Delay(_config.RecallSecond * 1000).ContinueWith(async _ => await SngletonInstance.Bot.RecallGroupMessage(chain.GroupUin.Value, result));
                    }
                }
                catch (Exception ex)
                {
                    await context.SendMessageToAdmin($"查询 {symbol} 最新价格失败，{ex.Message}");
                    await chain.ReplyAsync("查询失败，请稍后再试");
                }
            }
        }
    }
}