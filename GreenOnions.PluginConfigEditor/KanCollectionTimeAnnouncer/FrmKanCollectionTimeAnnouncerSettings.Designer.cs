namespace GreenOnions.PluginConfigEditor.KanCollectionTimeAnnouncer
{
    partial class FrmKanCollectionTimeAnnouncerSettings
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
            this.rdoDesignateGroup = new System.Windows.Forms.RadioButton();
            this.rdoAllGroup = new System.Windows.Forms.RadioButton();
            this.txbDesignatedGroups = new System.Windows.Forms.TextBox();
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.pnlKanGirl = new System.Windows.Forms.Panel();
            this.btnRefreshKanGirlList = new System.Windows.Forms.Button();
            this.cboDesignatedKanGirl = new System.Windows.Forms.ComboBox();
            this.rdoRandomKanGirl = new System.Windows.Forms.RadioButton();
            this.rdoDesignateKanGirl = new System.Windows.Forms.RadioButton();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.pnlDesignatedTime = new System.Windows.Forms.FlowLayoutPanel();
            this.lblGroup = new System.Windows.Forms.Label();
            this.lblKanGirl = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.chkSendChineseText = new System.Windows.Forms.CheckBox();
            this.chkSendJapaneseText = new System.Windows.Forms.CheckBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.lblGoCqhttpInfoFront = new System.Windows.Forms.Label();
            this.lblGoCqhttpInfoBack = new System.Windows.Forms.Label();
            this.lnkFFMPEG = new System.Windows.Forms.LinkLabel();
            this.pnlGroup.SuspendLayout();
            this.pnlKanGirl.SuspendLayout();
            this.pnlTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoDesignateGroup
            // 
            this.rdoDesignateGroup.AutoSize = true;
            this.rdoDesignateGroup.Checked = true;
            this.rdoDesignateGroup.Location = new System.Drawing.Point(3, 3);
            this.rdoDesignateGroup.Name = "rdoDesignateGroup";
            this.rdoDesignateGroup.Size = new System.Drawing.Size(74, 21);
            this.rdoDesignateGroup.TabIndex = 0;
            this.rdoDesignateGroup.TabStop = true;
            this.rdoDesignateGroup.Text = "指定群组";
            this.rdoDesignateGroup.UseVisualStyleBackColor = true;
            this.rdoDesignateGroup.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // rdoAllGroup
            // 
            this.rdoAllGroup.AutoSize = true;
            this.rdoAllGroup.Location = new System.Drawing.Point(211, 3);
            this.rdoAllGroup.Name = "rdoAllGroup";
            this.rdoAllGroup.Size = new System.Drawing.Size(74, 21);
            this.rdoAllGroup.TabIndex = 1;
            this.rdoAllGroup.Text = "所有群组";
            this.rdoAllGroup.UseVisualStyleBackColor = true;
            this.rdoAllGroup.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // txbDesignatedGroups
            // 
            this.txbDesignatedGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDesignatedGroups.Enabled = false;
            this.txbDesignatedGroups.Location = new System.Drawing.Point(3, 30);
            this.txbDesignatedGroups.Multiline = true;
            this.txbDesignatedGroups.Name = "txbDesignatedGroups";
            this.txbDesignatedGroups.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDesignatedGroups.Size = new System.Drawing.Size(382, 76);
            this.txbDesignatedGroups.TabIndex = 2;
            // 
            // pnlGroup
            // 
            this.pnlGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGroup.Controls.Add(this.txbDesignatedGroups);
            this.pnlGroup.Controls.Add(this.rdoDesignateGroup);
            this.pnlGroup.Controls.Add(this.rdoAllGroup);
            this.pnlGroup.Location = new System.Drawing.Point(12, 29);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(390, 109);
            this.pnlGroup.TabIndex = 3;
            // 
            // pnlKanGirl
            // 
            this.pnlKanGirl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlKanGirl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKanGirl.Controls.Add(this.btnRefreshKanGirlList);
            this.pnlKanGirl.Controls.Add(this.cboDesignatedKanGirl);
            this.pnlKanGirl.Controls.Add(this.rdoRandomKanGirl);
            this.pnlKanGirl.Controls.Add(this.rdoDesignateKanGirl);
            this.pnlKanGirl.Location = new System.Drawing.Point(12, 161);
            this.pnlKanGirl.Name = "pnlKanGirl";
            this.pnlKanGirl.Size = new System.Drawing.Size(390, 58);
            this.pnlKanGirl.TabIndex = 4;
            // 
            // btnRefreshKanGirlList
            // 
            this.btnRefreshKanGirlList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshKanGirlList.Location = new System.Drawing.Point(310, 30);
            this.btnRefreshKanGirlList.Name = "btnRefreshKanGirlList";
            this.btnRefreshKanGirlList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshKanGirlList.TabIndex = 3;
            this.btnRefreshKanGirlList.Text = "刷新";
            this.btnRefreshKanGirlList.UseVisualStyleBackColor = true;
            this.btnRefreshKanGirlList.Click += new System.EventHandler(this.btnRefreshKanGirlList_Click);
            // 
            // cboDesignatedKanGirl
            // 
            this.cboDesignatedKanGirl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDesignatedKanGirl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDesignatedKanGirl.Enabled = false;
            this.cboDesignatedKanGirl.FormattingEnabled = true;
            this.cboDesignatedKanGirl.Items.AddRange(new object[] {
            "获取中..."});
            this.cboDesignatedKanGirl.Location = new System.Drawing.Point(3, 30);
            this.cboDesignatedKanGirl.Name = "cboDesignatedKanGirl";
            this.cboDesignatedKanGirl.Size = new System.Drawing.Size(301, 25);
            this.cboDesignatedKanGirl.TabIndex = 2;
            // 
            // rdoRandomKanGirl
            // 
            this.rdoRandomKanGirl.AutoSize = true;
            this.rdoRandomKanGirl.Location = new System.Drawing.Point(211, 3);
            this.rdoRandomKanGirl.Name = "rdoRandomKanGirl";
            this.rdoRandomKanGirl.Size = new System.Drawing.Size(74, 21);
            this.rdoRandomKanGirl.TabIndex = 1;
            this.rdoRandomKanGirl.Text = "随机舰娘";
            this.rdoRandomKanGirl.UseVisualStyleBackColor = true;
            this.rdoRandomKanGirl.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // rdoDesignateKanGirl
            // 
            this.rdoDesignateKanGirl.AutoSize = true;
            this.rdoDesignateKanGirl.Checked = true;
            this.rdoDesignateKanGirl.Location = new System.Drawing.Point(3, 3);
            this.rdoDesignateKanGirl.Name = "rdoDesignateKanGirl";
            this.rdoDesignateKanGirl.Size = new System.Drawing.Size(74, 21);
            this.rdoDesignateKanGirl.TabIndex = 0;
            this.rdoDesignateKanGirl.TabStop = true;
            this.rdoDesignateKanGirl.Text = "指定舰娘";
            this.rdoDesignateKanGirl.UseVisualStyleBackColor = true;
            this.rdoDesignateKanGirl.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // pnlTime
            // 
            this.pnlTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTime.Controls.Add(this.pnlDesignatedTime);
            this.pnlTime.Location = new System.Drawing.Point(12, 242);
            this.pnlTime.Name = "pnlTime";
            this.pnlTime.Size = new System.Drawing.Size(390, 112);
            this.pnlTime.TabIndex = 5;
            // 
            // pnlDesignatedTime
            // 
            this.pnlDesignatedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDesignatedTime.Location = new System.Drawing.Point(3, 3);
            this.pnlDesignatedTime.Name = "pnlDesignatedTime";
            this.pnlDesignatedTime.Size = new System.Drawing.Size(382, 103);
            this.pnlDesignatedTime.TabIndex = 2;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(12, 9);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(83, 17);
            this.lblGroup.TabIndex = 3;
            this.lblGroup.Text = "发送报时群组:";
            // 
            // lblKanGirl
            // 
            this.lblKanGirl.AutoSize = true;
            this.lblKanGirl.Location = new System.Drawing.Point(12, 141);
            this.lblKanGirl.Name = "lblKanGirl";
            this.lblKanGirl.Size = new System.Drawing.Size(83, 17);
            this.lblKanGirl.TabIndex = 6;
            this.lblKanGirl.Text = "报时语音舰娘:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 222);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(155, 17);
            this.lblTime.TabIndex = 7;
            this.lblTime.Text = "在勾选的时间发送报时消息:";
            // 
            // chkSendChineseText
            // 
            this.chkSendChineseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSendChineseText.AutoSize = true;
            this.chkSendChineseText.Location = new System.Drawing.Point(12, 360);
            this.chkSendChineseText.Name = "chkSendChineseText";
            this.chkSendChineseText.Size = new System.Drawing.Size(99, 21);
            this.chkSendChineseText.TabIndex = 8;
            this.chkSendChineseText.Text = "发送中文文字";
            this.chkSendChineseText.UseVisualStyleBackColor = true;
            // 
            // chkSendJapaneseText
            // 
            this.chkSendJapaneseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSendJapaneseText.AutoSize = true;
            this.chkSendJapaneseText.Location = new System.Drawing.Point(117, 360);
            this.chkSendJapaneseText.Name = "chkSendJapaneseText";
            this.chkSendJapaneseText.Size = new System.Drawing.Size(99, 21);
            this.chkSendJapaneseText.TabIndex = 9;
            this.chkSendJapaneseText.Text = "发送日文文字";
            this.chkSendJapaneseText.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(351, 222);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(51, 21);
            this.chkSelectAll.TabIndex = 10;
            this.chkSelectAll.Text = "全选";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // lblGoCqhttpInfoFront
            // 
            this.lblGoCqhttpInfoFront.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGoCqhttpInfoFront.AutoSize = true;
            this.lblGoCqhttpInfoFront.Location = new System.Drawing.Point(12, 384);
            this.lblGoCqhttpInfoFront.Name = "lblGoCqhttpInfoFront";
            this.lblGoCqhttpInfoFront.Size = new System.Drawing.Size(241, 17);
            this.lblGoCqhttpInfoFront.TabIndex = 11;
            this.lblGoCqhttpInfoFront.Text = "如果您使用的是go-cqhttp平台, 需要先安装";
            // 
            // lblGoCqhttpInfoBack
            // 
            this.lblGoCqhttpInfoBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGoCqhttpInfoBack.AutoSize = true;
            this.lblGoCqhttpInfoBack.Location = new System.Drawing.Point(295, 384);
            this.lblGoCqhttpInfoBack.Name = "lblGoCqhttpInfoBack";
            this.lblGoCqhttpInfoBack.Size = new System.Drawing.Size(116, 17);
            this.lblGoCqhttpInfoBack.TabIndex = 12;
            this.lblGoCqhttpInfoBack.Text = "才能支持发送语音。";
            // 
            // lnkFFMPEG
            // 
            this.lnkFFMPEG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkFFMPEG.AutoSize = true;
            this.lnkFFMPEG.LinkColor = System.Drawing.Color.Blue;
            this.lnkFFMPEG.Location = new System.Drawing.Point(249, 384);
            this.lnkFFMPEG.Name = "lnkFFMPEG";
            this.lnkFFMPEG.Size = new System.Drawing.Size(50, 17);
            this.lnkFFMPEG.TabIndex = 13;
            this.lnkFFMPEG.TabStop = true;
            this.lnkFFMPEG.Text = "ffmpeg";
            this.lnkFFMPEG.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkFFMPEG.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFFMPEG_LinkClicked);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 410);
            this.Controls.Add(this.lnkFFMPEG);
            this.Controls.Add(this.lblGoCqhttpInfoFront);
            this.Controls.Add(this.chkSendJapaneseText);
            this.Controls.Add(this.chkSendChineseText);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblKanGirl);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.pnlTime);
            this.Controls.Add(this.pnlKanGirl);
            this.Controls.Add(this.pnlGroup);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.lblGoCqhttpInfoBack);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "舰C报时-设置";
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.pnlKanGirl.ResumeLayout(false);
            this.pnlKanGirl.PerformLayout();
            this.pnlTime.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}