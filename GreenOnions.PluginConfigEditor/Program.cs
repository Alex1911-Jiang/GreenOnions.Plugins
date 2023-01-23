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
                MessageBox.Show("请不要直接运行此程序","提示");
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