using System.Text;
using System.Web;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Models.Soutubot;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal static class SoutubotSearcher
    {
        private static long _m = 0;

        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            try
            {
                return await SearchInner(commonConfig, config, context, chain, imageUrl);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"搜图bot酱搜图发生异常，错误信息：{ex.Message}");
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", ex.Message));
                return 0;
            }
        }

        public static async Task<double> SearchInner(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            using HttpClientHandler httpClientHandler = new() { UseProxy = config.UseProxy };
            using HttpClient client = new(httpClientHandler);

            Stream searchImg = await client.GetStreamAsync(imageUrl);

            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0";
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

            if (_m == 0)
            {
                LogHelper.LogMessage("不存在令牌，访问 https://soutubot.moe/ 获取IP令牌");
                try
                {
                    string html = await client.GetStringAsync("https://soutubot.moe/");
                    int start = html.IndexOf("m: ") + "m: ".Length;
                    int end = html.IndexOf(",", start);
                    string m = html.Substring(start, end - start).Trim();
                    _m = Convert.ToInt64(m);
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, "首次访问https://soutubot.moe/获取IP令牌失败");
                    await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", ex.Message));
                    return 0;
                }
            }

            double e = Math.Pow(DateTimeOffset.Now.ToUnixTimeSeconds(), 2) + Math.Pow(userAgent.Length, 2) + Convert.ToInt64(_m);
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(e.ToString()));
            char[] charArray = encoded.ToCharArray();
            Array.Reverse(charArray);
            string apiKey = new string(charArray).Replace("=", "");

            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
            using MultipartFormDataContent content = new()
            {
                { new StreamContent(searchImg), "file", "image"},
                { new StringContent("1.2"), "factor"},
            };

            string json;
            try
            {
                LogHelper.LogMessage("向 https://soutubot.moe/api/search 搜图");
                HttpResponseMessage response = await client.PostAsync("https://soutubot.moe/api/search", content);
                if (!response.IsSuccessStatusCode)
                {
                    string errMsg = "";
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        _m = 0;
                        errMsg = "令牌已过期，请重试（出现该问题请联系机器人作者）";
                    }
                    await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", errMsg));
                    return 0;
                }
                json = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "访问https://soutubot.moe/api/search搜索失败");
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", ex.Message));
                return 0;
            }

            SoutubotResult result;
            try
            {
                SoutubotResult? soutubotObj = JsonConvert.DeserializeObject<SoutubotResult>(json);
                if (soutubotObj is null)
                {
                    await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", ""));
                    return 0;
                }
                result = soutubotObj;
            }
            catch (Exception ex)
            {
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "搜图bot酱").Replace("<错误信息>", ex.Message));
                return 0;
            }

            if (result.data.Length == 0)
            {
                await chain.ReplyAsync(config.SearchNoResultReply.Replace("<搜索类型>", "搜图bot酱"));
                return 0;
            }

            LogHelper.LogMessage("搜图bot酱搜索成功");

            StringBuilder sb = new StringBuilder();
            SoutubotDatum data = result.data.First();
            sb.AppendLine($"来源：{data.source}");
            sb.AppendLine($"页数：{data.page}");
            sb.AppendLine($"标题：{data.title}");
            sb.AppendLine($"相似度：{data.similarity}%");

            sb.AppendLine("(搜图bot酱)");

            LogHelper.LogMessage($"执行搜图bot酱搜索成功");

            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
            msg = msg.Text(sb.ToString());

            if (data.similarity < config.SendThuImgSimilarity && !string.IsNullOrWhiteSpace(config.LowSimilarityReply))
            {
                await context.SendMessage(msg.Text(config.LowSimilarityReply.Replace("<SendThuImgSimilarity>", config.SendThuImgSimilarity.ToString())).Build());  //低于发送缩略图的相似度，并且有回复语
                return data.similarity;
            }

            if (!config.SauceNAOSendThuImg || data.similarity < config.SendThuImgSimilarity)
            {
                await context.SendMessage(msg.Build());  //没有打开发送缩略图或低于发送缩略图的相似度，但没有回复语
                return data.similarity;
            }

            string thuImgUrl = HttpUtility.UrlDecode(data.previewImageUrl.Replace("\\/", "/"));

            //高于或等于发送缩略图的相似度
            try
            {
                var resp = await client.GetAsync(thuImgUrl);
                if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                {
                    LogHelper.LogError($"下载SauceNAO搜索结果缩略图{thuImgUrl}失败 {$"{(int)resp.StatusCode} {resp.StatusCode}"}");
                    await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}")).Build());
                    return data.similarity;
                }
                byte[] resultImg = await resp.Content.ReadAsByteArrayAsync();
                await context.SendMessage(msg.Image(resultImg).Build());
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"下载SauceNAO搜索结果缩略图{thuImgUrl}失败，错误信息：{ex.Message}");
                await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", ex.Message)).Build());
            }
            return 0;
        }
    }
}
