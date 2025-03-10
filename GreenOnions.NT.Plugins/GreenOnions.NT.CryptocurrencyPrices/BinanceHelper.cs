using System.Collections.Concurrent;
using System.Text;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace GreenOnions.NT.CryptocurrencyPrices
{
    internal static class BinanceHelper
    {
        internal static SortedDictionary<long, ConcurrentDictionary<string, decimal>> SpotPrices { get; set; } = [];

        private static BinanceRestClient _binanceRestClient = new BinanceRestClient();
        private static BinanceSocketClient _binanceWsClient = new BinanceSocketClient();

        internal static async Task<string> GetTickerAsync(string symbol)
        {
            var binance24Hr = await _binanceRestClient.SpotApi.ExchangeData.GetTickerAsync($"{symbol}USDT");
            if (!binance24Hr.Success)
                throw new Exception($"{binance24Hr.Error!.Code} {binance24Hr.Error!.Message}");

            string add = binance24Hr.Data.PriceChangePercent > 0 ? "+" : "";
            return @$"【{symbol}/USDT】
现价：{binance24Hr.Data.LastPrice}
24h最高价↑：{binance24Hr.Data.HighPrice}
24h最低价↓：{binance24Hr.Data.LowPrice}
24h涨跌额：{add}{binance24Hr.Data.PriceChange}
24h涨跌比：{add}{binance24Hr.Data.PriceChangePercent}%";
        }

        internal static async void AutoGetPrice(BotContext bot, Config config)
        {
            WebCallResult<IEnumerable<BinancePrice>> binanceFirstPrice = await _binanceRestClient.SpotApi.ExchangeData.GetPricesAsync();
            if (!binanceFirstPrice.Success)
            {
                LogHelper.LogError($"首次查询价格失败，{binanceFirstPrice.Error!.Code} {binanceFirstPrice.Error!.Message}");
                await bot.SendMessageToAdmin($"首次查询价格失败，{binanceFirstPrice.Error!.Code} {binanceFirstPrice.Error!.Message}");
                return;
            }

            long now = DateTimeOffset.Now.ToUnixTimeSeconds();
            SpotPrices[now] = new ConcurrentDictionary<string, decimal>();
            foreach (var item in binanceFirstPrice.Data)
            {
                if (!item.Symbol.EndsWith("USDT"))
                    continue;
                if (item.Price == 0)
                    continue;
                SpotPrices[now][item.Symbol[..^4]] = item.Price;
            }

            await _binanceWsClient.SpotApi.ExchangeData.SubscribeToAllTickerUpdatesAsync(async onData =>
            {
                long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (SpotPrices.ContainsKey(timestamp))
                    return;
                var lastPrice = SpotPrices.Last().Value;
                SpotPrices[timestamp] = new ConcurrentDictionary<string, decimal>();
                foreach (var item in lastPrice)
                {
                    if (item.Value == 0)
                        continue;
                    SpotPrices[timestamp][item.Key] = item.Value;
                }
                foreach (var item in onData.Data)
                {
                    if (!item.Symbol.EndsWith("USDT"))
                        continue;
                    SpotPrices[timestamp][item.Symbol[..^4]] = item.LastPrice;
                }

                if (SpotPrices.ContainsKey(timestamp - 1))
                {
                    if (SpotPrices[timestamp].Count > SpotPrices[timestamp - 1].Count)
                    {
                        var newSymbols = SpotPrices[timestamp].Keys.Except(SpotPrices[timestamp - 1].Keys);

                        foreach (var item in config.NewSymbolNotifyGroups)
                        {
                            MessageBuilder msg = MessageBuilder.Group(item).Text($"新币：{string.Join(',', newSymbols)} 刚刚在币安上架了");
                            await bot.SendMessage(msg.Build());
                        }
                    }
                }

                while (SpotPrices.Count > 4 * 60 * 60 + 5)
                    SpotPrices.Remove(SpotPrices.Keys.First());
            });
        }

        internal static async Task<string> Get24hRiseOrFall(int top, bool isLong)
        {
            var binance24Hrs = await _binanceRestClient.SpotApi.ExchangeData.GetTickersAsync();  //https://www.binance.com/api/v3/ticker/24hr
            if (!binance24Hrs.Success)
                throw new Exception($"{binance24Hrs.Error!.Code} {binance24Hrs.Error!.Message}");
            IEnumerable<IBinanceTick> data24hr = binance24Hrs.Data;

            string sideText = isLong ? "涨幅" : "跌幅";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"币种          {sideText}          现价");
            if (isLong)
            {
                foreach (var item in data24hr.Where(s => s.Symbol.EndsWith("USDT")).OrderByDescending(p => p.PriceChangePercent).Take(top))
                {
                    string symbol = item.Symbol[..^4];
                    string priceChangePercent = $"+{item.PriceChangePercent:0.00}";
                    sb.AppendLine($"{symbol}  |  {priceChangePercent}%  |  {item.LastPrice.ToString()[..10]}");
                }
            }
            else
            {
                foreach (var item in data24hr.Where(s => s.Symbol.EndsWith("USDT")).OrderBy(p => p.PriceChangePercent).Take(top))
                {
                    string symbol = item.Symbol[..^4];
                    string priceChangePercent = $"{item.PriceChangePercent:0.00}";
                    sb.AppendLine($"{symbol}  |  {priceChangePercent}%  |  {item.LastPrice.ToString()[..10]}");
                }
            }
            return sb.ToString();
        }

        internal static string GetAnyTimeRiseOrFall(int top, bool isLong, long timeDif)
        {
            long now = DateTimeOffset.Now.ToUnixTimeSeconds();
            int retryCount = 0;
            while (!SpotPrices.ContainsKey(now))
            {
                now--;
                retryCount++;
                if (retryCount > 5)
                    return "获取当前数据失败，请稍后再试";
            }
            long before = now - timeDif;
            if (!SpotPrices.ContainsKey(before))
                return "数据收集中，请稍后再试";

            ConcurrentDictionary<string, decimal> nowPrice = SpotPrices[now];
            ConcurrentDictionary<string, decimal> beforePrice = SpotPrices[before];
            Dictionary<string, decimal> priceDif = new Dictionary<string, decimal>();
            foreach (var item in beforePrice)
            {
                decimal value = Math.Round((item.Value - nowPrice[item.Key]) / item.Value * 100, 2);
                priceDif.Add(item.Key, value);
            }

            string sideText = isLong ? "涨幅" : "跌幅";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"币种          {sideText}         现价");
            if (isLong)
            {
                foreach (var item in priceDif.OrderByDescending(p => p.Value).Take(top))
                {
                    decimal d = Math.Round(item.Value, 2);
                    string val = "+" + d.ToString("0.00");
                    sb.AppendLine($"{item.Key}  |  {val}%  |  {nowPrice[item.Key].ToString()[..10]}");
                }
            }
            else
            {
                foreach (var item in priceDif.OrderBy(p => p.Value).Take(top))
                {
                    decimal d = Math.Round(item.Value, 2);
                    string val = (d > 0 ? "+" : "") + d.ToString("0.00");
                    sb.AppendLine($"{item.Key}  |  {val}%  |  {nowPrice[item.Key].ToString()[..10]}");
                }
            }
            return sb.ToString();
        }


        internal static Dictionary<string, TimeData> CommandToData = new Dictionary<string, TimeData>
        {
            { "5sr", new TimeData{ Second = 5, Text = "5s涨跌幅榜", Side = PositionSide.Both, ShowCommand = false, } },
            { "30sr", new TimeData{ Second = 30, Text = "30s涨跌幅榜", Side = PositionSide.Both, ShowCommand = true, } },
            { "1mr", new TimeData{ Second = 60, Text = "1min涨跌幅榜", Side = PositionSide.Both, ShowCommand = false, } },
            { "5mr", new TimeData{ Second = 5 * 60, Text = "5min涨跌幅榜", Side = PositionSide.Both, ShowCommand = true, } },
            { "30mr", new TimeData{ Second = 30 * 60, Text = "30min涨跌幅榜", Side = PositionSide.Both, ShowCommand = false, } },
            { "1hr", new TimeData{ Second = 60 * 60, Text = "1h涨跌幅榜", Side = PositionSide.Both, ShowCommand = true, } },
            { "4hr", new TimeData{ Second = 4 * 60 * 60, Text = "4h涨跌幅榜", Side = PositionSide.Both, ShowCommand = true, } },
            { "24hr", new TimeData{ Second = 24 * 60 * 60, Text = "24h涨跌幅榜", Side = PositionSide.Both, ShowCommand = true, } },

            { "5srl", new TimeData{ Second = 5, Text = "5s涨幅榜", Side = PositionSide.Long, ShowCommand = false, } },
            { "30srl", new TimeData{ Second = 30, Text = "30s涨幅榜", Side = PositionSide.Long, ShowCommand = true, } },
            { "1mrl", new TimeData{ Second = 60, Text = "1min涨幅榜", Side = PositionSide.Long, ShowCommand = false, } },
            { "5mrl", new TimeData{ Second = 5 * 60, Text = "5min涨幅榜", Side = PositionSide.Long, ShowCommand = true, } },
            { "30mrl", new TimeData{ Second = 30 * 60, Text = "30min涨幅榜", Side = PositionSide.Long, ShowCommand = false, } },
            { "1hrl", new TimeData{ Second = 60 * 60, Text = "1h涨幅榜", Side = PositionSide.Long, ShowCommand = true, } },
            { "4hrl", new TimeData{ Second = 4 * 60 * 60, Text = "4h涨幅榜", Side = PositionSide.Long, ShowCommand = true, } },
            { "24hrl", new TimeData{ Second = 24 * 60 * 60, Text = "24h涨幅榜", Side = PositionSide.Long, ShowCommand = true, } },

            { "5srs", new TimeData{ Second = 5, Text = "5s跌幅榜", Side = PositionSide.Short, ShowCommand = false, } },
            { "30srs", new TimeData{ Second = 30, Text = "30s跌幅榜", Side = PositionSide.Short, ShowCommand = true, } },
            { "1mrs", new TimeData{ Second = 60, Text = "1min跌幅榜", Side = PositionSide.Short, ShowCommand = false, } },
            { "5mrs", new TimeData{ Second = 5 * 60, Text = "5min跌幅榜", Side = PositionSide.Short, ShowCommand = true, } },
            { "30mrs", new TimeData{ Second = 30 * 60, Text = "30min跌幅榜", Side = PositionSide.Short, ShowCommand = false, } },
            { "1hrs", new TimeData{ Second = 60 * 60, Text = "1h跌幅榜", Side = PositionSide.Short, ShowCommand = true, } },
            { "4hrs", new TimeData{ Second = 4 * 60 * 60, Text = "4h跌幅榜", Side = PositionSide.Short, ShowCommand = true, } },
            { "24hrs", new TimeData{ Second = 24 * 60 * 60, Text = "24h跌幅榜", Side = PositionSide.Short, ShowCommand = true, } },
        };
    }
}
