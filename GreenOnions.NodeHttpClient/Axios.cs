using System.Diagnostics;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;

namespace GreenOnions.NodeHttpClient
{
    [Obsolete("不知道怎么用它来下载文件，暂时不用了")]
    public class Axios //: IHttpClientSubstitutes
    {
        private string? _pluginPath;

        public string Name => "Node";

        public string Description => "使用Node替代系统发起SSL/TLS请求插件";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
        }

        public void OnDisconnected()
        {
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
            Console.OutputEncoding = Encoding.UTF8;
            Init();
        }

        public string GetAsString(string url)
        {
            string file = Path.Combine(_pluginPath, "node", "axiosClient.js");
            File.WriteAllText(file, @"const axios = require('axios');
axios.get('" + url + @"').then(res => {
    var json = JSON.stringify(res.data);
    console.log(json);
  }).catch(err => {
    console.log(err.message);
  });");
            return WriteRead("axiosClient.js");
        }

        private void Init()
        {
            string path = Path.Combine(_pluginPath, "node");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(Path.Combine(_pluginPath, "node", "node_modules", "axios")))
                Write("npm install axios --save");
        }

        private Process CreateCmdProcess()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.Combine(_pluginPath, "node");
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            return proc;
        }

        private string WriteRead(string executeFileName)
        {
            using (Process pc = CreateCmdProcess())
            {
                pc.StandardInput.WriteLine("node.exe " + executeFileName);
                pc.StandardInput.WriteLine("exit");
                pc.StandardInput.AutoFlush = true;
                bool bStart = false;
                StringBuilder result = new StringBuilder();
                string? line;
                do
                {
                    line = pc.StandardOutput.ReadLine();
                    if (bStart && line is not null && line.EndsWith("exit"))
                        break;
                    if (bStart)
                        result.AppendLine(line);
                    if (!bStart && line is not null && line.EndsWith(executeFileName))
                        bStart = true;
                } while (line is not null);
                pc.WaitForExit();
                pc.Close();
                return result.ToString();
            }
        }
        private void Write(string executeFileName)
        {
            using (Process pc = CreateCmdProcess())
            {
                pc.StandardInput.WriteLine("node.exe " + executeFileName);
                pc.StandardInput.WriteLine("exit");
                pc.StandardInput.AutoFlush = true;
                pc.WaitForExit();
                pc.Close();
            }
        }
    }
}