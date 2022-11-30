using System.ComponentModel;

namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class FrmSettings : Form
    {
        private readonly string _path;
        private readonly MainConfig _config;
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
            for (int i = 0; i < _config.ApiConfig.Count; i++)
            {
                CtrlListItem ctrl = new(_path, _config.ApiConfig[i], DeleteItemControl, HasSameCmd);
                pnlConfigList.Controls.Add(ctrl);
            }
            pnlConfigList.Controls.Add(btnAddConfig);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _config.HelpCmd = txbHelpCmd.Text;
        }

        private bool HasSameCmd(HttpApiConfig config) => _config.ApiConfig.Any(c => c != config && c.Cmd == config.Cmd);

        private void DeleteItemControl(CtrlListItem item)
        {
            _config.ApiConfig.RemoveAll(c => c == item.Config);
            pnlConfigList.Controls.Remove(item);
        }

        private void BtnAddConfig_Click(object sender, EventArgs e)
        {
            FrmEditor frmEditor = new(_path);
            frmEditor.ShowDialog();
            if (!string.IsNullOrEmpty(frmEditor.Config.Cmd))
            {
                if (HasSameCmd(frmEditor.Config))
                    MessageBox.Show($"添加失败，命令\"{frmEditor.Config.Cmd}\"已存在，请更换命令或改用编辑功能。","错误");
                else
                {
                    _config.ApiConfig.Add(frmEditor.Config);

                    pnlConfigList.Controls.Remove(btnAddConfig);
                    CtrlListItem ctrl = new(_path, frmEditor.Config, DeleteItemControl, HasSameCmd);
                    pnlConfigList.Controls.Add(ctrl);
                    pnlConfigList.Controls.Add(btnAddConfig);
                }
            }
        }
    }
}
