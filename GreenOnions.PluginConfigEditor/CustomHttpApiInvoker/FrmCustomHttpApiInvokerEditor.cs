using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using GreenOnions.PluginConfigs.CustomHttpApiInvoker;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.PluginConfigEditor.CustomHttpApiInvoker
{
    internal partial class FrmCustomHttpApiInvokerEditor : Form
    {
        public HttpApiItemConfig Config { get; private set; }

        public FrmCustomHttpApiInvokerEditor(HttpApiItemConfig? config = null)
        {
            InitializeComponent();

            if (config is null)
            {
                Config = new HttpApiItemConfig();
                cboHttpMethod.SelectedIndex = 0;
                cboEncoding.SelectedIndex = 0;
                cboMediaType.SelectedIndex = 0;
            }
            else
            {
                Config = config;
                txbUrl.Text = Config.Url;
                chkUseProxy.Checked = Config.UseProxy;
                txbCmd.Text = Config.Cmd;
                txbRemark.Text = Config.Remark;
                txbHelpMessage.Text = Config.HelpMessage;
                cboHttpMethod.SelectedIndex = (int)Config.HttpMethod;
                cboEncoding.SelectedIndex = (int)Config.Encoding;
                cboMediaType.SelectedIndex = cboMediaType.Items.IndexOf(Config.MediaType);

                if (Config.Headers is not null)
                {
                    foreach (var item in Config.Headers)
                        dgvHeader.Rows.Add(item.Key, item.Value);
                }

                switch (Config.ContentType)
                {
                    case ContentTypeEnum.raw:
                        rdoContentRaw.Checked = true;
                        break;
                    case ContentTypeEnum.form_data:
                        rdoContentFormData.Checked = true;
                        break;
                }
                txbContentRaw.Text = Config.RawContent;
                if (Config.FormDataContent is not null)
                {
                    foreach (var item in Config.FormDataContent)
                        dgvContentFormData.Rows.Add(item.Key, item.Value);
                }
                switch (Config.ParseMode)
                {
                    case ParseModeEnum.Text:
                        rdoParseText.Checked = true;
                        break;
                    case ParseModeEnum.Json:
                        rdoParseJson.Checked = true;
                        break;
                    case ParseModeEnum.Xml:
                        rdoParseXml.Checked = true;
                        break;
                    case ParseModeEnum.XPath:
                        rdoParseXPath.Checked = true;
                        break;
                    case ParseModeEnum.JavaScript:
                        rdoParseJavaScript.Checked = true;
                        break;
                    case ParseModeEnum.Stream:
                        rdoParseStream.Checked = true;
                        break;
                }
                if (Config.ParseExpression is not null)
                    txbParseExpression.Text = string.Join('\n', Config.ParseExpression);
                txbSubTextFrom.Text = Config.SubTextFrom;
                txbSubTextTo.Text = Config.SubTextTo;
                chkSubTextWithPrefix.Checked = Config.SubTextWithPrefix;
                chkSubTextWithSuffix.Checked = Config.SubTextWithSuffix;
                switch (Config.SendMode)
                {
                    case SendModeEnum.Text:
                        rdoSendText.Checked = true;
                        break;
                    case SendModeEnum.ImageUrl:
                        rdoSendImageByUrl.Checked = true;
                        break;
                    case SendModeEnum.ImageBase64:
                        rdoSendImageByBase64.Checked = true;
                        break;
                    case SendModeEnum.ImageStream:
                        rdoSendImageStream.Checked = true;
                        break;
                    case SendModeEnum.VoiceUrl:
                        rdoSendVoiceByUrl.Checked = true;
                        break;
                    case SendModeEnum.VoiceBase64:
                        rdoSendVoiceByBase64.Checked = true;
                        break;
                    case SendModeEnum.VoiceStream:
                        rdoSendVoiceStream.Checked = true;
                        break;
                }
                switch (Config.ChangeAtTo)
                {
                    case ChangeMessageTypeEnum.None:
                        rdoDontChangeAt.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.QQId:
                        rdoChangeAtToQQId.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.Nick:
                        rdoChangeAtToNick.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.ProfilePhotoUrl:
                        rdoChangeAtToProfileUrl.Checked = true;
                        break;
                }
                switch (Config.ChangeMeTo)
                {
                    case ChangeMessageTypeEnum.None:
                        rdoDontChangeMe.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.QQId:
                        rdoChangeMeToQQId.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.Nick:
                        rdoChangeMeToNick.Checked = true;
                        break;
                    case ChangeMessageTypeEnum.ProfilePhotoUrl:
                        rdoChangeMeToProfileUrl.Checked = true;
                        break;
                }
            }
        }

        private async void btnInvokeTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbUrl.Text))
            {
                MessageBox.Show("请先输入地址", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            try
            {
                HttpMethod httpMethod = cboHttpMethod.Text.Equals("GET", StringComparison.OrdinalIgnoreCase) ? HttpMethod.Get : HttpMethod.Post;
                using (HttpClient client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
                {
                    string url = txbUrl.Text;
                IL_Redirect:;
                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, url))
                    {
                        //Header
                        for (int i = 0; i < dgvHeader.Rows.Count; i++)
                        {
                            string? headerKey = dgvHeader.Rows[i].Cells[0].Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(headerKey))
                            {
                                string? headerValue = dgvHeader.Rows[i].Cells[1].Value?.ToString();
                                request.Headers.Add(headerKey, headerValue ?? string.Empty);
                            }
                        }

                        Encoding encoding = cboEncoding.SelectedIndex switch
                        {
                            0 => Encoding.UTF8,
                            1 => Encoding.Unicode,
                            2 => Encoding.BigEndianUnicode,
                            3 => Encoding.UTF32,
                            4 => Encoding.ASCII,
                            5 => Encoding.GetEncoding("GB2312"),
                            _ => throw new NotImplementedException("编码类型无效"),
                        };

                        //Content
                        if (rdoContentRaw.Checked)
                        {
                            request.Content = new StringContent(txbContentRaw.Text, encoding, cboMediaType.Text);
                        }
                        else
                        {
                            var form = new MultipartFormDataContent();
                            for (int i = 0; i < dgvContentFormData.Rows.Count; i++)
                            {
                                string? conentKey = dgvContentFormData.Rows[i].Cells[0].Value?.ToString();
                                if (!string.IsNullOrWhiteSpace(conentKey))
                                {
                                    string? contentValue = dgvContentFormData.Rows[i].Cells[1].Value?.ToString();
                                    form.Add(new StringContent(contentValue ?? string.Empty, encoding, cboMediaType.Text), conentKey);
                                }
                            }
                            request.Content = form;
                        }
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(cboMediaType.Text);

                        HttpResponseMessage response;
                        try
                        {
                            response = await client.SendAsync(request);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("发起请求异常。" + ex.Message, "请求失败", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            return;
                        }
                        string requestUrl = response.RequestMessage?.RequestUri!.ToString()!;
                        if ((int)response.StatusCode >= 400)
                        {
                            MessageBox.Show($"{(int)response.StatusCode} {response.ReasonPhrase}{(requestUrl != url ? "\n发生链接跳转，请尝试使用POST进行请求。" : "")}", $"请求错误", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            return;
                        }
                        if (requestUrl != url)
                        {
                            url = requestUrl;
                            goto IL_Redirect;
                        }

                        //请求成功
                        string valueText = string.Empty;
                        Stream? valueStream = null;
                        try
                        {
                            if (rdoParseText.Checked)
                            {
                                string text = await response.Content.ReadAsStringAsync();

                                int startIndex = 0;
                                if (!string.IsNullOrEmpty(txbSubTextFrom.Text))
                                {
                                    startIndex = text.IndexOf(txbSubTextFrom.Text);
                                    if (!chkSubTextWithPrefix.Checked)
                                        startIndex += txbSubTextFrom.Text.Length;
                                }

                                text = text.Substring(startIndex);
                                int endIndex = text.Length;
                                if (!string.IsNullOrEmpty(txbSubTextTo.Text))
                                {
                                    endIndex = text.IndexOf(txbSubTextTo.Text, chkSubTextWithPrefix.Checked ? txbSubTextFrom.Text.Length : 0);
                                    if (chkSubTextWithSuffix.Checked)
                                        endIndex += txbSubTextTo.Text.Length;
                                }

                                valueText = text.Substring(0, endIndex).TrimStart('\n');
                            }
                            else if (rdoParseJson.Checked)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                JToken topJt = JsonConvert.DeserializeObject<JToken>(json)!;
                                bool bOpen = false;
                                StringBuilder indexName = new StringBuilder();

                                string[] expression = txbParseExpression.Text.Split("\n");
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
                            }
                            //else if (rdoParseXml.Checked)
                            //{
                            //}
                            else if (rdoParseXPath.Checked)
                            {
                                string[] expression = txbParseExpression.Text.Split("\n");
                                StringBuilder valueLines = new StringBuilder();
                                for (int i = 0; i < expression.Length; i++)
                                {
                                    string html = await response.Content.ReadAsStringAsync();
                                    HtmlAgilityPack.HtmlDocument docSauceNAO = new HtmlAgilityPack.HtmlDocument();
                                    docSauceNAO.LoadHtml(html);
                                    if (expression[i].Contains('.'))
                                    {
                                        string[] xPathAndAttr = expression[i].Split('.');
                                        if (xPathAndAttr.Length > 2)
                                        {
                                            MessageBox.Show("表达式格式有误，通过'.'访问标签属性，每行'.'不可超过一个", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                                            return;
                                        }
                                        HtmlNode itemNode = docSauceNAO.DocumentNode.SelectSingleNode(xPathAndAttr[0]);
                                        valueLines.AppendLine(itemNode.Attributes[xPathAndAttr[1]].Value);
                                    }
                                    else
                                    {
                                        HtmlNode itemNode = docSauceNAO.DocumentNode.SelectSingleNode( expression[i]);
                                        valueLines.AppendLine(itemNode.InnerText);
                                    }
                                }
                                valueText = valueLines.ToString();
                            }
                            //else if (rdoParseJavaScript.Checked)
                            //{

                            //}
                            else if (rdoParseStream.Checked)
                            {
                                valueStream = await response.Content.ReadAsStreamAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (string.IsNullOrEmpty(valueText))
                            {
                                MessageBox.Show(ex.Message, $"请求成功，解析失败 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                            else
                            {
                                string respFileName = Path.Combine("响应.txt");
                                File.WriteAllText(respFileName, valueText);
                                MessageBox.Show($"{ex.Message}\n响应文已保存在\n{respFileName}", $"请求成功，解析失败 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                            return;
                        }

                        if (valueText.EndsWith("\n"))
                            valueText = valueText.Substring(0, valueText.Length - "\n".Length);

                        //解析成功
                        try
                        {
                            string saveMusicFileName = Path.Combine("msuic");
                            if (rdoSendText.Checked)
                            {
                                MessageBox.Show(valueText, $"成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                            else if (rdoSendImageByUrl.Checked)
                            {
                                using (HttpClient clientDownload = new HttpClient())
                                {
                                    var resp = await clientDownload.GetAsync(valueText);
                                    valueStream = await resp.Content.ReadAsStreamAsync();
                                    using (valueStream)
                                    {
                                        using (Image img = Image.FromStream(valueStream))
                                        {
                                            ImageBox.Show(img, $"{img.Width}×{img.Height}({img.RawFormat})");
                                        }
                                    }
                                }
                            }
                            else if (rdoSendImageByBase64.Checked)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                valueStream = new MemoryStream(data);
                                using (valueStream)
                                {
                                    using (Image img = Image.FromStream(valueStream))
                                    {
                                        ImageBox.Show(img, $"{img.Width}×{img.Height}({img.RawFormat})");
                                    }
                                }
                            }
                            else if (rdoSendImageStream.Checked)
                            {
                                if (valueStream is not null)
                                {
                                    using (valueStream)
                                    {
                                        using (Image img = Image.FromStream(valueStream))
                                        {
                                            ImageBox.Show(img, $"{img.Width}×{img.Height}({img.RawFormat})");
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("流为空", $"解析成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                                }
                            }
                            else if (rdoSendVoiceByUrl.Checked)
                            {
                                using (HttpClient clientDownload = new HttpClient())
                                {
                                    var rep = await clientDownload.GetAsync(valueText);
                                    byte[] data = await rep.Content.ReadAsByteArrayAsync();
                                    File.WriteAllBytes(saveMusicFileName, data);
                                }
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                            else if (rdoSendVoiceByBase64.Checked)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                File.WriteAllBytes(saveMusicFileName, data);
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            }
                            else if (rdoSendVoiceStream.Checked)
                            {
                                if (valueStream is not null)
                                {
                                    using (Stream outStream = File.OpenWrite(saveMusicFileName))
                                    {
                                        valueStream.CopyTo(outStream);
                                    }
                                    MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                                }
                                else
                                {
                                    MessageBox.Show("流为空", $"解析成功 {(int)response.StatusCode}", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}\n{(int)response.StatusCode} {response.ReasonPhrase}", "解析成功，转换失败", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        private void rdoContentType_CheckedChanged(object sender, EventArgs e)
        {
            txbContentRaw.Visible = rdoContentRaw.Checked;
            dgvContentFormData.Visible = rdoContentFormData.Checked;
        }

        private void rdoContentParseMode_CheckedChanged(object sender, EventArgs e)
        {
            pnlParse.Enabled = !rdoParseStream.Checked && !rdoParseText.Checked;
            pnlSubText.Enabled = rdoParseText.Checked;

            if (rdoParseStream.Checked)
            {
                rdoSendImageStream.Enabled = rdoSendVoiceStream.Enabled = true;
                rdoSendText.Enabled = rdoSendImageByUrl.Enabled = rdoSendImageByBase64.Enabled = rdoSendVoiceByUrl.Enabled = rdoSendVoiceByBase64.Enabled = false;
                if (!rdoSendImageStream.Checked && !rdoSendVoiceStream.Checked)
                    rdoSendImageStream.Checked = true;
            }
            else
            {
                rdoSendImageStream.Enabled = rdoSendVoiceStream.Enabled = false;
                rdoSendText.Enabled = rdoSendImageByUrl.Enabled = rdoSendImageByBase64.Enabled = rdoSendVoiceByUrl.Enabled = rdoSendVoiceByBase64.Enabled = true;
                if (rdoSendImageStream.Checked || rdoSendVoiceStream.Checked)
                    rdoSendText.Checked = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            dgvHeader.EndEdit();
            dgvContentFormData.EndEdit();
            Config.Url = txbUrl.Text;
            Config.UseProxy = chkUseProxy.Checked;
            Config.Cmd = txbCmd.Text;
            Config.Remark = txbRemark.Text;
            Config.HelpMessage = txbHelpMessage.Text;
            Config.HttpMethod = (HttpMethodEnum)cboHttpMethod.SelectedIndex;
            Config.Encoding = (EncodingEnum)cboEncoding.SelectedIndex;
            Config.MediaType = cboMediaType.Text;

            for (int i = 0; i < dgvHeader.Rows.Count; i++)
            {
                string? headerKey = dgvHeader.Rows[i].Cells[0].Value?.ToString();
                if (headerKey is not null)
                {
                    if (Config.Headers is null)
                        Config.Headers = new Dictionary<string, string>();
                    Config.Headers.Clear();
                    string? headerValue = dgvHeader.Rows[i].Cells[1].Value?.ToString();
                    Config.Headers.Add(headerKey, headerValue ?? string.Empty);
                }
            }

            Config.ContentType = rdoContentRaw.Checked ? ContentTypeEnum.raw : ContentTypeEnum.form_data;

            Config.RawContent = txbContentRaw.Text;

            for (int i = 0; i < dgvContentFormData.Rows.Count; i++)
            {
                string? conentKey = dgvContentFormData.Rows[i].Cells[0].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(conentKey))
                {
                    if (Config.FormDataContent is null)
                        Config.FormDataContent = new Dictionary<string, string>();
                    Config.FormDataContent.Clear();
                    string? contentValue = dgvContentFormData.Rows[i].Cells[1].Value?.ToString();
                    Config.FormDataContent.Add(conentKey, contentValue ?? string.Empty);
                }
            }

            #region -- 屎山if --
            if (rdoParseText.Checked)
                Config.ParseMode = ParseModeEnum.Text;
            else if (rdoParseJson.Checked)
                Config.ParseMode = ParseModeEnum.Json;
            else if (rdoParseXml.Checked)
                Config.ParseMode = ParseModeEnum.Xml;
            else if (rdoParseXPath.Checked)
                Config.ParseMode = ParseModeEnum.XPath;
            else if (rdoParseJavaScript.Checked)
                Config.ParseMode = ParseModeEnum.JavaScript;
            else if (rdoParseStream.Checked)
                Config.ParseMode = ParseModeEnum.Stream;

            Config.ParseExpression = txbParseExpression.Text.Split('\n');
            Config.SubTextFrom = txbSubTextFrom.Text;
            Config.SubTextTo = txbSubTextTo.Text;
            Config.SubTextWithPrefix = chkSubTextWithPrefix.Checked;
            Config.SubTextWithSuffix = chkSubTextWithSuffix.Checked;

            if (rdoSendText.Checked)
                Config.SendMode = SendModeEnum.Text;
            else if (rdoSendImageByUrl.Checked)
                Config.SendMode = SendModeEnum.ImageUrl;
            else if (rdoSendImageByBase64.Checked)
                Config.SendMode = SendModeEnum.ImageBase64;
            else if (rdoSendVoiceByUrl.Checked)
                Config.SendMode = SendModeEnum.VoiceUrl;
            else if (rdoSendVoiceByBase64.Checked)
                Config.SendMode = SendModeEnum.VoiceBase64;
            else if (rdoSendImageStream.Checked)
                Config.SendMode = SendModeEnum.ImageStream;
            else if (rdoSendVoiceStream.Checked)
                Config.SendMode = SendModeEnum.VoiceStream;

            if (rdoDontChangeAt.Checked)
                Config.ChangeAtTo = ChangeMessageTypeEnum.None;
            else if (rdoChangeAtToQQId.Checked)
                Config.ChangeAtTo = ChangeMessageTypeEnum.QQId;
            else if (rdoChangeAtToNick.Checked)
                Config.ChangeAtTo = ChangeMessageTypeEnum.Nick;
            else if (rdoChangeAtToProfileUrl.Checked)
                Config.ChangeAtTo = ChangeMessageTypeEnum.ProfilePhotoUrl;

            if (rdoDontChangeMe.Checked)
                Config.ChangeMeTo = ChangeMessageTypeEnum.None;
            else if (rdoChangeMeToQQId.Checked)
                Config.ChangeMeTo = ChangeMessageTypeEnum.QQId;
            else if (rdoChangeMeToNick.Checked)
                Config.ChangeMeTo = ChangeMessageTypeEnum.Nick;
            else if (rdoChangeMeToProfileUrl.Checked)
                Config.ChangeMeTo = ChangeMessageTypeEnum.ProfilePhotoUrl;
            #endregion -- 屎山if --

            base.OnClosing(e);
        }
    }
}
