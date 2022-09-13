using System.ComponentModel;

namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    internal partial class FrmSettings : Form
    {
        private AnnounceSetting Settings { get; set; }
        private Task<List<string>> _getKanGrilNameListTask;
        private Dictionary<int, CheckBox> timeCheckboxes = new Dictionary<int, CheckBox>();
        internal FrmSettings(AnnounceSetting originalSettings)
        {
            Settings = originalSettings;
            InitializeComponent();
            cboDesignatedKanGirl.SelectedIndex = 0;
            _getKanGrilNameListTask = MoeGirlHelper.GetKanGrilNameList();

            for (int i = 0; i < 24; i++)
            {
                CheckBox chkTime = new CheckBox();
                chkTime.AutoSize = true;
                string hour = i.ToString().PadLeft(2, '0');
                chkTime.Name = hour;
                chkTime.Text = $"{hour}:00";
                timeCheckboxes.Add(i, chkTime);
                pnlDesignatedTime.Controls.Add(chkTime);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            rdoDesignateGroup.Checked = Settings.DesignateGroup;
            rdoAllGroup.Checked = !Settings.DesignateGroup;
            for (int i = 0; i < Settings.DesignatedGroups.Count; i++)
                txbDesignatedGroups.AppendText($"{Settings.DesignatedGroups[i]}\r\n");

            rdoDesignateKanGirl.Checked = Settings.DesignateKanGirl;
            rdoRandomKanGirl.Checked = !Settings.DesignateKanGirl;

            for (int i = 0; i < Settings.DesignatedTime.Count; i++)
                timeCheckboxes[Settings.DesignatedTime[i]].Checked = true;

            chkSendJapaneseText.Checked = Settings.SendJapaneseText;
            chkSendChineseText.Checked = Settings.SendChineseText;

            SetControlEnabled();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_getKanGrilNameListTask.IsCompleted && !_getKanGrilNameListTask.IsFaulted && !_getKanGrilNameListTask.IsCanceled)
                await _getKanGrilNameListTask;

            if (_getKanGrilNameListTask.IsCompleted)
            {
                if (_getKanGrilNameListTask.Result == null)
                {
                    Clipboard.SetText("https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘");
                    MessageBox.Show("获取舰娘列表失败, 需要人机验证, \r\n请使用浏览器打开 https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘 \r\n进行滑动验证后重启葱葱(地址已拷贝到剪贴板)");
                    return;
                }

                cboDesignatedKanGirl.DataSource = _getKanGrilNameListTask.Result;
                cboDesignatedKanGirl.SelectedIndex = _getKanGrilNameListTask.Result.IndexOf(Settings.DesignatedKanGirl);
                if (cboDesignatedKanGirl.SelectedIndex == -1)
                    cboDesignatedKanGirl.SelectedIndex = 0;

                SetControlEnabled();
            }
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_getKanGrilNameListTask.IsCompleted && !_getKanGrilNameListTask.IsFaulted && !_getKanGrilNameListTask.IsCanceled)
                await _getKanGrilNameListTask;

            if (_getKanGrilNameListTask.IsCompleted)
            {
                SetControlEnabled();

                Settings.DesignateGroup = rdoDesignateGroup.Checked;
                Settings.DesignatedGroups = new List<long>();
                foreach (string strGroup in txbDesignatedGroups.Text.Split("\r\n"))
                {
                    if (long.TryParse(strGroup, out long lGroup))
                        Settings.DesignatedGroups.Add(lGroup);
                }
                Settings.DesignateKanGirl = rdoDesignateKanGirl.Checked;
                Settings.DesignatedKanGirl = cboDesignatedKanGirl.Text;
                Settings.DesignatedTime = new List<int>();
                foreach (KeyValuePair<int, CheckBox> item in timeCheckboxes)
                {
                    if (item.Value.Checked)
                        Settings.DesignatedTime.Add(item.Key);
                }
                Settings.SendJapaneseText = chkSendJapaneseText.Checked;
                Settings.SendChineseText = chkSendChineseText.Checked;
            }
        }

        private void rdo_CheckedChanged(object sender, EventArgs e) => SetControlEnabled();

        private void SetControlEnabled()
        {
            txbDesignatedGroups.Enabled = rdoDesignateGroup.Checked;
            txbDesignatedGroups.Enabled = !rdoAllGroup.Checked;
            if (_getKanGrilNameListTask.IsCompleted)
            {
                cboDesignatedKanGirl.Enabled = rdoDesignateKanGirl.Checked;
                cboDesignatedKanGirl.Enabled = !rdoRandomKanGirl.Checked;
            }
        }
    }
}
