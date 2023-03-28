namespace GreenOnions.PluginConfigEditor.NovelAiClient
{
    partial class FrmNovelAiEditParams
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
            this.txbParams = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txbParams
            // 
            this.txbParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbParams.Location = new System.Drawing.Point(0, 0);
            this.txbParams.Multiline = true;
            this.txbParams.Name = "txbParams";
            this.txbParams.Size = new System.Drawing.Size(458, 968);
            this.txbParams.TabIndex = 0;
            // 
            // FrmNovelAiEditParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 968);
            this.Controls.Add(this.txbParams);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNovelAiEditParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑请求参数";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txbParams;
    }
}