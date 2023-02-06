using System.Diagnostics;
using System.Text;
using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using GreenOnions.Interface.Subinterface;

namespace GreenOnions.PythonHttpClient
{
    public class Requests : IHttpClientSubstitutes
    {
        private string? _pluginPath;

        public string Name => "Http替代库";

        public string Description => "使用Python替代系统发起SSL/TLS请求插件";

        public async void OnLoad(string pluginPath, IBotConfig config)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _pluginPath = pluginPath;
            await WriteAsync("pip install requests");
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
            CreateGetStringPy(url, header);
            return WriteRead("python requestsClient.py");
        }
        public Task<string> GetStringAsync(string url, IDictionary<string, string>? header = null)
        {
            CreateGetStringPy(url, header);
            return WriteReadAsync("python requestsClient.py");
        }

        public byte[] GetByteArray(string url, IDictionary<string, string>? header = null)
        {
            CreateGetFilePy(url, header);
            Write("python requestsClient.py");
            return File.ReadAllBytes(Path.Combine(_pluginPath!, "data.bin"));
        }

        public async Task<byte[]> GetByteArrayAsync(string url, IDictionary<string, string>? header = null)
        {
            CreateGetFilePy(url, header);
            await WriteAsync("python requestsClient.py");
            return await File.ReadAllBytesAsync(Path.Combine(_pluginPath!, "data.bin"));
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
            CreatePostStringPy(url, jsonBody, header);
            return WriteRead("python requestsClient.py");
        }

        public Task<string> PostStringAsync(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostStringPy(url, jsonBody, header);
            return WriteReadAsync("python requestsClient.py");
        }

        public byte[] PostByteArray(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostFilePy(url, jsonBody, header);
            Write("python requestsClient.py");
            return File.ReadAllBytes(Path.Combine(_pluginPath!, "data.bin"));
        }

        public async Task<byte[]> PostByteArrayAsync(string url, string jsonBody, IDictionary<string, string>? header = null)
        {
            CreatePostFilePy(url, jsonBody, header);
            await WriteAsync("python requestsClient.py");
            return await File.ReadAllBytesAsync(Path.Combine(_pluginPath!, "data.bin"));
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

        private void CreateGetStringPy(string url, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "requestsClient.py");
            string strHeader = HeaderDictionaryToString(header);
            File.WriteAllText(file, $@"import requests
resp = requests.get('{url}'{strHeader})
resp.encoding = resp.apparent_encoding
print(resp.text)");
        }

        private void CreateGetFilePy(string url, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "requestsClient.py");
            string strHeader = HeaderDictionaryToString(header);
            File.WriteAllText(file, $@"import requests
resp = requests.get('{url}'{strHeader},stream=True)
resp.encoding = resp.apparent_encoding
with open('data.bin','wb+') as f:
    for chunk in resp.iter_content(chunk_size=4096):
        if chunk:
            f.write(chunk)
print('end')");
        }

        private void CreatePostStringPy(string url, string jsonBody, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "requestsClient.py");
            string strHeader = HeaderDictionaryToString(header);
            File.WriteAllText(file, $@"import requests
resp = requests.post('{url}'{strHeader},json={jsonBody})
resp.encoding = resp.apparent_encoding
print(resp.text)");
        }

        private void CreatePostFilePy(string url, string jsonBody, IDictionary<string, string>? header)
        {
            string file = Path.Combine(_pluginPath!, "requestsClient.py");
            string strHeader = HeaderDictionaryToString(header);
            File.WriteAllText(file, $@"import requests
resp = requests.get('{url}'{strHeader},json={jsonBody},stream=True)
resp.encoding = resp.apparent_encoding
with open('data.bin','wb+') as f:
    for chunk in resp.iter_content(chunk_size=4096):
        if chunk:
            f.write(chunk)
print('end')");
        }

        private string HeaderDictionaryToString(IDictionary<string, string>? header)
        {
            string strHeader = string.Empty;
            if (header is not null)
                strHeader = $",headers={{{string.Join(',', header.Select(kv => $"'{kv.Key}':'{kv.Value}'"))}}}";
            return strHeader;
        }

        #endregion -- script --

        #region -- CMD --

        private Process CreateCmdProcess()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = _pluginPath!;
            proc.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
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