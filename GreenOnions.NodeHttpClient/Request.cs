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

        public string Name => "Http替代库";

        public string Description => "使用Node替代系统发起SSL/TLS请求插件";

        public async void OnLoad(string pluginPath, IBotConfig config)
        {
            _pluginPath = pluginPath;

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

        #region -- Get --

        public string GetString(string url, IDictionary<string, string>? header = null)
        {
            CreateGetStringJs(url, header);
            return WriteRead("node requestClient.js");
        }
        public Task<string> GetStringAsync(string url, IDictionary<string, string>? header = null)
        {
            CreateGetStringJs(url, header);
            return WriteReadAsync("node requestClient.js");
        }

        public byte[] GetByteArray(string url, IDictionary<string, string>? header = null)
        {
            CreateGetFileJs(url, header);
            Write("node requestClient.js");
            return File.ReadAllBytes(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public async Task<byte[]> GetByteArrayAsync(string url, IDictionary<string, string>? header = null)
        {
            CreateGetFileJs(url, header);
            await WriteAsync("node requestClient.js");
            return await File.ReadAllBytesAsync(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public Stream GetStream(string url, IDictionary<string, string>? header = null)
        {
            return new MemoryStream(GetByteArray(url, header));
        }

        public async Task<Stream> GetStreamAsync(string url, IDictionary<string, string>? header = null)
        {
            return new MemoryStream(await GetByteArrayAsync(url, header));
        }

        #endregion -- Get --

        #region -- Post --
        public string PostString(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostStringJs(url, jsonBody, header);
            return WriteRead("node requestClient.js");
        }

        public Task<string> PostStringAsync(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostStringJs(url, jsonBody, header);
            return WriteReadAsync("node requestClient.js");
        }

        public byte[] PostByteArray(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostFileJs(url, jsonBody, header);
            Write("node requestClient.js");
            return File.ReadAllBytes(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public async Task<byte[]> PostByteArrayAsync(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostFileJs(url, jsonBody, header);
            await WriteAsync("node requestClient.js");
            return await File.ReadAllBytesAsync(Path.Combine(_pluginPath!, "node", "data.bin"));
        }

        public Stream PostStream(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            return new MemoryStream(PostByteArray(url, jsonBody, header));
        }

        public async Task<Stream> PostStreamAsync(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            return new MemoryStream(await PostByteArrayAsync(url, jsonBody, header));
        }
        #endregion -- Post --

        #region -- script --

        private void CreateGetStringJs(string url, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            string options = CreateOptionsString(false, header, null);
            File.WriteAllText(file, $@"const request = require('request');
request(encodeURI('{url}'),{options}(err,resp,body) => {{
  var json = '';
  if (err) {{ 
     json = err; 
  }}
  else{{
    json = body;
  }}
  console.log(json);
}});");
        }

        private void CreateGetFileJs(string url, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            string options = CreateOptionsString(false, header, null);
            File.WriteAllText(file, $@"const request = require('request');
const fs = require('fs');
var stream = fs.createWriteStream('data.bin');
request(encodeURI('{url}'),{options}(err,resp,body) => {{console.log('end')}}).pipe(stream);");
            Write("requestClient.js");
        }

        private void CreatePostStringJs(string url, string jsonBody, IDictionary<string,string>? header)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            string options = CreateOptionsString(true, header, jsonBody);
            File.WriteAllText(file, $@"const request = require('request');
request.post(encodeURI('{url}'),{options}(err,resp,body) => {{
  var json = '';
  if (err) {{ 
     json = err; 
  }}
  else{{
    json = body;
  }}
  console.log(json);
}});");
        }

        private void CreatePostFileJs(string url, string jsonBody, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "node", "requestClient.js");
            string options = CreateOptionsString(false, header, jsonBody);
            File.WriteAllText(file, $@"const request = require('request');
const fs = require('fs');
var stream = fs.createWriteStream('data.bin');
request.post(encodeURI('{url}'),{options}(err,resp,body) => {{console.log('end')}}).pipe(stream);");
        }

        private string CreateOptionsString(bool json, IDictionary<string, string>? header, string? jsonBody)
        {
            List<string> options = new List<string>();
            if (json)
                options.Add("json:true");
            if (header is not null)
                options.Add(HeaderDictionaryToString(header));
            if (!string.IsNullOrWhiteSpace(jsonBody))
                options.Add($"body:{jsonBody}");
            string strOptions = string.Join(',', options);
            if (string.IsNullOrWhiteSpace(strOptions))
                return string.Empty;
            return $"{{{strOptions}}},";
        }

        private string HeaderDictionaryToString(IDictionary<string, string>? header)
        {
            string strHeader = string.Empty;
            if (header is not null)
                strHeader = $"headers:{{{string.Join(',', header.Select(kv => $"'{kv.Key}':'{kv.Value}'"))}}}";
            return strHeader;
        }

        #endregion -- script --

        #region -- CMD --

        private Process CreateCmdProcess()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.Combine(_pluginPath!, "node");
            proc.StartInfo.StandardOutputEncoding = Encoding.UTF8;
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
                pc.StandardInput.WriteLine(script);
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
                await pc.StandardInput.WriteLineAsync(script);
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
                pc.StandardInput.WriteLine(script);
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
                await pc.StandardInput.WriteLineAsync(script);
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