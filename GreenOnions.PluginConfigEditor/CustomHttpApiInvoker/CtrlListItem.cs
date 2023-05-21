using GreenOnions.CustomHttpApiInvoker;

namespace GreenOnions.PluginConfigEditor.CustomHttpApiInvoker
{
    internal partial class CtrlListItem : UserControl
    {
        public HttpApiItemConfig Config { get; private set; }

        public event Action<CtrlListItem> DeleteEvent;
        public event Func<HttpApiItemConfig, bool> CheckHasSameCmd;

        public CtrlListItem(HttpApiItemConfig config, Action<CtrlListItem> deleteEvent, Func<HttpApiItemConfig, bool> checkHasSameCmd)
        {
            Config = config;
            DeleteEvent = deleteEvent;
            CheckHasSameCmd = checkHasSameCmd;
            InitializeComponent();
            lblUrl.Text = Config.Url;
            lblCmd.Text = config.Cmd;
            lbllblRemark.Text = Config.Remark;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            HttpApiItemConfig editedConfig = Config;
        IL_Retry:;
            FrmCustomHttpApiInvokerEditor frmEditor = new(editedConfig);
            frmEditor.ShowDialog();
            editedConfig = frmEditor.Config;
            if (string.IsNullOrWhiteSpace(editedConfig.Cmd))
            {
                if (MessageBox.Show("命令不能为空，点击确定删除当前记录，点击取消重新编辑", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    DeleteEvent(this);
                    return;
                }
                else
                    goto IL_Retry;
            }
            if (CheckHasSameCmd(editedConfig))
            {
                if (MessageBox.Show("命令已存在，点击确定重新编辑，点击取消放弃保存", "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    goto IL_Retry;
                return;
            }

            Config = frmEditor.Config!;
            
            lblUrl.Text = Config.Url;
            lbllblRemark.Text = Config.Remark;
            lblCmd.Text = Config.Cmd;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteEvent(this);
        }
    }
}
