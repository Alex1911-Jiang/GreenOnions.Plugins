using System.ComponentModel;
using System.Diagnostics;
using GreenOnions.ChatGPTClient;
using Newtonsoft.Json;

namespace GreenOnions.PluginConfigEditor.ChatGPTClient
{
    public partial class FrmChatGPTClientSetting : Form
    {
        private string _configDirect;
        private string _pluginPath;
        private ChatGPTClientConfig _config;
        public FrmChatGPTClientSetting(string configDirect)
        {
            _configDirect = configDirect;
            _config = ConfigLoader.LoadConfig<ChatGPTClientConfig>(_configDirect);
            string pluginPath = Path.GetDirectoryName(configDirect)!;
            _pluginPath = pluginPath;
            if (!Directory.Exists(_pluginPath))
                Directory.CreateDirectory(_pluginPath);

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txbApiKey.Text = _config.APIkey;
            cboModel.SelectedIndex = cboModel.Items.IndexOf(_config.Model);
            txbTemperature.Text = _config.Temperature.ToString();
            chkUseProxy.Checked = _config.UseProxy;
            chkRequestByPlugin.Checked = _config.RequestByPlugin;
            chkSendMessageByReply.Checked = _config.SendMessageByReply;
            chkClearContextAfterExit.Checked = _config.ClearContextAfterExit;
            chkRemoveMarkdownExpression.Checked = _config.RemoveMarkdownExpression;
            cboContext.SelectedIndex = (int)_config.Context;
            txbStartCommands.Text = string.Join("\r\n", _config.StartCommands ?? new string[0]);
            txbExitCommands.Text = string.Join("\r\n", _config.ExitCommands ?? new string[0]);
            txbClearContextCommands.Text = string.Join("\r\n", _config.ClearContextCommands ?? new string[0]);
            txbChatStartMessage.Text = _config.ChatStartMessage;
            txbExitChatMessage.Text = _config.ExitChatMessage;
            txbTimeOutMessage.Text = _config.TimeOutMessage;
            txbCleanContextMessage.Text = _config.CleanContextMessage;
            txbErrorMessage.Text = _config.ErrorMessage;
            txbTimeOutSecond.Text = _config.TimeOutSecond.ToString();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _config.APIkey = txbApiKey.Text;
            if (!float.TryParse(txbTemperature.Text, out float fTemperature) || fTemperature < 0 || fTemperature > 2)
            {
                MessageBox.Show("采样温度不合法，取值范围应在0-2之间（小数点后1位）", "错误");
                e.Cancel = true;
                return;
            }
            _config.Temperature = fTemperature;
            _config.Model = cboModel.Text;
            _config.UseProxy = chkUseProxy.Checked;
            _config.RequestByPlugin = chkRequestByPlugin.Checked;
            _config.SendMessageByReply = chkSendMessageByReply.Checked;
            _config.ClearContextAfterExit = chkClearContextAfterExit.Checked;
            _config.RemoveMarkdownExpression = chkRemoveMarkdownExpression.Checked;
            _config.Context = (ContextSetting)cboContext.SelectedIndex;
            _config.StartCommands = txbStartCommands.Text.Split("\r\n");
            _config.ExitCommands = txbExitCommands.Text.Split("\r\n");
            _config.ClearContextCommands = txbClearContextCommands.Text.Split("\r\n");
            _config.ChatStartMessage = txbChatStartMessage.Text;
            _config.ExitChatMessage = txbExitChatMessage.Text;
            _config.TimeOutMessage = txbTimeOutMessage.Text;
            _config.CleanContextMessage = txbCleanContextMessage.Text;
            _config.ErrorMessage = txbErrorMessage.Text;

            if (!uint.TryParse(txbTimeOutSecond.Text, out uint iTimeOutSecond) || iTimeOutSecond < 0)
            {
                MessageBox.Show($"超时时间不合法，取值范围应在0-{uint.MaxValue}之间", "错误");
                e.Cancel = true;
                return;
            }
            _config.TimeOutSecond = iTimeOutSecond;

            string strConfig = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_configDirect, strConfig);

            base.OnClosing(e);
        }

        private void lblCreateApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "https://beta.openai.com/account/api-keys");
        }

        private void lnkModel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "https://platform.openai.com/docs/models/gpt-3-5");
        }

        private void lnkTemperature_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "https://platform.openai.com/docs/api-reference/chat/create#chat/create-temperature");
        }
    }
}
