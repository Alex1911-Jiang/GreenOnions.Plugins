using System.ComponentModel;

namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class FrmSettings : Form
    {
        private string _path;
        private MainConfig _config;
        private int _itemCtrlWidth = 592;
        public FrmSettings(string path, MainConfig config)
        {
            _path = path;
            _config = config;
            InitializeComponent();
            txbHelpCmd.Text = config.HelpCmd;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            pnlConfigList.Controls.Remove(btnAddConfig);
            foreach (var item in _config.ApiConfig)
            {
                CtrlListItem ctrl = new CtrlListItem(_path, item.Key, item.Value, DeleteItemControl);
                pnlConfigList.Controls.Add(ctrl);
            }
            pnlConfigList.Controls.Add(btnAddConfig);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _config.HelpCmd = txbHelpCmd.Text;
        }

        private void DeleteItemControl(CtrlListItem item)
        {
            _config.ApiConfig.Remove(item.Cmd);
            Controls.Remove(item);
        }

        private void btnAddConfig_Click(object sender, EventArgs e)
        {
            FrmEditor frmEditor = new FrmEditor(_path);
            frmEditor.ShowDialog();
            if (!string.IsNullOrEmpty(frmEditor.Cmd))
            {
                if (_config.ApiConfig.ContainsKey(frmEditor.Cmd))
                    MessageBox.Show($"添加失败，命令\"{frmEditor.Cmd}\"已存在，请更换命令或改用编辑功能。","错误");
                else
                {
                    _config.ApiConfig.Add(frmEditor.Cmd, frmEditor.Config);

                    pnlConfigList.Controls.Remove(btnAddConfig);
                    CtrlListItem ctrl = new CtrlListItem(_path, frmEditor.Cmd, frmEditor.Config, DeleteItemControl);
                    pnlConfigList.Controls.Add(ctrl);
                    pnlConfigList.Controls.Add(btnAddConfig);
                }
            }
        }

        private void pnlConfigList_SizeChanged(object sender, EventArgs e) => ComputeRssItemWidth();
        private void pnlConfigList_ControlChanged(object sender, ControlEventArgs e) => ComputeRssItemWidth();

        private void ComputeRssItemWidth()
        {
            _itemCtrlWidth = pnlConfigList.Controls.Count * pnlConfigList.Height + pnlConfigList.Controls.Count * pnlConfigList.Margin.Top * 2 + pnlConfigList.Margin.Top - 1 > pnlConfigList.Height ? pnlConfigList.Width - 25 : pnlConfigList.Width - 8;
            foreach (Control item in pnlConfigList.Controls)
                item.Width = _itemCtrlWidth;
        }
    }
}
