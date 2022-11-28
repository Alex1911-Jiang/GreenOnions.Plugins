namespace GreenOnions.CustomHttpApiInvoker
{
    partial class CtrlListItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUrlTitle = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblRemarkTitle = new System.Windows.Forms.Label();
            this.lbllblRemark = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblCmd = new System.Windows.Forms.Label();
            this.lblCmdTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblUrlTitle
            // 
            this.lblUrlTitle.AutoSize = true;
            this.lblUrlTitle.Location = new System.Drawing.Point(9, 12);
            this.lblUrlTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUrlTitle.Name = "lblUrlTitle";
            this.lblUrlTitle.Size = new System.Drawing.Size(44, 17);
            this.lblUrlTitle.TabIndex = 0;
            this.lblUrlTitle.Text = "地址：";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(76, 12);
            this.lblUrl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(33, 17);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "{Url}";
            // 
            // lblRemarkTitle
            // 
            this.lblRemarkTitle.AutoSize = true;
            this.lblRemarkTitle.Location = new System.Drawing.Point(9, 41);
            this.lblRemarkTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRemarkTitle.Name = "lblRemarkTitle";
            this.lblRemarkTitle.Size = new System.Drawing.Size(44, 17);
            this.lblRemarkTitle.TabIndex = 2;
            this.lblRemarkTitle.Text = "备注：";
            // 
            // lbllblRemark
            // 
            this.lbllblRemark.AutoSize = true;
            this.lbllblRemark.Location = new System.Drawing.Point(76, 41);
            this.lbllblRemark.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbllblRemark.Name = "lbllblRemark";
            this.lbllblRemark.Size = new System.Drawing.Size(61, 17);
            this.lbllblRemark.TabIndex = 3;
            this.lbllblRemark.Text = "{Remark}";
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(444, 74);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(71, 24);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(519, 74);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(71, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblCmd
            // 
            this.lblCmd.AutoSize = true;
            this.lblCmd.Location = new System.Drawing.Point(77, 70);
            this.lblCmd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd.Name = "lblCmd";
            this.lblCmd.Size = new System.Drawing.Size(43, 17);
            this.lblCmd.TabIndex = 10;
            this.lblCmd.Text = "{Cmd}";
            // 
            // lblCmdTitle
            // 
            this.lblCmdTitle.AutoSize = true;
            this.lblCmdTitle.Location = new System.Drawing.Point(9, 70);
            this.lblCmdTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmdTitle.Name = "lblCmdTitle";
            this.lblCmdTitle.Size = new System.Drawing.Size(44, 17);
            this.lblCmdTitle.TabIndex = 9;
            this.lblCmdTitle.Text = "命令：";
            // 
            // CtrlListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblCmd);
            this.Controls.Add(this.lblCmdTitle);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lbllblRemark);
            this.Controls.Add(this.lblRemarkTitle);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.lblUrlTitle);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CtrlListItem";
            this.Size = new System.Drawing.Size(592, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblUrlTitle;
        private Label lblUrl;
        private Label lblRemarkTitle;
        private Label lbllblRemark;
        private Button btnEdit;
        private Button btnDelete;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label lblCmd;
        private Label lblCmdTitle;
    }
}
