using System.ComponentModel;
using GreenOnions.PluginConfigs.CustomHttpApiInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.PluginConfigEditor.CustomHttpApiInvoker
{
    internal partial class FrmCustomHttpApiInvokerSettings : Form
    {
        private readonly string _configDirect;
        private readonly HttpApiConfig _config;

        public FrmCustomHttpApiInvokerSettings(string configDirect)
        {
            _configDirect = configDirect;

            string strConfigJson;
            if (!File.Exists(_configDirect) || string.IsNullOrWhiteSpace(strConfigJson = File.ReadAllText(_configDirect)))
            {
                MessageBox.Show($"配置文件 {_configDirect} 不存在，即将重新生成");
                _config = new HttpApiConfig();
                strConfigJson = JsonConvert.SerializeObject(_config, Formatting.Indented, new StringEnumConverter());
                File.WriteAllText(_configDirect, strConfigJson);
            }
            else
            {
                HttpApiConfig? config = JsonConvert.DeserializeObject<HttpApiConfig>(strConfigJson);
                if (config is null)
                {
                    MessageBox.Show("配置文件读取失败，重新生成");
                    config = new HttpApiConfig();
                }
                _config = config;
            }
            InitializeComponent();
            txbHelpCmd.Text = _config.HelpCmd;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            pnlConfigList.Controls.Remove(btnAddConfig);
            for (int i = 0; i < _config.ApiConfig.Count; i++)
            {
                CtrlListItem ctrl = new(_config.ApiConfig[i], DeleteItemControl, HasSameCmd);
                pnlConfigList.Controls.Add(ctrl);
            }
            pnlConfigList.Controls.Add(btnAddConfig);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _config.HelpCmd = txbHelpCmd.Text;
            File.WriteAllText(_configDirect, JsonConvert.SerializeObject(_config, Formatting.Indented, new StringEnumConverter()));
        }

        private bool HasSameCmd(HttpApiItemConfig config) => _config.ApiConfig.Any(c => c != config && c.Cmd == config.Cmd);

        private void DeleteItemControl(CtrlListItem item)
        {
            _config.ApiConfig.RemoveAll(c => c == item.Config);
            pnlConfigList.Controls.Remove(item);
        }

        private void BtnAddConfig_Click(object sender, EventArgs e)
        {
            FrmCustomHttpApiInvokerEditor frmEditor = new();
            frmEditor.ShowDialog();
            if (!string.IsNullOrEmpty(frmEditor.Config.Cmd))
            {
                if (HasSameCmd(frmEditor.Config))
                    MessageBox.Show($"添加失败，命令\"{frmEditor.Config.Cmd}\"已存在，请更换命令或改用编辑功能。","错误");
                else
                {
                    _config.ApiConfig.Add(frmEditor.Config);

                    pnlConfigList.Controls.Remove(btnAddConfig);
                    CtrlListItem ctrl = new( frmEditor.Config, DeleteItemControl, HasSameCmd);
                    pnlConfigList.Controls.Add(ctrl);
                    pnlConfigList.Controls.Add(btnAddConfig);
                }
            }
        }
    }
}
