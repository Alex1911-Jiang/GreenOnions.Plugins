using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using GreenOnions.NT.Base;
using HtmlAgilityPack;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.NT.RSS
{
    public class RssPlugin : IPlugin
    {
        private Dictionary<string, string> _rssSendRecord = new Dictionary<string, string>();
        private Dictionary<string, Timer> _subscribers = new Dictionary<string, Timer>();
        private Config? _config;
        private string? _pluginPath;
        private BotContext? _bot;
        private ICommonConfig? _commonConfig;

        public string Name => "RSS订阅";
        public string Description => "RSS订阅转发插件";
        public void OnConfigUpdated(ICommonConfig commonConfig)
        {
            _commonConfig = commonConfig;
            if (_pluginPath is null)
                return;
            Config config = LoadConfig(_pluginPath);
            StopReadRss();
            StartReadRss();
        }
        private Config LoadConfig(string pluginPath)
        {
            var configPath = Path.Combine(pluginPath, "config.yml");
            if (File.Exists(configPath))
            {
                string yamlConfig = File.ReadAllText(configPath);
                _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
            }
            _config ??= new Config();
            var json = JsonConvert.SerializeObject(_config);
            File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));
            return _config;
        }

        public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
        {
            _pluginPath = pluginPath;
            _bot = bot;
            _commonConfig = commonConfig;
            LoadConfig(_pluginPath);

            bot.Invoker.OnBotOnlineEvent -= OnBotOnlined;
            bot.Invoker.OnBotOnlineEvent += OnBotOnlined;

            bot.Invoker.OnBotOfflineEvent -= OnBotOffline;
            bot.Invoker.OnBotOfflineEvent += OnBotOffline;
        }

        private void OnBotOffline(BotContext context, BotOfflineEvent e) => StopReadRss();
        private void OnBotOnlined(BotContext context, BotOnlineEvent e) => StartReadRss();

        private void StartReadRss()
        {
            if (_config is null)
                return;

            int subscribeCount = 0;
            foreach (SubscriptionItem item in _config.RssSubscription)
            {
                if (string.IsNullOrWhiteSpace(item.Url))
                    continue;
                if (_subscribers.ContainsKey(item.Url))
                {
                    _subscribers[item.Url].Dispose();
                    _subscribers.Remove(item.Url);
                }

                if (item.ReadInterval <= 0)
                {
                    LogHelper.LogWarning($"RSS订阅对象 {item.Remark}（{item.Url}）获取间隔不能为0 ");
                    continue;
                }

                Timer timer = new Timer(RssReaderPipe, item, 0, Convert.ToInt64(item.ReadInterval * 1000));
                _subscribers.Add(item.Url, timer);
                subscribeCount++;
            }
            LogHelper.LogMessage($"{subscribeCount}个RSS订阅线程已启动");
        }

        private void StopReadRss()
        {
            foreach (var item in _subscribers)
                item.Value.Dispose();
            _subscribers.Clear();
            _rssSendRecord.Clear();
        }

        private async void RssReaderPipe(object? state)
        {
            if (System.Diagnostics.Debugger.IsAttached)  //调试用，防止断点时重复进入中断
                return;

            if (state is not SubscriptionItem item)
            {
                LogHelper.LogWarning("参数错误");
                return;
            }
            if (_commonConfig is null)
            {
                LogHelper.LogWarning("机器人配置为空");
                return;
            }
            if (_config is null)
            {
                LogHelper.LogWarning("RSS订阅插件配置为空");
                return;
            }

            if (_bot is null)
            {
                LogHelper.LogWarning("RSS订阅插件机器人实例为空");
                return;
            }

            if (!_config.Enabled)  //没有启用RSS订阅插件
                return;

            await GetAllRssXmlPipe(item, _bot, _commonConfig);
        }

        private async Task GetAllRssXmlPipe(SubscriptionItem item, BotContext bot, ICommonConfig commonConfig)
        {
            if (string.IsNullOrWhiteSpace(item.Url))
            {
                LogHelper.LogWarning($"RSS订阅对象 {item.Remark} 没有设置Url，不进行获取");
                return;
            }

            if (item.SendToGroups.Length == 0 && item.SendToFriends.Length == 0)
            {
                LogHelper.LogWarning($"RSS订阅对象 {item.Remark}（{item.Url}）没有要转发的群和好友，跳过读取");
                return;
            }

            XmlDocument doc;
            try
            {
                doc = await GetXmlDocument(item);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"{item.Url}获取RSS错误 {ex.Message}");
                return;
            }

            await DeserializeXmlPipe(bot, commonConfig, doc, item);
        }


        private async Task DeserializeXmlPipe(BotContext bot, ICommonConfig commonConfig, XmlDocument doc, SubscriptionItem item)
        {
            string url = item.Url!;
            foreach (RssResult rssResult in DeserializeDocument(doc, url))  //只查找第一篇文章
            {
                if (!_rssSendRecord.ContainsKey(url))
                {
                    LogHelper.LogMessage($"首次读取到RSS订阅对象 {item.Remark}（{item.Url}）的内容，仅作为变更对比依据，不发送");
                    _rssSendRecord.Add(url, rssResult.InnerTitle!);
                    break;
                }

                if (rssResult.InnerTitle == _rssSendRecord[url])
                    break;  //没有新内容

                if (!FilterMessage(item, rssResult.InnerTitle!, rssResult.Text.ToString()))
                {
                    LogHelper.LogMessage($"{item.Remark}（{url}）的 {rssResult.PubDate} 新内容由于触发过滤规则而不发送");
                    _rssSendRecord[url] = rssResult.InnerTitle!;  //记录更新为最新的标题
                    break;
                }

                //内容变更，发送内容
                foreach (var group in item.SendToGroups)
                {
                    if (commonConfig.DebugMode && !commonConfig.DebugGroups.Contains(group))
                    {
                        LogHelper.LogWarning($"当前处于调试模式，不推送RSS更新到非测试群：{group}");
                        continue;
                    }

                    MessageBuilder msg = MessageBuilder.Group(group);
                    await AddMessagePipe(item, rssResult, msg);
                    await bot.SendMessage(msg.Build());
                }
                foreach (var friend in item.SendToFriends)
                {
                    if (commonConfig.DebugMode && !commonConfig.AdminQQ.Contains(friend))
                    {
                        LogHelper.LogWarning($"当前处于调试模式，不推送RSS更新给非管理员好友：{friend}");
                        continue;
                    }

                    MessageBuilder msg = MessageBuilder.Friend(friend);
                    await AddMessagePipe(item, rssResult, msg);
                    await bot.SendMessage(msg.Build());
                }

                _rssSendRecord[url] = rssResult.InnerTitle!;  //更新最新标题（由于部分源没有发布时间字段，使用标题判断是否变更）
                LogHelper.LogMessage($"已推送 {item.Remark}（{item.Url}）更新");
                break;
            }
        }

        private string[] Split(string format, IEnumerable<string> tags)
        {
            string pattern = string.Join("|", tags);
            List<string> parts = new List<string>();
            int lastIndex = 0;
            foreach (Match match in Regex.Matches(format, pattern))
            {
                if (match.Index > lastIndex)
                    parts.Add(format.Substring(lastIndex, match.Index - lastIndex));
                parts.Add(match.Value);
                lastIndex = match.Index + match.Length;
            }
            if (lastIndex < format.Length)
                parts.Add(format.Substring(lastIndex));
           return parts.ToArray();
        }

        private async Task AddMessagePipe(SubscriptionItem item, RssResult result, MessageBuilder msg)
        {
            Dictionary<string, bool> needRelpaceTags = new()
            {
                { "<标题>", !string.IsNullOrWhiteSpace(result.Title) },
                { "<订阅地址>", !string.IsNullOrWhiteSpace(result.Url) },
                { "<Url>", !string.IsNullOrWhiteSpace(result.Url) },
                { "<url>", !string.IsNullOrWhiteSpace(result.Url) },
                { "<备注>", !string.IsNullOrWhiteSpace(item.Remark) },
                { "<remark>", !string.IsNullOrWhiteSpace(item.Remark) },
                { "<Remark>", !string.IsNullOrWhiteSpace(item.Remark) },
                { "<文章标题>", !string.IsNullOrWhiteSpace(result.InnerTitle) },
                { "<正文>", !string.IsNullOrWhiteSpace(result.Text.ToString()) },
                { "<图片>", result.ImageUrls.Count > 0 },
                { "<视频>", result.VideoUrls.Count > 0 },
                { "<图片地址>", result.ImageUrls.Count > 0 },
                { "<视频地址>", result.VideoUrls.Count > 0 },
                { "<嵌入页面地址>", result.IFrameUrls.Count > 0 },
                { "<正文翻译>", !string.IsNullOrWhiteSpace(result.Text.ToString()) },
                { "<B站直播封面>", !string.IsNullOrWhiteSpace(result.Url) && (result.Url.Contains("bilibili") || result.Url.Contains("/room/")) },
                { "<作者>", !string.IsNullOrWhiteSpace(result.Author) },
                { "<发布时间>", result.PubDate is not null },
                { "<原文地址>", !string.IsNullOrWhiteSpace(result.Link) },
            };

            Dictionary<string, Func<MessageBuilder, Task>> relpaceTags = new()
            {
                { "<标题>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(result.Title) ? "" : result.Title))},
                { "<订阅地址>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Url) ? "" : item.Url))},
                { "<Url>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Url) ? "" : item.Url))},
                { "<url>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Url) ? "" : item.Url))},
                { "<备注>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Remark) ? "" : item.Remark))},
                { "<remark>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Remark) ? "" : item.Remark))},
                { "<Remark>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(item.Remark) ? "" : item.Remark))},
                { "<文章标题>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(result.InnerTitle) ? "" : result.InnerTitle))},
                { "<正文>", async msg => await Task.FromResult(msg.Text(result.Text.ToString()))},
                { "<图片>", async msg => await AddImages(item, result.ImageUrls, msg) },
                { "<视频>", async msg => await AddVideoes(item, result.VideoUrls, msg) },
                { "<图片地址>", async msg => await Task.FromResult(msg.Text(string.Join("\n",result.ImageUrls)))},
                { "<视频地址>", async msg => await Task.FromResult(msg.Text(string.Join("\n",result.VideoUrls)))},
                { "<嵌入页面地址>", async msg => await Task.FromResult(msg.Text(string.Join("\n",result.IFrameUrls)))},
                { "<正文翻译>", async msg => await Translate(result.Text.ToString(), msg)},
                { "<B站直播封面>", async msg => await BilibiliLiveCover(item, result, msg)},
                { "<作者>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(result.Author) ? "" : result.Author))},
                { "<发布时间>", async msg => await Task.FromResult(msg.Text(result.PubDate is null ? "" : result.PubDate.Value.ToString("yyyy-MM-dd HH:mm:ss")))},
                { "<原文地址>", async msg => await Task.FromResult(msg.Text(string.IsNullOrWhiteSpace(result.Link) ? "" : result.Link))},
            };

            foreach (var temp in item.Format)
            {
                string? formatLine = temp;
                if (formatLine is null || formatLine.Length == 0)
                {
                    msg.Text("\n");
                    continue;
                }

                bool firstIsQuestion = formatLine[0] == '?';
                if (firstIsQuestion)
                    formatLine = formatLine[1..];

                string[] fromat = Split(formatLine, relpaceTags.Keys);

                bool skipLine = false;
                foreach (var tagOrText in fromat)  //第一次循环检查有没有空的标签
                {
                    if (!needRelpaceTags.TryGetValue(tagOrText, out bool tag))
                        continue;
                    if (!tag && firstIsQuestion)
                    {
                        skipLine = true;
                        break;
                    }
                }

                if (skipLine)  //如果有空的标签，跳过整行
                    continue;

                foreach (var tagOrText in fromat)
                {
                    if (!relpaceTags.TryGetValue(tagOrText, out Func<MessageBuilder, Task>? AddMessageMethod))
                    {
                        msg.Text(tagOrText);
                        continue;
                    }
                    await AddMessageMethod(msg);
                }
                msg.Text("\n");
            }
        }

        private async Task AddImages(SubscriptionItem item, List<string> imgUrls, MessageBuilder msg)
        {
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = item.UseProxy };
            using HttpClient client = new HttpClient(handler);

            foreach (var url in imgUrls)
            {
                try
                {
                    byte[] img = await client.GetByteArrayAsync(url);
                    msg.Image(img);
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"RSS订阅插件中下载图片失败，错误信息：{ex.Message}");
                    msg.Text("（下载图片失败）");
                }
            }
        }

        private async Task AddVideoes(SubscriptionItem item, List<string> imgUrls, MessageBuilder msg)
        {
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = item.UseProxy };
            using HttpClient client = new HttpClient(handler);

            foreach (var url in imgUrls)
            {
                try
                {
                    byte[] vdo = await client.GetByteArrayAsync(url);
                    msg.Video(vdo);
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, $"RSS订阅插件中下载视频 {url} 失败，错误信息：{ex.Message}");
                    msg.Text("（下载视频失败）");
                }
            }
        }

        private async Task Translate(string originalText, MessageBuilder msg)
        {
            dynamic translatePlugin = SngletonInstance.Plugins.FirstOrDefault(p => p.Key == "GreenOnions.NT.Translate").Value;
            if (translatePlugin is null)
            {
                LogHelper.LogWarning("未安装翻译插件，无法为RSS订阅插件提供翻译");
                LogHelper.LogDebug($"已安装插件：{string.Join(',', SngletonInstance.Plugins.Keys)}");
                return;
            }

            try
            {
                string dstText = await translatePlugin.TranslateToChinese(originalText);
                msg.Text(dstText);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"RSS订阅翻译失败，错误信息：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取B站直播封面
        /// </summary>
        private async Task BilibiliLiveCover(SubscriptionItem config, RssResult result, MessageBuilder msg)
        {
            if (string.IsNullOrWhiteSpace(result.Url))
                return;
            string roomId = result.Url[(result.Url.LastIndexOf("/room/") + "/room/".Length)..];
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = config.UseProxy };
            using HttpClient client = new HttpClient(handler);
            try
            {
                var apiResponse = await client.GetAsync($@"https://api.live.bilibili.com/xlive/web-room/v1/index/getInfoByRoom?room_id={roomId}");
                if (!apiResponse.IsSuccessStatusCode)
                {
                    LogHelper.LogError($"调用接口查询B站直播间封面图地址失败：{(int)apiResponse.StatusCode} {apiResponse.StatusCode}");
                    return;
                }
                string apiResult = await apiResponse.Content.ReadAsStringAsync();
                JObject jo = JsonConvert.DeserializeObject<JObject>(apiResult)!;
                string? imgUrl = jo?["data"]?["room_info"]?["cover"]?.ToString();
                if (imgUrl is null)
                {
                    LogHelper.LogWarning($"B站直播间{roomId}不存在封面图");
                    return;
                }
                var imgResponse = await client.GetAsync(imgUrl);
                if (!imgResponse.IsSuccessStatusCode)
                {
                    LogHelper.LogError($"下载B站直播间{roomId}封面图{imgUrl}失败：{(int)imgResponse.StatusCode} {imgResponse.StatusCode}");
                    return;
                }
                byte[] img = await imgResponse.Content.ReadAsByteArrayAsync();
                msg.Image(img);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"解析B站直播间{roomId}封面图失败：{ex.Message}");
            }
        }

        private async Task<XmlDocument> GetXmlDocument(SubscriptionItem item)
        {
            XmlDocument xmlDoc = new();

            ////测试用，不访问地址，读取本地文件解析
            //string xml = File.ReadAllText("sasakirico.xml");
            //xmlDoc.LoadXml(xml);
            //return xmlDoc;

            using HttpClientHandler handler = new() { UseProxy = item.UseProxy };
            using HttpClient client = new(handler);
            using HttpRequestMessage request = new(HttpMethod.Get, item.Url);
            foreach (var header in item.Headers)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            HttpResponseMessage response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                LogHelper.LogError($"{item.Url}获取RSS错误 {(int)response.StatusCode} {response.StatusCode}");
                throw new Exception($"{(int)response.StatusCode} {response.StatusCode}");
            }
            string xml;
            if (item.IsStreamSource)
            {
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                xml = Encoding.UTF8.GetString(buffer);
            }
            else
            {
                xml = await response.Content.ReadAsStringAsync();
            }
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        /// <summary>
        /// 反序列化一篇文章
        /// </summary>
        private IEnumerable<RssResult> DeserializeDocument(XmlDocument xmlDoc, string url)
        {
            bool isContent = xmlDoc.GetElementsByTagName("rss")?[0]?.Attributes?["xmlns:content"] is not null;
            bool isAtom = xmlDoc.GetElementsByTagName("rss")?[0]?.Attributes?["xmlns:atom"] is not null;

            if (!isContent && !isAtom)
                isAtom = xmlDoc.GetElementsByTagName("feed")?[0]?.Attributes?["xmlns"]?.Value.EndsWith("Atom") == true;  //Github

            string title = xmlDoc.GetElementsByTagName("title")[0]!.InnerText;
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("item");
            if (nodeList.Count == 0)
                nodeList = xmlDoc.GetElementsByTagName("entry");

            if (isContent)
            {
                return ReadRssContent(nodeList, title, url);
            }
            if (!isAtom)
            {
                string xmlns = "未知";
                if (xmlDoc.GetElementsByTagName("rss")?[0]?.Attributes?["xmlns:dc"] is not null)
                    xmlns = "Dc";
                else if (xmlDoc.GetElementsByTagName("rss")?[0]?.Attributes?["xmlns:media"] is not null)
                    xmlns = "Media";
                LogHelper.LogWarning($"{url}的Rss规范类型\"{xmlns}\"没有支持，尝试视为Atom解析，如果解析内容有误请联系机器人作者");
            }
            //Atom
            return ReadRssAtom(nodeList, title, url);
        }

        private IEnumerable<RssResult> ReadRssContent(XmlNodeList nodeList, string title, string url)
        {
            foreach (XmlNode node in nodeList)
            {
                if (!node.HasChildNodes)
                    continue;

                XmlNode? noteDate = node.ChildNodes.OfType<XmlNode>().Where(n => n.Name.ToLower() == "pubdate").FirstOrDefault();
                if (noteDate is null)
                    continue;

                bool bHasEncoded = false;
                if (node.ChildNodes.OfType<XmlNode>().Any(n => n.Name == "content:encoded"))
                    bHasEncoded = true;

                HtmlDocument htmlDoc;
                DateTime pubDate = pubDate = DateTime.Parse(noteDate.InnerText);
                RssResult result = new RssResult();
                result.Url = url;
                result.Title = title;
                result.PubDate = pubDate;
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    switch (subNode.Name.ToLower())
                    {
                        case "title":
                            result.InnerTitle = HttpUtility.HtmlDecode(subNode.InnerText);
                            break;
                        case "content:encoded":
                            htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(subNode.InnerText);
                            foreach (var itemNode in htmlDoc.DocumentNode.ChildNodes)
                            {
                                RssBody body = new RssBody();
                                HtmlToRssBody(itemNode, body);
                                if (result.Text.Length > 0)
                                    result.Text.AppendLine();
                                result.Text.Append(body.Text.ToString());
                                result.ImageUrls.AddRange(body.ImageUrls);
                                result.VideoUrls.AddRange(body.VideoUrls);
                                result.IFrameUrls.AddRange(body.IFrameUrls);
                            }
                            if (result.Text.Length > 0)
                                result.Text.AppendLine();
                            result.Text.Append(HttpUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText));
                            break;
                        case "description":
                        case "content":
                            if (!bHasEncoded)
                            {
                                htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(subNode.InnerText);
                                string description = HttpUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText);
                                if (result.Text.Length > 0)
                                    result.Text.AppendLine();
                                result.Text.Append(description);
                            }
                            break;
                        case "link":
                            if (subNode.Attributes?["href"] is not null)
                                result.Link = subNode.Attributes["href"]!.Value;
                            else
                                result.Link = subNode.InnerText;
                            break;
                        case "author":
                        case "dc:creator":
                            result.Author = subNode.InnerText;
                            break;
                        case "media:content":
                            if (subNode.Attributes is null || subNode.Attributes["url"] is null || subNode.Attributes["medium"] is null)
                                continue;
                            string? mediaUrl = subNode.Attributes["url"]?.Value;
                            if (string.IsNullOrWhiteSpace(mediaUrl))
                                continue;
                            if (subNode.Attributes["medium"]?.Value == "image")
                                result.ImageUrls.Add(mediaUrl);
                            else if (subNode.Attributes["medium"]?.Value == "image")
                                result.VideoUrls.Add(mediaUrl);
                            else
                            {
                                if (result.Text.Length > 0)
                                    result.Text.AppendLine();
                                result.Text.Append($"{subNode.Attributes["medium"]?.Value}: {mediaUrl}");
                            }
                            break;
                    }
                }
                yield return result;
            }
        }

        private IEnumerable<RssResult> ReadRssAtom(XmlNodeList nodeList, string title, string url)
        {
            foreach (XmlNode node in nodeList)
            {
                if (!node.HasChildNodes)
                    continue;

                XmlNode? noteDate = node.ChildNodes.OfType<XmlNode>().Where(n => n.Name.ToLower() == "pubdate").FirstOrDefault();
                noteDate ??= node.ChildNodes.OfType<XmlNode>().Where(n => n.Name.ToLower() == "updated").FirstOrDefault();

                RssResult result = new RssResult();
                result.Url = url;
                result.Title = title;

                if (noteDate is not null)
                {
                    DateTime pubDate = DateTime.Parse(noteDate.InnerText);
                    result.PubDate = pubDate;
                }
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    switch (subNode.Name.ToLower())
                    {
                        case "title":
                            result.InnerTitle = HttpUtility.HtmlDecode(subNode.InnerText);
                            break;
                        case "description":
                        case "content":
                            HtmlDocument htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(subNode.InnerText);
                            RssBody body = new RssBody();
                            HtmlToRssBody(htmlDoc.DocumentNode, body);
                            if (result.Text.Length > 0)
                                result.Text.AppendLine();
                            result.Text.Append(HttpUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText));
                            result.ImageUrls = body.ImageUrls;
                            result.VideoUrls = body.VideoUrls;
                            result.IFrameUrls = body.IFrameUrls;
                            break;
                        case "link":
                            if (subNode.Attributes?["href"] is not null)
                                result.Link = subNode.Attributes["href"]!.Value;
                            else
                                result.Link = subNode.InnerText;
                            break;
                        case "author":
                        case "dc:creator":
                            result.Author = subNode.InnerText;
                            break;
                        case "media:content":
                            if (subNode.Attributes is null || subNode.Attributes["url"] is null || subNode.Attributes["medium"] is null)
                                continue;
                            string? mediaUrl = subNode.Attributes["url"]?.Value;
                            if (string.IsNullOrWhiteSpace(mediaUrl))
                                continue;
                            if (subNode.Attributes["medium"]?.Value == "image")
                                result.ImageUrls.Add(mediaUrl);
                            else if (subNode.Attributes["medium"]?.Value == "image")
                                result.VideoUrls.Add(mediaUrl);
                            else
                            {
                                if (result.Text.Length > 0)
                                    result.Text.AppendLine();
                                result.Text.Append($"{subNode.Attributes["medium"]?.Value}: {mediaUrl}");
                            }
                            break;
                    }
                }
                yield return result;
            }
        }

        private void HtmlToRssBody(HtmlNode node, RssBody body)
        {
            if (node.Name == "img")
                body.ImageUrls.Add(HttpUtility.HtmlDecode(node.Attributes["src"].Value));
            else if (node.Name == "video")
                body.VideoUrls.Add(HttpUtility.HtmlDecode(node.Attributes["src"].Value));
            else if (node.Name == "iframe")
                body.IFrameUrls.Add(HttpUtility.HtmlDecode(node.Attributes["src"].Value));
            else if (!string.IsNullOrWhiteSpace(node.InnerText))
                body.Text.Append(HttpUtility.HtmlDecode(node.InnerText));

            foreach (var itemNode in node.ChildNodes)
                HtmlToRssBody(itemNode, body);
        }

        private bool FilterMessage(SubscriptionItem item, string innerTitle, string description)
        {
            bool bSend = false;
            int bContainCount = 0;
            switch (item.FilterMode)
            {
                case FilterModes.Disabled:  //不过滤
                    bSend = true;
                    break;
                case FilterModes.SendWhenAny:  //包含任意发送
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (innerTitle.Contains(item.FilterKeyWords[i]))
                        {
                            bSend = true;
                            break;
                        }
                        if (description.Contains(item.FilterKeyWords[i]))
                        {
                            bSend = true;
                            break;
                        }
                    }
                    break;
                case FilterModes.SendWhenAll:  //包含所有发送
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (innerTitle.Contains(item.FilterKeyWords[i]))
                            bContainCount++;
                    }
                    if (bContainCount == item.FilterKeyWords?.Length)
                    {
                        bSend = true;
                        break;
                    }
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (description.Contains(item.FilterKeyWords[i]))
                            bContainCount++;
                    }
                    if (bContainCount == item.FilterKeyWords?.Length)
                        bSend = true;
                    break;
                case FilterModes.NotSendWhenAny:  //包含任意不发送
                    bSend = true;
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (innerTitle.Contains(item.FilterKeyWords[i]))
                        {
                            bSend = false;
                            break;
                        }
                        if (description.Contains(item.FilterKeyWords[i]))
                        {
                            bSend = false;
                            break;
                        }
                    }
                    break;
                case FilterModes.NotSendWhenAll:  //包含所有不发送
                    bSend = true;
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (innerTitle.Contains(item.FilterKeyWords[i]))
                            bContainCount++;
                    }
                    if (bContainCount == item.FilterKeyWords?.Length)
                    {
                        bSend = false;
                        break;
                    }
                    for (int i = 0; i < item.FilterKeyWords?.Length; i++)
                    {
                        if (description.Contains(item.FilterKeyWords[i]))
                            bContainCount++;
                    }
                    if (bContainCount == item.FilterKeyWords?.Length)
                        bSend = false;
                    break;
            }
            return bSend;
        }
    }
}
