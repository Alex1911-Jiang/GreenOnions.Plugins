using System;
using System.Diagnostics;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.Interface.Subinterface;

namespace GreenOnions.NodeHttpClient
{
    public class Axios : IHttpClientSubstitutes
    {
        private string? _pluginPath;

        public string Name => "Node";

        public string Description => "使用Node替代系统发起SSL/TLS请求插件";

        public string GetAsString(string url)
        {
            WriteUrl(url);
            return InvokeByCmd("node.exe httpClient.js");
        }

        public async Task<string> GetAsStringAsync(string url)
        {
            WriteUrl(url);
            return await InvokeByCmdAsync("node.exe httpClient.js");
        }

        private void WriteUrl(string url)
        {
            string file = Path.Combine(Environment.CurrentDirectory, "node", "httpClient.js");
            File.WriteAllText(file, @"const axios = require('axios');
axios.get('" +
url +
@"').then(res => {
    var json = JSON.stringify(res.data);
    console.log(json);
  }).catch(err => {
    console.log(err);
  });");
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {
            if (true)  //TODO:设置中添加是否启用的选项
            {
                Init();
            }
        }

        public void OnDisconnected()
        {
        }

        public void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
            Console.OutputEncoding = Encoding.UTF8;
        }

        private async void Init()
        {
            string path = Path.Combine(_pluginPath!, "node");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string axiosJsFile = Path.Combine(_pluginPath!, "node", "node_modules","axios","lib", "axios.js");
            if (!File.Exists(axiosJsFile))
                await InvokeByCmdAsync("npm install axios --save");
        }

        private Process CreateCmdProcess()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.Combine(_pluginPath!, "node");
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            return proc;
        }

        private string InvokeByCmd(params string[] command)
        {
            using (Process pc = CreateCmdProcess())
            {
                foreach (string com in command)
                    pc.StandardInput.WriteLine(com);
                pc.StandardInput.WriteLine("exit");
                pc.StandardInput.AutoFlush = true;
                string outPut = pc.StandardOutput.ReadToEnd();    
                pc.WaitForExit();
                pc.Close();
                return outPut;
            }
        }

        private async Task<string> InvokeByCmdAsync(params string[] command)
        {
            using (Process pc = CreateCmdProcess())
            {
                foreach (string com in command)
                    await pc.StandardInput.WriteLineAsync(com);
                await pc.StandardInput.WriteLineAsync("exit");
                pc.StandardInput.AutoFlush = true;
                string outPut = await pc.StandardOutput.ReadToEndAsync();     
                await pc.WaitForExitAsync();
                pc.Close();
                return outPut;
            }
        }
    }
}