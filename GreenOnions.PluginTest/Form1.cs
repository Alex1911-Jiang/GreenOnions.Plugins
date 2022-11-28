using GreenOnions.CustomHttpApiInvoker;

namespace GreenOnions.PluginTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Dictionary<string, HttpApiConfig> configs = new Dictionary<string, HttpApiConfig>();
            FrmSettings frmSettings = new FrmSettings("", configs);
            frmSettings.ShowDialog();
        }
    }
}