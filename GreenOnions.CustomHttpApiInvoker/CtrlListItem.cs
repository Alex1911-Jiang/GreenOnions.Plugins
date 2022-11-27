namespace GreenOnions.CustomHttpApiInvoker
{
    public partial class CtrlListItem : UserControl
    {
        private string _path;
        private Config _config;
        public event Action DeleteEvent;

        public CtrlListItem(string path, Config config, Action deleteEvent)
        {
            _path = path;
            _config = config;
            DeleteEvent = deleteEvent;
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FrmEditor frmEditor = new FrmEditor(_path, _config);
            frmEditor.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteEvent?.Invoke();
        }
    }
}
