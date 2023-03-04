namespace GreenOnions.PluginConfigEditor.Replier
{
    partial class FrmReplierSetting
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.dgvReplies = new System.Windows.Forms.DataGridView();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colTriggerMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colReplyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReplyMode = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lvImages = new System.Windows.Forms.ListView();
            this.btnAddMedia = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReplies)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // dgvReplies
            // 
            this.dgvReplies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReplies.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReplies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvReplies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReplies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMessage,
            this.colMatchMode,
            this.colTriggerMode,
            this.colReplyValue,
            this.colPriority,
            this.colReplyMode,
            this.colRemove});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReplies.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvReplies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReplies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvReplies.GridColor = System.Drawing.Color.Silver;
            this.dgvReplies.Location = new System.Drawing.Point(0, 30);
            this.dgvReplies.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.dgvReplies.Name = "dgvReplies";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReplies.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvReplies.RowHeadersVisible = false;
            this.dgvReplies.RowHeadersWidth = 62;
            this.dgvReplies.RowTemplate.Height = 25;
            this.dgvReplies.Size = new System.Drawing.Size(1483, 719);
            this.dgvReplies.TabIndex = 2;
            this.dgvReplies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReplies_CellContentClick);
            // 
            // colMessage
            // 
            this.colMessage.DataPropertyName = "Message";
            this.colMessage.HeaderText = "触发消息";
            this.colMessage.MinimumWidth = 8;
            this.colMessage.Name = "colMessage";
            // 
            // colMatchMode
            // 
            this.colMatchMode.DataPropertyName = "MatchMode";
            this.colMatchMode.FillWeight = 80F;
            this.colMatchMode.HeaderText = "匹配模式";
            this.colMatchMode.Items.AddRange(new object[] {
            "完全",
            "包含",
            "前缀",
            "后缀",
            "正则表达式"});
            this.colMatchMode.MinimumWidth = 8;
            this.colMatchMode.Name = "colMatchMode";
            // 
            // colTriggerMode
            // 
            this.colTriggerMode.DataPropertyName = "TriggerMode";
            this.colTriggerMode.FillWeight = 80F;
            this.colTriggerMode.HeaderText = "触发模式";
            this.colTriggerMode.Items.AddRange(new object[] {
            "私聊消息",
            "群组消息",
            "私聊/群组消息"});
            this.colTriggerMode.MinimumWidth = 8;
            this.colTriggerMode.Name = "colTriggerMode";
            // 
            // colReplyValue
            // 
            this.colReplyValue.DataPropertyName = "ReplyValue";
            this.colReplyValue.HeaderText = "回复内容";
            this.colReplyValue.MinimumWidth = 8;
            this.colReplyValue.Name = "colReplyValue";
            // 
            // colPriority
            // 
            this.colPriority.DataPropertyName = "Priority";
            this.colPriority.FillWeight = 40F;
            this.colPriority.HeaderText = "优先级";
            this.colPriority.MinimumWidth = 8;
            this.colPriority.Name = "colPriority";
            // 
            // colReplyMode
            // 
            this.colReplyMode.DataPropertyName = "ReplyMode";
            this.colReplyMode.FillWeight = 60F;
            this.colReplyMode.HeaderText = "以回复方式发送";
            this.colReplyMode.MinimumWidth = 8;
            this.colReplyMode.Name = "colReplyMode";
            // 
            // colRemove
            // 
            this.colRemove.FillWeight = 30F;
            this.colRemove.HeaderText = "删除";
            this.colRemove.MinimumWidth = 8;
            this.colRemove.Name = "colRemove";
            this.colRemove.Text = "删除";
            this.colRemove.UseColumnTextForButtonValue = true;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblInfo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1483, 30);
            this.pnlTop.TabIndex = 3;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(30, 3);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(1357, 24);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "提示：插入图片格式：<图片名称.后缀>，例如\"回复测试<测试图片.jpg>\"。优先级：数字越小优先度越高。随机回复：触发消息和优先级均相等时会随机发送一条回复。";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvImages);
            this.pnlBottom.Controls.Add(this.btnAddMedia);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 749);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1483, 212);
            this.pnlBottom.TabIndex = 3;
            // 
            // lvImages
            // 
            this.lvImages.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvImages.LargeImageList = this.imageList;
            this.lvImages.Location = new System.Drawing.Point(0, 0);
            this.lvImages.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lvImages.Name = "lvImages";
            this.lvImages.Size = new System.Drawing.Size(1420, 212);
            this.lvImages.TabIndex = 3;
            this.lvImages.UseCompatibleStateImageBehavior = false;
            this.lvImages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvImages_MouseUp);
            // 
            // btnAddMedia
            // 
            this.btnAddMedia.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddMedia.Location = new System.Drawing.Point(1420, 0);
            this.btnAddMedia.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddMedia.Name = "btnAddMedia";
            this.btnAddMedia.Size = new System.Drawing.Size(63, 212);
            this.btnAddMedia.TabIndex = 2;
            this.btnAddMedia.Text = "添加媒体文件";
            this.btnAddMedia.UseVisualStyleBackColor = true;
            this.btnAddMedia.Click += new System.EventHandler(this.btnAddMedia_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemRemove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(117, 34);
            // 
            // menuItemRemove
            // 
            this.menuItemRemove.Name = "menuItemRemove";
            this.menuItemRemove.Size = new System.Drawing.Size(116, 30);
            this.menuItemRemove.Text = "删除";
            this.menuItemRemove.Click += new System.EventHandler(this.MenuItemRemove_Click);
            // 
            // FrmReplierSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1483, 961);
            this.Controls.Add(this.dgvReplies);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FrmReplierSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义回复-设置";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReplies)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ImageList imageList;
        private DataGridView dgvReplies;
        private Panel pnlTop;
        private Panel pnlBottom;
        private Button btnAddMedia;
        private Label lblInfo;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem menuItemRemove;
        private ListView lvImages;
        private DataGridViewTextBoxColumn colMessage;
        private DataGridViewComboBoxColumn colMatchMode;
        private DataGridViewComboBoxColumn colTriggerMode;
        private DataGridViewTextBoxColumn colReplyValue;
        private DataGridViewTextBoxColumn colPriority;
        private DataGridViewCheckBoxColumn colReplyMode;
        private DataGridViewButtonColumn colRemove;
    }
}