﻿using System.Text;
using System.Text.RegularExpressions;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.CustomHttpApiInvoker
{
    public class ApiInvoker : IPlugin
    {
        private string? _path;
        private MainConfig _config = new MainConfig();
        private Dictionary<HttpApiConfig, Regex> _regexs = new Dictionary<HttpApiConfig, Regex>();
        private IGreenOnionsApi? _botApi;

        public string Name => "自定义API客户端";

        public string Description => "自定义调用HttpApi插件";

        public bool DisplayedInTheHelp => false;

        public GreenOnionsMessages? HelpMessage => null;

        public void ConsoleSetting()
        {
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            _botApi = api;
        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _path = pluginPath;
            string configFileName = Path.Combine(_path, "config.json");
            if (File.Exists(configFileName))
            {
                string strConfigJson = File.ReadAllText(configFileName);
                if (!string.IsNullOrWhiteSpace(strConfigJson))
                {
                    _config = JsonConvert.DeserializeObject<MainConfig>(strConfigJson)!;
                    _regexs.Clear();
                    foreach (var item in _config.ApiConfig)
                    {
                        if (item.Cmd!.Contains("(?<参数>"))
                            _regexs.Add(item, new Regex(item.Cmd));
                    }
                }
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.FirstOrDefault() is GreenOnionsTextMessage msg)
            {
                if (string.Equals(_botApi!.ReplaceGreenOnionsStringTags(_config.HelpCmd), msg.Text, StringComparison.OrdinalIgnoreCase))
                {
                    StringBuilder helpMessage = new StringBuilder();
                    for (int i = 0; i < _config.ApiConfig.Count; i++)
                    {
                        helpMessage.AppendLine(_config.ApiConfig[i].Cmd + (string.IsNullOrEmpty(_config.ApiConfig[i].HelpMessage) ? "" : $":{_config.ApiConfig[i].HelpMessage}"));
                    }
                    Response(new GreenOnionsMessages(helpMessage.ToString()) { Reply = false });
                    return true;
                }
                else
                {
                    for (int i = 0; i < _config.ApiConfig.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(_config.ApiConfig[i].Cmd))
                        {
                            Task<GreenOnionsMessages?>? task = null;
                            if (_regexs.ContainsKey(_config.ApiConfig[i]))
                            {
                                if (_regexs[_config.ApiConfig[i]].IsMatch(msg.Text))
                                {
                                    Match match = _regexs[_config.ApiConfig[i]].Match(msg.Text);
                                    string param = string.Empty;
                                    if (match.Groups.Count > 1)
                                        param = match.Groups[1].Value;
                                    task = InvokeApi(_config.ApiConfig[i], param);
                                }
                            }
                            else if (string.Equals(_botApi.ReplaceGreenOnionsStringTags(_config.ApiConfig[i].Cmd!), msg.Text, StringComparison.OrdinalIgnoreCase))
                            {
                                task = InvokeApi(_config.ApiConfig[i]);
                            }
                            if (task != null)
                            {
                                task.ContinueWith(callback =>
                                {
                                    if (callback.Result != null)
                                        Response(callback.Result);
                                });
                                return true;
                            } 
                        }
                    }
                }
            }
            return false;
        }

        private async Task<GreenOnionsMessages?> InvokeApi(HttpApiConfig api, string param = "")
        {
            GreenOnionsMessages msg = new GreenOnionsMessages() { Reply = false };
            if (string.IsNullOrEmpty(api.Url))
            {
                msg.Add("此API没有设置地址，请联系机器人管理员");
                return msg;
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpMethod httpMethod = api.HttpMethod == HttpMethodEnum.GET ? HttpMethod.Get : HttpMethod.Post;

                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, api.Url.Replace("<参数>", param)))
                    {
                        if (api.Headers != null)
                        {
                            foreach (var item in api.Headers)
                                request.Headers.Add(item.Key, item.Value.Replace("<参数>", param));
                        }

                        Encoding encoding = api.Encoding switch
                        {
                            EncodingEnum.UTF8 => Encoding.UTF8,
                            EncodingEnum.Unicode => Encoding.Unicode,
                            EncodingEnum.BigEndianUnicode => Encoding.BigEndianUnicode,
                            EncodingEnum.UTF32 => Encoding.UTF32,
                            EncodingEnum.ASCII => Encoding.ASCII,
                            EncodingEnum.GBK => Encoding.GetEncoding("GB2312"),
                            _ => throw new NotImplementedException("编码类型无效"),
                        };

                        if (api.ContentType == ContentTypeEnum.raw)
                        {
                            if (!string.IsNullOrEmpty(api.RowContent))
                                request.Content = new StringContent(api.RowContent.Replace("<参数>", param), encoding, api.MediaType);
                        }
                        else
                        {
                            if (api.FormDataContent != null)
                            {
                                var form = new MultipartFormDataContent();
                                foreach (var item in api.FormDataContent)
                                {
                                    form.Add(new StringContent(item.Value.Replace("<参数>", param), encoding, api.MediaType), item.Key);
                                }
                                request.Content = form;
                            }
                        }

                        HttpResponseMessage response;
                        try
                        {
                            response = await client.SendAsync(request);
                        }
                        catch (Exception)
                        {
                            msg.Add("请求失败，请联系机器人管理员");
                            return msg;
                        }
                        if ((int)response.StatusCode >= 400)
                        {
                            msg.Add("请求被拒绝，请联系机器人管理员");
                            return msg;
                        }

                        //请求成功
                        string valueText = string.Empty;
                        Stream? valueStream = null;
                        try
                        {
                            if (api.ParseMode == ParseModeEnum.Text)
                            {
                                string text = await response.Content.ReadAsStringAsync();

                                int startIndex = 0;
                                if (!string.IsNullOrEmpty(api.SubTextFrom))
                                {
                                    startIndex = text.IndexOf(api.SubTextFrom);
                                    if (!api.SubTextWithPrefix)
                                        startIndex += api.SubTextFrom.Length;
                                }

                                int SubTextFromLength = 0;
                                if (!string.IsNullOrEmpty(api.SubTextFrom))
                                    SubTextFromLength = api.SubTextFrom.Length;

                                text = text.Substring(startIndex);
                                int endIndex = text.Length;
                                if (!string.IsNullOrEmpty(api.SubTextTo))
                                {
                                    endIndex = text.IndexOf(api.SubTextTo, api.SubTextWithPrefix ? SubTextFromLength : 0);
                                    if (api.SubTextWithSuffix)
                                        endIndex += api.SubTextTo.Length;
                                }

                                valueText = text.Substring(0, endIndex).TrimStart('\n').TrimStart('\r').TrimStart('\n');
                            }
                            else if (api.ParseMode == ParseModeEnum.Json)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                if (string.IsNullOrEmpty(api.ParseExpression))
                                {
                                    valueText = json;
                                }
                                else
                                {
                                    JToken topJt = JsonConvert.DeserializeObject<JToken>(json)!;
                                    bool bOpen = false;
                                    StringBuilder indexName = new StringBuilder();

                                    string[] expression = api.ParseExpression.Split("\r\n");
                                    StringBuilder valueLines = new StringBuilder();
                                    for (int i = 0; i < expression.Length; i++)
                                    {
                                        JToken jt = topJt;
                                        for (int j = 0; j < expression[i].Length; j++)
                                        {
                                            if (bOpen)
                                            {
                                                if (expression[i][j] == ']')
                                                {
                                                    bOpen = false;
                                                    if (string.Equals(indexName.ToString(), "<random>", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        var arr = jt.ToArray();
                                                        Random rdm = new Random(Guid.NewGuid().GetHashCode());
                                                        jt = arr[rdm.Next(0, arr.Length)];
                                                    }
                                                    else if (long.TryParse(indexName.ToString(), out long numberIndex))
                                                        jt = jt.ToArray()[numberIndex]!;
                                                    else
                                                        jt = jt[indexName.ToString()]!;
                                                    indexName.Clear();
                                                    continue;
                                                }
                                                if (expression[i][j] == '\'' || expression[i][j] == '\"')
                                                {
                                                    continue;
                                                }
                                                indexName.Append(expression[i][j]);
                                            }
                                            else
                                            {
                                                if (expression[i][j] == '[')
                                                {
                                                    bOpen = true;
                                                    continue;
                                                }
                                            }
                                        }
                                        valueLines.AppendLine(jt.ToString());
                                    }
                                    valueText = valueLines.ToString();
                                    if (valueText.EndsWith("\r\n"))
                                        valueText = valueText.Substring(0, valueText.Length - "\r\n".Length);
                                }
                            }
                            else if (api.ParseMode == ParseModeEnum.Stream)
                            {
                                valueStream = await response.Content.ReadAsStreamAsync();
                            }
                        }
                        catch (Exception)
                        {
                            msg.Add("解析失败，请联系机器人管理员");
                            return msg;
                        }
                        //解析成功
                        try
                        {
                            if (api.SendMode == SendModeEnum.Text)
                            {
                                msg.Add(valueText);
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.ImageUrl)
                            {
                                msg.Add(new GreenOnionsImageMessage(valueText));
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.ImageBase64)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                MemoryStream ms = new MemoryStream(data);
                                msg.Add(new GreenOnionsImageMessage(ms));
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.ImageStream)
                            {
                                if (valueStream == null)
                                    msg.Add("Api响应为空，请联系机器人管理员");
                                else
                                    msg.Add(new GreenOnionsImageMessage(valueStream));
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.VoiceUrl)
                            {
                                msg.Add(new GreenOnionsVoiceMessage(valueText));
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.VoiceBase64)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                MemoryStream ms = new MemoryStream(data);
                                msg.Add(new GreenOnionsVoiceMessage(ms));
                                return msg;
                            }
                            else if (api.SendMode == SendModeEnum.VoiceStream)
                            {
                                if (valueStream == null)
                                    msg.Add("Api响应为空，请联系机器人管理员");
                                else
                                    msg.Add(new GreenOnionsVoiceMessage(valueStream));
                                return msg;
                            }
                        }
                        catch (Exception)
                        {
                            msg.Add("Api响应成功，但不是图片或音频，请联系机器人管理员");
                            return msg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Add($"发生错误，请联系机器人管理员，{ex.Message}");
                return msg;
            }
            return null;
        }

        public bool WindowSetting()
        {
            new FrmSettings(_path!, _config).ShowDialog();
            _regexs.Clear();
            foreach (var item in _config.ApiConfig)
            {
                if (item.Cmd!.Contains("(?<参数>"))
                    _regexs.Add(item, new Regex(item.Cmd));
            }
            string configFileName = Path.Combine(_path!, "config.json");
            string jsonConfig = JsonConvert.SerializeObject(_config);
            File.WriteAllText(configFileName, jsonConfig);
            return true;
        }
    }
}
