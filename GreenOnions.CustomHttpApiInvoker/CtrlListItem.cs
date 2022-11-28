namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class CtrlListItem : UserControl
    {
        private string _path;
        public string Cmd { get; private set; }
        public HttpApiConfig Config { get; private set; }

        public event Action<CtrlListItem> DeleteEvent;

        public CtrlListItem(string path, string cmd, HttpApiConfig config, Action<CtrlListItem> deleteEvent)
        {
            _path = path;
            Cmd = cmd;
            Config = config;
            DeleteEvent = deleteEvent;
            InitializeComponent();
            lblUrl.Text = Config.Url;
            lbllblRemark.Text = Config.Remark;
            lblCmd.Text = cmd;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FrmEditor frmEditor = new FrmEditor(_path, Cmd, Config);
            frmEditor.ShowDialog();
            Cmd = frmEditor.Cmd!;
            Config = frmEditor.Config!;

            lblUrl.Text = Config.Url;
            lbllblRemark.Text = Config.Remark;
            lblCmd.Text = Cmd;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteEvent?.Invoke(this);
        }
    }
}
