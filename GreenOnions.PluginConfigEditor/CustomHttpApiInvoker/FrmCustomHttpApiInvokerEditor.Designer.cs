namespace GreenOnions.PluginConfigEditor.CustomHttpApiInvoker
{
    partial class FrmCustomHttpApiInvokerEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txbContentRaw = new System.Windows.Forms.TextBox();
            this.pnlContentType = new System.Windows.Forms.Panel();
            this.rdoContentRaw = new System.Windows.Forms.RadioButton();
            this.rdoContentFormData = new System.Windows.Forms.RadioButton();
            this.dgvContentFormData = new System.Windows.Forms.DataGridView();
            this.dgvContentKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvContentValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnInvokeTest = new System.Windows.Forms.Button();
            this.pnlParse = new System.Windows.Forms.Panel();
            this.txbParseExpression = new System.Windows.Forms.TextBox();
            this.lblParseExpression = new System.Windows.Forms.Label();
            this.pnlSubText = new System.Windows.Forms.Panel();
            this.lblSubText = new System.Windows.Forms.Label();
            this.chkSubTextWithSuffix = new System.Windows.Forms.CheckBox();
            this.chkSubTextWithPrefix = new System.Windows.Forms.CheckBox();
            this.lblSubTextFrom = new System.Windows.Forms.Label();
            this.txbSubTextFrom = new System.Windows.Forms.TextBox();
            this.txbSubTextTo = new System.Windows.Forms.TextBox();
            this.lblSubTextTo = new System.Windows.Forms.Label();
            this.pnlSendMode = new System.Windows.Forms.Panel();
            this.rdoSendImageStream = new System.Windows.Forms.RadioButton();
            this.rdoSendVoiceStream = new System.Windows.Forms.RadioButton();
            this.rdoSendVoiceByUrl = new System.Windows.Forms.RadioButton();
            this.rdoSendVoiceByBase64 = new System.Windows.Forms.RadioButton();
            this.rdoSendImageByBase64 = new System.Windows.Forms.RadioButton();
            this.rdoSendImageByUrl = new System.Windows.Forms.RadioButton();
            this.rdoSendText = new System.Windows.Forms.RadioButton();
            this.pnlParseMode = new System.Windows.Forms.Panel();
            this.rdoParseJavaScript = new System.Windows.Forms.RadioButton();
            this.rdoParseStream = new System.Windows.Forms.RadioButton();
            this.rdoParseText = new System.Windows.Forms.RadioButton();
            this.rdoParseJson = new System.Windows.Forms.RadioButton();
            this.rdoParseXml = new System.Windows.Forms.RadioButton();
            this.rdoParseXPath = new System.Windows.Forms.RadioButton();
            this.lblSendMode = new System.Windows.Forms.Label();
            this.lblParseMode = new System.Windows.Forms.Label();
            this.txbCmd = new System.Windows.Forms.TextBox();
            this.lblCmdTitle = new System.Windows.Forms.Label();
            this.dgvHeader = new System.Windows.Forms.DataGridView();
            this.dgvHeaderKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHeaderValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboEncoding = new System.Windows.Forms.ComboBox();
            this.lblEncodeTitle = new System.Windows.Forms.Label();
            this.cboMediaType = new System.Windows.Forms.ComboBox();
            this.lblMediaTypeTitle = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.cboHttpMethod = new System.Windows.Forms.ComboBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblHttpMethodTitle = new System.Windows.Forms.Label();
            this.txbUrl = new System.Windows.Forms.TextBox();
            this.lblUrlTitle = new System.Windows.Forms.Label();
            this.txbRemark = new System.Windows.Forms.TextBox();
            this.lblRemarkTitle = new System.Windows.Forms.Label();
            this.txbHelpMessage = new System.Windows.Forms.TextBox();
            this.lblHelpMessageTitle = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblChangeAtTo = new System.Windows.Forms.Label();
            this.lblChangeMeTo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoChangeAtToProfileUrl = new System.Windows.Forms.RadioButton();
            this.rdoChangeAtToNick = new System.Windows.Forms.RadioButton();
            this.rdoChangeAtToQQId = new System.Windows.Forms.RadioButton();
            this.rdoDontChangeAt = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoChangeMeToProfileUrl = new System.Windows.Forms.RadioButton();
            this.rdoChangeMeToNick = new System.Windows.Forms.RadioButton();
            this.rdoChangeMeToQQId = new System.Windows.Forms.RadioButton();
            this.rdoDontChangeMe = new System.Windows.Forms.RadioButton();
            this.lblChangeTypeInfo = new System.Windows.Forms.Label();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.pnlContentType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContentFormData)).BeginInit();
            this.pnlParse.SuspendLayout();
            this.pnlSubText.SuspendLayout();
            this.pnlSendMode.SuspendLayout();
            this.pnlParseMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbContentRaw
            // 
            this.txbContentRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbContentRaw.Location = new System.Drawing.Point(25, 559);
            this.txbContentRaw.Multiline = true;
            this.txbContentRaw.Name = "txbContentRaw";
            this.txbContentRaw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbContentRaw.Size = new System.Drawing.Size(900, 165);
            this.txbContentRaw.TabIndex = 60;
            // 
            // pnlContentType
            // 
            this.pnlContentType.AutoSize = true;
            this.pnlContentType.Controls.Add(this.rdoContentRaw);
            this.pnlContentType.Controls.Add(this.rdoContentFormData);
            this.pnlContentType.Location = new System.Drawing.Point(184, 522);
            this.pnlContentType.Name = "pnlContentType";
            this.pnlContentType.Size = new System.Drawing.Size(220, 35);
            this.pnlContentType.TabIndex = 59;
            // 
            // rdoContentRaw
            // 
            this.rdoContentRaw.AutoSize = true;
            this.rdoContentRaw.Checked = true;
            this.rdoContentRaw.Location = new System.Drawing.Point(3, 3);
            this.rdoContentRaw.Name = "rdoContentRaw";
            this.rdoContentRaw.Size = new System.Drawing.Size(66, 28);
            this.rdoContentRaw.TabIndex = 0;
            this.rdoContentRaw.TabStop = true;
            this.rdoContentRaw.Text = "raw";
            this.rdoContentRaw.UseVisualStyleBackColor = true;
            this.rdoContentRaw.AppearanceChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            this.rdoContentRaw.CheckedChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            // 
            // rdoContentFormData
            // 
            this.rdoContentFormData.AutoSize = true;
            this.rdoContentFormData.Location = new System.Drawing.Point(83, 3);
            this.rdoContentFormData.Name = "rdoContentFormData";
            this.rdoContentFormData.Size = new System.Drawing.Size(123, 28);
            this.rdoContentFormData.TabIndex = 1;
            this.rdoContentFormData.Text = "form-data";
            this.rdoContentFormData.UseVisualStyleBackColor = true;
            this.rdoContentFormData.AppearanceChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            this.rdoContentFormData.CheckedChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            // 
            // dgvContentFormData
            // 
            this.dgvContentFormData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContentFormData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContentFormData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContentFormData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvContentKeyColumn,
            this.dgvContentValueColumn});
            this.dgvContentFormData.Location = new System.Drawing.Point(25, 559);
            this.dgvContentFormData.Name = "dgvContentFormData";
            this.dgvContentFormData.RowHeadersVisible = false;
            this.dgvContentFormData.RowHeadersWidth = 62;
            this.dgvContentFormData.RowTemplate.Height = 32;
            this.dgvContentFormData.Size = new System.Drawing.Size(900, 165);
            this.dgvContentFormData.TabIndex = 58;
            this.dgvContentFormData.Visible = false;
            // 
            // dgvContentKeyColumn
            // 
            this.dgvContentKeyColumn.FillWeight = 20F;
            this.dgvContentKeyColumn.HeaderText = "Key";
            this.dgvContentKeyColumn.MinimumWidth = 8;
            this.dgvContentKeyColumn.Name = "dgvContentKeyColumn";
            // 
            // dgvContentValueColumn
            // 
            this.dgvContentValueColumn.HeaderText = "Value";
            this.dgvContentValueColumn.MinimumWidth = 8;
            this.dgvContentValueColumn.Name = "dgvContentValueColumn";
            // 
            // btnInvokeTest
            // 
            this.btnInvokeTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInvokeTest.Location = new System.Drawing.Point(809, 73);
            this.btnInvokeTest.Name = "btnInvokeTest";
            this.btnInvokeTest.Size = new System.Drawing.Size(113, 32);
            this.btnInvokeTest.TabIndex = 57;
            this.btnInvokeTest.Text = "测试调用";
            this.btnInvokeTest.UseVisualStyleBackColor = true;
            this.btnInvokeTest.Click += new System.EventHandler(this.btnInvokeTest_Click);
            // 
            // pnlParse
            // 
            this.pnlParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlParse.Controls.Add(this.txbParseExpression);
            this.pnlParse.Controls.Add(this.lblParseExpression);
            this.pnlParse.Enabled = false;
            this.pnlParse.Location = new System.Drawing.Point(322, 775);
            this.pnlParse.Name = "pnlParse";
            this.pnlParse.Size = new System.Drawing.Size(603, 150);
            this.pnlParse.TabIndex = 56;
            // 
            // txbParseExpression
            // 
            this.txbParseExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbParseExpression.Location = new System.Drawing.Point(0, 24);
            this.txbParseExpression.Multiline = true;
            this.txbParseExpression.Name = "txbParseExpression";
            this.txbParseExpression.Size = new System.Drawing.Size(603, 126);
            this.txbParseExpression.TabIndex = 19;
            // 
            // lblParseExpression
            // 
            this.lblParseExpression.AutoSize = true;
            this.lblParseExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblParseExpression.Location = new System.Drawing.Point(0, 0);
            this.lblParseExpression.Name = "lblParseExpression";
            this.lblParseExpression.Size = new System.Drawing.Size(591, 24);
            this.lblParseExpression.TabIndex = 18;
            this.lblParseExpression.Text = "解析表达式：（Json解析特殊表达式<random>随机取数组中一个元素）";
            // 
            // pnlSubText
            // 
            this.pnlSubText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSubText.Controls.Add(this.lblSubText);
            this.pnlSubText.Controls.Add(this.chkSubTextWithSuffix);
            this.pnlSubText.Controls.Add(this.chkSubTextWithPrefix);
            this.pnlSubText.Controls.Add(this.lblSubTextFrom);
            this.pnlSubText.Controls.Add(this.txbSubTextFrom);
            this.pnlSubText.Controls.Add(this.txbSubTextTo);
            this.pnlSubText.Controls.Add(this.lblSubTextTo);
            this.pnlSubText.Location = new System.Drawing.Point(25, 772);
            this.pnlSubText.Name = "pnlSubText";
            this.pnlSubText.Size = new System.Drawing.Size(291, 150);
            this.pnlSubText.TabIndex = 55;
            // 
            // lblSubText
            // 
            this.lblSubText.AutoSize = true;
            this.lblSubText.Location = new System.Drawing.Point(0, 121);
            this.lblSubText.Name = "lblSubText";
            this.lblSubText.Size = new System.Drawing.Size(46, 24);
            this.lblSubText.TabIndex = 30;
            this.lblSubText.Text = "之间";
            // 
            // chkSubTextWithSuffix
            // 
            this.chkSubTextWithSuffix.AutoSize = true;
            this.chkSubTextWithSuffix.Location = new System.Drawing.Point(171, 121);
            this.chkSubTextWithSuffix.Name = "chkSubTextWithSuffix";
            this.chkSubTextWithSuffix.Size = new System.Drawing.Size(108, 28);
            this.chkSubTextWithSuffix.TabIndex = 29;
            this.chkSubTextWithSuffix.Text = "包含后缀";
            this.chkSubTextWithSuffix.UseVisualStyleBackColor = true;
            // 
            // chkSubTextWithPrefix
            // 
            this.chkSubTextWithPrefix.AutoSize = true;
            this.chkSubTextWithPrefix.Location = new System.Drawing.Point(57, 121);
            this.chkSubTextWithPrefix.Name = "chkSubTextWithPrefix";
            this.chkSubTextWithPrefix.Size = new System.Drawing.Size(108, 28);
            this.chkSubTextWithPrefix.TabIndex = 28;
            this.chkSubTextWithPrefix.Text = "包含前缀";
            this.chkSubTextWithPrefix.UseVisualStyleBackColor = true;
            // 
            // lblSubTextFrom
            // 
            this.lblSubTextFrom.AutoSize = true;
            this.lblSubTextFrom.Location = new System.Drawing.Point(0, 0);
            this.lblSubTextFrom.Name = "lblSubTextFrom";
            this.lblSubTextFrom.Size = new System.Drawing.Size(46, 24);
            this.lblSubTextFrom.TabIndex = 24;
            this.lblSubTextFrom.Text = "从：";
            // 
            // txbSubTextFrom
            // 
            this.txbSubTextFrom.Location = new System.Drawing.Point(0, 27);
            this.txbSubTextFrom.Name = "txbSubTextFrom";
            this.txbSubTextFrom.Size = new System.Drawing.Size(288, 30);
            this.txbSubTextFrom.TabIndex = 23;
            // 
            // txbSubTextTo
            // 
            this.txbSubTextTo.Location = new System.Drawing.Point(0, 88);
            this.txbSubTextTo.Name = "txbSubTextTo";
            this.txbSubTextTo.Size = new System.Drawing.Size(288, 30);
            this.txbSubTextTo.TabIndex = 25;
            // 
            // lblSubTextTo
            // 
            this.lblSubTextTo.AutoSize = true;
            this.lblSubTextTo.Location = new System.Drawing.Point(0, 59);
            this.lblSubTextTo.Name = "lblSubTextTo";
            this.lblSubTextTo.Size = new System.Drawing.Size(46, 24);
            this.lblSubTextTo.TabIndex = 26;
            this.lblSubTextTo.Text = "到：";
            // 
            // pnlSendMode
            // 
            this.pnlSendMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSendMode.AutoSize = true;
            this.pnlSendMode.Controls.Add(this.rdoSendImageStream);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceStream);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceByUrl);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceByBase64);
            this.pnlSendMode.Controls.Add(this.rdoSendImageByBase64);
            this.pnlSendMode.Controls.Add(this.rdoSendImageByUrl);
            this.pnlSendMode.Controls.Add(this.rdoSendText);
            this.pnlSendMode.Location = new System.Drawing.Point(149, 929);
            this.pnlSendMode.Name = "pnlSendMode";
            this.pnlSendMode.Size = new System.Drawing.Size(778, 35);
            this.pnlSendMode.TabIndex = 54;
            // 
            // rdoSendImageStream
            // 
            this.rdoSendImageStream.AutoSize = true;
            this.rdoSendImageStream.Enabled = false;
            this.rdoSendImageStream.Location = new System.Drawing.Point(581, 3);
            this.rdoSendImageStream.Name = "rdoSendImageStream";
            this.rdoSendImageStream.Size = new System.Drawing.Size(89, 28);
            this.rdoSendImageStream.TabIndex = 5;
            this.rdoSendImageStream.TabStop = true;
            this.rdoSendImageStream.Text = "图片流";
            this.rdoSendImageStream.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceStream
            // 
            this.rdoSendVoiceStream.AutoSize = true;
            this.rdoSendVoiceStream.Enabled = false;
            this.rdoSendVoiceStream.Location = new System.Drawing.Point(677, 3);
            this.rdoSendVoiceStream.Name = "rdoSendVoiceStream";
            this.rdoSendVoiceStream.Size = new System.Drawing.Size(89, 28);
            this.rdoSendVoiceStream.TabIndex = 6;
            this.rdoSendVoiceStream.TabStop = true;
            this.rdoSendVoiceStream.Text = "音频流";
            this.rdoSendVoiceStream.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceByUrl
            // 
            this.rdoSendVoiceByUrl.AutoSize = true;
            this.rdoSendVoiceByUrl.Location = new System.Drawing.Point(332, 3);
            this.rdoSendVoiceByUrl.Name = "rdoSendVoiceByUrl";
            this.rdoSendVoiceByUrl.Size = new System.Drawing.Size(107, 28);
            this.rdoSendVoiceByUrl.TabIndex = 3;
            this.rdoSendVoiceByUrl.Text = "音频地址";
            this.rdoSendVoiceByUrl.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceByBase64
            // 
            this.rdoSendVoiceByBase64.AutoSize = true;
            this.rdoSendVoiceByBase64.Location = new System.Drawing.Point(445, 3);
            this.rdoSendVoiceByBase64.Name = "rdoSendVoiceByBase64";
            this.rdoSendVoiceByBase64.Size = new System.Drawing.Size(132, 28);
            this.rdoSendVoiceByBase64.TabIndex = 4;
            this.rdoSendVoiceByBase64.Text = "音频Base64";
            this.rdoSendVoiceByBase64.UseVisualStyleBackColor = true;
            // 
            // rdoSendImageByBase64
            // 
            this.rdoSendImageByBase64.AutoSize = true;
            this.rdoSendImageByBase64.Location = new System.Drawing.Point(193, 3);
            this.rdoSendImageByBase64.Name = "rdoSendImageByBase64";
            this.rdoSendImageByBase64.Size = new System.Drawing.Size(132, 28);
            this.rdoSendImageByBase64.TabIndex = 2;
            this.rdoSendImageByBase64.Text = "图片Base64";
            this.rdoSendImageByBase64.UseVisualStyleBackColor = true;
            // 
            // rdoSendImageByUrl
            // 
            this.rdoSendImageByUrl.AutoSize = true;
            this.rdoSendImageByUrl.Location = new System.Drawing.Point(80, 3);
            this.rdoSendImageByUrl.Name = "rdoSendImageByUrl";
            this.rdoSendImageByUrl.Size = new System.Drawing.Size(107, 28);
            this.rdoSendImageByUrl.TabIndex = 1;
            this.rdoSendImageByUrl.Text = "图片地址";
            this.rdoSendImageByUrl.UseVisualStyleBackColor = true;
            // 
            // rdoSendText
            // 
            this.rdoSendText.AutoSize = true;
            this.rdoSendText.Checked = true;
            this.rdoSendText.Location = new System.Drawing.Point(3, 3);
            this.rdoSendText.Name = "rdoSendText";
            this.rdoSendText.Size = new System.Drawing.Size(71, 28);
            this.rdoSendText.TabIndex = 0;
            this.rdoSendText.TabStop = true;
            this.rdoSendText.Text = "文本";
            this.rdoSendText.UseVisualStyleBackColor = true;
            // 
            // pnlParseMode
            // 
            this.pnlParseMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlParseMode.AutoSize = true;
            this.pnlParseMode.Controls.Add(this.rdoParseJavaScript);
            this.pnlParseMode.Controls.Add(this.rdoParseStream);
            this.pnlParseMode.Controls.Add(this.rdoParseText);
            this.pnlParseMode.Controls.Add(this.rdoParseJson);
            this.pnlParseMode.Controls.Add(this.rdoParseXml);
            this.pnlParseMode.Controls.Add(this.rdoParseXPath);
            this.pnlParseMode.Location = new System.Drawing.Point(138, 733);
            this.pnlParseMode.Name = "pnlParseMode";
            this.pnlParseMode.Size = new System.Drawing.Size(787, 37);
            this.pnlParseMode.TabIndex = 53;
            // 
            // rdoParseJavaScript
            // 
            this.rdoParseJavaScript.AutoSize = true;
            this.rdoParseJavaScript.Enabled = false;
            this.rdoParseJavaScript.Location = new System.Drawing.Point(358, 4);
            this.rdoParseJavaScript.Name = "rdoParseJavaScript";
            this.rdoParseJavaScript.Size = new System.Drawing.Size(121, 28);
            this.rdoParseJavaScript.TabIndex = 24;
            this.rdoParseJavaScript.Text = "JavaScript";
            this.rdoParseJavaScript.UseVisualStyleBackColor = true;
            this.rdoParseJavaScript.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseStream
            // 
            this.rdoParseStream.AutoSize = true;
            this.rdoParseStream.Location = new System.Drawing.Point(497, 4);
            this.rdoParseStream.Name = "rdoParseStream";
            this.rdoParseStream.Size = new System.Drawing.Size(53, 28);
            this.rdoParseStream.TabIndex = 23;
            this.rdoParseStream.Text = "流";
            this.rdoParseStream.UseVisualStyleBackColor = true;
            this.rdoParseStream.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseText
            // 
            this.rdoParseText.AutoSize = true;
            this.rdoParseText.Checked = true;
            this.rdoParseText.Location = new System.Drawing.Point(5, 4);
            this.rdoParseText.Name = "rdoParseText";
            this.rdoParseText.Size = new System.Drawing.Size(71, 28);
            this.rdoParseText.TabIndex = 14;
            this.rdoParseText.TabStop = true;
            this.rdoParseText.Text = "文本";
            this.rdoParseText.UseVisualStyleBackColor = true;
            this.rdoParseText.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseJson
            // 
            this.rdoParseJson.AutoSize = true;
            this.rdoParseJson.Location = new System.Drawing.Point(90, 4);
            this.rdoParseJson.Name = "rdoParseJson";
            this.rdoParseJson.Size = new System.Drawing.Size(72, 28);
            this.rdoParseJson.TabIndex = 15;
            this.rdoParseJson.Text = "Json";
            this.rdoParseJson.UseVisualStyleBackColor = true;
            this.rdoParseJson.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseXml
            // 
            this.rdoParseXml.AutoSize = true;
            this.rdoParseXml.Enabled = false;
            this.rdoParseXml.Location = new System.Drawing.Point(178, 4);
            this.rdoParseXml.Name = "rdoParseXml";
            this.rdoParseXml.Size = new System.Drawing.Size(69, 28);
            this.rdoParseXml.TabIndex = 19;
            this.rdoParseXml.Text = "Xml";
            this.rdoParseXml.UseVisualStyleBackColor = true;
            this.rdoParseXml.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseXPath
            // 
            this.rdoParseXPath.AutoSize = true;
            this.rdoParseXPath.Location = new System.Drawing.Point(259, 4);
            this.rdoParseXPath.Name = "rdoParseXPath";
            this.rdoParseXPath.Size = new System.Drawing.Size(86, 28);
            this.rdoParseXPath.TabIndex = 20;
            this.rdoParseXPath.Text = "XPath";
            this.rdoParseXPath.UseVisualStyleBackColor = true;
            this.rdoParseXPath.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // lblSendMode
            // 
            this.lblSendMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSendMode.AutoSize = true;
            this.lblSendMode.Location = new System.Drawing.Point(25, 935);
            this.lblSendMode.Name = "lblSendMode";
            this.lblSendMode.Size = new System.Drawing.Size(118, 24);
            this.lblSendMode.TabIndex = 52;
            this.lblSendMode.Text = "解析后视为：";
            // 
            // lblParseMode
            // 
            this.lblParseMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblParseMode.AutoSize = true;
            this.lblParseMode.Location = new System.Drawing.Point(25, 740);
            this.lblParseMode.Name = "lblParseMode";
            this.lblParseMode.Size = new System.Drawing.Size(100, 24);
            this.lblParseMode.TabIndex = 51;
            this.lblParseMode.Text = "解析方式：";
            // 
            // txbCmd
            // 
            this.txbCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCmd.Location = new System.Drawing.Point(129, 73);
            this.txbCmd.Name = "txbCmd";
            this.txbCmd.Size = new System.Drawing.Size(674, 30);
            this.txbCmd.TabIndex = 50;
            // 
            // lblCmdTitle
            // 
            this.lblCmdTitle.AutoSize = true;
            this.lblCmdTitle.Location = new System.Drawing.Point(24, 76);
            this.lblCmdTitle.Name = "lblCmdTitle";
            this.lblCmdTitle.Size = new System.Drawing.Size(64, 24);
            this.lblCmdTitle.TabIndex = 49;
            this.lblCmdTitle.Text = "命令：";
            // 
            // dgvHeader
            // 
            this.dgvHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvHeaderKeyColumn,
            this.dgvHeaderValueColumn});
            this.dgvHeader.Location = new System.Drawing.Point(25, 343);
            this.dgvHeader.Name = "dgvHeader";
            this.dgvHeader.RowHeadersVisible = false;
            this.dgvHeader.RowHeadersWidth = 62;
            this.dgvHeader.RowTemplate.Height = 32;
            this.dgvHeader.Size = new System.Drawing.Size(900, 175);
            this.dgvHeader.TabIndex = 48;
            // 
            // dgvHeaderKeyColumn
            // 
            this.dgvHeaderKeyColumn.FillWeight = 20F;
            this.dgvHeaderKeyColumn.HeaderText = "Key";
            this.dgvHeaderKeyColumn.MinimumWidth = 8;
            this.dgvHeaderKeyColumn.Name = "dgvHeaderKeyColumn";
            // 
            // dgvHeaderValueColumn
            // 
            this.dgvHeaderValueColumn.HeaderText = "Value";
            this.dgvHeaderValueColumn.MinimumWidth = 8;
            this.dgvHeaderValueColumn.Name = "dgvHeaderValueColumn";
            // 
            // cboEncoding
            // 
            this.cboEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncoding.FormattingEnabled = true;
            this.cboEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "Unicode(UTF-16)",
            "BigEndianUnicode(UTF-16)",
            "UTF-32",
            "ASCII",
            "GBK(GB2312)"});
            this.cboEncoding.Location = new System.Drawing.Point(435, 282);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Size = new System.Drawing.Size(182, 32);
            this.cboEncoding.TabIndex = 47;
            // 
            // lblEncodeTitle
            // 
            this.lblEncodeTitle.AutoSize = true;
            this.lblEncodeTitle.Location = new System.Drawing.Point(328, 287);
            this.lblEncodeTitle.Name = "lblEncodeTitle";
            this.lblEncodeTitle.Size = new System.Drawing.Size(100, 24);
            this.lblEncodeTitle.TabIndex = 46;
            this.lblEncodeTitle.Text = "编码格式：";
            // 
            // cboMediaType
            // 
            this.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMediaType.FormattingEnabled = true;
            this.cboMediaType.Items.AddRange(new object[] {
            "application/text",
            "application/json",
            "application/xml"});
            this.cboMediaType.Location = new System.Drawing.Point(742, 282);
            this.cboMediaType.Name = "cboMediaType";
            this.cboMediaType.Size = new System.Drawing.Size(182, 32);
            this.cboMediaType.TabIndex = 45;
            // 
            // lblMediaTypeTitle
            // 
            this.lblMediaTypeTitle.AutoSize = true;
            this.lblMediaTypeTitle.Location = new System.Drawing.Point(633, 287);
            this.lblMediaTypeTitle.Name = "lblMediaTypeTitle";
            this.lblMediaTypeTitle.Size = new System.Drawing.Size(100, 24);
            this.lblMediaTypeTitle.TabIndex = 44;
            this.lblMediaTypeTitle.Text = "媒体类型：";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(25, 528);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(153, 24);
            this.lblContent.TabIndex = 43;
            this.lblContent.Text = "Content(Body)：";
            // 
            // cboHttpMethod
            // 
            this.cboHttpMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHttpMethod.FormattingEnabled = true;
            this.cboHttpMethod.Items.AddRange(new object[] {
            "GET",
            "POST"});
            this.cboHttpMethod.Location = new System.Drawing.Point(129, 282);
            this.cboHttpMethod.Name = "cboHttpMethod";
            this.cboHttpMethod.Size = new System.Drawing.Size(182, 32);
            this.cboHttpMethod.TabIndex = 42;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(25, 318);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(91, 24);
            this.lblHeader.TabIndex = 41;
            this.lblHeader.Text = "Header：";
            // 
            // lblHttpMethodTitle
            // 
            this.lblHttpMethodTitle.AutoSize = true;
            this.lblHttpMethodTitle.Location = new System.Drawing.Point(25, 287);
            this.lblHttpMethodTitle.Name = "lblHttpMethodTitle";
            this.lblHttpMethodTitle.Size = new System.Drawing.Size(100, 24);
            this.lblHttpMethodTitle.TabIndex = 40;
            this.lblHttpMethodTitle.Text = "请求方法：";
            // 
            // txbUrl
            // 
            this.txbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbUrl.Location = new System.Drawing.Point(129, 37);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(674, 30);
            this.txbUrl.TabIndex = 39;
            // 
            // lblUrlTitle
            // 
            this.lblUrlTitle.AutoSize = true;
            this.lblUrlTitle.Location = new System.Drawing.Point(24, 41);
            this.lblUrlTitle.Name = "lblUrlTitle";
            this.lblUrlTitle.Size = new System.Drawing.Size(64, 24);
            this.lblUrlTitle.TabIndex = 38;
            this.lblUrlTitle.Text = "地址：";
            // 
            // txbRemark
            // 
            this.txbRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbRemark.Location = new System.Drawing.Point(129, 112);
            this.txbRemark.Name = "txbRemark";
            this.txbRemark.Size = new System.Drawing.Size(793, 30);
            this.txbRemark.TabIndex = 62;
            // 
            // lblRemarkTitle
            // 
            this.lblRemarkTitle.AutoSize = true;
            this.lblRemarkTitle.Location = new System.Drawing.Point(24, 117);
            this.lblRemarkTitle.Name = "lblRemarkTitle";
            this.lblRemarkTitle.Size = new System.Drawing.Size(64, 24);
            this.lblRemarkTitle.TabIndex = 61;
            this.lblRemarkTitle.Text = "备注：";
            // 
            // txbHelpMessage
            // 
            this.txbHelpMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbHelpMessage.Location = new System.Drawing.Point(129, 150);
            this.txbHelpMessage.Name = "txbHelpMessage";
            this.txbHelpMessage.Size = new System.Drawing.Size(793, 30);
            this.txbHelpMessage.TabIndex = 64;
            // 
            // lblHelpMessageTitle
            // 
            this.lblHelpMessageTitle.AutoSize = true;
            this.lblHelpMessageTitle.Location = new System.Drawing.Point(24, 152);
            this.lblHelpMessageTitle.Name = "lblHelpMessageTitle";
            this.lblHelpMessageTitle.Size = new System.Drawing.Size(100, 24);
            this.lblHelpMessageTitle.TabIndex = 63;
            this.lblHelpMessageTitle.Text = "帮助信息：";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(38, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(873, 24);
            this.lblInfo.TabIndex = 65;
            this.lblInfo.Text = "您可以在命令中添加\"(?<参数>)\"正则表达式提取子串，并在地址地址或参数栏中添加<参数>来进行带参调用";
            // 
            // lblChangeAtTo
            // 
            this.lblChangeAtTo.AutoSize = true;
            this.lblChangeAtTo.Location = new System.Drawing.Point(25, 196);
            this.lblChangeAtTo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChangeAtTo.Name = "lblChangeAtTo";
            this.lblChangeAtTo.Size = new System.Drawing.Size(137, 24);
            this.lblChangeAtTo.TabIndex = 66;
            this.lblChangeAtTo.Text = "转换@消息为：";
            // 
            // lblChangeMeTo
            // 
            this.lblChangeMeTo.AutoSize = true;
            this.lblChangeMeTo.Location = new System.Drawing.Point(25, 244);
            this.lblChangeMeTo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblChangeMeTo.Name = "lblChangeMeTo";
            this.lblChangeMeTo.Size = new System.Drawing.Size(132, 24);
            this.lblChangeMeTo.TabIndex = 67;
            this.lblChangeMeTo.Text = "转换“我”字为：";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.rdoChangeAtToProfileUrl);
            this.panel1.Controls.Add(this.rdoChangeAtToNick);
            this.panel1.Controls.Add(this.rdoChangeAtToQQId);
            this.panel1.Controls.Add(this.rdoDontChangeAt);
            this.panel1.Location = new System.Drawing.Point(179, 189);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 38);
            this.panel1.TabIndex = 68;
            // 
            // rdoChangeAtToProfileUrl
            // 
            this.rdoChangeAtToProfileUrl.AutoSize = true;
            this.rdoChangeAtToProfileUrl.Location = new System.Drawing.Point(340, 4);
            this.rdoChangeAtToProfileUrl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeAtToProfileUrl.Name = "rdoChangeAtToProfileUrl";
            this.rdoChangeAtToProfileUrl.Size = new System.Drawing.Size(107, 28);
            this.rdoChangeAtToProfileUrl.TabIndex = 4;
            this.rdoChangeAtToProfileUrl.Text = "头像地址";
            this.rdoChangeAtToProfileUrl.UseVisualStyleBackColor = true;
            // 
            // rdoChangeAtToNick
            // 
            this.rdoChangeAtToNick.AutoSize = true;
            this.rdoChangeAtToNick.Location = new System.Drawing.Point(197, 4);
            this.rdoChangeAtToNick.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeAtToNick.Name = "rdoChangeAtToNick";
            this.rdoChangeAtToNick.Size = new System.Drawing.Size(133, 28);
            this.rdoChangeAtToNick.TabIndex = 3;
            this.rdoChangeAtToNick.Text = "群名片/昵称";
            this.rdoChangeAtToNick.UseVisualStyleBackColor = true;
            // 
            // rdoChangeAtToQQId
            // 
            this.rdoChangeAtToQQId.AutoSize = true;
            this.rdoChangeAtToQQId.Location = new System.Drawing.Point(104, 4);
            this.rdoChangeAtToQQId.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeAtToQQId.Name = "rdoChangeAtToQQId";
            this.rdoChangeAtToQQId.Size = new System.Drawing.Size(83, 28);
            this.rdoChangeAtToQQId.TabIndex = 2;
            this.rdoChangeAtToQQId.Text = "QQ号";
            this.rdoChangeAtToQQId.UseVisualStyleBackColor = true;
            // 
            // rdoDontChangeAt
            // 
            this.rdoDontChangeAt.AutoSize = true;
            this.rdoDontChangeAt.Checked = true;
            this.rdoDontChangeAt.Location = new System.Drawing.Point(5, 4);
            this.rdoDontChangeAt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoDontChangeAt.Name = "rdoDontChangeAt";
            this.rdoDontChangeAt.Size = new System.Drawing.Size(89, 28);
            this.rdoDontChangeAt.TabIndex = 0;
            this.rdoDontChangeAt.TabStop = true;
            this.rdoDontChangeAt.Text = "不处理";
            this.rdoDontChangeAt.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.rdoChangeMeToProfileUrl);
            this.panel2.Controls.Add(this.rdoChangeMeToNick);
            this.panel2.Controls.Add(this.rdoChangeMeToQQId);
            this.panel2.Controls.Add(this.rdoDontChangeMe);
            this.panel2.Location = new System.Drawing.Point(179, 237);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(526, 38);
            this.panel2.TabIndex = 69;
            // 
            // rdoChangeMeToProfileUrl
            // 
            this.rdoChangeMeToProfileUrl.AutoSize = true;
            this.rdoChangeMeToProfileUrl.Location = new System.Drawing.Point(340, 5);
            this.rdoChangeMeToProfileUrl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeMeToProfileUrl.Name = "rdoChangeMeToProfileUrl";
            this.rdoChangeMeToProfileUrl.Size = new System.Drawing.Size(107, 28);
            this.rdoChangeMeToProfileUrl.TabIndex = 4;
            this.rdoChangeMeToProfileUrl.Text = "头像地址";
            this.rdoChangeMeToProfileUrl.UseVisualStyleBackColor = true;
            // 
            // rdoChangeMeToNick
            // 
            this.rdoChangeMeToNick.AutoSize = true;
            this.rdoChangeMeToNick.Location = new System.Drawing.Point(197, 5);
            this.rdoChangeMeToNick.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeMeToNick.Name = "rdoChangeMeToNick";
            this.rdoChangeMeToNick.Size = new System.Drawing.Size(133, 28);
            this.rdoChangeMeToNick.TabIndex = 3;
            this.rdoChangeMeToNick.Text = "群名片/昵称";
            this.rdoChangeMeToNick.UseVisualStyleBackColor = true;
            // 
            // rdoChangeMeToQQId
            // 
            this.rdoChangeMeToQQId.AutoSize = true;
            this.rdoChangeMeToQQId.Location = new System.Drawing.Point(104, 4);
            this.rdoChangeMeToQQId.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoChangeMeToQQId.Name = "rdoChangeMeToQQId";
            this.rdoChangeMeToQQId.Size = new System.Drawing.Size(83, 28);
            this.rdoChangeMeToQQId.TabIndex = 2;
            this.rdoChangeMeToQQId.Text = "QQ号";
            this.rdoChangeMeToQQId.UseVisualStyleBackColor = true;
            // 
            // rdoDontChangeMe
            // 
            this.rdoDontChangeMe.AutoSize = true;
            this.rdoDontChangeMe.Checked = true;
            this.rdoDontChangeMe.Location = new System.Drawing.Point(5, 4);
            this.rdoDontChangeMe.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rdoDontChangeMe.Name = "rdoDontChangeMe";
            this.rdoDontChangeMe.Size = new System.Drawing.Size(89, 28);
            this.rdoDontChangeMe.TabIndex = 0;
            this.rdoDontChangeMe.TabStop = true;
            this.rdoDontChangeMe.Text = "不转换";
            this.rdoDontChangeMe.UseVisualStyleBackColor = true;
            // 
            // lblChangeTypeInfo
            // 
            this.lblChangeTypeInfo.AutoSize = true;
            this.lblChangeTypeInfo.Location = new System.Drawing.Point(720, 196);
            this.lblChangeTypeInfo.Name = "lblChangeTypeInfo";
            this.lblChangeTypeInfo.Size = new System.Drawing.Size(191, 72);
            this.lblChangeTypeInfo.TabIndex = 70;
            this.lblChangeTypeInfo.Text = "仅限命令不包含正则且\r\n拼在命令之后的字或@\r\n消息, 只拼接在URL后";
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(809, 39);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(108, 28);
            this.chkUseProxy.TabIndex = 71;
            this.chkUseProxy.Text = "使用代理";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            // 
            // FrmCustomHttpApiInvokerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 971);
            this.Controls.Add(this.chkUseProxy);
            this.Controls.Add(this.lblChangeTypeInfo);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblChangeMeTo);
            this.Controls.Add(this.lblChangeAtTo);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txbHelpMessage);
            this.Controls.Add(this.lblHelpMessageTitle);
            this.Controls.Add(this.txbRemark);
            this.Controls.Add(this.lblRemarkTitle);
            this.Controls.Add(this.txbContentRaw);
            this.Controls.Add(this.pnlContentType);
            this.Controls.Add(this.dgvContentFormData);
            this.Controls.Add(this.btnInvokeTest);
            this.Controls.Add(this.pnlParse);
            this.Controls.Add(this.pnlSubText);
            this.Controls.Add(this.pnlSendMode);
            this.Controls.Add(this.pnlParseMode);
            this.Controls.Add(this.lblSendMode);
            this.Controls.Add(this.lblParseMode);
            this.Controls.Add(this.txbCmd);
            this.Controls.Add(this.lblCmdTitle);
            this.Controls.Add(this.dgvHeader);
            this.Controls.Add(this.cboEncoding);
            this.Controls.Add(this.lblEncodeTitle);
            this.Controls.Add(this.cboMediaType);
            this.Controls.Add(this.lblMediaTypeTitle);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.cboHttpMethod);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblHttpMethodTitle);
            this.Controls.Add(this.txbUrl);
            this.Controls.Add(this.lblUrlTitle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCustomHttpApiInvokerEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑请求";
            this.pnlContentType.ResumeLayout(false);
            this.pnlContentType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContentFormData)).EndInit();
            this.pnlParse.ResumeLayout(false);
            this.pnlParse.PerformLayout();
            this.pnlSubText.ResumeLayout(false);
            this.pnlSubText.PerformLayout();
            this.pnlSendMode.ResumeLayout(false);
            this.pnlSendMode.PerformLayout();
            this.pnlParseMode.ResumeLayout(false);
            this.pnlParseMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txbContentRaw;
        private Panel pnlContentType;
        private RadioButton rdoContentRaw;
        private RadioButton rdoContentFormData;
        private DataGridView dgvContentFormData;
        private Button btnInvokeTest;
        private Panel pnlParse;
        private Label lblParseExpression;
        private Panel pnlSubText;
        private Label lblSubText;
        private CheckBox chkSubTextWithSuffix;
        private CheckBox chkSubTextWithPrefix;
        private Label lblSubTextFrom;
        private TextBox txbSubTextFrom;
        private TextBox txbSubTextTo;
        private Label lblSubTextTo;
        private Panel pnlSendMode;
        private RadioButton rdoSendVoiceByUrl;
        private RadioButton rdoSendVoiceByBase64;
        private RadioButton rdoSendVoiceStream;
        private RadioButton rdoSendImageStream;
        private RadioButton rdoSendImageByBase64;
        private RadioButton rdoSendImageByUrl;
        private RadioButton rdoSendText;
        private Panel pnlParseMode;
        private RadioButton rdoParseJavaScript;
        private RadioButton rdoParseStream;
        private RadioButton rdoParseJson;
        private RadioButton rdoParseXml;
        private RadioButton rdoParseXPath;
        private RadioButton rdoParseText;
        private Label lblSendMode;
        private Label lblParseMode;
        private TextBox txbCmd;
        private Label lblCmdTitle;
        private DataGridView dgvHeader;
        private ComboBox cboEncoding;
        private Label lblEncodeTitle;
        private ComboBox cboMediaType;
        private Label lblMediaTypeTitle;
        private Label lblContent;
        private ComboBox cboHttpMethod;
        private Label lblHeader;
        private Label lblHttpMethodTitle;
        private TextBox txbUrl;
        private Label lblUrlTitle;
        private TextBox txbParseExpression;
        private TextBox txbRemark;
        private Label lblRemarkTitle;
        private TextBox txbHelpMessage;
        private Label lblHelpMessageTitle;
        private Label lblInfo;
        private Label lblChangeAtTo;
        private Label lblChangeMeTo;
        private Panel panel1;
        private RadioButton rdoChangeAtToProfileUrl;
        private RadioButton rdoChangeAtToNick;
        private RadioButton rdoChangeAtToQQId;
        private RadioButton rdoDontChangeAt;
        private Panel panel2;
        private RadioButton rdoChangeMeToProfileUrl;
        private RadioButton rdoChangeMeToNick;
        private RadioButton rdoChangeMeToQQId;
        private RadioButton rdoDontChangeMe;
        private Label lblChangeTypeInfo;
        private DataGridViewTextBoxColumn dgvHeaderKeyColumn;
        private DataGridViewTextBoxColumn dgvHeaderValueColumn;
        private DataGridViewTextBoxColumn dgvContentKeyColumn;
        private DataGridViewTextBoxColumn dgvContentValueColumn;
        private CheckBox chkUseProxy;
    }
}