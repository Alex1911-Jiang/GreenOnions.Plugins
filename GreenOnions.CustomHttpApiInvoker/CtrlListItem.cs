namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class CtrlListItem : UserControl
    {
        private string _path;
        public HttpApiConfig Config { get; private set; }

        public event Action<CtrlListItem> DeleteEvent;

        public CtrlListItem(string path, HttpApiConfig config, Action<CtrlListItem> deleteEvent)
        {
            _path = path;
            Config = config;
            DeleteEvent = deleteEvent;
            InitializeComponent();
            lblUrl.Text = Config.Url;
            lblCmd.Text = config.Cmd;
            lbllblRemark.Text = Config.Remark;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FrmEditor frmEditor = new FrmEditor(_path, Config);
            frmEditor.ShowDialog();
            Config = frmEditor.Config!;

            lblUrl.Text = Config.Url;
            lbllblRemark.Text = Config.Remark;
            lblCmd.Text = Config.Cmd;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteEvent?.Invoke(this);
        }
    }
}
