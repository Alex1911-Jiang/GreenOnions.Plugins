using System.Diagnostics;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.Interface.Subinterface;

namespace GreenOnions.NodeHttpClient
{
    public class Request : IHttpClientSubstitutes
    {
        private string? _pluginPath;

        public string Name => "Node";

        public string Description => "使用Node替代系统发起SSL/TLS请求插件";

        public async void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;
            Console.OutputEncoding = Encoding.UTF8;

            string path = Path.Combine(_pluginPath!, "node");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(Path.Combine(_pluginPath!, "node", "node_modules", "request")))
                await WriteAsync("npm install request --save");

            if (!Directory.Exists(Path.Combine(_pluginPath!, "node", "node_modules", "fs")))
                await WriteAsync("npm install fs --save");
        }

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {

        }

        public void OnDisconnected()
        {

        }

        public string GetAsString(string url)
        {
            CreateGetStringJs(url);
            return WriteRead("requestClient.js");
        }
        public Task<string> GetAsStringAsync(string url)
        {
            CreateGetStringJs(url);
            return WriteReadAsync("requestClient.js");
        }

        public byte[] GetAsByteArray(string url)
        {
            CreateGetFileJs(url);
            Write("requestClient.js");
            return File.ReadAllBytes(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public async Task<byte[]> GetAsByteArrayAsync(string url)
        {
            CreateGetFileJs(url);
            await WriteAsync("requestClient.js");
            return await File.ReadAllBytesAsync(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public Stream GetAsStream(string url)
        {
            return new MemoryStream(GetAsByteArray(url));
        }

        public async Task<Stream> GetAsStreamAsync(string url)
        {
            return new MemoryStream(await GetAsByteArrayAsync(url));
        }

        #region -- script --

        private void CreateGetStringJs(string url)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            File.WriteAllText(file, @"const request = require('request');
request('" + url + @"', { json: true }, (err, res, body) => {
  var json = '';
  if (err) { 
     json = JSON.stringify(err); 
  }
  else{
    json = JSON.stringify(body);
  }
  console.log(json);
});");
        }

        private void CreateGetFileJs(string url)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            File.WriteAllText(file, @"const request = require('request');
const fs = require('fs');
var stream = fs.createWriteStream('data.bin');
request('" + url + @"',(error, response, body)=>{
    console.log('end');
}).pipe(stream);");
            Write("requestClient.js");
        }

        #endregion -- script --

        #region -- CMD --

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

        private void Write(string script)
        {
            using (Process pc = CreateCmdProcess())
            {
                pc.StandardInput.WriteLine("node " + script);
                pc.StandardInput.WriteLine("exit");
                pc.StandardInput.AutoFlush = true;
                pc.WaitForExit();
                pc.Close();
            }
        }

        private async Task WriteAsync(string script)
        {
            using (Process pc = CreateCmdProcess())
            {
                await pc.StandardInput.WriteLineAsync("node " + script);
                await pc.StandardInput.WriteLineAsync("exit");
                pc.StandardInput.AutoFlush = true;
                await pc.WaitForExitAsync();
                pc.Close();
            }
        }

        private string WriteRead(string script)
        {
            using (Process pc = CreateCmdProcess())
            {
                pc.StandardInput.WriteLine("node " + script);
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
                    if (!bStart && line is not null && line.EndsWith(script))
                        bStart = true;
                } while (line is not null);
                pc.WaitForExit();
                pc.Close();
                return result.ToString();
            }
        }

        private async Task<string> WriteReadAsync(string script)
        {
            using (Process pc = CreateCmdProcess())
            {
                await pc.StandardInput.WriteLineAsync("node " + script);
                await pc.StandardInput.WriteLineAsync("exit");
                pc.StandardInput.AutoFlush = true;
                bool bStart = false;
                StringBuilder result = new StringBuilder();
                string? line;
                do
                {
                    line = await pc.StandardOutput.ReadLineAsync();
                    if (bStart && line is not null && line.EndsWith("exit"))
                        break;
                    if (bStart)
                        result.AppendLine(line);
                    if (!bStart && line is not null && line.EndsWith(script))
                        bStart = true;
                } while (line is not null);
                await pc.WaitForExitAsync();
                pc.Close();
                return result.ToString();
            }
        }

        #endregion -- CMD --
    }
}