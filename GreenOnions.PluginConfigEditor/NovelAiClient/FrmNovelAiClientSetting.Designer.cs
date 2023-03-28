namespace GreenOnions.PluginConfigEditor.NovelAiClient
{
    partial class FrmNovelAiClientSetting
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
            this.txbCmd = new System.Windows.Forms.TextBox();
            this.txbURL = new System.Windows.Forms.TextBox();
            this.txbfn_Index = new System.Windows.Forms.TextBox();
            this.txbPromptIndex = new System.Windows.Forms.TextBox();
            this.txbUndesiredIndex = new System.Windows.Forms.TextBox();
            this.lblCmd = new System.Windows.Forms.Label();
            this.lblConnectFrontEnd = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblfn_Index = new System.Windows.Forms.Label();
            this.lblPromptIndex = new System.Windows.Forms.Label();
            this.lblUndesiredIndex = new System.Windows.Forms.Label();
            this.lblStartDrawMessage = new System.Windows.Forms.Label();
            this.lblDrawEndMessage = new System.Windows.Forms.Label();
            this.lblDrawErrorMessage = new System.Windows.Forms.Label();
            this.lblRevokeSecond = new System.Windows.Forms.Label();
            this.lblDefaultPrompt = new System.Windows.Forms.Label();
            this.lblDefaultUndesired = new System.Windows.Forms.Label();
            this.cboConnectFrontEnd = new System.Windows.Forms.ComboBox();
            this.txbStartDrawMessage = new System.Windows.Forms.TextBox();
            this.txbDrawEndMessage = new System.Windows.Forms.TextBox();
            this.txbDrawErrorMessage = new System.Windows.Forms.TextBox();
            this.txbRevokeSecond = new System.Windows.Forms.TextBox();
            this.txbDefaultPrompt = new System.Windows.Forms.TextBox();
            this.txbDefaultUndesired = new System.Windows.Forms.TextBox();
            this.btnTestInvoke = new System.Windows.Forms.Button();
            this.txbTestPrompt = new System.Windows.Forms.TextBox();
            this.lblTestPrompt = new System.Windows.Forms.Label();
            this.picTest = new System.Windows.Forms.PictureBox();
            this.btnEditParams = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picTest)).BeginInit();
            this.SuspendLayout();
            // 
            // txbCmd
            // 
            this.txbCmd.Location = new System.Drawing.Point(241, 30);
            this.txbCmd.Name = "txbCmd";
            this.txbCmd.Size = new System.Drawing.Size(426, 30);
            this.txbCmd.TabIndex = 0;
            // 
            // txbURL
            // 
            this.txbURL.Location = new System.Drawing.Point(241, 104);
            this.txbURL.Name = "txbURL";
            this.txbURL.Size = new System.Drawing.Size(426, 30);
            this.txbURL.TabIndex = 1;
            // 
            // txbfn_Index
            // 
            this.txbfn_Index.Location = new System.Drawing.Point(241, 140);
            this.txbfn_Index.Name = "txbfn_Index";
            this.txbfn_Index.Size = new System.Drawing.Size(426, 30);
            this.txbfn_Index.TabIndex = 2;
            // 
            // txbPromptIndex
            // 
            this.txbPromptIndex.Location = new System.Drawing.Point(241, 176);
            this.txbPromptIndex.Name = "txbPromptIndex";
            this.txbPromptIndex.Size = new System.Drawing.Size(426, 30);
            this.txbPromptIndex.TabIndex = 3;
            // 
            // txbUndesiredIndex
            // 
            this.txbUndesiredIndex.Location = new System.Drawing.Point(241, 212);
            this.txbUndesiredIndex.Name = "txbUndesiredIndex";
            this.txbUndesiredIndex.Size = new System.Drawing.Size(426, 30);
            this.txbUndesiredIndex.TabIndex = 4;
            // 
            // lblCmd
            // 
            this.lblCmd.AutoSize = true;
            this.lblCmd.Location = new System.Drawing.Point(68, 33);
            this.lblCmd.Name = "lblCmd";
            this.lblCmd.Size = new System.Drawing.Size(64, 24);
            this.lblCmd.TabIndex = 5;
            this.lblCmd.Text = "命令：";
            // 
            // lblConnectFrontEnd
            // 
            this.lblConnectFrontEnd.AutoSize = true;
            this.lblConnectFrontEnd.Location = new System.Drawing.Point(68, 69);
            this.lblConnectFrontEnd.Name = "lblConnectFrontEnd";
            this.lblConnectFrontEnd.Size = new System.Drawing.Size(100, 24);
            this.lblConnectFrontEnd.TabIndex = 6;
            this.lblConnectFrontEnd.Text = "连接前端：";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(68, 107);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(100, 24);
            this.lblURL.TabIndex = 7;
            this.lblURL.Text = "前端地址：";
            // 
            // lblfn_Index
            // 
            this.lblfn_Index.AutoSize = true;
            this.lblfn_Index.Location = new System.Drawing.Point(68, 143);
            this.lblfn_Index.Name = "lblfn_Index";
            this.lblfn_Index.Size = new System.Drawing.Size(100, 24);
            this.lblfn_Index.TabIndex = 8;
            this.lblfn_Index.Text = "fn_Index：";
            // 
            // lblPromptIndex
            // 
            this.lblPromptIndex.AutoSize = true;
            this.lblPromptIndex.Location = new System.Drawing.Point(68, 179);
            this.lblPromptIndex.Name = "lblPromptIndex";
            this.lblPromptIndex.Size = new System.Drawing.Size(118, 24);
            this.lblPromptIndex.TabIndex = 9;
            this.lblPromptIndex.Text = "提示词索引：";
            // 
            // lblUndesiredIndex
            // 
            this.lblUndesiredIndex.AutoSize = true;
            this.lblUndesiredIndex.Location = new System.Drawing.Point(68, 215);
            this.lblUndesiredIndex.Name = "lblUndesiredIndex";
            this.lblUndesiredIndex.Size = new System.Drawing.Size(118, 24);
            this.lblUndesiredIndex.TabIndex = 10;
            this.lblUndesiredIndex.Text = "屏蔽词索引：";
            // 
            // lblStartDrawMessage
            // 
            this.lblStartDrawMessage.AutoSize = true;
            this.lblStartDrawMessage.Location = new System.Drawing.Point(68, 251);
            this.lblStartDrawMessage.Name = "lblStartDrawMessage";
            this.lblStartDrawMessage.Size = new System.Drawing.Size(136, 24);
            this.lblStartDrawMessage.TabIndex = 11;
            this.lblStartDrawMessage.Text = "开始绘制回复：";
            // 
            // lblDrawEndMessage
            // 
            this.lblDrawEndMessage.AutoSize = true;
            this.lblDrawEndMessage.Location = new System.Drawing.Point(68, 287);
            this.lblDrawEndMessage.Name = "lblDrawEndMessage";
            this.lblDrawEndMessage.Size = new System.Drawing.Size(136, 24);
            this.lblDrawEndMessage.TabIndex = 12;
            this.lblDrawEndMessage.Text = "绘制完成回复：";
            // 
            // lblDrawErrorMessage
            // 
            this.lblDrawErrorMessage.AutoSize = true;
            this.lblDrawErrorMessage.Location = new System.Drawing.Point(68, 323);
            this.lblDrawErrorMessage.Name = "lblDrawErrorMessage";
            this.lblDrawErrorMessage.Size = new System.Drawing.Size(136, 24);
            this.lblDrawErrorMessage.TabIndex = 13;
            this.lblDrawErrorMessage.Text = "绘制错误回复：";
            // 
            // lblRevokeSecond
            // 
            this.lblRevokeSecond.AutoSize = true;
            this.lblRevokeSecond.Location = new System.Drawing.Point(68, 359);
            this.lblRevokeSecond.Name = "lblRevokeSecond";
            this.lblRevokeSecond.Size = new System.Drawing.Size(100, 24);
            this.lblRevokeSecond.TabIndex = 14;
            this.lblRevokeSecond.Text = "撤回时间：";
            // 
            // lblDefaultPrompt
            // 
            this.lblDefaultPrompt.AutoSize = true;
            this.lblDefaultPrompt.Location = new System.Drawing.Point(68, 395);
            this.lblDefaultPrompt.Name = "lblDefaultPrompt";
            this.lblDefaultPrompt.Size = new System.Drawing.Size(118, 24);
            this.lblDefaultPrompt.TabIndex = 15;
            this.lblDefaultPrompt.Text = "默认提示词：";
            // 
            // lblDefaultUndesired
            // 
            this.lblDefaultUndesired.AutoSize = true;
            this.lblDefaultUndesired.Location = new System.Drawing.Point(68, 521);
            this.lblDefaultUndesired.Name = "lblDefaultUndesired";
            this.lblDefaultUndesired.Size = new System.Drawing.Size(118, 24);
            this.lblDefaultUndesired.TabIndex = 16;
            this.lblDefaultUndesired.Text = "默认屏蔽词：";
            // 
            // cboConnectFrontEnd
            // 
            this.cboConnectFrontEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConnectFrontEnd.FormattingEnabled = true;
            this.cboConnectFrontEnd.Items.AddRange(new object[] {
            "WebUI",
            "Naifu"});
            this.cboConnectFrontEnd.Location = new System.Drawing.Point(241, 66);
            this.cboConnectFrontEnd.Name = "cboConnectFrontEnd";
            this.cboConnectFrontEnd.Size = new System.Drawing.Size(426, 32);
            this.cboConnectFrontEnd.TabIndex = 17;
            // 
            // txbStartDrawMessage
            // 
            this.txbStartDrawMessage.Location = new System.Drawing.Point(241, 248);
            this.txbStartDrawMessage.Name = "txbStartDrawMessage";
            this.txbStartDrawMessage.Size = new System.Drawing.Size(426, 30);
            this.txbStartDrawMessage.TabIndex = 18;
            // 
            // txbDrawEndMessage
            // 
            this.txbDrawEndMessage.Location = new System.Drawing.Point(241, 284);
            this.txbDrawEndMessage.Name = "txbDrawEndMessage";
            this.txbDrawEndMessage.Size = new System.Drawing.Size(426, 30);
            this.txbDrawEndMessage.TabIndex = 19;
            // 
            // txbDrawErrorMessage
            // 
            this.txbDrawErrorMessage.Location = new System.Drawing.Point(241, 320);
            this.txbDrawErrorMessage.Name = "txbDrawErrorMessage";
            this.txbDrawErrorMessage.Size = new System.Drawing.Size(426, 30);
            this.txbDrawErrorMessage.TabIndex = 20;
            // 
            // txbRevokeSecond
            // 
            this.txbRevokeSecond.Location = new System.Drawing.Point(241, 356);
            this.txbRevokeSecond.Name = "txbRevokeSecond";
            this.txbRevokeSecond.Size = new System.Drawing.Size(426, 30);
            this.txbRevokeSecond.TabIndex = 21;
            // 
            // txbDefaultPrompt
            // 
            this.txbDefaultPrompt.Location = new System.Drawing.Point(241, 392);
            this.txbDefaultPrompt.Multiline = true;
            this.txbDefaultPrompt.Name = "txbDefaultPrompt";
            this.txbDefaultPrompt.Size = new System.Drawing.Size(426, 120);
            this.txbDefaultPrompt.TabIndex = 22;
            // 
            // txbDefaultUndesired
            // 
            this.txbDefaultUndesired.Location = new System.Drawing.Point(241, 518);
            this.txbDefaultUndesired.Multiline = true;
            this.txbDefaultUndesired.Name = "txbDefaultUndesired";
            this.txbDefaultUndesired.Size = new System.Drawing.Size(426, 120);
            this.txbDefaultUndesired.TabIndex = 23;
            // 
            // btnTestInvoke
            // 
            this.btnTestInvoke.Location = new System.Drawing.Point(567, 1003);
            this.btnTestInvoke.Name = "btnTestInvoke";
            this.btnTestInvoke.Size = new System.Drawing.Size(112, 34);
            this.btnTestInvoke.TabIndex = 24;
            this.btnTestInvoke.Text = "测试调用";
            this.btnTestInvoke.UseVisualStyleBackColor = true;
            this.btnTestInvoke.Click += new System.EventHandler(this.btnTestInvoke_Click);
            // 
            // txbTestPrompt
            // 
            this.txbTestPrompt.Location = new System.Drawing.Point(241, 1010);
            this.txbTestPrompt.Name = "txbTestPrompt";
            this.txbTestPrompt.Size = new System.Drawing.Size(320, 30);
            this.txbTestPrompt.TabIndex = 25;
            // 
            // lblTestPrompt
            // 
            this.lblTestPrompt.AutoSize = true;
            this.lblTestPrompt.Location = new System.Drawing.Point(68, 1008);
            this.lblTestPrompt.Name = "lblTestPrompt";
            this.lblTestPrompt.Size = new System.Drawing.Size(118, 24);
            this.lblTestPrompt.TabIndex = 26;
            this.lblTestPrompt.Text = "测试提示词：";
            // 
            // picTest
            // 
            this.picTest.Location = new System.Drawing.Point(241, 684);
            this.picTest.Name = "picTest";
            this.picTest.Size = new System.Drawing.Size(320, 320);
            this.picTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTest.TabIndex = 27;
            this.picTest.TabStop = false;
            // 
            // btnEditParams
            // 
            this.btnEditParams.Location = new System.Drawing.Point(241, 644);
            this.btnEditParams.Name = "btnEditParams";
            this.btnEditParams.Size = new System.Drawing.Size(426, 34);
            this.btnEditParams.TabIndex = 28;
            this.btnEditParams.Text = "编辑参数";
            this.btnEditParams.UseVisualStyleBackColor = true;
            this.btnEditParams.Click += new System.EventHandler(this.btnEditParams_Click);
            // 
            // FrmNovelAiClientSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 1080);
            this.Controls.Add(this.btnEditParams);
            this.Controls.Add(this.btnTestInvoke);
            this.Controls.Add(this.picTest);
            this.Controls.Add(this.lblTestPrompt);
            this.Controls.Add(this.txbTestPrompt);
            this.Controls.Add(this.txbDefaultUndesired);
            this.Controls.Add(this.txbDefaultPrompt);
            this.Controls.Add(this.txbRevokeSecond);
            this.Controls.Add(this.txbDrawErrorMessage);
            this.Controls.Add(this.txbDrawEndMessage);
            this.Controls.Add(this.txbStartDrawMessage);
            this.Controls.Add(this.cboConnectFrontEnd);
            this.Controls.Add(this.lblDefaultUndesired);
            this.Controls.Add(this.lblDefaultPrompt);
            this.Controls.Add(this.lblRevokeSecond);
            this.Controls.Add(this.lblDrawErrorMessage);
            this.Controls.Add(this.lblDrawEndMessage);
            this.Controls.Add(this.lblStartDrawMessage);
            this.Controls.Add(this.lblUndesiredIndex);
            this.Controls.Add(this.lblPromptIndex);
            this.Controls.Add(this.lblfn_Index);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lblConnectFrontEnd);
            this.Controls.Add(this.lblCmd);
            this.Controls.Add(this.txbUndesiredIndex);
            this.Controls.Add(this.txbPromptIndex);
            this.Controls.Add(this.txbfn_Index);
            this.Controls.Add(this.txbURL);
            this.Controls.Add(this.txbCmd);
            this.Name = "FrmNovelAiClientSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NovelAi连接器设置";
            ((System.ComponentModel.ISupportInitialize)(this.picTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txbCmd;
        private TextBox txbURL;
        private TextBox txbfn_Index;
        private TextBox txbPromptIndex;
        private TextBox txbUndesiredIndex;
        private Label lblCmd;
        private Label lblConnectFrontEnd;
        private Label lblURL;
        private Label lblfn_Index;
        private Label lblPromptIndex;
        private Label lblUndesiredIndex;
        private Label lblStartDrawMessage;
        private Label lblDrawEndMessage;
        private Label lblDrawErrorMessage;
        private Label lblRevokeSecond;
        private Label lblDefaultPrompt;
        private Label lblDefaultUndesired;
        private ComboBox cboConnectFrontEnd;
        private TextBox txbStartDrawMessage;
        private TextBox txbDrawEndMessage;
        private TextBox txbDrawErrorMessage;
        private TextBox txbRevokeSecond;
        private TextBox txbDefaultPrompt;
        private TextBox txbDefaultUndesired;
        private Button btnTestInvoke;
        private TextBox txbTestPrompt;
        private Label lblTestPrompt;
        private PictureBox picTest;
        private Button btnEditParams;
    }
}