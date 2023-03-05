using GreenOnions.PluginConfigEditor.ChatGPTClient;
using GreenOnions.PluginConfigEditor.CustomHttpApiInvoker;
using GreenOnions.PluginConfigEditor.GPT3Client;
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
                MessageBox.Show("请不要直接运行此程序", "提示");
                Environment.Exit(0);
            }

            switch (args[0])
            {
                case "GreenOnions.CustomHttpApiInvoker":
                    Application.Run(new FrmCustomHttpApiInvokerSetting(args[1]));
                    break;
                case "GreenOnions.KanCollectionTimeAnnouncer":
                    Application.Run(new FrmKanCollectionTimeAnnouncerSetting(args[1]));
                    break;
                case "GreenOnions.Replier":
                    Application.Run(new FrmReplierSetting(args[1]));
                    break;
                case "GreenOnions.GPT3Client":
                    Application.Run(new FrmGPT3ClientSetting(args[1]));
                    break;
                case "GreenOnions.ChatGPTClient":
                    Application.Run(new FrmChatGPTClientSetting(args[1]));
                    break;
            }
        }
    }
}