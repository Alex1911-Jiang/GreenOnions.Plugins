using GreenOnions.PluginConfigEditor.CustomHttpApiInvoker;
using GreenOnions.PluginConfigEditor.KanCollectionTimeAnnouncer;
using GreenOnions.PluginConfigEditor.Replier;

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

            if (args.Length != 2)
            {
                MessageBox.Show("�벻Ҫֱ�����д˳���","��ʾ");
                Environment.Exit(0);
            }

            switch (args[0])
            {
                case "GreenOnions.CustomHttpApiInvoker":
                    Application.Run(new FrmCustomHttpApiInvokerSettings(args[1]));
                    break;
                case "GreenOnions.KanCollectionTimeAnnouncer":
                    Application.Run(new FrmKanCollectionTimeAnnouncerSettings(args[1]));
                    break;
                case "GreenOnions.Replier":
                    Application.Run(new FrmReplierSetting(args[1]));
                    break;
            }
        }
    }
}