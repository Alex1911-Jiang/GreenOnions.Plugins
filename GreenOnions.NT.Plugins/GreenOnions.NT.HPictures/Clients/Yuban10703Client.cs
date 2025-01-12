﻿using System.Net;
using System.Text;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Helpers;
using GreenOnions.NT.HPictures.Models.Yuban10703;
using Lagrange.Core;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.HPictures.Clients
{
    internal static class Yuban10703Client
    {
        internal static async IAsyncEnumerable<MessageBuilder> CreateMessage(string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            LogHelper.LogMessage($"在Yuban10703C查找{num}张{(r18 ? "R-18" : "")}的{keyword}色图");

            string keywordParam = keyword.KeyworkToParams();
            string numParam = $"num={num}";
            string r18Param = $"r18={(r18 ? 1 : 0)}";
            string proxyParam = $"replace_url=https://{config.PrixvProxy}";
            List<string> requestParams = new(3) { numParam, r18Param, proxyParam };
            if (!string.IsNullOrEmpty(keywordParam))
                requestParams.Add(keywordParam);
            string paramUrl = string.Join('&', requestParams);

            string strUrl = $@"https://setu.yuban10703.xyz/setu?{paramUrl}";

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
                LogHelper.LogException(ex, $"访问Yuban10703色图API发生错误，请求地址为：{strUrl}");
                throw;
            }
            Yuban10703RestResult? restResult = JsonConvert.DeserializeObject<Yuban10703RestResult>(respJson);

            if (restResult is null)
            {
                await context.ReplyAsync(chain, config.ErrorReply.Replace("<错误信息>", ""));
                yield break;
            }
            if (!string.IsNullOrWhiteSpace(restResult.detail))
            {
                await context.ReplyAsync(chain, config.ErrorReply.Replace("<错误信息>", restResult.detail));
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
                    sb.AppendLine($"作品页：https://www.pixiv.net/artworks/{item.artwork.id} (p{item.page})");
                if (config.SendProxyUrl)
                    sb.AppendLine($"图片代理地址：{item.urls.original}");
                if (config.SendTitle)
                    sb.AppendLine($"标题:{item.artwork.title}\r\n作者:{item.author.name}");
                if (config.SendTags)
                    sb.AppendLine($"标签:{string.Join(',', item.tags)}");

                HttpClientHandler httpClientHandler = new HttpClientHandler();
                if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                    httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
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
    }
}
