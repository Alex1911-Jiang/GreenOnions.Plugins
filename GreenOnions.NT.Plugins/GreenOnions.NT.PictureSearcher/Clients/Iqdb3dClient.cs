using System.Text;
using GreenOnions.NT.Base;
using IqdbApi;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.Core.Common.Interface.Api;
using System.Net;
using IqdbApi.Enums;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal class Iqdb3DClient
    {
        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            LogHelper.LogMessage($"请求Iqdb 3d搜索{imageUrl}");

            using IIqdbClient iqdbClient = new Iqdb3dClient();
            var iqdbResults = await iqdbClient.SearchUrl(imageUrl);
            if (iqdbResults is null || iqdbResults.Matches.Count == 0)
            {
                LogHelper.LogMessage($"Iqdb 3d没有搜索到任何结果");
                await context.ReplyAsync(chain, config.SearchNoResultReply.Replace("<搜索类型>", "Iqdb 3d"));
                return 0;
            }

            var firstItem = iqdbResults.Matches.First();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"作品页：{firstItem.Url}");
            sb.AppendLine($"相似度：{firstItem.Similarity}% ({firstItem.Source.ToString().Replace("_", "")})");
            sb.AppendLine($"年龄分级：" + firstItem.Rating);
            if (config.IqdbSendTags)  //发送标签(英文)
                sb.AppendLine($"标签：{string.Join(',', firstItem.Tags)}");

            LogHelper.LogMessage("执行Iqdb 3d搜索成功");

            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
            msg = msg.Text(sb.ToString());

            if (firstItem.Similarity < config.SendThuImgSimilarity && !string.IsNullOrWhiteSpace(config.LowSimilarityReply))
            {
                await context.SendMessage(msg.Text(config.LowSimilarityReply.Replace("<SendThuImgSimilarity>", config.SendThuImgSimilarity.ToString())).Build());  //低于发送缩略图的相似度，并且有回复语
                return firstItem.Similarity;
            }

            if (!config.Iqdb3dSendThuImage || firstItem.Similarity < config.SendThuImgSimilarity)
            {
                await context.SendMessage(msg.Build());  //没有打开发送缩略图或低于发送缩略图的相似度，但没有回复语
                return firstItem.Similarity;
            }

            if (config.IqdbSendThuImageMustSafe && firstItem.Rating != Rating.Safe)
            {
                await context.SendMessage(msg.Build());  //只发送Safe的缩略图，且搜索结果不是Safe
                return firstItem.Similarity;
            }

            //高于或等于发送缩略图的相似度
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
            using HttpClient client = new HttpClient(httpClientHandler);
            try
            {
                string thuImgUrl = $"https://3d.iqdb.org{firstItem.PreviewUrl}";
                var resp = await client.GetAsync(thuImgUrl);
                if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                {
                    await context.SendMessage(msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}")).Build());
                    return firstItem.Similarity;
                }
                byte[] img = await resp.Content.ReadAsByteArrayAsync();
                await context.SendMessage(msg.Image(img).Build());
            }
            catch (Exception ex)
            {
                await context.SendMessage(msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", ex.Message)).Build());
            }
            return firstItem.Similarity;
        }
    }
}
