using System.Text;
using GreenOnions.NovelAiClient.Models;
using Newtonsoft.Json;

namespace GreenOnions.NovelAiClient
{
    /// <summary>
    /// Naifu客户端
    /// </summary>
    public class NaifuClient : BaseClient
    {
        private bool _addWebDefaultPrompt;
        /// <summary>
        /// 实例化一个Naifu客户端对象
        /// Naifu默认地址为：http://127.0.0.1:6969/
        /// </summary>
        /// <param name="addWebDefaultPrompt">是否添加Web前端默认优化质量的参数</param>
        /// <param name="host">服务地址</param>
        public NaifuClient(string host = "http://127.0.0.1:6969/", bool addWebDefaultPrompt = true) : base(host)
        {
            _addWebDefaultPrompt = addWebDefaultPrompt;
        }

        /// <summary>
        /// 最简单的方式请求地址并返回图片字节数组
        /// </summary>
        /// <param name="prompt">生成提示词(标签)，多个提示词以英文逗号分隔</param>
        /// <returns></returns>
        public Task<byte[]?> PostAsync(string prompt)
        {
            if (prompt == null)
                throw new ArgumentNullException(nameof(prompt));
            return PostAsync(prompt.Replace("，", ",").Split(','));
        }

        /// <summary>
        /// 最简单的方式请求地址并返回图片字节数组
        /// </summary>
        /// <param name="prompt">生成提示词(标签)</param>
        /// <returns></returns>
        public Task<byte[]?> PostAsync(params string[] prompt)
        {
            if (prompt == null)
                throw new ArgumentNullException(nameof(prompt));
            return PostAsync(prompt, null);
        }

        /// <summary>
        /// 比较简单的方式请求地址并返回图片字节数组
        /// </summary>
        /// <param name="prompt">生成提示词(标签)，多个提示词以英文逗号分隔</param>
        /// <param name="uc">屏蔽词(标签)，多个屏蔽词以英文逗号分隔</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高 ( 为 -1 时，如果 addWebDefaultPrompt = true 则为 768，否则为 512 )</param>
        /// <returns></returns>
        public Task<byte[]?> PostAsync(string prompt, string? uc = null, int width = 512, int height = -1)
        {
            if (prompt == null)
                throw new ArgumentNullException(nameof(prompt));
            string[] arrPrompt = prompt.Replace("，", ",").Split(',');
            string[]? arrUC = null;
            if (uc != null)
                arrUC = uc.Replace("，", ",").Split(',');
            return PostAsync(arrPrompt, arrUC, width, height);
        }

        /// <summary>
        /// 比较简单的方式请求地址并返回图片字节数组
        /// </summary>
        /// <param name="prompt">生成提示词(标签)</param>
        /// <param name="uc">屏蔽词(标签)</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高 ( 为 -1 时，如果 addWebDefaultPrompt = true 则为 768，否则为 512 )</param>
        /// <returns></returns>
        public Task<byte[]?> PostAsync(IEnumerable<string> prompt, IEnumerable<string>? uc = null, int width = 512, int height = -1)
        {
            if (prompt == null)
                throw new ArgumentNullException(nameof(prompt));
            if (height == -1)
                height = _addWebDefaultPrompt ? 768 : 512;
            Random rdm = new Random(Guid.NewGuid().GetHashCode());
            GenerationRequest data = new GenerationRequest(_addWebDefaultPrompt)
            {
                width = width,
                height = height,
                seed = rdm.Next(0, int.MaxValue),
            };
            data.AddPrompts(prompt);
            data.AddUndesiredContent(uc);
            return PostAsync(data);
        }

        /// <summary>
        /// 使用完整自定义参数进行请求
        /// </summary>
        /// <param name="data">请求参数实体</param>
        /// <returns></returns>
        public async Task<byte[]?> PostAsync(GenerationRequest data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            string json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_host}generate-stream");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient() { Timeout = Timeout.InfiniteTimeSpan })
            {
                var response = await client.SendAsync(request);
                var str = await response.Content.ReadAsStringAsync();
                int dataIndex = str.IndexOf("data:");
                if (dataIndex == -1)
                    throw new Exception($"生成错误，返回内容为：{str}");
                str = str.Substring(dataIndex + "data:".Length);
                return Convert.FromBase64String(str);
            }
        }
    }
}
