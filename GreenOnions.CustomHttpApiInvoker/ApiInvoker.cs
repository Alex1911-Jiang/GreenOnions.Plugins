using System.Text;
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
                            if (string.Equals(_botApi.ReplaceGreenOnionsStringTags(_config.ApiConfig[i].Cmd!), msg.Text, StringComparison.OrdinalIgnoreCase))
                            {
                                InvokeApi(_config.ApiConfig[i]).ContinueWith(callback =>
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

        private async Task<GreenOnionsMessages?> InvokeApi(HttpApiConfig api)
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

                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, api.Url))
                    {
                        if (api.Headers != null)
                        {
                            foreach (var item in api.Headers)
                                request.Headers.Add(item.Key, item.Value);
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
                                request.Content = new StringContent(api.RowContent, encoding, api.MediaType);
                        }
                        else
                        {
                            if (api.FormDataContent != null)
                            {
                                var form = new MultipartFormDataContent();
                                foreach (var item in api.FormDataContent)
                                {
                                    form.Add(new StringContent(item.Value, encoding, api.MediaType), item.Key);
                                }
                                request.Content = form;
                            }
                        }

                        HttpResponseMessage response;
                        try
                        {
                            response = await client.SendAsync(request);
                        }
                        catch (Exception ex)
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
                                    JToken jt = JsonConvert.DeserializeObject<JToken>(json)!;
                                    bool bOpen = false;
                                    StringBuilder indexName = new StringBuilder();
                                    for (int i = 0; i < api.ParseExpression.Length; i++)
                                    {
                                        if (bOpen)
                                        {
                                            if (api.ParseExpression[i] == ']')
                                            {
                                                bOpen = false;
                                                if (long.TryParse(indexName.ToString(), out long numberIndex))
                                                    jt = jt[numberIndex]!;
                                                else
                                                    jt = jt[indexName.ToString()]!;
                                                indexName.Clear();
                                                continue;
                                            }
                                            if (api.ParseExpression[i] == '\'' || api.ParseExpression[i] == '\"')
                                            {
                                                continue;
                                            }
                                            indexName.Append(api.ParseExpression[i]);
                                        }
                                        else
                                        {
                                            if (api.ParseExpression[i] == '[')
                                            {
                                                bOpen = true;
                                                continue;
                                            }
                                        }
                                    }
                                    valueText = jt.ToString();
                                }
                            }
                            else if (api.ParseMode == ParseModeEnum.Stream)
                            {
                                valueStream = await response.Content.ReadAsStreamAsync();
                            }
                        }
                        catch (Exception)
                        {
                            string respFileName = Path.Combine(_path!, "响应.txt");
                            await File.WriteAllTextAsync(respFileName, valueText);
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
                        catch (Exception ex)
                        {
                            string respFileName = Path.Combine(_path!, "响应.txt");
                            await File.WriteAllTextAsync(respFileName, valueText);

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
            string configFileName = Path.Combine(_path!, "config.json");
            string jsonConfig = JsonConvert.SerializeObject(_config);
            File.WriteAllText(configFileName, jsonConfig);
            return true;
        }
    }
}
