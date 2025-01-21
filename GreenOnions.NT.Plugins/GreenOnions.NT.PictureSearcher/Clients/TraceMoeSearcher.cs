using System.Net;
using System.Text;
using System.Web;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Models.TraceMoe;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal class TraceMoeSearcher
    {
        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            LogHelper.LogMessage($"请求trace.moe搜索{imageUrl}");
            HttpClientHandler httpClientHandler = new HttpClientHandler() { UseProxy = config.UseProxy };
            using HttpClient client = new HttpClient(httpClientHandler);

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"https://api.trace.moe/search?cutBorders&anilistInfo&url={HttpUtility.UrlEncode(imageUrl)}");
            }
            catch (Exception ex)
            {
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "Trace.moe").Replace("<错误信息>", ex.Message));
                return 0;
            }

            string json = await response.Content.ReadAsStringAsync();

            TraceMoeJsonResult traceMoeJsonResult;
            try
            {
                TraceMoeJsonResult? traceMoeObj = JsonConvert.DeserializeObject<TraceMoeJsonResult>(json);
                if (traceMoeObj is null)
                {
                    await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "trace.moe").Replace("<错误信息>", ""));
                    return 0;
                }
                traceMoeJsonResult = traceMoeObj;
            }
            catch (Exception ex)
            {
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "trace.moe").Replace("<错误信息>", ex.Message));
                return 0;
            }

            if (traceMoeJsonResult.result.Length == 0)
            {
                await chain.ReplyAsync(config.SearchNoResultReply.Replace("<搜索类型>", "trace.moe"));
                return 0;
            }

            var result = traceMoeJsonResult.result.First();

            double similarity = result.similarity * 100;
            TimeSpan timeSpan = new TimeSpan(0, 0, (int)result.from);
            string time;
            if (timeSpan.Hours > 0)
                time = $"{timeSpan.Hours}小时{timeSpan.Minutes}分{timeSpan.Seconds}秒";
            else
                time = $"{timeSpan.Minutes}分{timeSpan.Seconds}秒";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"动画名称：{result.anilist.title.native}");
            sb.AppendLine($"相似度：{similarity:0.00}% (trace.moe)");
            sb.AppendLine($"里：{(result.anilist.isAdult ? "是" : "否")}");
            sb.AppendLine($"第{result.episode ?? 1}集 {time}");

            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
            msg = msg.Text(sb.ToString());

            string thuImgUrl = $"{result.image}&size=m";

            if (similarity < config.SendThuImgSimilarity && !string.IsNullOrWhiteSpace(config.LowSimilarityReply))
            {
                await context.SendMessage(msg.Text(config.LowSimilarityReply.Replace("<SendThuImgSimilarity>", config.SendThuImgSimilarity.ToString())).Build());  //低于发送缩略图的相似度，并且有回复语
                return similarity;
            }

            if (!config.TraceMoeSendThuImg || similarity < config.SendThuImgSimilarity)
            {
                await context.SendMessage(msg.Build());  //没有打开发送缩略图或低于发送缩略图的相似度，但没有回复语
                return similarity;
            }

            if (!config.TraceMoeSendAdultThuImg && result.anilist.isAdult)
            {
                await context.SendMessage(msg.Build());  //不发送里番缩略图，并且结果是里番
                return similarity;
            }

            //高于或等于发送缩略图的相似度
            try
            {
                var resp = await client.GetAsync(thuImgUrl);
                if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                {
                    await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}")).Build());
                    return similarity;
                }
                byte[] img = await resp.Content.ReadAsByteArrayAsync();
                await context.SendMessage(msg.Image(img).Build());
            }
            catch (Exception ex)
            {
                await context.SendMessage(msg.Text(config.DownloadThuImgFailReply.ReplaceTags().Replace("<错误信息>", ex.Message)).Build());
            }
            return similarity;
        }
    }
}
