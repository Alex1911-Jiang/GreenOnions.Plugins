using System.Net;
using System.Text;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Helpers;
using GreenOnions.NT.HPictures.Models.Lolicon;
using Lagrange.Core;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.HPictures.Clients
{
    internal static class LoliconClient
    {
        internal static async IAsyncEnumerable<MessageBuilder> CreateMessage(string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            string keywordParam = KeyworkToParams(keyword);
            string numParam = $"num={num}";
            string r18Param = $"r18={(r18 ? 1 : 0)}";
            string proxyParam = $"proxy=i.{config.PrixvProxy}";
            List<string> requestParams = new(3) { numParam, r18Param, proxyParam };
            if (!string.IsNullOrEmpty(keywordParam))
                requestParams.Add(keywordParam);
            string paramUrl = string.Join('&', requestParams);

            string strUrl = $@"https://api.lolicon.app/setu/v2?{paramUrl}";

            string? respJson = null;

            try
            {
                if (config.LoliconRequestByChromium)
                {
                    respJson = await Chromium.GetStringAsync(strUrl);
                }
                else
                {
                    using HttpClient client = new HttpClient();
                    respJson = await client.GetStringAsync(strUrl);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"访问色图API发生错误，请求地址为：{strUrl}");
                throw;
            }
            LoliconRestResult? restResult = JsonConvert.DeserializeObject<LoliconRestResult>(respJson);

            if (restResult is null)
            {
                await context.ReplyAsync(chain, config.ErrorReply.Replace("<错误信息>", ""));
                yield break;
            }
            if (!string.IsNullOrWhiteSpace(restResult.error))
            {
                await context.ReplyAsync(chain, config.ErrorReply.Replace("<错误信息>", restResult.error));
                yield break;
            }
            if (restResult.data.Length == 0)
            {
                await context.ReplyAsync(chain, config.NoResultReply);
                yield break;
            }

            foreach (var item in restResult.data)
            {
                StringBuilder sb = new StringBuilder();
                if (config.SendUrl)
                    sb.AppendLine($"作品页：https://www.pixiv.net/artworks/{item.pid} (p{item.p})");
                if (config.SendProxyUrl)
                    sb.AppendLine($"图片代理地址：{item.urls.original}");
                if (config.SendTitle)
                    sb.AppendLine($"标题:{item.title}\r\n作者:{item.author}");
                if (config.SendTags)
                    sb.AppendLine($"标签:{string.Join(',', item.tags)}");

                HttpClientHandler httpClientHandler = new HttpClientHandler();
                if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl)) { httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) }; }
                using HttpClient client = new HttpClient(httpClientHandler);

                byte[] img = await client.GetByteArrayAsync(item.urls.original);

                if (config.AntiShielding)
                    img = ImageHelper.AntiShielding(img);

                if (chain.GroupUin is null)
                    yield return MessageBuilder.Friend(chain.FriendUin).Text(sb.ToString()).Image(img);
                else
                    yield return MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(sb.ToString()).Image(img);
            }
        }

        private static string KeyworkToParams(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return string.Empty;

            if (keyword.Contains('&') || keyword.Contains('|'))
            {
                string[] ands = keyword.Split('&');
                return "tag=" + string.Join("&tag=", ands);
            }
            else
                return "&tag=" + keyword;
        }
    }
}
