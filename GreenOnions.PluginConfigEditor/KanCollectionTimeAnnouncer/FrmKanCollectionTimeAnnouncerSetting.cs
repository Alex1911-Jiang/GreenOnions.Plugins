using System.ComponentModel;
using System.Diagnostics;
using GreenOnions.KanCollectionTimeAnnouncer;
using HtmlAgilityPack;
using Newtonsoft.Json;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace GreenOnions.PluginConfigEditor.KanCollectionTimeAnnouncer
{
    internal partial class FrmKanCollectionTimeAnnouncerSetting : Form
    {
        private readonly string _configDirect;
        private readonly KanCollectionConfig? _config;
        private readonly Dictionary<int, CheckBox> _timeCheckboxes = new Dictionary<int, CheckBox>();

        internal FrmKanCollectionTimeAnnouncerSetting(string configDirect)
        {
            _configDirect = configDirect;
            _config = ConfigLoader.LoadConfig<KanCollectionConfig>(_configDirect);
            InitializeComponent();
            cboDesignatedKanGirl.SelectedIndex = 0;

            for (int i = 0; i < 24; i++)
            {
                CheckBox chkTime = new CheckBox();
                chkTime.AutoSize = true;
                string hour = i.ToString().PadLeft(2, '0');
                chkTime.Name = hour;
                chkTime.Text = $"{hour}:00";
                _timeCheckboxes.Add(i, chkTime);
                pnlDesignatedTime.Controls.Add(chkTime);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            rdoDesignateGroup.Checked = _config!.DesignateGroup;
            rdoAllGroup.Checked = !_config.DesignateGroup;
            for (int i = 0; i < _config.DesignatedGroups.Count; i++)
                txbDesignatedGroups.AppendText($"{_config.DesignatedGroups[i]}\r\n");

            rdoDesignateKanGirl.Checked = _config.DesignateKanGirl;
            rdoRandomKanGirl.Checked = !_config.DesignateKanGirl;

            for (int i = 0; i < _config.DesignatedTime.Count; i++)
                _timeCheckboxes[_config.DesignatedTime[i]].Checked = true;

            chkSendJapaneseText.Checked = _config.SendJapaneseText;
            chkSendChineseText.Checked = _config.SendChineseText;

            SetControlEnabled();
        }

        private void CheckList(List<string>? kanGirlList)
        {
            if (kanGirlList == null)
            {
                Clipboard.SetText("https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘");
                MessageBox.Show("获取舰娘列表失败, 需要人机验证, \r\n请使用浏览器打开 https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘 \r\n进行滑动验证后重启葱葱(地址已拷贝到剪贴板)");
                return;
            }

            cboDesignatedKanGirl.DataSource = kanGirlList;
            cboDesignatedKanGirl.SelectedIndex = kanGirlList.IndexOf(_config!.DesignatedKanGirl!);
            if (cboDesignatedKanGirl.SelectedIndex == -1)
                cboDesignatedKanGirl.SelectedIndex = 0;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            List<string>? kanGirlList = await GetKanGrilNameListAsync(false);
            CheckList(kanGirlList);
            SetControlEnabled();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SetControlEnabled();

            if (cboDesignatedKanGirl.DataSource != null)
            {
                _config.DesignateGroup = rdoDesignateGroup.Checked;
                _config.DesignatedGroups = new List<long>();
                foreach (string strGroup in txbDesignatedGroups.Text.Split("\r\n"))
                {
                    if (long.TryParse(strGroup, out long lGroup))
                        _config.DesignatedGroups.Add(lGroup);
                }
                _config.DesignateKanGirl = rdoDesignateKanGirl.Checked;
                _config.DesignatedKanGirl = cboDesignatedKanGirl.Text;
                _config.DesignatedTime = new List<int>();
                foreach (KeyValuePair<int, CheckBox> item in _timeCheckboxes)
                {
                    if (item.Value.Checked)
                        _config.DesignatedTime.Add(item.Key);
                }
                _config.SendJapaneseText = chkSendJapaneseText.Checked;
                _config.SendChineseText = chkSendChineseText.Checked;
            }

            string jsonConfig = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_configDirect, jsonConfig);

            base.OnClosing(e);
        }

        private void rdo_CheckedChanged(object sender, EventArgs e) => SetControlEnabled();

        private void SetControlEnabled()
        {
            txbDesignatedGroups.Enabled = rdoDesignateGroup.Checked;
            txbDesignatedGroups.Enabled = !rdoAllGroup.Checked;
            cboDesignatedKanGirl.Enabled = rdoDesignateKanGirl.Checked;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            pnlDesignatedTime.Controls.OfType<CheckBox>().ToList().ForEach(c => c.Checked = chkSelectAll.Checked);
        }

        private async void btnRefreshKanGirlList_Click(object sender, EventArgs e)
        {
            cboDesignatedKanGirl.DataSource = null;
            cboDesignatedKanGirl.Items.AddRange(new[] { "获取中..." });
            cboDesignatedKanGirl.SelectedIndex = 0;
            cboDesignatedKanGirl.Enabled = false;

            List<string>? kanGirlList = await GetKanGrilNameListAsync(true);

            CheckList(kanGirlList);
            SetControlEnabled();
        }

        private void lnkFFMPEG_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "https://docs.go-cqhttp.org/guide/quick_start.html#安装-ffmpeg");
        }

        /// <summary>
        /// 请求萌娘百科获取舰娘名称列表
        /// </summary>
        /// <returns></returns>
        internal async Task<List<string>?> GetKanGrilNameListAsync(bool reget)
        {
            List<string>? kanGirlList;
            string jsonCache;

            string pluginPath = Path.GetDirectoryName(_configDirect)!;

            string kanGirlListFileName = Path.Combine(pluginPath, "KanGirlList.json");

            if (reget)
                File.Delete(kanGirlListFileName);
            if (!File.Exists(kanGirlListFileName) || new FileInfo(kanGirlListFileName).Length == 0)
                goto IL_GetMoeGirl;

            jsonCache = File.ReadAllText(kanGirlListFileName);
            if (string.IsNullOrEmpty(jsonCache))
                goto IL_GetMoeGirl;

            kanGirlList = JsonConvert.DeserializeObject<List<string>>(jsonCache);
            if (kanGirlList is null || kanGirlList.Count == 0)
                goto IL_GetMoeGirl;

            return kanGirlList;

        IL_GetMoeGirl:;
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
            string html;
            try
            {
                HttpResponseMessage response = await client.GetAsync(@"https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘");
                html = await response.Content.ReadAsStringAsync() + "</body></html>";
            }
            catch (OperationCanceledException)
            {
                return new List<string>();
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            //按图鉴编号排列的Table
            var tables = doc.DocumentNode.SelectNodes(@"/html/body/template[@id='MOE_SKIN_TEMPLATE_BODYCONTENT']/div[@id='mw-content-text']/div[@class='mw-parser-output']/table[@class='wikitable']");

            if (tables == null)
            {
                File.WriteAllText(Path.Combine(pluginPath, "获取舰娘失败.html"), html);
                MessageBox.Show("葱葱舰C报时插件获取舰娘列表失败，请手动打开 https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘 进行滑动验证，如果无需滑动验证，请打开插件目录下\"获取舰娘失败.html\"查看错误信息(地址已拷贝到剪贴板)");
                return null;  //滑动验证
            }

            HashSet<string> collectionNames = new HashSet<string>();
            foreach (HtmlNode table in tables)
            {
                foreach (HtmlNode tr in table.SelectNodes("tbody/tr"))
                {
                    foreach (string title in tr.SelectNodes("td/a").Select(a => a.Attributes["title"].Value))
                    {
                        if (title.Contains("舰队Collection:"))  //排除联动的舰娘
                            collectionNames.Add(title.Substring("舰队Collection:".Length));
                    }
                }
            }

            kanGirlList = collectionNames.ToList();
            jsonCache = JsonConvert.SerializeObject(kanGirlList, Formatting.Indented);
            File.WriteAllText(kanGirlListFileName, jsonCache);
            return kanGirlList;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            Announcer announcer = new Announcer();
            if (await announcer.Test(_configDirect))
            {
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show("下载失败，请打开萌娘百科（https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘）查看是否需要滑动验证");
            }
        }
    }
}