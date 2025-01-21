using System.Text;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Helpers;
using GreenOnions.NT.HPictures.Models.Lolicon;
using Lagrange.Core;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.HPictures.Clients
{
    internal static class BaseLoliconClient
    {
        internal static async IAsyncEnumerable<MessageBuilder> CreateMessage(string apiName, string host, string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            string keywordParam = keyword.KeyworkToParams();
            string numParam = $"num={num}";
            string r18Param = $"r18={(r18 ? 1 : 0)}";
            string proxyParam = $"proxy={config.PrixvProxy}";
            List<string> requestParams = new(3) { numParam, r18Param, proxyParam };
            if (!string.IsNullOrEmpty(keywordParam))
                requestParams.Add(keywordParam);
            string paramUrl = string.Join('&', requestParams);

            string strUrl = $@"{host}?{paramUrl}";

            string? respJson = null;

            try
            {
                if (config.RequestApiByChromium)
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
                LogHelper.LogException(ex, $"访问{apiName}色图API发生错误，请求地址为：{strUrl}");
                throw;
            }
            LoliconRestResult? restResult = JsonConvert.DeserializeObject<LoliconRestResult>(respJson);

            if (restResult is null)
            {
                await chain.ReplyAsync(config.ErrorReply.Replace("<错误信息>", ""));
                yield break;
            }
            if (!string.IsNullOrWhiteSpace(restResult.error))
            {
                await chain.ReplyAsync(config.ErrorReply.Replace("<错误信息>", restResult.error));
                yield break;
            }
            if (restResult.data.Length == 0)
            {
                await chain.ReplyAsync(config.NoResultReply);
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

                MessageBuilder msg;
                if (chain.GroupUin is null)
                    msg = MessageBuilder.Friend(chain.FriendUin).Text(sb.ToString());
                else
                    msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(sb.ToString());

                using HttpClientHandler httpClientHandler = new HttpClientHandler { UseProxy = config.UseProxy };
                using HttpClient client = new HttpClient(httpClientHandler);

                var resp = await client.GetAsync(item.urls.original);

                if (!resp.IsSuccessStatusCode)
                    yield return msg.Text(config.DownloadFailReply.Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}"));

                byte[] img = await resp.Content.ReadAsByteArrayAsync();

                if (config.AntiShielding)
                    img = ImageHelper.AntiShielding(img);

                yield return msg.Image(img);
            }
        }

        internal static string KeyworkToParams(this string keyword)
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
