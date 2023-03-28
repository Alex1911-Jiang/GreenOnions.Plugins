namespace GreenOnions.PluginConfigEditor.ChatGPTClient
{
    partial class FrmChatGPTClientSetting
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
            this.lblCreateApiKey = new System.Windows.Forms.LinkLabel();
            this.chkSendMessageByReply = new System.Windows.Forms.CheckBox();
            this.lblTimeOutSecond = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.lblTimeOutMessage = new System.Windows.Forms.Label();
            this.lblExitChatMessage = new System.Windows.Forms.Label();
            this.lblChatStartMessage = new System.Windows.Forms.Label();
            this.lblExitCommands = new System.Windows.Forms.Label();
            this.lblStartCommands = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.lnkTemperature = new System.Windows.Forms.LinkLabel();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.chkRequestByPlugin = new System.Windows.Forms.CheckBox();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.txbTimeOutSecond = new System.Windows.Forms.TextBox();
            this.txbErrorMessage = new System.Windows.Forms.TextBox();
            this.txbTimeOutMessage = new System.Windows.Forms.TextBox();
            this.txbExitChatMessage = new System.Windows.Forms.TextBox();
            this.txbChatStartMessage = new System.Windows.Forms.TextBox();
            this.txbExitCommands = new System.Windows.Forms.TextBox();
            this.txbStartCommands = new System.Windows.Forms.TextBox();
            this.txbTemperature = new System.Windows.Forms.TextBox();
            this.txbApiKey = new System.Windows.Forms.TextBox();
            this.cboContext = new System.Windows.Forms.ComboBox();
            this.lblContext = new System.Windows.Forms.Label();
            this.chkClearContextAfterExit = new System.Windows.Forms.CheckBox();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.lnkModel = new System.Windows.Forms.LinkLabel();
            this.lblClearContextCommands = new System.Windows.Forms.Label();
            this.txbClearContextCommands = new System.Windows.Forms.TextBox();
            this.chkRemoveMarkdownExpression = new System.Windows.Forms.CheckBox();
            this.txbCleanContextMessage = new System.Windows.Forms.TextBox();
            this.lblCleanContextMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCreateApiKey
            // 
            this.lblCreateApiKey.ActiveLinkColor = System.Drawing.Color.Red;
            this.lblCreateApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreateApiKey.AutoSize = true;
            this.lblCreateApiKey.LinkColor = System.Drawing.Color.Blue;
            this.lblCreateApiKey.Location = new System.Drawing.Point(697, 53);
            this.lblCreateApiKey.Name = "lblCreateApiKey";
            this.lblCreateApiKey.Size = new System.Drawing.Size(46, 24);
            this.lblCreateApiKey.TabIndex = 45;
            this.lblCreateApiKey.TabStop = true;
            this.lblCreateApiKey.Text = "创建";
            this.lblCreateApiKey.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblCreateApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCreateApiKey_LinkClicked);
            // 
            // chkSendMessageByReply
            // 
            this.chkSendMessageByReply.AutoSize = true;
            this.chkSendMessageByReply.Location = new System.Drawing.Point(80, 228);
            this.chkSendMessageByReply.Name = "chkSendMessageByReply";
            this.chkSendMessageByReply.Size = new System.Drawing.Size(212, 28);
            this.chkSendMessageByReply.TabIndex = 44;
            this.chkSendMessageByReply.Text = "以“回复”方式发送消息";
            this.chkSendMessageByReply.UseVisualStyleBackColor = true;
            // 
            // lblTimeOutSecond
            // 
            this.lblTimeOutSecond.AutoSize = true;
            this.lblTimeOutSecond.Location = new System.Drawing.Point(80, 932);
            this.lblTimeOutSecond.Name = "lblTimeOutSecond";
            this.lblTimeOutSecond.Size = new System.Drawing.Size(190, 24);
            this.lblTimeOutSecond.TabIndex = 43;
            this.lblTimeOutSecond.Text = "聊天超时时间（秒）：";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.Location = new System.Drawing.Point(80, 896);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(136, 24);
            this.lblErrorMessage.TabIndex = 42;
            this.lblErrorMessage.Text = "发生错误回复：";
            // 
            // lblTimeOutMessage
            // 
            this.lblTimeOutMessage.AutoSize = true;
            this.lblTimeOutMessage.Location = new System.Drawing.Point(80, 824);
            this.lblTimeOutMessage.Name = "lblTimeOutMessage";
            this.lblTimeOutMessage.Size = new System.Drawing.Size(172, 24);
            this.lblTimeOutMessage.TabIndex = 41;
            this.lblTimeOutMessage.Text = "聊天超时结束回复：";
            // 
            // lblExitChatMessage
            // 
            this.lblExitChatMessage.AutoSize = true;
            this.lblExitChatMessage.Location = new System.Drawing.Point(80, 788);
            this.lblExitChatMessage.Name = "lblExitChatMessage";
            this.lblExitChatMessage.Size = new System.Drawing.Size(172, 24);
            this.lblExitChatMessage.TabIndex = 40;
            this.lblExitChatMessage.Text = "主动结束聊天回复：";
            // 
            // lblChatStartMessage
            // 
            this.lblChatStartMessage.AutoSize = true;
            this.lblChatStartMessage.Location = new System.Drawing.Point(80, 752);
            this.lblChatStartMessage.Name = "lblChatStartMessage";
            this.lblChatStartMessage.Size = new System.Drawing.Size(136, 24);
            this.lblChatStartMessage.TabIndex = 39;
            this.lblChatStartMessage.Text = "开始聊天回复：";
            // 
            // lblExitCommands
            // 
            this.lblExitCommands.AutoSize = true;
            this.lblExitCommands.Location = new System.Drawing.Point(80, 543);
            this.lblExitCommands.Name = "lblExitCommands";
            this.lblExitCommands.Size = new System.Drawing.Size(244, 24);
            this.lblExitCommands.TabIndex = 38;
            this.lblExitCommands.Text = "结束聊天命令（一行一个）：";
            // 
            // lblStartCommands
            // 
            this.lblStartCommands.AutoSize = true;
            this.lblStartCommands.Location = new System.Drawing.Point(80, 416);
            this.lblStartCommands.Name = "lblStartCommands";
            this.lblStartCommands.Size = new System.Drawing.Size(244, 24);
            this.lblStartCommands.TabIndex = 37;
            this.lblStartCommands.Text = "开始聊天命令（一行一个）：";
            // 
            // lblTemperature
            // 
            this.lblTemperature.AutoSize = true;
            this.lblTemperature.Location = new System.Drawing.Point(80, 127);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(100, 24);
            this.lblTemperature.TabIndex = 36;
            this.lblTemperature.Text = "采样温度：";
            // 
            // lnkTemperature
            // 
            this.lnkTemperature.ActiveLinkColor = System.Drawing.Color.Red;
            this.lnkTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkTemperature.AutoSize = true;
            this.lnkTemperature.LinkColor = System.Drawing.Color.Blue;
            this.lnkTemperature.Location = new System.Drawing.Point(697, 127);
            this.lnkTemperature.Name = "lnkTemperature";
            this.lnkTemperature.Size = new System.Drawing.Size(46, 24);
            this.lnkTemperature.TabIndex = 35;
            this.lnkTemperature.TabStop = true;
            this.lnkTemperature.Text = "说明";
            this.lnkTemperature.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkTemperature.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTemperature_LinkClicked);
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(80, 53);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(96, 24);
            this.lblApiKey.TabIndex = 34;
            this.lblApiKey.Text = "API-Key：";
            // 
            // chkRequestByPlugin
            // 
            this.chkRequestByPlugin.AutoSize = true;
            this.chkRequestByPlugin.Location = new System.Drawing.Point(80, 194);
            this.chkRequestByPlugin.Name = "chkRequestByPlugin";
            this.chkRequestByPlugin.Size = new System.Drawing.Size(508, 28);
            this.chkRequestByPlugin.TabIndex = 33;
            this.chkRequestByPlugin.Text = "使用插件替代基本库完成Http请求（取决于装了什么插件）";
            this.chkRequestByPlugin.UseVisualStyleBackColor = true;
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Location = new System.Drawing.Point(80, 160);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(360, 28);
            this.chkUseProxy.TabIndex = 32;
            this.chkUseProxy.Text = "使用代理（无法在插件替代模式下生效）";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            // 
            // txbTimeOutSecond
            // 
            this.txbTimeOutSecond.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTimeOutSecond.Location = new System.Drawing.Point(348, 929);
            this.txbTimeOutSecond.Name = "txbTimeOutSecond";
            this.txbTimeOutSecond.Size = new System.Drawing.Size(343, 30);
            this.txbTimeOutSecond.TabIndex = 31;
            // 
            // txbErrorMessage
            // 
            this.txbErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbErrorMessage.Location = new System.Drawing.Point(348, 893);
            this.txbErrorMessage.Name = "txbErrorMessage";
            this.txbErrorMessage.Size = new System.Drawing.Size(343, 30);
            this.txbErrorMessage.TabIndex = 30;
            // 
            // txbTimeOutMessage
            // 
            this.txbTimeOutMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTimeOutMessage.Location = new System.Drawing.Point(348, 821);
            this.txbTimeOutMessage.Name = "txbTimeOutMessage";
            this.txbTimeOutMessage.Size = new System.Drawing.Size(343, 30);
            this.txbTimeOutMessage.TabIndex = 29;
            // 
            // txbExitChatMessage
            // 
            this.txbExitChatMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbExitChatMessage.Location = new System.Drawing.Point(348, 785);
            this.txbExitChatMessage.Name = "txbExitChatMessage";
            this.txbExitChatMessage.Size = new System.Drawing.Size(343, 30);
            this.txbExitChatMessage.TabIndex = 28;
            // 
            // txbChatStartMessage
            // 
            this.txbChatStartMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbChatStartMessage.Location = new System.Drawing.Point(348, 749);
            this.txbChatStartMessage.Name = "txbChatStartMessage";
            this.txbChatStartMessage.Size = new System.Drawing.Size(343, 30);
            this.txbChatStartMessage.TabIndex = 27;
            // 
            // txbExitCommands
            // 
            this.txbExitCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbExitCommands.Location = new System.Drawing.Point(348, 495);
            this.txbExitCommands.Multiline = true;
            this.txbExitCommands.Name = "txbExitCommands";
            this.txbExitCommands.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbExitCommands.Size = new System.Drawing.Size(343, 121);
            this.txbExitCommands.TabIndex = 26;
            // 
            // txbStartCommands
            // 
            this.txbStartCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStartCommands.Location = new System.Drawing.Point(348, 368);
            this.txbStartCommands.Multiline = true;
            this.txbStartCommands.Name = "txbStartCommands";
            this.txbStartCommands.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbStartCommands.Size = new System.Drawing.Size(343, 121);
            this.txbStartCommands.TabIndex = 25;
            // 
            // txbTemperature
            // 
            this.txbTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTemperature.Location = new System.Drawing.Point(348, 124);
            this.txbTemperature.Name = "txbTemperature";
            this.txbTemperature.Size = new System.Drawing.Size(343, 30);
            this.txbTemperature.TabIndex = 24;
            // 
            // txbApiKey
            // 
            this.txbApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbApiKey.Location = new System.Drawing.Point(348, 50);
            this.txbApiKey.Name = "txbApiKey";
            this.txbApiKey.Size = new System.Drawing.Size(343, 30);
            this.txbApiKey.TabIndex = 23;
            // 
            // cboContext
            // 
            this.cboContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContext.FormattingEnabled = true;
            this.cboContext.Items.AddRange(new object[] {
            "尽量保持上下文（如果没超过令牌上限）",
            "仅保持上一条上下文",
            "不保持上下文"});
            this.cboContext.Location = new System.Drawing.Point(348, 330);
            this.cboContext.Name = "cboContext";
            this.cboContext.Size = new System.Drawing.Size(343, 32);
            this.cboContext.TabIndex = 46;
            // 
            // lblContext
            // 
            this.lblContext.AutoSize = true;
            this.lblContext.Location = new System.Drawing.Point(80, 333);
            this.lblContext.Name = "lblContext";
            this.lblContext.Size = new System.Drawing.Size(118, 24);
            this.lblContext.TabIndex = 47;
            this.lblContext.Text = "上下文设置：";
            // 
            // chkClearContextAfterExit
            // 
            this.chkClearContextAfterExit.AutoSize = true;
            this.chkClearContextAfterExit.Location = new System.Drawing.Point(80, 262);
            this.chkClearContextAfterExit.Name = "chkClearContextAfterExit";
            this.chkClearContextAfterExit.Size = new System.Drawing.Size(216, 28);
            this.chkClearContextAfterExit.TabIndex = 48;
            this.chkClearContextAfterExit.Text = "聊天结束时清除上下文";
            this.chkClearContextAfterExit.UseVisualStyleBackColor = true;
            // 
            // cboModel
            // 
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Items.AddRange(new object[] {
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0301",
            "NewBing"});
            this.cboModel.Location = new System.Drawing.Point(348, 86);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(343, 32);
            this.cboModel.TabIndex = 49;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(80, 89);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(64, 24);
            this.lblModel.TabIndex = 50;
            this.lblModel.Text = "模型：";
            // 
            // lnkModel
            // 
            this.lnkModel.ActiveLinkColor = System.Drawing.Color.Red;
            this.lnkModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkModel.AutoSize = true;
            this.lnkModel.LinkColor = System.Drawing.Color.Blue;
            this.lnkModel.Location = new System.Drawing.Point(697, 89);
            this.lnkModel.Name = "lnkModel";
            this.lnkModel.Size = new System.Drawing.Size(46, 24);
            this.lnkModel.TabIndex = 51;
            this.lnkModel.TabStop = true;
            this.lnkModel.Text = "说明";
            this.lnkModel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkModel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkModel_LinkClicked);
            // 
            // lblClearContextCommands
            // 
            this.lblClearContextCommands.AutoSize = true;
            this.lblClearContextCommands.Location = new System.Drawing.Point(80, 670);
            this.lblClearContextCommands.Name = "lblClearContextCommands";
            this.lblClearContextCommands.Size = new System.Drawing.Size(262, 24);
            this.lblClearContextCommands.TabIndex = 53;
            this.lblClearContextCommands.Text = "清除上下文命令（一行一个）：";
            // 
            // txbClearContextCommands
            // 
            this.txbClearContextCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbClearContextCommands.Location = new System.Drawing.Point(348, 622);
            this.txbClearContextCommands.Multiline = true;
            this.txbClearContextCommands.Name = "txbClearContextCommands";
            this.txbClearContextCommands.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbClearContextCommands.Size = new System.Drawing.Size(343, 121);
            this.txbClearContextCommands.TabIndex = 52;
            // 
            // chkRemoveMarkdownExpression
            // 
            this.chkRemoveMarkdownExpression.AutoSize = true;
            this.chkRemoveMarkdownExpression.Location = new System.Drawing.Point(80, 296);
            this.chkRemoveMarkdownExpression.Name = "chkRemoveMarkdownExpression";
            this.chkRemoveMarkdownExpression.Size = new System.Drawing.Size(219, 28);
            this.chkRemoveMarkdownExpression.TabIndex = 54;
            this.chkRemoveMarkdownExpression.Text = "移除Markdown表达式";
            this.chkRemoveMarkdownExpression.UseVisualStyleBackColor = true;
            // 
            // txbCleanContextMessage
            // 
            this.txbCleanContextMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCleanContextMessage.Location = new System.Drawing.Point(348, 857);
            this.txbCleanContextMessage.Name = "txbCleanContextMessage";
            this.txbCleanContextMessage.Size = new System.Drawing.Size(343, 30);
            this.txbCleanContextMessage.TabIndex = 55;
            // 
            // lblCleanContextMessage
            // 
            this.lblCleanContextMessage.AutoSize = true;
            this.lblCleanContextMessage.Location = new System.Drawing.Point(80, 860);
            this.lblCleanContextMessage.Name = "lblCleanContextMessage";
            this.lblCleanContextMessage.Size = new System.Drawing.Size(190, 24);
            this.lblCleanContextMessage.TabIndex = 56;
            this.lblCleanContextMessage.Text = "主动清除上下文回复：";
            // 
            // FrmChatGPTClientSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 1021);
            this.Controls.Add(this.lblCleanContextMessage);
            this.Controls.Add(this.txbCleanContextMessage);
            this.Controls.Add(this.chkRemoveMarkdownExpression);
            this.Controls.Add(this.lblClearContextCommands);
            this.Controls.Add(this.txbClearContextCommands);
            this.Controls.Add(this.lnkModel);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.cboModel);
            this.Controls.Add(this.chkClearContextAfterExit);
            this.Controls.Add(this.lblContext);
            this.Controls.Add(this.cboContext);
            this.Controls.Add(this.lblCreateApiKey);
            this.Controls.Add(this.chkSendMessageByReply);
            this.Controls.Add(this.lblTimeOutSecond);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.lblTimeOutMessage);
            this.Controls.Add(this.lblExitChatMessage);
            this.Controls.Add(this.lblChatStartMessage);
            this.Controls.Add(this.lblExitCommands);
            this.Controls.Add(this.lblStartCommands);
            this.Controls.Add(this.lblTemperature);
            this.Controls.Add(this.lnkTemperature);
            this.Controls.Add(this.lblApiKey);
            this.Controls.Add(this.chkRequestByPlugin);
            this.Controls.Add(this.chkUseProxy);
            this.Controls.Add(this.txbTimeOutSecond);
            this.Controls.Add(this.txbErrorMessage);
            this.Controls.Add(this.txbTimeOutMessage);
            this.Controls.Add(this.txbExitChatMessage);
            this.Controls.Add(this.txbChatStartMessage);
            this.Controls.Add(this.txbExitCommands);
            this.Controls.Add(this.txbStartCommands);
            this.Controls.Add(this.txbTemperature);
            this.Controls.Add(this.txbApiKey);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChatGPTClientSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatGPT聊天-设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LinkLabel lblCreateApiKey;
        private CheckBox chkSendMessageByReply;
        private Label lblTimeOutSecond;
        private Label lblErrorMessage;
        private Label lblTimeOutMessage;
        private Label lblExitChatMessage;
        private Label lblChatStartMessage;
        private Label lblExitCommands;
        private Label lblStartCommands;
        private Label lblTemperature;
        private LinkLabel lnkTemperature;
        private Label lblApiKey;
        private CheckBox chkRequestByPlugin;
        private CheckBox chkUseProxy;
        private TextBox txbTimeOutSecond;
        private TextBox txbErrorMessage;
        private TextBox txbTimeOutMessage;
        private TextBox txbExitChatMessage;
        private TextBox txbChatStartMessage;
        private TextBox txbExitCommands;
        private TextBox txbStartCommands;
        private TextBox txbTemperature;
        private TextBox txbApiKey;
        private ComboBox cboContext;
        private Label lblContext;
        private CheckBox chkClearContextAfterExit;
        private ComboBox cboModel;
        private Label lblModel;
        private LinkLabel lnkModel;
        private Label lblClearContextCommands;
        private TextBox txbClearContextCommands;
        private CheckBox chkRemoveMarkdownExpression;
        private TextBox txbCleanContextMessage;
        private Label lblCleanContextMessage;
    }
}