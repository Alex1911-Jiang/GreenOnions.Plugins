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
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblCmd = new System.Windows.Forms.Label();
            this.lblCmdTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblUrlTitle
            // 
            this.lblUrlTitle.AutoSize = true;
            this.lblUrlTitle.Location = new System.Drawing.Point(17, 22);
            this.lblUrlTitle.Name = "lblUrlTitle";
            this.lblUrlTitle.Size = new System.Drawing.Size(64, 24);
            this.lblUrlTitle.TabIndex = 0;
            this.lblUrlTitle.Text = "地址：";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(123, 22);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(47, 24);
            this.lblUrl.TabIndex = 1;
            this.lblUrl.Text = "{Url}";
            // 
            // lblRemarkTitle
            // 
            this.lblRemarkTitle.AutoSize = true;
            this.lblRemarkTitle.Location = new System.Drawing.Point(17, 63);
            this.lblRemarkTitle.Name = "lblRemarkTitle";
            this.lblRemarkTitle.Size = new System.Drawing.Size(64, 24);
            this.lblRemarkTitle.TabIndex = 2;
            this.lblRemarkTitle.Text = "备注：";
            // 
            // lbllblRemark
            // 
            this.lbllblRemark.AutoSize = true;
            this.lbllblRemark.Location = new System.Drawing.Point(123, 63);
            this.lbllblRemark.Name = "lbllblRemark";
            this.lbllblRemark.Size = new System.Drawing.Size(88, 24);
            this.lbllblRemark.TabIndex = 3;
            this.lbllblRemark.Text = "{Remark}";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(289, 100);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(112, 34);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(407, 100);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(112, 34);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(525, 104);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(72, 28);
            this.chkEnabled.TabIndex = 8;
            this.chkEnabled.Text = "启用";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // lblCmd
            // 
            this.lblCmd.AutoSize = true;
            this.lblCmd.Location = new System.Drawing.Point(124, 105);
            this.lblCmd.Name = "lblCmd";
            this.lblCmd.Size = new System.Drawing.Size(63, 24);
            this.lblCmd.TabIndex = 10;
            this.lblCmd.Text = "{Cmd}";
            // 
            // lblCmdTitle
            // 
            this.lblCmdTitle.AutoSize = true;
            this.lblCmdTitle.Location = new System.Drawing.Point(18, 105);
            this.lblCmdTitle.Name = "lblCmdTitle";
            this.lblCmdTitle.Size = new System.Drawing.Size(64, 24);
            this.lblCmdTitle.TabIndex = 9;
            this.lblCmdTitle.Text = "命令：";
            // 
            // CtrlListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCmd);
            this.Controls.Add(this.lblCmdTitle);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lbllblRemark);
            this.Controls.Add(this.lblRemarkTitle);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.lblUrlTitle);
            this.Name = "CtrlListItem";
            this.Size = new System.Drawing.Size(600, 150);
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
        private CheckBox chkEnabled;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label lblCmd;
        private Label lblCmdTitle;
    }
}
