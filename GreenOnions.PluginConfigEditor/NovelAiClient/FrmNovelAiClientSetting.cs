using System.ComponentModel;
using GreenOnions.NovelAiClient;
using GreenOnions.PluginConfigs.NovelAiClient;
using Newtonsoft.Json;

namespace GreenOnions.PluginConfigEditor.NovelAiClient
{
    public partial class FrmNovelAiClientSetting : Form
    {
        private string _configDirect;
        private string _paramConfigDirect;
        private NovelAiConfig _config;
        public FrmNovelAiClientSetting(string configDirect, string paramConfigDirect)
        {
            _configDirect = configDirect;
            _paramConfigDirect = paramConfigDirect;
            _config = ConfigLoader.LoadConfig<NovelAiConfig>(_configDirect);

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txbCmd.Text = _config.Cmd;
            cboConnectFrontEnd.SelectedIndex = (int)_config.ConnectFrontEnd;
            txbURL.Text = _config.URL;
            txbfn_Index.Text = _config.fn_index.ToString();
            txbPromptIndex.Text = _config.PromptIndex.ToString();
            txbUndesiredIndex.Text = _config.UndesiredIndex.ToString();
            txbStartDrawMessage.Text = _config.StartDrawMessage;
            txbDrawEndMessage.Text = _config.DrawEndMessage;
            txbDrawErrorMessage.Text = _config.DrawErrorMessage;
            txbRevokeSecond.Text = _config.RevokeSecond.ToString();
            txbDefaultPrompt.Text = _config.DefaultPrompt;
            txbDefaultUndesired.Text = _config.DefaultUndesired;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _config.Cmd = txbCmd.Text;
            _config.ConnectFrontEnd = (FrontEnd)cboConnectFrontEnd.SelectedIndex;
            _config.URL = txbURL.Text;
            _config.fn_index = Convert.ToInt32(txbfn_Index.Text);
            _config.PromptIndex = Convert.ToInt32(txbPromptIndex.Text);
            _config.UndesiredIndex = Convert.ToInt32(txbUndesiredIndex.Text);
            _config.StartDrawMessage = txbStartDrawMessage.Text;
            _config.DrawEndMessage = txbDrawEndMessage.Text;
            _config.DrawErrorMessage = txbDrawErrorMessage.Text;
            _config.RevokeSecond = Convert.ToInt32(txbRevokeSecond.Text);
            _config.DefaultPrompt = txbDefaultPrompt.Text;
            _config.DefaultUndesired = txbDefaultUndesired.Text;

            string strConfig = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_configDirect, strConfig);
        }

        private void btnEditParams_Click(object sender, EventArgs e)
        {
            new FrmNovelAiEditParams(_paramConfigDirect).ShowDialog();
        }

        private async void btnTestInvoke_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txbfn_Index.Text, out int fn_index))
            {
                MessageBox.Show("请输入正确的fn_index。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            if (!int.TryParse(txbPromptIndex.Text, out int promptIndex))
            {
                MessageBox.Show("请输入正确的提示词索引。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            if (!int.TryParse(txbUndesiredIndex.Text, out int undesiredIndex))
            {
                MessageBox.Show("请输入正确的屏蔽词索引。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            try
            {
                Test_WebUIClient test_WebUIClient = new Test_WebUIClient(txbURL.Text, fn_index, promptIndex, undesiredIndex);
                string strDatas = File.ReadAllText(_paramConfigDirect);

                List<string> prompts = txbDefaultPrompt.Text.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                prompts.Add(txbTestPrompt.Text);
                string prompt = string.Join(',', prompts);

                byte[] img = await test_WebUIClient.PostAsync(strDatas, prompt, txbUndesiredIndex.Text);
                picTest.Image = Image.FromStream(new MemoryStream(img));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"请求失败，{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }
    }
}
