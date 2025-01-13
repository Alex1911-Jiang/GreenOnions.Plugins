using System.Net;
using System.Text;
using System.Web;
using GreenOnions.NT.Base;
using HtmlAgilityPack;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal static class Ascii2dClient
    {
        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            try
            {
                if (config.Ascii2dUseChromium)
                    await SearchByChromium(commonConfig, config, context, chain, imageUrl);
                else
                    await SearchByHttpClient(commonConfig, config, context, chain, imageUrl);
            }
            catch (Exception ex)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "Ascii2d").Replace("<错误信息>", ex.Message));
            }
            return 0;
        }

        private static async Task SearchByHttpClient(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
            using HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("DotNetRuntime/8.0");

            var colorRequest = new HttpRequestMessage(HttpMethod.Post, "https://ascii2d.net/search/uri");
            string sessionId = Guid.NewGuid().ToString().Replace("-", "");
            colorRequest.Headers.Add("Cookie", $"_session_id={sessionId}");
            var collection = new List<KeyValuePair<string, string>>
                {
                    new("uri", imageUrl)
                };
            var content = new FormUrlEncodedContent(collection);
            colorRequest.Content = content;
            var colorResponse = await client.SendAsync(colorRequest);

            if (colorResponse.IsSuccessStatusCode)
            {
                string colorHtml = await colorResponse.Content.ReadAsStringAsync();
                await AnalysisHtml(commonConfig, config, context, chain, colorHtml);
            }
            else
            {
                LogHelper.LogError($"Ascii2d色合検索{imageUrl}失败 {(int)colorResponse.StatusCode} {colorResponse.StatusCode}");
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "Ascii2d 色合検索").Replace("<错误信息>", $"{(int)colorResponse.StatusCode} {colorResponse.StatusCode}"));
            }

            string bovwUrl = colorResponse.RequestMessage!.RequestUri!.AbsoluteUri.Replace("/color/", "/bovw/");
            var bovwRequest = new HttpRequestMessage(HttpMethod.Get, bovwUrl);
            bovwRequest.Headers.Add("Cookie", $"_session_id={sessionId}");
            var bovwResponse = await client.SendAsync(bovwRequest);
            if (bovwResponse.IsSuccessStatusCode)
            {
                string bovwHtml = await bovwResponse.Content.ReadAsStringAsync();
                await AnalysisHtml(commonConfig, config, context, chain, bovwHtml);
            }
            else
            {
                LogHelper.LogError($"Ascii2d特徴検索{imageUrl}失败 {(int)colorResponse.StatusCode} {colorResponse.StatusCode}");
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "Ascii2d 特徴検索").Replace("<错误信息>", $"{(int)colorResponse.StatusCode} {colorResponse.StatusCode}"));
            }
        }

        private static async Task SearchByChromium(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            string colorUrl = await Chromium.GetNavigationUrlAsync($"https://ascii2d.net/search/url/{HttpUtility.UrlEncode(imageUrl)}");
            string colorHtml = await Chromium.GetStringAsync(colorUrl);
            await AnalysisHtml(commonConfig, config, context, chain, colorHtml);

            string bovwUrl = colorUrl.Replace("/color/", "/bovw/");
            string bovwHtml = await Chromium.GetStringAsync(bovwUrl);
            await AnalysisHtml(commonConfig, config, context, chain, bovwHtml);
        }

        public static async Task AnalysisHtml(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string html)
        {
            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string searchMode = doc.DocumentNode.SelectSingleNode("//div[@class='col-xs-12 col-lg-8 col-xl-8']/h5[@class='p-t-1 text-xs-center']").InnerText;
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='col-xs-12 col-lg-8 col-xl-8']/div[@class='row item-box']");
            int sendCount = Math.Min(config.Ascii2dResultNum, nodes.Count);
            for (int i = 0; i < sendCount; i++)
            {
                string thuImgUrl = nodes[i].SelectSingleNode("div[@class='col-xs-12 col-sm-12 col-md-4 col-xl-4 text-xs-center image-box']/img").Attributes["src"].Value;
                HtmlNode? titleNode = nodes[i].SelectSingleNode("div[@class='col-xs-12 col-sm-12 col-md-8 col-xl-8 info-box']/div[@class='detail-box gray-link']/h6/a[1]");

                if (titleNode is null)  //拿搜索过的缩略图去搜索第一个完全匹配的会是Ascii的缓存，没有地址，跳过
                {
                    sendCount++;
                    continue;
                }

                string title = titleNode.InnerText;
                string url = titleNode.Attributes["href"].Value;
                string anchor = nodes[i].SelectSingleNode("div[@class='col-xs-12 col-sm-12 col-md-8 col-xl-8 info-box']/div[@class='detail-box gray-link']/h6/a[2]").InnerText;
                string source = nodes[i].SelectSingleNode("div[@class='col-xs-12 col-sm-12 col-md-8 col-xl-8 info-box']/div[@class='detail-box gray-link']/h6/small").InnerText;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"作品页：{url}");
                sb.AppendLine($"标题：{title}");
                sb.AppendLine($"作者：{anchor}");
                sb.AppendLine($"来源：{source.Trim()}");

                msg = msg.Text(sb.ToString());

                try
                {
                    HttpClientHandler httpClientHandler = new HttpClientHandler();
                    if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                        httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
                    using HttpClient client = new HttpClient(httpClientHandler);
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("DotNetRuntime/8.0");
                    var resp = await client.GetAsync($"https://ascii2d.net{thuImgUrl}");
                    if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                    {
                        msg = msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}"));
                        continue;
                    }
                    byte[] img = await resp.Content.ReadAsByteArrayAsync();
                    msg = msg.Image(img);
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"下载Ascii2d缩略图{thuImgUrl}失败");
                    msg = msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", ex.Message));
                }
            }
            msg = msg.Text($"(Ascii2d {searchMode})");
            await context.SendMessage(msg.Build());
        }
    }
}
