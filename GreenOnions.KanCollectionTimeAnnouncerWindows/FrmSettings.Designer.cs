namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    partial class FrmSettings
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
            this.cboDesignatedKanGirl = new System.Windows.Forms.ComboBox();
            this.rdoRandomKanGirl = new System.Windows.Forms.RadioButton();
            this.rdoDesignateKanGirl = new System.Windows.Forms.RadioButton();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.pnlDesignatedTime = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSendChineseText = new System.Windows.Forms.CheckBox();
            this.chkSendJapaneseText = new System.Windows.Forms.CheckBox();
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
            this.pnlKanGirl.Controls.Add(this.cboDesignatedKanGirl);
            this.pnlKanGirl.Controls.Add(this.rdoRandomKanGirl);
            this.pnlKanGirl.Controls.Add(this.rdoDesignateKanGirl);
            this.pnlKanGirl.Location = new System.Drawing.Point(12, 161);
            this.pnlKanGirl.Name = "pnlKanGirl";
            this.pnlKanGirl.Size = new System.Drawing.Size(390, 58);
            this.pnlKanGirl.TabIndex = 4;
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
            this.cboDesignatedKanGirl.Size = new System.Drawing.Size(382, 25);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "发送报时群组:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "报时语音舰娘:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "报时时间过滤:";
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
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 386);
            this.Controls.Add(this.chkSendJapaneseText);
            this.Controls.Add(this.chkSendChineseText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlTime);
            this.Controls.Add(this.pnlKanGirl);
            this.Controls.Add(this.pnlGroup);
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
        private Label label1;
        private Panel pnlKanGirl;
        private ComboBox cboDesignatedKanGirl;
        private RadioButton rdoRandomKanGirl;
        private RadioButton rdoDesignateKanGirl;
        private Panel pnlTime;
        private FlowLayoutPanel pnlDesignatedTime;
        private Label label2;
        private Label label3;
        private CheckBox chkSendChineseText;
        private CheckBox chkSendJapaneseText;
    }
}