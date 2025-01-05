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
            lblCreateApiKey = new LinkLabel();
            chkSendMessageByReply = new CheckBox();
            lblTimeOutSecond = new Label();
            lblErrorMessage = new Label();
            lblTimeOutMessage = new Label();
            lblExitChatMessage = new Label();
            lblChatStartMessage = new Label();
            lblExitCommands = new Label();
            lblStartCommands = new Label();
            lblTemperature = new Label();
            lnkTemperature = new LinkLabel();
            lblApiKey = new Label();
            chkRequestByPlugin = new CheckBox();
            chkUseProxy = new CheckBox();
            txbTimeOutSecond = new TextBox();
            txbErrorMessage = new TextBox();
            txbTimeOutMessage = new TextBox();
            txbExitChatMessage = new TextBox();
            txbChatStartMessage = new TextBox();
            txbExitCommands = new TextBox();
            txbStartCommands = new TextBox();
            txbTemperature = new TextBox();
            txbApiKey = new TextBox();
            cboContext = new ComboBox();
            lblContext = new Label();
            chkClearContextAfterExit = new CheckBox();
            cboModel = new ComboBox();
            lblModel = new Label();
            lnkModel = new LinkLabel();
            lblClearContextCommands = new Label();
            txbClearContextCommands = new TextBox();
            chkRemoveMarkdownExpression = new CheckBox();
            txbCleanContextMessage = new TextBox();
            lblCleanContextMessage = new Label();
            SuspendLayout();
            // 
            // lblCreateApiKey
            // 
            lblCreateApiKey.ActiveLinkColor = Color.Red;
            lblCreateApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCreateApiKey.AutoSize = true;
            lblCreateApiKey.LinkColor = Color.Blue;
            lblCreateApiKey.Location = new Point(697, 53);
            lblCreateApiKey.Name = "lblCreateApiKey";
            lblCreateApiKey.Size = new Size(46, 24);
            lblCreateApiKey.TabIndex = 45;
            lblCreateApiKey.TabStop = true;
            lblCreateApiKey.Text = "创建";
            lblCreateApiKey.VisitedLinkColor = Color.Blue;
            lblCreateApiKey.LinkClicked += lblCreateApiKey_LinkClicked;
            // 
            // chkSendMessageByReply
            // 
            chkSendMessageByReply.AutoSize = true;
            chkSendMessageByReply.Location = new Point(80, 228);
            chkSendMessageByReply.Name = "chkSendMessageByReply";
            chkSendMessageByReply.Size = new Size(212, 28);
            chkSendMessageByReply.TabIndex = 44;
            chkSendMessageByReply.Text = "以“回复”方式发送消息";
            chkSendMessageByReply.UseVisualStyleBackColor = true;
            // 
            // lblTimeOutSecond
            // 
            lblTimeOutSecond.AutoSize = true;
            lblTimeOutSecond.Location = new Point(80, 932);
            lblTimeOutSecond.Name = "lblTimeOutSecond";
            lblTimeOutSecond.Size = new Size(190, 24);
            lblTimeOutSecond.TabIndex = 43;
            lblTimeOutSecond.Text = "聊天超时时间（秒）：";
            // 
            // lblErrorMessage
            // 
            lblErrorMessage.AutoSize = true;
            lblErrorMessage.Location = new Point(80, 896);
            lblErrorMessage.Name = "lblErrorMessage";
            lblErrorMessage.Size = new Size(136, 24);
            lblErrorMessage.TabIndex = 42;
            lblErrorMessage.Text = "发生错误回复：";
            // 
            // lblTimeOutMessage
            // 
            lblTimeOutMessage.AutoSize = true;
            lblTimeOutMessage.Location = new Point(80, 824);
            lblTimeOutMessage.Name = "lblTimeOutMessage";
            lblTimeOutMessage.Size = new Size(172, 24);
            lblTimeOutMessage.TabIndex = 41;
            lblTimeOutMessage.Text = "聊天超时结束回复：";
            // 
            // lblExitChatMessage
            // 
            lblExitChatMessage.AutoSize = true;
            lblExitChatMessage.Location = new Point(80, 788);
            lblExitChatMessage.Name = "lblExitChatMessage";
            lblExitChatMessage.Size = new Size(172, 24);
            lblExitChatMessage.TabIndex = 40;
            lblExitChatMessage.Text = "主动结束聊天回复：";
            // 
            // lblChatStartMessage
            // 
            lblChatStartMessage.AutoSize = true;
            lblChatStartMessage.Location = new Point(80, 752);
            lblChatStartMessage.Name = "lblChatStartMessage";
            lblChatStartMessage.Size = new Size(136, 24);
            lblChatStartMessage.TabIndex = 39;
            lblChatStartMessage.Text = "开始聊天回复：";
            // 
            // lblExitCommands
            // 
            lblExitCommands.AutoSize = true;
            lblExitCommands.Location = new Point(80, 543);
            lblExitCommands.Name = "lblExitCommands";
            lblExitCommands.Size = new Size(244, 24);
            lblExitCommands.TabIndex = 38;
            lblExitCommands.Text = "结束聊天命令（一行一个）：";
            // 
            // lblStartCommands
            // 
            lblStartCommands.AutoSize = true;
            lblStartCommands.Location = new Point(80, 416);
            lblStartCommands.Name = "lblStartCommands";
            lblStartCommands.Size = new Size(244, 24);
            lblStartCommands.TabIndex = 37;
            lblStartCommands.Text = "开始聊天命令（一行一个）：";
            // 
            // lblTemperature
            // 
            lblTemperature.AutoSize = true;
            lblTemperature.Location = new Point(80, 127);
            lblTemperature.Name = "lblTemperature";
            lblTemperature.Size = new Size(100, 24);
            lblTemperature.TabIndex = 36;
            lblTemperature.Text = "采样温度：";
            // 
            // lnkTemperature
            // 
            lnkTemperature.ActiveLinkColor = Color.Red;
            lnkTemperature.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkTemperature.AutoSize = true;
            lnkTemperature.LinkColor = Color.Blue;
            lnkTemperature.Location = new Point(697, 127);
            lnkTemperature.Name = "lnkTemperature";
            lnkTemperature.Size = new Size(46, 24);
            lnkTemperature.TabIndex = 35;
            lnkTemperature.TabStop = true;
            lnkTemperature.Text = "说明";
            lnkTemperature.VisitedLinkColor = Color.Blue;
            lnkTemperature.LinkClicked += lnkTemperature_LinkClicked;
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(80, 53);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(96, 24);
            lblApiKey.TabIndex = 34;
            lblApiKey.Text = "API-Key：";
            // 
            // chkRequestByPlugin
            // 
            chkRequestByPlugin.AutoSize = true;
            chkRequestByPlugin.Location = new Point(80, 194);
            chkRequestByPlugin.Name = "chkRequestByPlugin";
            chkRequestByPlugin.Size = new Size(508, 28);
            chkRequestByPlugin.TabIndex = 33;
            chkRequestByPlugin.Text = "使用插件替代基本库完成Http请求（取决于装了什么插件）";
            chkRequestByPlugin.UseVisualStyleBackColor = true;
            // 
            // chkUseProxy
            // 
            chkUseProxy.AutoSize = true;
            chkUseProxy.Location = new Point(80, 160);
            chkUseProxy.Name = "chkUseProxy";
            chkUseProxy.Size = new Size(360, 28);
            chkUseProxy.TabIndex = 32;
            chkUseProxy.Text = "使用代理（无法在插件替代模式下生效）";
            chkUseProxy.UseVisualStyleBackColor = true;
            // 
            // txbTimeOutSecond
            // 
            txbTimeOutSecond.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbTimeOutSecond.Location = new Point(348, 929);
            txbTimeOutSecond.Name = "txbTimeOutSecond";
            txbTimeOutSecond.Size = new Size(343, 30);
            txbTimeOutSecond.TabIndex = 31;
            // 
            // txbErrorMessage
            // 
            txbErrorMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbErrorMessage.Location = new Point(348, 893);
            txbErrorMessage.Name = "txbErrorMessage";
            txbErrorMessage.Size = new Size(343, 30);
            txbErrorMessage.TabIndex = 30;
            // 
            // txbTimeOutMessage
            // 
            txbTimeOutMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbTimeOutMessage.Location = new Point(348, 821);
            txbTimeOutMessage.Name = "txbTimeOutMessage";
            txbTimeOutMessage.Size = new Size(343, 30);
            txbTimeOutMessage.TabIndex = 29;
            // 
            // txbExitChatMessage
            // 
            txbExitChatMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbExitChatMessage.Location = new Point(348, 785);
            txbExitChatMessage.Name = "txbExitChatMessage";
            txbExitChatMessage.Size = new Size(343, 30);
            txbExitChatMessage.TabIndex = 28;
            // 
            // txbChatStartMessage
            // 
            txbChatStartMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbChatStartMessage.Location = new Point(348, 749);
            txbChatStartMessage.Name = "txbChatStartMessage";
            txbChatStartMessage.Size = new Size(343, 30);
            txbChatStartMessage.TabIndex = 27;
            // 
            // txbExitCommands
            // 
            txbExitCommands.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbExitCommands.Location = new Point(348, 495);
            txbExitCommands.Multiline = true;
            txbExitCommands.Name = "txbExitCommands";
            txbExitCommands.ScrollBars = ScrollBars.Vertical;
            txbExitCommands.Size = new Size(343, 121);
            txbExitCommands.TabIndex = 26;
            // 
            // txbStartCommands
            // 
            txbStartCommands.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbStartCommands.Location = new Point(348, 368);
            txbStartCommands.Multiline = true;
            txbStartCommands.Name = "txbStartCommands";
            txbStartCommands.ScrollBars = ScrollBars.Vertical;
            txbStartCommands.Size = new Size(343, 121);
            txbStartCommands.TabIndex = 25;
            // 
            // txbTemperature
            // 
            txbTemperature.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbTemperature.Location = new Point(348, 124);
            txbTemperature.Name = "txbTemperature";
            txbTemperature.Size = new Size(343, 30);
            txbTemperature.TabIndex = 24;
            // 
            // txbApiKey
            // 
            txbApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbApiKey.Location = new Point(348, 50);
            txbApiKey.Name = "txbApiKey";
            txbApiKey.Size = new Size(343, 30);
            txbApiKey.TabIndex = 23;
            // 
            // cboContext
            // 
            cboContext.DropDownStyle = ComboBoxStyle.DropDownList;
            cboContext.FormattingEnabled = true;
            cboContext.Items.AddRange(new object[] { "尽量保持上下文（如果没超过令牌上限）", "仅保持上一条上下文", "不保持上下文" });
            cboContext.Location = new Point(348, 330);
            cboContext.Name = "cboContext";
            cboContext.Size = new Size(343, 32);
            cboContext.TabIndex = 46;
            // 
            // lblContext
            // 
            lblContext.AutoSize = true;
            lblContext.Location = new Point(80, 333);
            lblContext.Name = "lblContext";
            lblContext.Size = new Size(118, 24);
            lblContext.TabIndex = 47;
            lblContext.Text = "上下文设置：";
            // 
            // chkClearContextAfterExit
            // 
            chkClearContextAfterExit.AutoSize = true;
            chkClearContextAfterExit.Location = new Point(80, 262);
            chkClearContextAfterExit.Name = "chkClearContextAfterExit";
            chkClearContextAfterExit.Size = new Size(216, 28);
            chkClearContextAfterExit.TabIndex = 48;
            chkClearContextAfterExit.Text = "聊天结束时清除上下文";
            chkClearContextAfterExit.UseVisualStyleBackColor = true;
            // 
            // cboModel
            // 
            cboModel.FormattingEnabled = true;
            cboModel.Items.AddRange(new object[] { "gpt-3.5-turbo", "gpt-3.5-turbo-0301", "gpt-3.5-turbo-0613", "gpt-3.5-turbo-16k", "gpt-3.5-turbo-16k-0613", "gpt-4", "gpt-4-0613", "gpt-4-32k", "gpt-4-32k-0613", "gpt-4-turbo", "gpt-4o", "gpt-4o-mini", "NewBing" });
            cboModel.Location = new Point(348, 86);
            cboModel.Name = "cboModel";
            cboModel.Size = new Size(343, 32);
            cboModel.TabIndex = 49;
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Location = new Point(80, 89);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(64, 24);
            lblModel.TabIndex = 50;
            lblModel.Text = "模型：";
            // 
            // lnkModel
            // 
            lnkModel.ActiveLinkColor = Color.Red;
            lnkModel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkModel.AutoSize = true;
            lnkModel.LinkColor = Color.Blue;
            lnkModel.Location = new Point(697, 89);
            lnkModel.Name = "lnkModel";
            lnkModel.Size = new Size(46, 24);
            lnkModel.TabIndex = 51;
            lnkModel.TabStop = true;
            lnkModel.Text = "说明";
            lnkModel.VisitedLinkColor = Color.Blue;
            lnkModel.LinkClicked += lnkModel_LinkClicked;
            // 
            // lblClearContextCommands
            // 
            lblClearContextCommands.AutoSize = true;
            lblClearContextCommands.Location = new Point(80, 670);
            lblClearContextCommands.Name = "lblClearContextCommands";
            lblClearContextCommands.Size = new Size(262, 24);
            lblClearContextCommands.TabIndex = 53;
            lblClearContextCommands.Text = "清除上下文命令（一行一个）：";
            // 
            // txbClearContextCommands
            // 
            txbClearContextCommands.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbClearContextCommands.Location = new Point(348, 622);
            txbClearContextCommands.Multiline = true;
            txbClearContextCommands.Name = "txbClearContextCommands";
            txbClearContextCommands.ScrollBars = ScrollBars.Vertical;
            txbClearContextCommands.Size = new Size(343, 121);
            txbClearContextCommands.TabIndex = 52;
            // 
            // chkRemoveMarkdownExpression
            // 
            chkRemoveMarkdownExpression.AutoSize = true;
            chkRemoveMarkdownExpression.Location = new Point(80, 296);
            chkRemoveMarkdownExpression.Name = "chkRemoveMarkdownExpression";
            chkRemoveMarkdownExpression.Size = new Size(219, 28);
            chkRemoveMarkdownExpression.TabIndex = 54;
            chkRemoveMarkdownExpression.Text = "移除Markdown表达式";
            chkRemoveMarkdownExpression.UseVisualStyleBackColor = true;
            // 
            // txbCleanContextMessage
            // 
            txbCleanContextMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txbCleanContextMessage.Location = new Point(348, 857);
            txbCleanContextMessage.Name = "txbCleanContextMessage";
            txbCleanContextMessage.Size = new Size(343, 30);
            txbCleanContextMessage.TabIndex = 55;
            // 
            // lblCleanContextMessage
            // 
            lblCleanContextMessage.AutoSize = true;
            lblCleanContextMessage.Location = new Point(80, 860);
            lblCleanContextMessage.Name = "lblCleanContextMessage";
            lblCleanContextMessage.Size = new Size(190, 24);
            lblCleanContextMessage.TabIndex = 56;
            lblCleanContextMessage.Text = "主动清除上下文回复：";
            // 
            // FrmChatGPTClientSetting
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 1021);
            Controls.Add(lblCleanContextMessage);
            Controls.Add(txbCleanContextMessage);
            Controls.Add(chkRemoveMarkdownExpression);
            Controls.Add(lblClearContextCommands);
            Controls.Add(txbClearContextCommands);
            Controls.Add(lnkModel);
            Controls.Add(lblModel);
            Controls.Add(cboModel);
            Controls.Add(chkClearContextAfterExit);
            Controls.Add(lblContext);
            Controls.Add(cboContext);
            Controls.Add(lblCreateApiKey);
            Controls.Add(chkSendMessageByReply);
            Controls.Add(lblTimeOutSecond);
            Controls.Add(lblErrorMessage);
            Controls.Add(lblTimeOutMessage);
            Controls.Add(lblExitChatMessage);
            Controls.Add(lblChatStartMessage);
            Controls.Add(lblExitCommands);
            Controls.Add(lblStartCommands);
            Controls.Add(lblTemperature);
            Controls.Add(lnkTemperature);
            Controls.Add(lblApiKey);
            Controls.Add(chkRequestByPlugin);
            Controls.Add(chkUseProxy);
            Controls.Add(txbTimeOutSecond);
            Controls.Add(txbErrorMessage);
            Controls.Add(txbTimeOutMessage);
            Controls.Add(txbExitChatMessage);
            Controls.Add(txbChatStartMessage);
            Controls.Add(txbExitCommands);
            Controls.Add(txbStartCommands);
            Controls.Add(txbTemperature);
            Controls.Add(txbApiKey);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmChatGPTClientSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChatGPT聊天-设置";
            ResumeLayout(false);
            PerformLayout();
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