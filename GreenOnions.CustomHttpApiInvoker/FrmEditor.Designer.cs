namespace GreenOnions.CustomHttpApiInvoker
{
    partial class FrmEditor
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
            this.pnlContentType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContentFormData)).BeginInit();
            this.pnlParse.SuspendLayout();
            this.pnlSubText.SuspendLayout();
            this.pnlSendMode.SuspendLayout();
            this.pnlParseMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // txbContentRaw
            // 
            this.txbContentRaw.Location = new System.Drawing.Point(15, 327);
            this.txbContentRaw.Margin = new System.Windows.Forms.Padding(2);
            this.txbContentRaw.Multiline = true;
            this.txbContentRaw.Name = "txbContentRaw";
            this.txbContentRaw.Size = new System.Drawing.Size(573, 118);
            this.txbContentRaw.TabIndex = 60;
            // 
            // pnlContentType
            // 
            this.pnlContentType.AutoSize = true;
            this.pnlContentType.Controls.Add(this.rdoContentRaw);
            this.pnlContentType.Controls.Add(this.rdoContentFormData);
            this.pnlContentType.Location = new System.Drawing.Point(116, 301);
            this.pnlContentType.Margin = new System.Windows.Forms.Padding(2);
            this.pnlContentType.Name = "pnlContentType";
            this.pnlContentType.Size = new System.Drawing.Size(140, 25);
            this.pnlContentType.TabIndex = 59;
            // 
            // rdoContentRaw
            // 
            this.rdoContentRaw.AutoSize = true;
            this.rdoContentRaw.Checked = true;
            this.rdoContentRaw.Location = new System.Drawing.Point(2, 2);
            this.rdoContentRaw.Margin = new System.Windows.Forms.Padding(2);
            this.rdoContentRaw.Name = "rdoContentRaw";
            this.rdoContentRaw.Size = new System.Drawing.Size(47, 21);
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
            this.rdoContentFormData.Location = new System.Drawing.Point(53, 2);
            this.rdoContentFormData.Margin = new System.Windows.Forms.Padding(2);
            this.rdoContentFormData.Name = "rdoContentFormData";
            this.rdoContentFormData.Size = new System.Drawing.Size(85, 21);
            this.rdoContentFormData.TabIndex = 1;
            this.rdoContentFormData.Text = "form-data";
            this.rdoContentFormData.UseVisualStyleBackColor = true;
            this.rdoContentFormData.AppearanceChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            this.rdoContentFormData.CheckedChanged += new System.EventHandler(this.rdoContentType_CheckedChanged);
            // 
            // dgvContentFormData
            // 
            this.dgvContentFormData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContentFormData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContentFormData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvContentKeyColumn,
            this.dgvContentValueColumn});
            this.dgvContentFormData.Location = new System.Drawing.Point(15, 327);
            this.dgvContentFormData.Margin = new System.Windows.Forms.Padding(2);
            this.dgvContentFormData.Name = "dgvContentFormData";
            this.dgvContentFormData.RowHeadersVisible = false;
            this.dgvContentFormData.RowHeadersWidth = 62;
            this.dgvContentFormData.RowTemplate.Height = 32;
            this.dgvContentFormData.Size = new System.Drawing.Size(573, 117);
            this.dgvContentFormData.TabIndex = 58;
            this.dgvContentFormData.Visible = false;
            // 
            // dgvContentKeyColumn
            // 
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
            this.btnInvokeTest.Location = new System.Drawing.Point(517, 52);
            this.btnInvokeTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnInvokeTest.Name = "btnInvokeTest";
            this.btnInvokeTest.Size = new System.Drawing.Size(72, 23);
            this.btnInvokeTest.TabIndex = 57;
            this.btnInvokeTest.Text = "测试调用";
            this.btnInvokeTest.UseVisualStyleBackColor = true;
            this.btnInvokeTest.Click += new System.EventHandler(this.btnInvokeTest_Click);
            // 
            // pnlParse
            // 
            this.pnlParse.Controls.Add(this.txbParseExpression);
            this.pnlParse.Controls.Add(this.lblParseExpression);
            this.pnlParse.Enabled = false;
            this.pnlParse.Location = new System.Drawing.Point(204, 480);
            this.pnlParse.Margin = new System.Windows.Forms.Padding(2);
            this.pnlParse.Name = "pnlParse";
            this.pnlParse.Size = new System.Drawing.Size(384, 106);
            this.pnlParse.TabIndex = 56;
            // 
            // txbParseExpression
            // 
            this.txbParseExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbParseExpression.Location = new System.Drawing.Point(0, 17);
            this.txbParseExpression.Margin = new System.Windows.Forms.Padding(2);
            this.txbParseExpression.Multiline = true;
            this.txbParseExpression.Name = "txbParseExpression";
            this.txbParseExpression.Size = new System.Drawing.Size(384, 89);
            this.txbParseExpression.TabIndex = 19;
            // 
            // lblParseExpression
            // 
            this.lblParseExpression.AutoSize = true;
            this.lblParseExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblParseExpression.Location = new System.Drawing.Point(0, 0);
            this.lblParseExpression.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblParseExpression.Name = "lblParseExpression";
            this.lblParseExpression.Size = new System.Drawing.Size(408, 17);
            this.lblParseExpression.TabIndex = 18;
            this.lblParseExpression.Text = "解析表达式：（特殊表达式<random>含尖括号，随机取数组中一个元素）";
            // 
            // pnlSubText
            // 
            this.pnlSubText.Controls.Add(this.lblSubText);
            this.pnlSubText.Controls.Add(this.chkSubTextWithSuffix);
            this.pnlSubText.Controls.Add(this.chkSubTextWithPrefix);
            this.pnlSubText.Controls.Add(this.lblSubTextFrom);
            this.pnlSubText.Controls.Add(this.txbSubTextFrom);
            this.pnlSubText.Controls.Add(this.txbSubTextTo);
            this.pnlSubText.Controls.Add(this.lblSubTextTo);
            this.pnlSubText.Location = new System.Drawing.Point(15, 478);
            this.pnlSubText.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSubText.Name = "pnlSubText";
            this.pnlSubText.Size = new System.Drawing.Size(185, 106);
            this.pnlSubText.TabIndex = 55;
            // 
            // lblSubText
            // 
            this.lblSubText.AutoSize = true;
            this.lblSubText.Location = new System.Drawing.Point(0, 86);
            this.lblSubText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubText.Name = "lblSubText";
            this.lblSubText.Size = new System.Drawing.Size(32, 17);
            this.lblSubText.TabIndex = 30;
            this.lblSubText.Text = "之间";
            // 
            // chkSubTextWithSuffix
            // 
            this.chkSubTextWithSuffix.AutoSize = true;
            this.chkSubTextWithSuffix.Location = new System.Drawing.Point(109, 86);
            this.chkSubTextWithSuffix.Margin = new System.Windows.Forms.Padding(2);
            this.chkSubTextWithSuffix.Name = "chkSubTextWithSuffix";
            this.chkSubTextWithSuffix.Size = new System.Drawing.Size(75, 21);
            this.chkSubTextWithSuffix.TabIndex = 29;
            this.chkSubTextWithSuffix.Text = "包含后缀";
            this.chkSubTextWithSuffix.UseVisualStyleBackColor = true;
            // 
            // chkSubTextWithPrefix
            // 
            this.chkSubTextWithPrefix.AutoSize = true;
            this.chkSubTextWithPrefix.Location = new System.Drawing.Point(36, 86);
            this.chkSubTextWithPrefix.Margin = new System.Windows.Forms.Padding(2);
            this.chkSubTextWithPrefix.Name = "chkSubTextWithPrefix";
            this.chkSubTextWithPrefix.Size = new System.Drawing.Size(75, 21);
            this.chkSubTextWithPrefix.TabIndex = 28;
            this.chkSubTextWithPrefix.Text = "包含前缀";
            this.chkSubTextWithPrefix.UseVisualStyleBackColor = true;
            // 
            // lblSubTextFrom
            // 
            this.lblSubTextFrom.AutoSize = true;
            this.lblSubTextFrom.Location = new System.Drawing.Point(0, 0);
            this.lblSubTextFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubTextFrom.Name = "lblSubTextFrom";
            this.lblSubTextFrom.Size = new System.Drawing.Size(32, 17);
            this.lblSubTextFrom.TabIndex = 24;
            this.lblSubTextFrom.Text = "从：";
            // 
            // txbSubTextFrom
            // 
            this.txbSubTextFrom.Location = new System.Drawing.Point(0, 19);
            this.txbSubTextFrom.Margin = new System.Windows.Forms.Padding(2);
            this.txbSubTextFrom.Name = "txbSubTextFrom";
            this.txbSubTextFrom.Size = new System.Drawing.Size(185, 23);
            this.txbSubTextFrom.TabIndex = 23;
            // 
            // txbSubTextTo
            // 
            this.txbSubTextTo.Location = new System.Drawing.Point(0, 62);
            this.txbSubTextTo.Margin = new System.Windows.Forms.Padding(2);
            this.txbSubTextTo.Name = "txbSubTextTo";
            this.txbSubTextTo.Size = new System.Drawing.Size(185, 23);
            this.txbSubTextTo.TabIndex = 25;
            // 
            // lblSubTextTo
            // 
            this.lblSubTextTo.AutoSize = true;
            this.lblSubTextTo.Location = new System.Drawing.Point(0, 42);
            this.lblSubTextTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubTextTo.Name = "lblSubTextTo";
            this.lblSubTextTo.Size = new System.Drawing.Size(32, 17);
            this.lblSubTextTo.TabIndex = 26;
            this.lblSubTextTo.Text = "到：";
            // 
            // pnlSendMode
            // 
            this.pnlSendMode.AutoSize = true;
            this.pnlSendMode.Controls.Add(this.rdoSendImageStream);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceStream);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceByUrl);
            this.pnlSendMode.Controls.Add(this.rdoSendVoiceByBase64);
            this.pnlSendMode.Controls.Add(this.rdoSendImageByBase64);
            this.pnlSendMode.Controls.Add(this.rdoSendImageByUrl);
            this.pnlSendMode.Controls.Add(this.rdoSendText);
            this.pnlSendMode.Location = new System.Drawing.Point(94, 589);
            this.pnlSendMode.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSendMode.Name = "pnlSendMode";
            this.pnlSendMode.Size = new System.Drawing.Size(495, 25);
            this.pnlSendMode.TabIndex = 54;
            // 
            // rdoSendImageStream
            // 
            this.rdoSendImageStream.AutoSize = true;
            this.rdoSendImageStream.Enabled = false;
            this.rdoSendImageStream.Location = new System.Drawing.Point(370, 2);
            this.rdoSendImageStream.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendImageStream.Name = "rdoSendImageStream";
            this.rdoSendImageStream.Size = new System.Drawing.Size(62, 21);
            this.rdoSendImageStream.TabIndex = 5;
            this.rdoSendImageStream.TabStop = true;
            this.rdoSendImageStream.Text = "图片流";
            this.rdoSendImageStream.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceStream
            // 
            this.rdoSendVoiceStream.AutoSize = true;
            this.rdoSendVoiceStream.Enabled = false;
            this.rdoSendVoiceStream.Location = new System.Drawing.Point(431, 2);
            this.rdoSendVoiceStream.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendVoiceStream.Name = "rdoSendVoiceStream";
            this.rdoSendVoiceStream.Size = new System.Drawing.Size(62, 21);
            this.rdoSendVoiceStream.TabIndex = 6;
            this.rdoSendVoiceStream.TabStop = true;
            this.rdoSendVoiceStream.Text = "音频流";
            this.rdoSendVoiceStream.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceByUrl
            // 
            this.rdoSendVoiceByUrl.AutoSize = true;
            this.rdoSendVoiceByUrl.Location = new System.Drawing.Point(211, 2);
            this.rdoSendVoiceByUrl.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendVoiceByUrl.Name = "rdoSendVoiceByUrl";
            this.rdoSendVoiceByUrl.Size = new System.Drawing.Size(74, 21);
            this.rdoSendVoiceByUrl.TabIndex = 3;
            this.rdoSendVoiceByUrl.Text = "音频地址";
            this.rdoSendVoiceByUrl.UseVisualStyleBackColor = true;
            // 
            // rdoSendVoiceByBase64
            // 
            this.rdoSendVoiceByBase64.AutoSize = true;
            this.rdoSendVoiceByBase64.Location = new System.Drawing.Point(283, 2);
            this.rdoSendVoiceByBase64.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendVoiceByBase64.Name = "rdoSendVoiceByBase64";
            this.rdoSendVoiceByBase64.Size = new System.Drawing.Size(92, 21);
            this.rdoSendVoiceByBase64.TabIndex = 4;
            this.rdoSendVoiceByBase64.Text = "音频Base64";
            this.rdoSendVoiceByBase64.UseVisualStyleBackColor = true;
            // 
            // rdoSendImageByBase64
            // 
            this.rdoSendImageByBase64.AutoSize = true;
            this.rdoSendImageByBase64.Location = new System.Drawing.Point(123, 2);
            this.rdoSendImageByBase64.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendImageByBase64.Name = "rdoSendImageByBase64";
            this.rdoSendImageByBase64.Size = new System.Drawing.Size(92, 21);
            this.rdoSendImageByBase64.TabIndex = 2;
            this.rdoSendImageByBase64.Text = "图片Base64";
            this.rdoSendImageByBase64.UseVisualStyleBackColor = true;
            // 
            // rdoSendImageByUrl
            // 
            this.rdoSendImageByUrl.AutoSize = true;
            this.rdoSendImageByUrl.Location = new System.Drawing.Point(51, 2);
            this.rdoSendImageByUrl.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendImageByUrl.Name = "rdoSendImageByUrl";
            this.rdoSendImageByUrl.Size = new System.Drawing.Size(74, 21);
            this.rdoSendImageByUrl.TabIndex = 1;
            this.rdoSendImageByUrl.Text = "图片地址";
            this.rdoSendImageByUrl.UseVisualStyleBackColor = true;
            // 
            // rdoSendText
            // 
            this.rdoSendText.AutoSize = true;
            this.rdoSendText.Checked = true;
            this.rdoSendText.Location = new System.Drawing.Point(2, 2);
            this.rdoSendText.Margin = new System.Windows.Forms.Padding(2);
            this.rdoSendText.Name = "rdoSendText";
            this.rdoSendText.Size = new System.Drawing.Size(50, 21);
            this.rdoSendText.TabIndex = 0;
            this.rdoSendText.TabStop = true;
            this.rdoSendText.Text = "文本";
            this.rdoSendText.UseVisualStyleBackColor = true;
            // 
            // pnlParseMode
            // 
            this.pnlParseMode.AutoSize = true;
            this.pnlParseMode.Controls.Add(this.rdoParseJavaScript);
            this.pnlParseMode.Controls.Add(this.rdoParseStream);
            this.pnlParseMode.Controls.Add(this.rdoParseText);
            this.pnlParseMode.Controls.Add(this.rdoParseJson);
            this.pnlParseMode.Controls.Add(this.rdoParseXml);
            this.pnlParseMode.Controls.Add(this.rdoParseXPath);
            this.pnlParseMode.Location = new System.Drawing.Point(87, 450);
            this.pnlParseMode.Margin = new System.Windows.Forms.Padding(2);
            this.pnlParseMode.Name = "pnlParseMode";
            this.pnlParseMode.Size = new System.Drawing.Size(501, 26);
            this.pnlParseMode.TabIndex = 53;
            // 
            // rdoParseJavaScript
            // 
            this.rdoParseJavaScript.AutoSize = true;
            this.rdoParseJavaScript.Enabled = false;
            this.rdoParseJavaScript.Location = new System.Drawing.Point(228, 3);
            this.rdoParseJavaScript.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseJavaScript.Name = "rdoParseJavaScript";
            this.rdoParseJavaScript.Size = new System.Drawing.Size(84, 21);
            this.rdoParseJavaScript.TabIndex = 24;
            this.rdoParseJavaScript.Text = "JavaScript";
            this.rdoParseJavaScript.UseVisualStyleBackColor = true;
            this.rdoParseJavaScript.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseStream
            // 
            this.rdoParseStream.AutoSize = true;
            this.rdoParseStream.Location = new System.Drawing.Point(316, 3);
            this.rdoParseStream.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseStream.Name = "rdoParseStream";
            this.rdoParseStream.Size = new System.Drawing.Size(38, 21);
            this.rdoParseStream.TabIndex = 23;
            this.rdoParseStream.Text = "流";
            this.rdoParseStream.UseVisualStyleBackColor = true;
            this.rdoParseStream.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseText
            // 
            this.rdoParseText.AutoSize = true;
            this.rdoParseText.Checked = true;
            this.rdoParseText.Location = new System.Drawing.Point(3, 3);
            this.rdoParseText.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseText.Name = "rdoParseText";
            this.rdoParseText.Size = new System.Drawing.Size(50, 21);
            this.rdoParseText.TabIndex = 14;
            this.rdoParseText.TabStop = true;
            this.rdoParseText.Text = "文本";
            this.rdoParseText.UseVisualStyleBackColor = true;
            this.rdoParseText.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseJson
            // 
            this.rdoParseJson.AutoSize = true;
            this.rdoParseJson.Location = new System.Drawing.Point(57, 3);
            this.rdoParseJson.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseJson.Name = "rdoParseJson";
            this.rdoParseJson.Size = new System.Drawing.Size(52, 21);
            this.rdoParseJson.TabIndex = 15;
            this.rdoParseJson.Text = "Json";
            this.rdoParseJson.UseVisualStyleBackColor = true;
            this.rdoParseJson.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseXml
            // 
            this.rdoParseXml.AutoSize = true;
            this.rdoParseXml.Enabled = false;
            this.rdoParseXml.Location = new System.Drawing.Point(113, 3);
            this.rdoParseXml.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseXml.Name = "rdoParseXml";
            this.rdoParseXml.Size = new System.Drawing.Size(48, 21);
            this.rdoParseXml.TabIndex = 19;
            this.rdoParseXml.Text = "Xml";
            this.rdoParseXml.UseVisualStyleBackColor = true;
            this.rdoParseXml.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // rdoParseXPath
            // 
            this.rdoParseXPath.AutoSize = true;
            this.rdoParseXPath.Enabled = false;
            this.rdoParseXPath.Location = new System.Drawing.Point(165, 3);
            this.rdoParseXPath.Margin = new System.Windows.Forms.Padding(2);
            this.rdoParseXPath.Name = "rdoParseXPath";
            this.rdoParseXPath.Size = new System.Drawing.Size(59, 21);
            this.rdoParseXPath.TabIndex = 20;
            this.rdoParseXPath.Text = "XPath";
            this.rdoParseXPath.UseVisualStyleBackColor = true;
            this.rdoParseXPath.CheckedChanged += new System.EventHandler(this.rdoContentParseMode_CheckedChanged);
            // 
            // lblSendMode
            // 
            this.lblSendMode.AutoSize = true;
            this.lblSendMode.Location = new System.Drawing.Point(15, 593);
            this.lblSendMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSendMode.Name = "lblSendMode";
            this.lblSendMode.Size = new System.Drawing.Size(80, 17);
            this.lblSendMode.TabIndex = 52;
            this.lblSendMode.Text = "解析后视为：";
            // 
            // lblParseMode
            // 
            this.lblParseMode.AutoSize = true;
            this.lblParseMode.Location = new System.Drawing.Point(15, 455);
            this.lblParseMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblParseMode.Name = "lblParseMode";
            this.lblParseMode.Size = new System.Drawing.Size(68, 17);
            this.lblParseMode.TabIndex = 51;
            this.lblParseMode.Text = "解析方式：";
            // 
            // txbCmd
            // 
            this.txbCmd.Location = new System.Drawing.Point(82, 52);
            this.txbCmd.Margin = new System.Windows.Forms.Padding(2);
            this.txbCmd.Name = "txbCmd";
            this.txbCmd.Size = new System.Drawing.Size(430, 23);
            this.txbCmd.TabIndex = 50;
            // 
            // lblCmdTitle
            // 
            this.lblCmdTitle.AutoSize = true;
            this.lblCmdTitle.Location = new System.Drawing.Point(15, 54);
            this.lblCmdTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmdTitle.Name = "lblCmdTitle";
            this.lblCmdTitle.Size = new System.Drawing.Size(44, 17);
            this.lblCmdTitle.TabIndex = 49;
            this.lblCmdTitle.Text = "命令：";
            // 
            // dgvHeader
            // 
            this.dgvHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvHeaderKeyColumn,
            this.dgvHeaderValueColumn});
            this.dgvHeader.Location = new System.Drawing.Point(15, 174);
            this.dgvHeader.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHeader.Name = "dgvHeader";
            this.dgvHeader.RowHeadersVisible = false;
            this.dgvHeader.RowHeadersWidth = 62;
            this.dgvHeader.RowTemplate.Height = 32;
            this.dgvHeader.Size = new System.Drawing.Size(573, 124);
            this.dgvHeader.TabIndex = 48;
            // 
            // dgvHeaderKeyColumn
            // 
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
            this.cboEncoding.Location = new System.Drawing.Point(276, 131);
            this.cboEncoding.Margin = new System.Windows.Forms.Padding(2);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Size = new System.Drawing.Size(117, 25);
            this.cboEncoding.TabIndex = 47;
            // 
            // lblEncodeTitle
            // 
            this.lblEncodeTitle.AutoSize = true;
            this.lblEncodeTitle.Location = new System.Drawing.Point(208, 134);
            this.lblEncodeTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEncodeTitle.Name = "lblEncodeTitle";
            this.lblEncodeTitle.Size = new System.Drawing.Size(68, 17);
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
            this.cboMediaType.Location = new System.Drawing.Point(471, 131);
            this.cboMediaType.Margin = new System.Windows.Forms.Padding(2);
            this.cboMediaType.Name = "cboMediaType";
            this.cboMediaType.Size = new System.Drawing.Size(117, 25);
            this.cboMediaType.TabIndex = 45;
            // 
            // lblMediaTypeTitle
            // 
            this.lblMediaTypeTitle.AutoSize = true;
            this.lblMediaTypeTitle.Location = new System.Drawing.Point(402, 134);
            this.lblMediaTypeTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMediaTypeTitle.Name = "lblMediaTypeTitle";
            this.lblMediaTypeTitle.Size = new System.Drawing.Size(68, 17);
            this.lblMediaTypeTitle.TabIndex = 44;
            this.lblMediaTypeTitle.Text = "媒体类型：";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(15, 305);
            this.lblContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(103, 17);
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
            this.cboHttpMethod.Location = new System.Drawing.Point(81, 131);
            this.cboHttpMethod.Margin = new System.Windows.Forms.Padding(2);
            this.cboHttpMethod.Name = "cboHttpMethod";
            this.cboHttpMethod.Size = new System.Drawing.Size(117, 25);
            this.cboHttpMethod.TabIndex = 42;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(15, 156);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(63, 17);
            this.lblHeader.TabIndex = 41;
            this.lblHeader.Text = "Header：";
            // 
            // lblHttpMethodTitle
            // 
            this.lblHttpMethodTitle.AutoSize = true;
            this.lblHttpMethodTitle.Location = new System.Drawing.Point(15, 134);
            this.lblHttpMethodTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHttpMethodTitle.Name = "lblHttpMethodTitle";
            this.lblHttpMethodTitle.Size = new System.Drawing.Size(68, 17);
            this.lblHttpMethodTitle.TabIndex = 40;
            this.lblHttpMethodTitle.Text = "请求方法：";
            // 
            // txbUrl
            // 
            this.txbUrl.Location = new System.Drawing.Point(82, 26);
            this.txbUrl.Margin = new System.Windows.Forms.Padding(2);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(506, 23);
            this.txbUrl.TabIndex = 39;
            // 
            // lblUrlTitle
            // 
            this.lblUrlTitle.AutoSize = true;
            this.lblUrlTitle.Location = new System.Drawing.Point(15, 29);
            this.lblUrlTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUrlTitle.Name = "lblUrlTitle";
            this.lblUrlTitle.Size = new System.Drawing.Size(44, 17);
            this.lblUrlTitle.TabIndex = 38;
            this.lblUrlTitle.Text = "地址：";
            // 
            // txbRemark
            // 
            this.txbRemark.Location = new System.Drawing.Point(82, 79);
            this.txbRemark.Margin = new System.Windows.Forms.Padding(2);
            this.txbRemark.Name = "txbRemark";
            this.txbRemark.Size = new System.Drawing.Size(506, 23);
            this.txbRemark.TabIndex = 62;
            // 
            // lblRemarkTitle
            // 
            this.lblRemarkTitle.AutoSize = true;
            this.lblRemarkTitle.Location = new System.Drawing.Point(15, 83);
            this.lblRemarkTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRemarkTitle.Name = "lblRemarkTitle";
            this.lblRemarkTitle.Size = new System.Drawing.Size(44, 17);
            this.lblRemarkTitle.TabIndex = 61;
            this.lblRemarkTitle.Text = "备注：";
            // 
            // txbHelpMessage
            // 
            this.txbHelpMessage.Location = new System.Drawing.Point(82, 106);
            this.txbHelpMessage.Margin = new System.Windows.Forms.Padding(2);
            this.txbHelpMessage.Name = "txbHelpMessage";
            this.txbHelpMessage.Size = new System.Drawing.Size(506, 23);
            this.txbHelpMessage.TabIndex = 64;
            // 
            // lblHelpMessageTitle
            // 
            this.lblHelpMessageTitle.AutoSize = true;
            this.lblHelpMessageTitle.Location = new System.Drawing.Point(15, 108);
            this.lblHelpMessageTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHelpMessageTitle.Name = "lblHelpMessageTitle";
            this.lblHelpMessageTitle.Size = new System.Drawing.Size(68, 17);
            this.lblHelpMessageTitle.TabIndex = 63;
            this.lblHelpMessageTitle.Text = "帮助信息：";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(11, 7);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(584, 17);
            this.lblInfo.TabIndex = 65;
            this.lblInfo.Text = "您可以在命令中添加\"(?<参数>)\"正则表达式提取子串，并在地址地址或参数栏中添加<参数>来进行带参调用";
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 621);
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txbContentRaw;
        private Panel pnlContentType;
        private RadioButton rdoContentRaw;
        private RadioButton rdoContentFormData;
        private DataGridView dgvContentFormData;
        private DataGridViewTextBoxColumn dgvContentKeyColumn;
        private DataGridViewTextBoxColumn dgvContentValueColumn;
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
        private DataGridViewTextBoxColumn dgvHeaderKeyColumn;
        private DataGridViewTextBoxColumn dgvHeaderValueColumn;
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
    }
}