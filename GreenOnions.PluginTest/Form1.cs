using System.Diagnostics;

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
            string pluginName = "GreenOnions.KanCollectionTimeAnnouncer";
            string editorDirect = Path.Combine(new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.Parent.FullName, "GreenOnions.PluginConfigEditor", "bin", "Debug", "net6.0-windows", "GreenOnions.PluginConfigEditor.exe");
            string configDirect = Path.Combine(new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.Parent.FullName, pluginName, "bin", "Debug", "net6.0-windows", "config.json");
            Process.Start(editorDirect, new[] { pluginName, configDirect }).WaitForExit();
        }
    }
}