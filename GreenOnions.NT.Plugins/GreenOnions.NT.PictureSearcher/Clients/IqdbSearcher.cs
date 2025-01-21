using System.Net;
using System.Text;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Enums;
using IqdbApi;
using IqdbApi.Enums;
using Lagrange.Core;
using Lagrange.Core.Message;
using Lagrange.Core.Common.Interface.Api;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal static class IqdbSearcher
    {
        public static async Task<double> SearchAnime(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            return await Search(commonConfig, config, context, chain, imageUrl, IqdbModes.Anime);
        }

        public static async Task<double> Search3d(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            return await Search(commonConfig, config, context, chain, imageUrl, IqdbModes.ThreeDimensional);
        }

        private static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl, IqdbModes mode)
        {
            string hostName = mode == IqdbModes.Anime ? "anime" : "3d;";

            LogHelper.LogMessage($"请求{hostName}.iqdb.org搜索{imageUrl}");

            using IIqdbClient iqdbClient = new IqdbClient();
            var iqdbResults = await iqdbClient.SearchUrl(imageUrl);
            if (iqdbResults is null || iqdbResults.Matches.Count == 0)
            {
                LogHelper.LogMessage($"{hostName}没有搜索到任何结果");
                await chain.ReplyAsync(config.SearchNoResultReply.Replace("<搜索类型>", hostName));
                return 0;
            }

            var firstItem = iqdbResults.Matches.First();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"作品页：{firstItem.Url}");
            sb.AppendLine($"相似度：{firstItem.Similarity}% ({firstItem.Source.ToString().Replace("_", "")})");
            sb.AppendLine($"年龄分级：" + firstItem.Rating);
            if (config.IqdbSendTags)  //发送标签(英文)
                sb.AppendLine($"标签：{string.Join(',', firstItem.Tags)}");

            LogHelper.LogMessage($"执行{hostName}搜索成功");

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

            bool sendThuImg = mode == IqdbModes.Anime ? config.IqdbAnimeSendThuImg : config.Iqdb3dSendThuImg;
            if (!sendThuImg || firstItem.Similarity < config.SendThuImgSimilarity)
            {
                await context.SendMessage(msg.Build());  //没有打开发送缩略图或低于发送缩略图的相似度，但没有回复语
                return firstItem.Similarity;
            }

            if (config.IqdbSendThuImgMustSafe && firstItem.Rating != Rating.Safe)
            {
                await context.SendMessage(msg.Build());  //只发送Safe的缩略图，且搜索结果不是Safe
                return firstItem.Similarity;
            }

            //高于或等于发送缩略图的相似度
            using HttpClientHandler httpClientHandler = new HttpClientHandler { UseProxy = config.UseProxy };
            using HttpClient client = new HttpClient(httpClientHandler);
            try
            {
                string domain = mode == IqdbModes.Anime ? "www" : "3d;";
                string thuImgUrl = $"https://{domain}.iqdb.org{firstItem.PreviewUrl}";
                var resp = await client.GetAsync(thuImgUrl);
                if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                {
                    await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}")).Build());
                    return firstItem.Similarity;
                }
                byte[] img = await resp.Content.ReadAsByteArrayAsync();
                await context.SendMessage(msg.Image(img).Build());
            }
            catch (Exception ex)
            {
                await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", ex.Message)).Build());
            }
            return firstItem.Similarity;
        }
    }
}
