﻿namespace GreenOnions.CustomHttpApiInvoker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.pnlConfigList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddConfig = new System.Windows.Forms.Button();
            this.lblHelpCmd = new System.Windows.Forms.Label();
            this.txbHelpCmd = new System.Windows.Forms.TextBox();
            this.pnlConfigList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConfigList
            // 
            this.pnlConfigList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfigList.AutoScroll = true;
            this.pnlConfigList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConfigList.Controls.Add(this.btnAddConfig);
            this.pnlConfigList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlConfigList.Location = new System.Drawing.Point(19, 49);
            this.pnlConfigList.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pnlConfigList.Name = "pnlConfigList";
            this.pnlConfigList.Size = new System.Drawing.Size(946, 787);
            this.pnlConfigList.TabIndex = 8;
            this.pnlConfigList.WrapContents = false;
            this.pnlConfigList.SizeChanged += new System.EventHandler(this.pnlConfigList_SizeChanged);
            this.pnlConfigList.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.pnlConfigList_ControlChanged);
            this.pnlConfigList.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.pnlConfigList_ControlChanged);
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnAddConfig.Image")));
            this.btnAddConfig.Location = new System.Drawing.Point(5, 4);
            this.btnAddConfig.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(930, 141);
            this.btnAddConfig.TabIndex = 0;
            this.btnAddConfig.UseVisualStyleBackColor = true;
            this.btnAddConfig.Click += new System.EventHandler(this.btnAddConfig_Click);
            // 
            // lblHelpCmd
            // 
            this.lblHelpCmd.AutoSize = true;
            this.lblHelpCmd.Location = new System.Drawing.Point(19, 13);
            this.lblHelpCmd.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblHelpCmd.Name = "lblHelpCmd";
            this.lblHelpCmd.Size = new System.Drawing.Size(274, 24);
            this.lblHelpCmd.TabIndex = 9;
            this.lblHelpCmd.Text = "列出所有Api命令及说明的命令：";
            // 
            // txbHelpCmd
            // 
            this.txbHelpCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbHelpCmd.Location = new System.Drawing.Point(316, 8);
            this.txbHelpCmd.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txbHelpCmd.Name = "txbHelpCmd";
            this.txbHelpCmd.Size = new System.Drawing.Size(648, 30);
            this.txbHelpCmd.TabIndex = 10;
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 850);
            this.Controls.Add(this.txbHelpCmd);
            this.Controls.Add(this.lblHelpCmd);
            this.Controls.Add(this.pnlConfigList);
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmSettings";
            this.pnlConfigList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FlowLayoutPanel pnlConfigList;
        private Button btnAddConfig;
        private Label lblHelpCmd;
        private TextBox txbHelpCmd;
    }
}