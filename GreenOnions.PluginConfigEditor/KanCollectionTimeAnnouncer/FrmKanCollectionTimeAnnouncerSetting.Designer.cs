namespace GreenOnions.PluginConfigEditor.KanCollectionTimeAnnouncer
{
    partial class FrmKanCollectionTimeAnnouncerSetting
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
            rdoDesignateGroup = new RadioButton();
            rdoAllGroup = new RadioButton();
            txbDesignatedGroups = new TextBox();
            pnlGroup = new Panel();
            pnlKanGirl = new Panel();
            btnRefreshKanGirlList = new Button();
            cboDesignatedKanGirl = new ComboBox();
            rdoRandomKanGirl = new RadioButton();
            rdoDesignateKanGirl = new RadioButton();
            pnlTime = new Panel();
            pnlDesignatedTime = new FlowLayoutPanel();
            lblGroup = new Label();
            lblKanGirl = new Label();
            lblTime = new Label();
            chkSendChineseText = new CheckBox();
            chkSendJapaneseText = new CheckBox();
            chkSelectAll = new CheckBox();
            lblGoCqhttpInfoFront = new Label();
            lblGoCqhttpInfoBack = new Label();
            lnkFFMPEG = new LinkLabel();
            btnTest = new Button();
            pnlGroup.SuspendLayout();
            pnlKanGirl.SuspendLayout();
            pnlTime.SuspendLayout();
            SuspendLayout();
            // 
            // rdoDesignateGroup
            // 
            rdoDesignateGroup.AutoSize = true;
            rdoDesignateGroup.Checked = true;
            rdoDesignateGroup.Location = new Point(5, 4);
            rdoDesignateGroup.Margin = new Padding(5, 4, 5, 4);
            rdoDesignateGroup.Name = "rdoDesignateGroup";
            rdoDesignateGroup.Size = new Size(107, 28);
            rdoDesignateGroup.TabIndex = 0;
            rdoDesignateGroup.TabStop = true;
            rdoDesignateGroup.Text = "指定群组";
            rdoDesignateGroup.UseVisualStyleBackColor = true;
            rdoDesignateGroup.CheckedChanged += rdo_CheckedChanged;
            // 
            // rdoAllGroup
            // 
            rdoAllGroup.AutoSize = true;
            rdoAllGroup.Location = new Point(332, 4);
            rdoAllGroup.Margin = new Padding(5, 4, 5, 4);
            rdoAllGroup.Name = "rdoAllGroup";
            rdoAllGroup.Size = new Size(107, 28);
            rdoAllGroup.TabIndex = 1;
            rdoAllGroup.Text = "所有群组";
            rdoAllGroup.UseVisualStyleBackColor = true;
            rdoAllGroup.CheckedChanged += rdo_CheckedChanged;
            // 
            // txbDesignatedGroups
            // 
            txbDesignatedGroups.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbDesignatedGroups.Enabled = false;
            txbDesignatedGroups.Location = new Point(5, 42);
            txbDesignatedGroups.Margin = new Padding(5, 4, 5, 4);
            txbDesignatedGroups.Multiline = true;
            txbDesignatedGroups.Name = "txbDesignatedGroups";
            txbDesignatedGroups.ScrollBars = ScrollBars.Vertical;
            txbDesignatedGroups.Size = new Size(598, 106);
            txbDesignatedGroups.TabIndex = 2;
            // 
            // pnlGroup
            // 
            pnlGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlGroup.BorderStyle = BorderStyle.FixedSingle;
            pnlGroup.Controls.Add(txbDesignatedGroups);
            pnlGroup.Controls.Add(rdoDesignateGroup);
            pnlGroup.Controls.Add(rdoAllGroup);
            pnlGroup.Location = new Point(19, 41);
            pnlGroup.Margin = new Padding(5, 4, 5, 4);
            pnlGroup.Name = "pnlGroup";
            pnlGroup.Size = new Size(612, 153);
            pnlGroup.TabIndex = 3;
            // 
            // pnlKanGirl
            // 
            pnlKanGirl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlKanGirl.BorderStyle = BorderStyle.FixedSingle;
            pnlKanGirl.Controls.Add(btnRefreshKanGirlList);
            pnlKanGirl.Controls.Add(cboDesignatedKanGirl);
            pnlKanGirl.Controls.Add(rdoRandomKanGirl);
            pnlKanGirl.Controls.Add(rdoDesignateKanGirl);
            pnlKanGirl.Location = new Point(19, 227);
            pnlKanGirl.Margin = new Padding(5, 4, 5, 4);
            pnlKanGirl.Name = "pnlKanGirl";
            pnlKanGirl.Size = new Size(612, 81);
            pnlKanGirl.TabIndex = 4;
            // 
            // btnRefreshKanGirlList
            // 
            btnRefreshKanGirlList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshKanGirlList.Location = new Point(487, 42);
            btnRefreshKanGirlList.Margin = new Padding(5, 4, 5, 4);
            btnRefreshKanGirlList.Name = "btnRefreshKanGirlList";
            btnRefreshKanGirlList.Size = new Size(118, 32);
            btnRefreshKanGirlList.TabIndex = 3;
            btnRefreshKanGirlList.Text = "刷新";
            btnRefreshKanGirlList.UseVisualStyleBackColor = true;
            btnRefreshKanGirlList.Click += btnRefreshKanGirlList_Click;
            // 
            // cboDesignatedKanGirl
            // 
            cboDesignatedKanGirl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboDesignatedKanGirl.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDesignatedKanGirl.Enabled = false;
            cboDesignatedKanGirl.FormattingEnabled = true;
            cboDesignatedKanGirl.Items.AddRange(new object[] { "获取中..." });
            cboDesignatedKanGirl.Location = new Point(5, 42);
            cboDesignatedKanGirl.Margin = new Padding(5, 4, 5, 4);
            cboDesignatedKanGirl.Name = "cboDesignatedKanGirl";
            cboDesignatedKanGirl.Size = new Size(471, 32);
            cboDesignatedKanGirl.TabIndex = 2;
            // 
            // rdoRandomKanGirl
            // 
            rdoRandomKanGirl.AutoSize = true;
            rdoRandomKanGirl.Location = new Point(332, 4);
            rdoRandomKanGirl.Margin = new Padding(5, 4, 5, 4);
            rdoRandomKanGirl.Name = "rdoRandomKanGirl";
            rdoRandomKanGirl.Size = new Size(107, 28);
            rdoRandomKanGirl.TabIndex = 1;
            rdoRandomKanGirl.Text = "随机舰娘";
            rdoRandomKanGirl.UseVisualStyleBackColor = true;
            rdoRandomKanGirl.CheckedChanged += rdo_CheckedChanged;
            // 
            // rdoDesignateKanGirl
            // 
            rdoDesignateKanGirl.AutoSize = true;
            rdoDesignateKanGirl.Checked = true;
            rdoDesignateKanGirl.Location = new Point(5, 4);
            rdoDesignateKanGirl.Margin = new Padding(5, 4, 5, 4);
            rdoDesignateKanGirl.Name = "rdoDesignateKanGirl";
            rdoDesignateKanGirl.Size = new Size(107, 28);
            rdoDesignateKanGirl.TabIndex = 0;
            rdoDesignateKanGirl.TabStop = true;
            rdoDesignateKanGirl.Text = "指定舰娘";
            rdoDesignateKanGirl.UseVisualStyleBackColor = true;
            rdoDesignateKanGirl.CheckedChanged += rdo_CheckedChanged;
            // 
            // pnlTime
            // 
            pnlTime.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTime.BorderStyle = BorderStyle.FixedSingle;
            pnlTime.Controls.Add(pnlDesignatedTime);
            pnlTime.Location = new Point(19, 342);
            pnlTime.Margin = new Padding(5, 4, 5, 4);
            pnlTime.Name = "pnlTime";
            pnlTime.Size = new Size(612, 157);
            pnlTime.TabIndex = 5;
            // 
            // pnlDesignatedTime
            // 
            pnlDesignatedTime.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDesignatedTime.Location = new Point(5, 4);
            pnlDesignatedTime.Margin = new Padding(5, 4, 5, 4);
            pnlDesignatedTime.Name = "pnlDesignatedTime";
            pnlDesignatedTime.Size = new Size(600, 145);
            pnlDesignatedTime.TabIndex = 2;
            // 
            // lblGroup
            // 
            lblGroup.AutoSize = true;
            lblGroup.Location = new Point(19, 13);
            lblGroup.Margin = new Padding(5, 0, 5, 0);
            lblGroup.Name = "lblGroup";
            lblGroup.Size = new Size(122, 24);
            lblGroup.TabIndex = 3;
            lblGroup.Text = "发送报时群组:";
            // 
            // lblKanGirl
            // 
            lblKanGirl.AutoSize = true;
            lblKanGirl.Location = new Point(19, 199);
            lblKanGirl.Margin = new Padding(5, 0, 5, 0);
            lblKanGirl.Name = "lblKanGirl";
            lblKanGirl.Size = new Size(122, 24);
            lblKanGirl.TabIndex = 6;
            lblKanGirl.Text = "报时语音舰娘:";
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new Point(19, 313);
            lblTime.Margin = new Padding(5, 0, 5, 0);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(230, 24);
            lblTime.TabIndex = 7;
            lblTime.Text = "在勾选的时间发送报时消息:";
            // 
            // chkSendChineseText
            // 
            chkSendChineseText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkSendChineseText.AutoSize = true;
            chkSendChineseText.Location = new Point(19, 510);
            chkSendChineseText.Margin = new Padding(5, 4, 5, 4);
            chkSendChineseText.Name = "chkSendChineseText";
            chkSendChineseText.Size = new Size(144, 28);
            chkSendChineseText.TabIndex = 8;
            chkSendChineseText.Text = "发送中文文字";
            chkSendChineseText.UseVisualStyleBackColor = true;
            // 
            // chkSendJapaneseText
            // 
            chkSendJapaneseText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkSendJapaneseText.AutoSize = true;
            chkSendJapaneseText.Location = new Point(184, 510);
            chkSendJapaneseText.Margin = new Padding(5, 4, 5, 4);
            chkSendJapaneseText.Name = "chkSendJapaneseText";
            chkSendJapaneseText.Size = new Size(144, 28);
            chkSendJapaneseText.TabIndex = 9;
            chkSendJapaneseText.Text = "发送日文文字";
            chkSendJapaneseText.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            chkSelectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkSelectAll.AutoSize = true;
            chkSelectAll.Location = new Point(560, 313);
            chkSelectAll.Margin = new Padding(5, 4, 5, 4);
            chkSelectAll.Name = "chkSelectAll";
            chkSelectAll.Size = new Size(72, 28);
            chkSelectAll.TabIndex = 10;
            chkSelectAll.Text = "全选";
            chkSelectAll.UseVisualStyleBackColor = true;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            // 
            // lblGoCqhttpInfoFront
            // 
            lblGoCqhttpInfoFront.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblGoCqhttpInfoFront.AutoSize = true;
            lblGoCqhttpInfoFront.Location = new Point(19, 542);
            lblGoCqhttpInfoFront.Margin = new Padding(5, 0, 5, 0);
            lblGoCqhttpInfoFront.Name = "lblGoCqhttpInfoFront";
            lblGoCqhttpInfoFront.Size = new Size(360, 24);
            lblGoCqhttpInfoFront.TabIndex = 11;
            lblGoCqhttpInfoFront.Text = "如果您使用的是go-cqhttp平台, 需要先安装";
            // 
            // lblGoCqhttpInfoBack
            // 
            lblGoCqhttpInfoBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblGoCqhttpInfoBack.AutoSize = true;
            lblGoCqhttpInfoBack.Location = new Point(464, 542);
            lblGoCqhttpInfoBack.Margin = new Padding(5, 0, 5, 0);
            lblGoCqhttpInfoBack.Name = "lblGoCqhttpInfoBack";
            lblGoCqhttpInfoBack.Size = new Size(172, 24);
            lblGoCqhttpInfoBack.TabIndex = 12;
            lblGoCqhttpInfoBack.Text = "才能支持发送语音。";
            // 
            // lnkFFMPEG
            // 
            lnkFFMPEG.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lnkFFMPEG.AutoSize = true;
            lnkFFMPEG.LinkColor = Color.Blue;
            lnkFFMPEG.Location = new Point(391, 542);
            lnkFFMPEG.Margin = new Padding(5, 0, 5, 0);
            lnkFFMPEG.Name = "lnkFFMPEG";
            lnkFFMPEG.Size = new Size(73, 24);
            lnkFFMPEG.TabIndex = 13;
            lnkFFMPEG.TabStop = true;
            lnkFFMPEG.Text = "ffmpeg";
            lnkFFMPEG.VisitedLinkColor = Color.Blue;
            lnkFFMPEG.LinkClicked += lnkFFMPEG_LinkClicked;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(519, 505);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(112, 34);
            btnTest.TabIndex = 14;
            btnTest.Text = "测试下载";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // FrmKanCollectionTimeAnnouncerSetting
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 579);
            Controls.Add(btnTest);
            Controls.Add(lnkFFMPEG);
            Controls.Add(lblGoCqhttpInfoFront);
            Controls.Add(chkSendJapaneseText);
            Controls.Add(chkSendChineseText);
            Controls.Add(lblTime);
            Controls.Add(lblKanGirl);
            Controls.Add(lblGroup);
            Controls.Add(pnlTime);
            Controls.Add(pnlKanGirl);
            Controls.Add(pnlGroup);
            Controls.Add(chkSelectAll);
            Controls.Add(lblGoCqhttpInfoBack);
            Margin = new Padding(5, 4, 5, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmKanCollectionTimeAnnouncerSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "舰C报时-设置";
            pnlGroup.ResumeLayout(false);
            pnlGroup.PerformLayout();
            pnlKanGirl.ResumeLayout(false);
            pnlKanGirl.PerformLayout();
            pnlTime.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton rdoDesignateGroup;
        private RadioButton rdoAllGroup;
        private TextBox txbDesignatedGroups;
        private Panel pnlGroup;
        private Label lblGroup;
        private Panel pnlKanGirl;
        private ComboBox cboDesignatedKanGirl;
        private RadioButton rdoRandomKanGirl;
        private RadioButton rdoDesignateKanGirl;
        private Panel pnlTime;
        private FlowLayoutPanel pnlDesignatedTime;
        private Label lblKanGirl;
        private Label lblTime;
        private CheckBox chkSendChineseText;
        private CheckBox chkSendJapaneseText;
        private CheckBox chkSelectAll;
        private Button btnRefreshKanGirlList;
        private Label lblGoCqhttpInfoFront;
        private Label lblGoCqhttpInfoBack;
        private LinkLabel lnkFFMPEG;
        private Button btnTest;
    }
}