using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class FrmEditor : Form
    {
        private string _path;
        private Config? _config;
        public FrmEditor(string path, Config? config)
        {
            InitializeComponent();
            _path = path;
            _config = config;

            if (_config != null)
            {
                txbUrl.Text = _config.Url;
                txbCmd.Text = _config.Cmd;
                txbRemark.Text = _config.Remark;
                txbHelpMessage.Text = _config.HelpMessage;
                cboHttpMethod.SelectedIndex = (int)_config.HttpMethod;
                cboEncoding.SelectedIndex = (int)_config.Encoding;
                cboMediaType.SelectedIndex = cboMediaType.Items.IndexOf(_config.MediaType);

                if (_config.Headers != null)
                {
                    foreach (var item in _config.Headers)
                        dgvHeader.Rows.Add(item.Key, item.Value);
                }

                switch (_config.ContentType)
                {
                    case ContentTypeEnum.raw:
                        rdoContentRaw.Checked = true;
                        break;
                    case ContentTypeEnum.form_data:
                        rdoContentFormData.Checked = true;
                        break;
                }
                txbContentRaw.Text = _config.RowContent;
                if (_config.FormDataContent != null)
                {
                    foreach (var item in _config.FormDataContent)
                        dgvContentFormData.Rows.Add(item.Key, item.Value);
                }
                switch (_config.ParseMode)
                {
                    case ParseModeEnum.None:
                        rdoDoNotParse.Checked = true;
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
                    case ParseModeEnum.Text:
                        rdoParseText.Checked = true;
                        break;
                }
                txbParseExpression.Text = _config.ParseExpression;
                txbSubTextFrom.Text = _config.SubTextFrom;
                txbSubTextTo.Text = _config.SubTextTo;
                chkSubTextWithPrefix.Checked = _config.SubTextWithPrefix;
                chkSubTextWithSuffix.Checked = _config.SubTextWithSuffix;
                switch (_config.SendMode)
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
                MessageBox.Show("请先输入地址","错误");
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
                            string? headerKey = dgvHeader.Rows[i].Cells[0].Value.ToString();
                            if (headerKey != null)
                                request.Headers.Add(headerKey, dgvHeader.Rows[i].Cells[1].Value.ToString());
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
                        if (rdoContentFormData.Checked)
                        {
                            request.Content = new StringContent(txbContentRaw.Text, encoding, cboMediaType.Text);
                        }
                        else
                        {
                            var form = new MultipartFormDataContent();
                            for (int i = 0; i < dgvContentFormData.Rows.Count; i++)
                            {
                                if (dgvContentFormData.Rows[i].Cells[0].Value != null && dgvContentFormData.Rows[i].Cells[0].Value != DBNull.Value)
                                {
                                    string? conentKey = dgvContentFormData.Rows[i].Cells[0].Value.ToString();
                                    if (!string.IsNullOrWhiteSpace(conentKey))
                                    {
                                        string contentValue = string.Empty;
                                        if (dgvContentFormData.Rows[i].Cells[1].Value != null && dgvContentFormData.Rows[i].Cells[1].Value != DBNull.Value)
                                        {
                                            string? tempContentValue = dgvContentFormData.Rows[i].Cells[1].Value.ToString();
                                            if (tempContentValue != null)
                                                contentValue = tempContentValue;
                                        }
                                        form.Add(new StringContent(contentValue, encoding, cboMediaType.Text), conentKey.ToString()!);
                                    }
                                }
                            }
                            request.Content = form;
                        }

                        HttpResponseMessage response;
                        try
                        {
                            response = await client.SendAsync(request);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("请求异常。" + ex.Message, "请求失败");
                            return;
                        }
                        //请求成功
                        string valueText = string.Empty;
                        Stream? valueStream = null;
                        try
                        {
                            if (rdoContentFormData.Checked)
                            {
                                string text = await response.Content.ReadAsStringAsync();
                                valueText= text;
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
                                            if (long.TryParse(indexName.ToString(), out long numberIndex))
                                                jt = jt[numberIndex]!;
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
                            else if (rdoParseText.Checked)
                            {
                                string text = await response.Content.ReadAsStringAsync();

                                int startIndex = 0;
                                if (!string.IsNullOrEmpty(txbSubTextFrom.Text))
                                    startIndex = text.IndexOf(txbSubTextFrom.Text);
                                if (!chkSubTextWithPrefix.Checked)
                                    startIndex += txbSubTextFrom.Text.Length;

                                text = text.Substring(startIndex);
                                int endIndex = text.Length - 1;
                                if (!string.IsNullOrEmpty(txbSubTextTo.Text))
                                    endIndex = text.IndexOf(txbSubTextTo.Text);
                                if (chkSubTextWithSuffix.Checked)
                                    endIndex += txbSubTextTo.Text.Length;

                                valueText = text.Substring(0, endIndex);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (string.IsNullOrEmpty(valueText))
                            {
                                MessageBox.Show(ex.Message, "请求成功，解析失败");
                            }
                            else
                            {
                                string respFileName = Path.Combine(_path, "响应.txt");
                                File.WriteAllText(respFileName, valueText);
                                MessageBox.Show($"{ex.Message}\r\n响应文已保存在\r\n{respFileName}", "请求成功，解析失败");
                            }
                            return;
                        }
                        //解析成功
                        try
                        {
                            string saveMusicFileName = Path.Combine(_path, "msuic");
                            if (rdoSendText.Checked)
                            {
                                MessageBox.Show(valueText, "成功");
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
                                    MessageBox.Show("流为空", "解析成功");
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
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}","成功");
                            }
                            else if (rdoSendVoiceByBase64.Checked)
                            {
                                byte[] data = Convert.FromBase64String(valueText);
                                File.WriteAllBytes(saveMusicFileName, data);
                                MessageBox.Show($"音频文件已保存在{saveMusicFileName}", "成功");
                            }
                            else if (rdoSendVoiceStream.Checked)
                            {
                                if (valueStream != null)
                                {
                                    using (Stream outStream = File.OpenWrite(saveMusicFileName))
                                    {
                                        valueStream.CopyTo(outStream);
                                    }
                                    MessageBox.Show($"音频文件已保存在{saveMusicFileName}", "成功");
                                }
                                else
                                {
                                    MessageBox.Show("流为空", "解析成功");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "解析成功，转换失败");
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
    }
}
