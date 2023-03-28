using System.ComponentModel;

namespace GreenOnions.PluginConfigEditor.NovelAiClient
{
    public partial class FrmNovelAiEditParams : Form
    {
        private string _paramConfigDirect;
        public FrmNovelAiEditParams(string paramConfigDirect)
        {
            _paramConfigDirect = paramConfigDirect;
            InitializeComponent();

            if (!File.Exists(_paramConfigDirect))
            {
                return;
            }

            txbParams.Text = File.ReadAllText(_paramConfigDirect);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            File.WriteAllText(_paramConfigDirect, txbParams.Text);
            base.OnClosing(e);
        }
    }
}
