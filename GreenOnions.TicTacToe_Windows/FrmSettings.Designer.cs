namespace GreenOnions.TicTacToe_Windows
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
            this.lblTicTacToeMoveFailReply = new System.Windows.Forms.Label();
            this.txbTicTacToeMoveFailReply = new System.Windows.Forms.TextBox();
            this.lblChessboard = new System.Windows.Forms.Label();
            this.imgChessboard = new System.Windows.Forms.Panel();
            this.lblTicTacToeIllegalMoveReply = new System.Windows.Forms.Label();
            this.txbTicTacToeIllegalMoveReply = new System.Windows.Forms.TextBox();
            this.lblTicTacToeNoMoveReply = new System.Windows.Forms.Label();
            this.txbTicTacToeNoMoveReply = new System.Windows.Forms.TextBox();
            this.lblTicTacToeDrawReply = new System.Windows.Forms.Label();
            this.lblTicTacToeBotWinReply = new System.Windows.Forms.Label();
            this.lblTicTacToePlayerWinReply = new System.Windows.Forms.Label();
            this.txbTicTacToeDrawReply = new System.Windows.Forms.TextBox();
            this.txbTicTacToeBotWinReply = new System.Windows.Forms.TextBox();
            this.lblTicTacToeTimeoutReply = new System.Windows.Forms.Label();
            this.txbTicTacToePlayerWinReply = new System.Windows.Forms.TextBox();
            this.txbTicTacToeTimeoutReply = new System.Windows.Forms.TextBox();
            this.pnlTicTacToeMoveMode = new System.Windows.Forms.Panel();
            this.chkTicTacToeMoveModeOpenCV = new System.Windows.Forms.CheckBox();
            this.chkTicTacToeMoveModeNomenclature = new System.Windows.Forms.CheckBox();
            this.txbTicTacToeAlreadStopReply = new System.Windows.Forms.TextBox();
            this.txbTicTacToeStoppedReply = new System.Windows.Forms.TextBox();
            this.txbStopTicTacToeCmd = new System.Windows.Forms.TextBox();
            this.txbTicTacToeAlreadyStartReply = new System.Windows.Forms.TextBox();
            this.txbTicTacToeStartedReply = new System.Windows.Forms.TextBox();
            this.txbStartTicTacToeCmd = new System.Windows.Forms.TextBox();
            this.lblTicTacToeMoveMode = new System.Windows.Forms.Label();
            this.lblTicTacToeAlreadStopReply = new System.Windows.Forms.Label();
            this.lblTicTacToeStartedReply = new System.Windows.Forms.Label();
            this.lblTicTacToeStoppedReply = new System.Windows.Forms.Label();
            this.lblTicTacToeAlreadyStartReply = new System.Windows.Forms.Label();
            this.lblStopTicTacToeCmd = new System.Windows.Forms.Label();
            this.lblStartTicTacToeCmd = new System.Windows.Forms.Label();
            this.pnlTicTacToeMoveMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTicTacToeMoveFailReply
            // 
            this.lblTicTacToeMoveFailReply.AutoSize = true;
            this.lblTicTacToeMoveFailReply.Location = new System.Drawing.Point(12, 357);
            this.lblTicTacToeMoveFailReply.Name = "lblTicTacToeMoveFailReply";
            this.lblTicTacToeMoveFailReply.Size = new System.Drawing.Size(159, 17);
            this.lblTicTacToeMoveFailReply.TabIndex = 59;
            this.lblTicTacToeMoveFailReply.Text = "识别到格子已有棋子回复语: ";
            // 
            // txbTicTacToeMoveFailReply
            // 
            this.txbTicTacToeMoveFailReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeMoveFailReply.Location = new System.Drawing.Point(180, 354);
            this.txbTicTacToeMoveFailReply.Name = "txbTicTacToeMoveFailReply";
            this.txbTicTacToeMoveFailReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeMoveFailReply.TabIndex = 58;
            // 
            // lblChessboard
            // 
            this.lblChessboard.AutoSize = true;
            this.lblChessboard.Location = new System.Drawing.Point(12, 517);
            this.lblChessboard.Name = "lblChessboard";
            this.lblChessboard.Size = new System.Drawing.Size(107, 17);
            this.lblChessboard.TabIndex = 57;
            this.lblChessboard.Text = "棋盘编号命名示例:";
            // 
            // imgChessboard
            // 
            this.imgChessboard.BackgroundImage = global::GreenOnions.TicTacToe_Windows.Resource.Chessboard;
            this.imgChessboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imgChessboard.Location = new System.Drawing.Point(222, 419);
            this.imgChessboard.Name = "imgChessboard";
            this.imgChessboard.Size = new System.Drawing.Size(220, 220);
            this.imgChessboard.TabIndex = 56;
            // 
            // lblTicTacToeIllegalMoveReply
            // 
            this.lblTicTacToeIllegalMoveReply.AutoSize = true;
            this.lblTicTacToeIllegalMoveReply.Location = new System.Drawing.Point(12, 328);
            this.lblTicTacToeIllegalMoveReply.Name = "lblTicTacToeIllegalMoveReply";
            this.lblTicTacToeIllegalMoveReply.Size = new System.Drawing.Size(159, 17);
            this.lblTicTacToeIllegalMoveReply.TabIndex = 55;
            this.lblTicTacToeIllegalMoveReply.Text = "识别到下子多于一格回复语: ";
            // 
            // txbTicTacToeIllegalMoveReply
            // 
            this.txbTicTacToeIllegalMoveReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeIllegalMoveReply.Location = new System.Drawing.Point(180, 325);
            this.txbTicTacToeIllegalMoveReply.Name = "txbTicTacToeIllegalMoveReply";
            this.txbTicTacToeIllegalMoveReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeIllegalMoveReply.TabIndex = 54;
            // 
            // lblTicTacToeNoMoveReply
            // 
            this.lblTicTacToeNoMoveReply.AutoSize = true;
            this.lblTicTacToeNoMoveReply.Location = new System.Drawing.Point(12, 299);
            this.lblTicTacToeNoMoveReply.Name = "lblTicTacToeNoMoveReply";
            this.lblTicTacToeNoMoveReply.Size = new System.Drawing.Size(135, 17);
            this.lblTicTacToeNoMoveReply.TabIndex = 53;
            this.lblTicTacToeNoMoveReply.Text = "没有识别到下子回复语: ";
            // 
            // txbTicTacToeNoMoveReply
            // 
            this.txbTicTacToeNoMoveReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeNoMoveReply.Location = new System.Drawing.Point(180, 296);
            this.txbTicTacToeNoMoveReply.Name = "txbTicTacToeNoMoveReply";
            this.txbTicTacToeNoMoveReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeNoMoveReply.TabIndex = 52;
            // 
            // lblTicTacToeDrawReply
            // 
            this.lblTicTacToeDrawReply.AutoSize = true;
            this.lblTicTacToeDrawReply.Location = new System.Drawing.Point(12, 270);
            this.lblTicTacToeDrawReply.Name = "lblTicTacToeDrawReply";
            this.lblTicTacToeDrawReply.Size = new System.Drawing.Size(75, 17);
            this.lblTicTacToeDrawReply.TabIndex = 51;
            this.lblTicTacToeDrawReply.Text = "平局回复语: ";
            // 
            // lblTicTacToeBotWinReply
            // 
            this.lblTicTacToeBotWinReply.AutoSize = true;
            this.lblTicTacToeBotWinReply.Location = new System.Drawing.Point(12, 241);
            this.lblTicTacToeBotWinReply.Name = "lblTicTacToeBotWinReply";
            this.lblTicTacToeBotWinReply.Size = new System.Drawing.Size(111, 17);
            this.lblTicTacToeBotWinReply.TabIndex = 50;
            this.lblTicTacToeBotWinReply.Text = "机器人获胜回复语: ";
            // 
            // lblTicTacToePlayerWinReply
            // 
            this.lblTicTacToePlayerWinReply.AutoSize = true;
            this.lblTicTacToePlayerWinReply.Location = new System.Drawing.Point(12, 212);
            this.lblTicTacToePlayerWinReply.Name = "lblTicTacToePlayerWinReply";
            this.lblTicTacToePlayerWinReply.Size = new System.Drawing.Size(99, 17);
            this.lblTicTacToePlayerWinReply.TabIndex = 49;
            this.lblTicTacToePlayerWinReply.Text = "玩家获胜回复语: ";
            // 
            // txbTicTacToeDrawReply
            // 
            this.txbTicTacToeDrawReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeDrawReply.Location = new System.Drawing.Point(180, 267);
            this.txbTicTacToeDrawReply.Name = "txbTicTacToeDrawReply";
            this.txbTicTacToeDrawReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeDrawReply.TabIndex = 48;
            // 
            // txbTicTacToeBotWinReply
            // 
            this.txbTicTacToeBotWinReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeBotWinReply.Location = new System.Drawing.Point(180, 238);
            this.txbTicTacToeBotWinReply.Name = "txbTicTacToeBotWinReply";
            this.txbTicTacToeBotWinReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeBotWinReply.TabIndex = 47;
            // 
            // lblTicTacToeTimeoutReply
            // 
            this.lblTicTacToeTimeoutReply.AutoSize = true;
            this.lblTicTacToeTimeoutReply.Location = new System.Drawing.Point(12, 183);
            this.lblTicTacToeTimeoutReply.Name = "lblTicTacToeTimeoutReply";
            this.lblTicTacToeTimeoutReply.Size = new System.Drawing.Size(99, 17);
            this.lblTicTacToeTimeoutReply.TabIndex = 46;
            this.lblTicTacToeTimeoutReply.Text = "对局超时回复语: ";
            // 
            // txbTicTacToePlayerWinReply
            // 
            this.txbTicTacToePlayerWinReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToePlayerWinReply.Location = new System.Drawing.Point(180, 209);
            this.txbTicTacToePlayerWinReply.Name = "txbTicTacToePlayerWinReply";
            this.txbTicTacToePlayerWinReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToePlayerWinReply.TabIndex = 45;
            // 
            // txbTicTacToeTimeoutReply
            // 
            this.txbTicTacToeTimeoutReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeTimeoutReply.Location = new System.Drawing.Point(180, 180);
            this.txbTicTacToeTimeoutReply.Name = "txbTicTacToeTimeoutReply";
            this.txbTicTacToeTimeoutReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeTimeoutReply.TabIndex = 44;
            // 
            // pnlTicTacToeMoveMode
            // 
            this.pnlTicTacToeMoveMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTicTacToeMoveMode.Controls.Add(this.chkTicTacToeMoveModeOpenCV);
            this.pnlTicTacToeMoveMode.Controls.Add(this.chkTicTacToeMoveModeNomenclature);
            this.pnlTicTacToeMoveMode.Location = new System.Drawing.Point(180, 383);
            this.pnlTicTacToeMoveMode.Name = "pnlTicTacToeMoveMode";
            this.pnlTicTacToeMoveMode.Size = new System.Drawing.Size(451, 30);
            this.pnlTicTacToeMoveMode.TabIndex = 43;
            // 
            // chkTicTacToeMoveModeOpenCV
            // 
            this.chkTicTacToeMoveModeOpenCV.AutoSize = true;
            this.chkTicTacToeMoveModeOpenCV.Location = new System.Drawing.Point(3, 6);
            this.chkTicTacToeMoveModeOpenCV.Name = "chkTicTacToeMoveModeOpenCV";
            this.chkTicTacToeMoveModeOpenCV.Size = new System.Drawing.Size(75, 21);
            this.chkTicTacToeMoveModeOpenCV.TabIndex = 5;
            this.chkTicTacToeMoveModeOpenCV.Tag = "2";
            this.chkTicTacToeMoveModeOpenCV.Text = "图像识别";
            this.chkTicTacToeMoveModeOpenCV.UseVisualStyleBackColor = true;
            // 
            // chkTicTacToeMoveModeNomenclature
            // 
            this.chkTicTacToeMoveModeNomenclature.AutoSize = true;
            this.chkTicTacToeMoveModeNomenclature.Location = new System.Drawing.Point(84, 6);
            this.chkTicTacToeMoveModeNomenclature.Name = "chkTicTacToeMoveModeNomenclature";
            this.chkTicTacToeMoveModeNomenclature.Size = new System.Drawing.Size(75, 21);
            this.chkTicTacToeMoveModeNomenclature.TabIndex = 5;
            this.chkTicTacToeMoveModeNomenclature.Tag = "4";
            this.chkTicTacToeMoveModeNomenclature.Text = "回复格号";
            this.chkTicTacToeMoveModeNomenclature.UseVisualStyleBackColor = true;
            // 
            // txbTicTacToeAlreadStopReply
            // 
            this.txbTicTacToeAlreadStopReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeAlreadStopReply.Location = new System.Drawing.Point(180, 151);
            this.txbTicTacToeAlreadStopReply.Name = "txbTicTacToeAlreadStopReply";
            this.txbTicTacToeAlreadStopReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeAlreadStopReply.TabIndex = 42;
            // 
            // txbTicTacToeStoppedReply
            // 
            this.txbTicTacToeStoppedReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeStoppedReply.Location = new System.Drawing.Point(180, 122);
            this.txbTicTacToeStoppedReply.Name = "txbTicTacToeStoppedReply";
            this.txbTicTacToeStoppedReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeStoppedReply.TabIndex = 41;
            // 
            // txbStopTicTacToeCmd
            // 
            this.txbStopTicTacToeCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStopTicTacToeCmd.Location = new System.Drawing.Point(180, 93);
            this.txbStopTicTacToeCmd.Name = "txbStopTicTacToeCmd";
            this.txbStopTicTacToeCmd.Size = new System.Drawing.Size(451, 23);
            this.txbStopTicTacToeCmd.TabIndex = 40;
            // 
            // txbTicTacToeAlreadyStartReply
            // 
            this.txbTicTacToeAlreadyStartReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeAlreadyStartReply.Location = new System.Drawing.Point(180, 64);
            this.txbTicTacToeAlreadyStartReply.Name = "txbTicTacToeAlreadyStartReply";
            this.txbTicTacToeAlreadyStartReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeAlreadyStartReply.TabIndex = 39;
            // 
            // txbTicTacToeStartedReply
            // 
            this.txbTicTacToeStartedReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTicTacToeStartedReply.Location = new System.Drawing.Point(180, 35);
            this.txbTicTacToeStartedReply.Name = "txbTicTacToeStartedReply";
            this.txbTicTacToeStartedReply.Size = new System.Drawing.Size(451, 23);
            this.txbTicTacToeStartedReply.TabIndex = 38;
            // 
            // txbStartTicTacToeCmd
            // 
            this.txbStartTicTacToeCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStartTicTacToeCmd.Location = new System.Drawing.Point(180, 6);
            this.txbStartTicTacToeCmd.Name = "txbStartTicTacToeCmd";
            this.txbStartTicTacToeCmd.Size = new System.Drawing.Size(451, 23);
            this.txbStartTicTacToeCmd.TabIndex = 37;
            // 
            // lblTicTacToeMoveMode
            // 
            this.lblTicTacToeMoveMode.AutoSize = true;
            this.lblTicTacToeMoveMode.Location = new System.Drawing.Point(12, 390);
            this.lblTicTacToeMoveMode.Name = "lblTicTacToeMoveMode";
            this.lblTicTacToeMoveMode.Size = new System.Drawing.Size(107, 17);
            this.lblTicTacToeMoveMode.TabIndex = 36;
            this.lblTicTacToeMoveMode.Text = "允许玩家下子方式:";
            // 
            // lblTicTacToeAlreadStopReply
            // 
            this.lblTicTacToeAlreadStopReply.AutoSize = true;
            this.lblTicTacToeAlreadStopReply.Location = new System.Drawing.Point(12, 154);
            this.lblTicTacToeAlreadStopReply.Name = "lblTicTacToeAlreadStopReply";
            this.lblTicTacToeAlreadStopReply.Size = new System.Drawing.Size(111, 17);
            this.lblTicTacToeAlreadStopReply.TabIndex = 35;
            this.lblTicTacToeAlreadStopReply.Text = "未在对局中回复语: ";
            // 
            // lblTicTacToeStartedReply
            // 
            this.lblTicTacToeStartedReply.AutoSize = true;
            this.lblTicTacToeStartedReply.Location = new System.Drawing.Point(12, 38);
            this.lblTicTacToeStartedReply.Name = "lblTicTacToeStartedReply";
            this.lblTicTacToeStartedReply.Size = new System.Drawing.Size(123, 17);
            this.lblTicTacToeStartedReply.TabIndex = 34;
            this.lblTicTacToeStartedReply.Text = "开启对局成功回复语: ";
            // 
            // lblTicTacToeStoppedReply
            // 
            this.lblTicTacToeStoppedReply.AutoSize = true;
            this.lblTicTacToeStoppedReply.Location = new System.Drawing.Point(12, 125);
            this.lblTicTacToeStoppedReply.Name = "lblTicTacToeStoppedReply";
            this.lblTicTacToeStoppedReply.Size = new System.Drawing.Size(123, 17);
            this.lblTicTacToeStoppedReply.TabIndex = 33;
            this.lblTicTacToeStoppedReply.Text = "中止对局成功回复语: ";
            // 
            // lblTicTacToeAlreadyStartReply
            // 
            this.lblTicTacToeAlreadyStartReply.AutoSize = true;
            this.lblTicTacToeAlreadyStartReply.Location = new System.Drawing.Point(12, 67);
            this.lblTicTacToeAlreadyStartReply.Name = "lblTicTacToeAlreadyStartReply";
            this.lblTicTacToeAlreadyStartReply.Size = new System.Drawing.Size(111, 17);
            this.lblTicTacToeAlreadyStartReply.TabIndex = 32;
            this.lblTicTacToeAlreadyStartReply.Text = "已在对局中回复语: ";
            // 
            // lblStopTicTacToeCmd
            // 
            this.lblStopTicTacToeCmd.AutoSize = true;
            this.lblStopTicTacToeCmd.Location = new System.Drawing.Point(12, 96);
            this.lblStopTicTacToeCmd.Name = "lblStopTicTacToeCmd";
            this.lblStopTicTacToeCmd.Size = new System.Drawing.Size(87, 17);
            this.lblStopTicTacToeCmd.TabIndex = 31;
            this.lblStopTicTacToeCmd.Text = "中止对局命令: ";
            // 
            // lblStartTicTacToeCmd
            // 
            this.lblStartTicTacToeCmd.AutoSize = true;
            this.lblStartTicTacToeCmd.Location = new System.Drawing.Point(12, 9);
            this.lblStartTicTacToeCmd.Name = "lblStartTicTacToeCmd";
            this.lblStartTicTacToeCmd.Size = new System.Drawing.Size(87, 17);
            this.lblStartTicTacToeCmd.TabIndex = 30;
            this.lblStartTicTacToeCmd.Text = "开启对局命令: ";
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 651);
            this.Controls.Add(this.lblTicTacToeMoveFailReply);
            this.Controls.Add(this.txbTicTacToeMoveFailReply);
            this.Controls.Add(this.lblChessboard);
            this.Controls.Add(this.imgChessboard);
            this.Controls.Add(this.lblTicTacToeIllegalMoveReply);
            this.Controls.Add(this.txbTicTacToeIllegalMoveReply);
            this.Controls.Add(this.lblTicTacToeNoMoveReply);
            this.Controls.Add(this.txbTicTacToeNoMoveReply);
            this.Controls.Add(this.lblTicTacToeDrawReply);
            this.Controls.Add(this.lblTicTacToeBotWinReply);
            this.Controls.Add(this.lblTicTacToePlayerWinReply);
            this.Controls.Add(this.txbTicTacToeDrawReply);
            this.Controls.Add(this.txbTicTacToeBotWinReply);
            this.Controls.Add(this.lblTicTacToeTimeoutReply);
            this.Controls.Add(this.txbTicTacToePlayerWinReply);
            this.Controls.Add(this.txbTicTacToeTimeoutReply);
            this.Controls.Add(this.pnlTicTacToeMoveMode);
            this.Controls.Add(this.txbTicTacToeAlreadStopReply);
            this.Controls.Add(this.txbTicTacToeStoppedReply);
            this.Controls.Add(this.txbStopTicTacToeCmd);
            this.Controls.Add(this.txbTicTacToeAlreadyStartReply);
            this.Controls.Add(this.txbTicTacToeStartedReply);
            this.Controls.Add(this.txbStartTicTacToeCmd);
            this.Controls.Add(this.lblTicTacToeMoveMode);
            this.Controls.Add(this.lblTicTacToeAlreadStopReply);
            this.Controls.Add(this.lblTicTacToeStartedReply);
            this.Controls.Add(this.lblTicTacToeStoppedReply);
            this.Controls.Add(this.lblTicTacToeAlreadyStartReply);
            this.Controls.Add(this.lblStopTicTacToeCmd);
            this.Controls.Add(this.lblStartTicTacToeCmd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "井字棋设置";
            this.pnlTicTacToeMoveMode.ResumeLayout(false);
            this.pnlTicTacToeMoveMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTicTacToeMoveFailReply;
        private TextBox txbTicTacToeMoveFailReply;
        private Label lblChessboard;
        private Panel imgChessboard;
        private Label lblTicTacToeIllegalMoveReply;
        private TextBox txbTicTacToeIllegalMoveReply;
        private Label lblTicTacToeNoMoveReply;
        private TextBox txbTicTacToeNoMoveReply;
        private Label lblTicTacToeDrawReply;
        private Label lblTicTacToeBotWinReply;
        private Label lblTicTacToePlayerWinReply;
        private TextBox txbTicTacToeDrawReply;
        private TextBox txbTicTacToeBotWinReply;
        private Label lblTicTacToeTimeoutReply;
        private TextBox txbTicTacToePlayerWinReply;
        private TextBox txbTicTacToeTimeoutReply;
        private Panel pnlTicTacToeMoveMode;
        private CheckBox chkTicTacToeMoveModeOpenCV;
        private CheckBox chkTicTacToeMoveModeNomenclature;
        private TextBox txbTicTacToeAlreadStopReply;
        private TextBox txbTicTacToeStoppedReply;
        private TextBox txbStopTicTacToeCmd;
        private TextBox txbTicTacToeAlreadyStartReply;
        private TextBox txbTicTacToeStartedReply;
        private TextBox txbStartTicTacToeCmd;
        private Label lblTicTacToeMoveMode;
        private Label lblTicTacToeAlreadStopReply;
        private Label lblTicTacToeStartedReply;
        private Label lblTicTacToeStoppedReply;
        private Label lblTicTacToeAlreadyStartReply;
        private Label lblStopTicTacToeCmd;
        private Label lblStartTicTacToeCmd;
    }
}