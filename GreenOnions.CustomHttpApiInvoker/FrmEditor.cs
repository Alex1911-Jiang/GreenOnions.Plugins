using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class FrmEditor : Form
    {
        private string _path;

        public HttpApiConfig Config { get; private set; }

        public FrmEditor(string path, HttpApiConfig? config = null)
        {
            _path = path;
            InitializeComponent();

            if (config == null)
            {
                Config = new HttpApiConfig();
                cboHttpMethod.SelectedIndex = 0;
                cboEncoding.SelectedIndex = 0;
                cboMediaType.SelectedIndex = 0;
            }
            else
            {
                Config = config;
                txbUrl.Text = Config.Url;
                txbCmd.Text = Config.Cmd;
                txbRemark.Text = Config.Remark;
                txbHelpMessage.Text = Config.HelpMessage;
                cboHttpMethod.SelectedIndex = (int)Config.HttpMethod;
                cboEncoding.SelectedIndex = (int)Config.Encoding;
                cboMediaType.SelectedIndex = cboMediaType.Items.IndexOf(Config.MediaType);

                if (Config.Headers != null)
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
                txbContentRaw.Text = Config.RowContent;
                if (Config.FormDataContent != null)
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
                txbParseExpression.Text = Config.ParseExpression;
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
            }
        }

        private async void btnInvokeTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbUrl.Text))
            {
                MessageBox.Show("请先输入地址", "错误");
                return;
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpMethod httpMethod = cboHttpMethod.Text.Equals("GET", StringComparison.OrdinalIgnoreCase) ? HttpMethod.Get : HttpMethod.Post;

                    using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, txbUrl.Text))
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

                        string? requestUrl = request.RequestUri?.ToString();
                        HttpResponseMessage response;
                        try
                        {
                            response = await client.SendAsync(request);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("发起请求异常。" + ex.Message, "请求失败");
                            return;
                        }
                        if ((int)response.StatusCode >= 400)
                        {
                            MessageBox.Show($"{(int)response.StatusCode} {response.ReasonPhrase}{(response.RequestMessage?.RequestUri?.ToString() != requestUrl ? "\r\n发生链接跳转，请尝试使用POST进行请求。" : "")}", $"请求错误");
                            return;
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

                                valueText = text.Substring(0, endIndex).TrimStart('\n').TrimStart('\r').TrimStart('\n');
                            }
                            else if (rdoParseJson.Checked)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                JToken jt = JsonConvert.DeserializeObject<JToken>(json)!;
                                bool bOpen = false;
                                StringBuilder indexName = new StringBuilder();
                                for (int i = 0; i < txbParseExpression.Text.Length; i++)
                                {
                                    if (bOpen)
                                    {
                                        if (txbParseExpression.Text[i] == ']')
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
                                        if (txbParseExpression.Text[i] == '\'' || txbParseExpression.Text[i] == '\"')
                                        {
                                            continue;
                                        }
                                        indexName.Append(txbParseExpression.Text[i]);
                                    }
                                    else
                                    {
                                        if (txbParseExpression.Text[i] == '[')
                                        {
                                            bOpen = true;
                                            continue;
                                        }
                                    }
                                }
                                valueText = jt.ToString();
                            }
                            //else if (rdoParseXml.Checked)
                            //{
                            //}
                            //else if (rdoParseXPath.Checked)
                            //{

                            //}
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
                                MessageBox.Show(ex.Message, $"请求成功，解析失败 {(int)response.StatusCode}");
                            }
                            else
                            {
                                string respFileName = Path.Combine(_path, "响应.txt");
                                File.WriteAllText(respFileName, valueText);
                                MessageBox.Show($"{ex.Message}\r\n响应文已保存在\r\n{respFileName}", $"请求成功，解析失败 {(int)response.StatusCode}");
                            }
                            return;
                        }
                        //解析成功
                        try
                        {
                            string saveMusicFileName = Path.Combine(_path, "msuic");
                            if (rdoSendText.Checked)
                            {
                                MessageBox.Show(valueText, $"成功 {(int)response.StatusCode}");
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
                                if (valueStream != null)
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
                                    MessageBox.Show("流为空", $"解析成功 {(int)response.StatusCode}");
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
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}");
                            }
                            else if (rdoSendVoiceByBase64.Checked)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                File.WriteAllBytes(saveMusicFileName, data);
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}");
                            }
                            else if (rdoSendVoiceStream.Checked)
                            {
                                if (valueStream != null)
                                {
                                    using (Stream outStream = File.OpenWrite(saveMusicFileName))
                                    {
                                        valueStream.CopyTo(outStream);
                                    }
                                    MessageBox.Show($"音频文件已保存在{saveMusicFileName}", $"成功 {(int)response.StatusCode}");
                                }
                                else
                                {
                                    MessageBox.Show("流为空", $"解析成功 {(int)response.StatusCode}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}\r\n{(int)response.StatusCode} {response.ReasonPhrase}", "解析成功，转换失败");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
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
            base.OnClosing(e);

            Config.Url = txbUrl.Text;
            Config.Cmd = txbCmd.Text;
            Config.Remark = txbRemark.Text;
            Config.HelpMessage = txbHelpMessage.Text;
            Config.HttpMethod = (HttpMethodEnum)cboHttpMethod.SelectedIndex;
            Config.Encoding = (EncodingEnum)cboEncoding.SelectedIndex;
            Config.MediaType = cboMediaType.Text;

            for (int i = 0; i < dgvHeader.Rows.Count; i++)
            {
                string? headerKey = dgvHeader.Rows[i].Cells[0].Value?.ToString();
                if (headerKey != null)
                {
                    if (Config.Headers == null)
                        Config.Headers = new Dictionary<string, string>();
                    string? headerValue = dgvHeader.Rows[i].Cells[1].Value?.ToString();
                    Config.Headers.Add(headerKey, headerValue ?? string.Empty);
                }
            }

            Config.ContentType = rdoContentRaw.Checked ? ContentTypeEnum.raw : ContentTypeEnum.form_data;

            Config.RowContent = txbContentRaw.Text;

            for (int i = 0; i < dgvContentFormData.Rows.Count; i++)
            {
                string? conentKey = dgvContentFormData.Rows[i].Cells[0].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(conentKey))
                {
                    if (Config.FormDataContent == null)
                        Config.FormDataContent = new Dictionary<string, string>();
                    string? contentValue = dgvContentFormData.Rows[i].Cells[1].Value?.ToString();
                    Config.FormDataContent.Add(conentKey, contentValue ?? string.Empty);
                }
            }

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

            Config.ParseExpression = txbParseExpression.Text;
            Config.SubTextFrom = txbSubTextFrom.Text;
            Config.SubTextTo = txbSubTextTo.Text;
            Config.SubTextWithPrefix = chkSubTextWithPrefix.Checked;
            Config.SubTextWithSuffix = chkSubTextWithSuffix.Checked;

            if (rdoSendText.Checked)
            {
                Config.SendMode = SendModeEnum.Text;
            }
            else if (rdoSendImageByUrl.Checked)
            {
                Config.SendMode = SendModeEnum.ImageUrl;
            }
            else if (rdoSendImageByBase64.Checked)
            {
                Config.SendMode = SendModeEnum.ImageBase64;
            }
            else if (rdoSendVoiceByUrl.Checked)
            {
                Config.SendMode = SendModeEnum.VoiceUrl;
            }
            else if (rdoSendVoiceByBase64.Checked)
            {
                Config.SendMode = SendModeEnum.VoiceBase64;
            }
            else if (rdoSendImageStream.Checked)
            {
                Config.SendMode = SendModeEnum.ImageStream;
            }
            else if (rdoSendVoiceStream.Checked)
            {
                Config.SendMode = SendModeEnum.VoiceStream;
            }
        }
    }
}
