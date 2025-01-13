﻿using System.Net;
using System.Text;
using System.Web;
using GreenOnions.NT.Base;
using GreenOnions.NT.PictureSearcher.Models.SauceNAO;
using HtmlAgilityPack;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Newtonsoft.Json;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal static class SauceNAOClient
    {
        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            try
            {
                if (config.SauceNAOUseChromium)
                    return await SearchWithChromium(commonConfig, config, context, chain, imageUrl);
                else
                    return await SearchWithHttpClient(commonConfig, config, context, chain, imageUrl);
            }
            catch (Exception ex)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", ex.Message));
                return 0;
            }
        }

        private async static Task<double> SearchWithHttpClient(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            bool withApiKey = config.SauceNAOApiKey.Count > 0;

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                {new StringContent("999"), "db" },
                {new StringContent("1"), "testmode" },
                {new StringContent("16"), "numres" },
                {new StringContent(imageUrl), "url" },
            };

            if (withApiKey)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                string[] apikeys = config.SauceNAOApiKey.ToArray();
                int index = random.Next(0, apikeys.Length);
                content.Add(new StringContent(apikeys[index]), "api_key");
                content.Add(new StringContent("2"), "output_type");
            }

            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
            using HttpClient client = new HttpClient(httpClientHandler);
            var resp = await client.PostAsync("https://SauceNAO.com/search.php", content);

            if (!resp.IsSuccessStatusCode)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}"));
                return 0;
            }

            string sauceNAOResult = await resp.Content.ReadAsStringAsync();

            if (withApiKey)
                return await AnalysisJson(commonConfig, config, context, chain, sauceNAOResult);
            else
                return await AnalysisHtml(commonConfig, config, context, chain, sauceNAOResult);
        }

        private static async Task<double> SearchWithChromium(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl)
        {
            bool withApiKey = config.SauceNAOApiKey.Count > 0;
            string apiKeyStr = "";
            if (withApiKey)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                string[] apikeys = config.SauceNAOApiKey.ToArray();
                int index = random.Next(0, apikeys.Length);
                apiKeyStr = $"&api_key={apikeys[index]}&output_type=2";
            }
            string sauceNAOUrl = @$"https://SauceNAO.com/search.php?db=999{apiKeyStr}&testmode=1&numres=16&url={HttpUtility.UrlEncode(imageUrl)}";

            string sauceNAOResult;
            try
            {
                sauceNAOResult = await Chromium.GetStringAsync(sauceNAOUrl);
            }
            catch (Exception ex)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", ex.Message));
                return 0;
            }

            if (withApiKey)
                return await AnalysisJson(commonConfig, config, context, chain, sauceNAOResult);
            else
                return await AnalysisHtml(commonConfig, config, context, chain, sauceNAOResult);
        }


        public static async Task<double> AnalysisJson(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string json)
        {
            SauceNAOJsonResult sauceNAOJsonResult;
            try
            {
                SauceNAOJsonResult? sauceNAOobj = JsonConvert.DeserializeObject<SauceNAOJsonResult>(json);
                if (sauceNAOobj is null)
                {
                    await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", ""));
                    return 0;
                }
                sauceNAOJsonResult = sauceNAOobj;
            }
            catch (Exception ex)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", ex.Message));
                return 0;
            }

            if (sauceNAOJsonResult.header.status != 0)
                LogHelper.LogWarning($"SauceNAO返回状态码为{sauceNAOJsonResult.header.status}，搜索可能没有成功");

            if (sauceNAOJsonResult.results.Length == 0)
            {
                await context.ReplyAsync(chain, config.SearchNoResultReply.Replace("<搜索类型>", "SauceNAO"));
                return 0;
            }

            SauceNAOResult result = sauceNAOJsonResult.results.First();

            double similarity = result.header.similarity;
            //缩略图地址
            string thuImgUrl = result.header.thumbnail;
            //作品地址
            string[] ext_urls = result.data.ext_urls;

            string? title = result.data.title;
            string? pixiv_id = result.data.pixiv_id;
            string? member_name = result.data.member_name;

            string? creator = null;
            if (result.data.creator is string)
                creator = result.data.creator.ToString();
            string? material = result.data.material;
            string? characters = result.data.characters;
            string? source = result.data.source;

            StringBuilder sb = new StringBuilder();
            foreach (var item in ext_urls)
                sb.AppendLine($"作品页：{item}");

            if (!string.IsNullOrEmpty(source))
                sb.AppendLine($"图片来源：{source}");
            sb.AppendLine($"相似度：{similarity}% (SauceNAO)");  //一定有相似度
            if (!string.IsNullOrEmpty(title))
                sb.AppendLine($"标题：{title}");
            if (!string.IsNullOrEmpty(member_name))
                sb.AppendLine($"作者：{member_name}");
            else if (!string.IsNullOrEmpty(creator))
                sb.AppendLine($"作者：{creator}");
            if (!string.IsNullOrEmpty(characters))
                sb.AppendLine($"角色：{characters}");
            if (!string.IsNullOrEmpty(material))
                sb.AppendLine($"所属：{material}");

            await SendSearchResult(commonConfig, config, context, chain, sb.ToString(), thuImgUrl, similarity);
            return similarity;
        }

        public static async Task<double> AnalysisHtml(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string html)
        {
            HtmlDocument sauceNAOdoc = new HtmlDocument();
            sauceNAOdoc.LoadHtml(html);
            string thuImgUrl;
            string similarityText;
            string? title;
            string? pid;
            string? url;
            string? author;
            try
            {
                HtmlNode resultNode = sauceNAOdoc.DocumentNode.SelectSingleNode("//div[@id='middle']/div[@class='result'][1]/table[@class='resulttable']");
                thuImgUrl = HttpUtility.HtmlDecode(resultNode.SelectSingleNode("//td[@class='resulttableimage']/div[@class='resultimage']/a[@class='linkify']/img[@id='resImage0']").Attributes["src"].Value);
                similarityText = resultNode.SelectSingleNode("//td[@class='resulttablecontent']/div[@class='resultmatchinfo']/div[@class='resultsimilarityinfo']").InnerText;
                title = resultNode.SelectSingleNode("//td[@class='resulttablecontent']/div[@class='resultcontent']/div[@class='resulttitle']/strong")?.InnerText;
                HtmlNode? idNode = resultNode.SelectSingleNode("//td[@class='resulttablecontent']/div[@class='resultcontent']/div[@class='resultcontentcolumn']/a[@class='linkify'][1]");
                pid = idNode?.InnerText;
                url = idNode?.Attributes["href"]?.Value;
                author = resultNode.SelectSingleNode("//td[@class='resulttablecontent']/div[@class='resultcontent']/div[@class='resultcontentcolumn']/a[@class='linkify'][2]")?.InnerText;

            }
            catch (Exception ex)
            {
                await context.ReplyAsync(chain, config.SearchErrorReply.Replace("<搜索类型>", "SauceNAO").Replace("<错误信息>", ex.Message));
                return 0;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"相似度：{similarityText} (SauceNAO)");
            if (!string.IsNullOrWhiteSpace(url))
                sb.AppendLine($"作品页：{HttpUtility.HtmlDecode(url)}");
            if (!string.IsNullOrWhiteSpace(title))
                sb.AppendLine($"标题：{title}");
            if (!string.IsNullOrWhiteSpace(author))
                sb.AppendLine($"作者：{author}");

            double similarity = Convert.ToDouble(similarityText.Replace("%", ""));
            await SendSearchResult(commonConfig, config, context, chain, sb.ToString(), thuImgUrl, similarity);
            return similarity;
        }

        private static async Task SendSearchResult(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string infoMsg, string thuImgUrl, double similarity)
        {
            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
            msg = msg.Text(infoMsg);

            if (similarity < config.SendThuImgSimilarity && !string.IsNullOrWhiteSpace(config.LowSimilarityReply))
            {
                await context.SendMessage(msg.Text(config.LowSimilarityReply).Build());  //低于发送缩略图的相似度，并且有回复语
                return;
            }

            if (similarity < config.SendThuImgSimilarity)
            {
                await context.SendMessage(msg.Build());  //低于发送缩略图的相似度，但没有回复语
                return;
            }

            //高于或等于发送缩略图的相似度
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            if (config.UseProxy && !string.IsNullOrWhiteSpace(commonConfig.ProxyUrl))
                httpClientHandler.Proxy = new WebProxy(commonConfig.ProxyUrl) { Credentials = new NetworkCredential(commonConfig.ProxyUserName, commonConfig.ProxyPassword) };
            using HttpClient client = new HttpClient(httpClientHandler);
            try
            {
                var resp = await client.GetAsync(thuImgUrl);
                if (!resp.IsSuccessStatusCode)  //下载缩略图失败
                {
                    await context.SendMessage(msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", $"{(int)resp.StatusCode} {resp.StatusCode}")).Build());
                    return;
                }
                byte[] img = await resp.Content.ReadAsByteArrayAsync();
                await context.SendMessage(msg.Image(img).Build());
            }
            catch (Exception ex)
            {
                await context.SendMessage(msg.Text(config.DownloadThuImageFailReply.Replace("<机器人名称>", commonConfig.BotName).Replace("<错误信息>", ex.Message)).Build());
            }
        }
    }
}
