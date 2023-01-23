using GreenOnions.PluginConfigEditor.CustomHttpApiInvoker;

namespace GreenOnions.PluginConfigEditor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length < 2)
            {
                MessageBox.Show("�벻Ҫֱ�����д˳���","��ʾ");
                Environment.Exit(0);
            }

            switch (args[0])
            {
                case "GreenOnions.CustomHttpApiInvoker":
                    Application.Run(new FrmCustomHttpApiInvokerSettings(args[1]));
                    break;
            }
        }
    }
}