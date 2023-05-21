using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GreenOnions.NovelAiClient.Models
{
    /// <summary>
    /// Naifu请求参数实体类
    /// </summary>
    public class GenerationRequest
    {
        /// <summary>
        /// 构建一个Naifu请求参数实体类
        /// </summary>
        /// <param name="addWebDefaultPrompt">是否填充网页端的默认参数</param>
        public GenerationRequest(bool addWebDefaultPrompt = false)
        {
            if (addWebDefaultPrompt)
            {
                Random rdm = new Random(Guid.NewGuid().GetHashCode());
                prompt = "masterpiece, best quality,";
                width = 512;
                height = 768;
                scale = 12;
                sampler = Sampling.k_euler_ancestral;
                steps = 28;
                seed = rdm.Next(0, int.MaxValue);
                n_samples = 1;
                uc = $"lowres, bad anatomy, bad hands, text, error, missing fingers, extra digit, fewer digits, cropped, worst quality, low quality, normal quality, jpeg artifacts, signature, watermark, username, blurry,";
            }
        }

        /// <summary>
        /// 添加生成提示词(标签)
        /// </summary>
        /// <param name="prompts">提示词</param>
        public void AddPrompts(string prompts)
        {
            if (string.IsNullOrWhiteSpace(prompts))
                return;
            string tempAddPrompts = prompts.Trim();
            tempAddPrompts = tempAddPrompts.Replace("，", ",");
            prompt = CheckStringValue(prompt) + tempAddPrompts;
        }

        /// <summary>
        /// 添加生成提示词(标签)
        /// </summary>
        /// <param name="prompts"></param>
        public void AddPrompts(IEnumerable<string>? prompts)
        {
            if (prompts == null)
                return;
            prompt = CheckStringValue(prompt) + string.Join(',', prompts);
        }

        /// <summary>
        /// 添加屏蔽词
        /// </summary>
        /// <param name="ucs"></param>
        public void AddUndesiredContent(string ucs)
        {
            if (string.IsNullOrWhiteSpace(ucs))
                return;
            string tempAddUC= ucs.Trim();
            tempAddUC = tempAddUC.Replace("，", ",");
            uc = CheckStringValue(uc) + tempAddUC;
        }

        /// <summary>
        /// 添加屏蔽词
        /// </summary>
        /// <param name="ucs"></param>
        public void AddUndesiredContent(IEnumerable<string>? ucs)
        {
            if (ucs == null)
                return;
            uc = CheckStringValue(uc) + string.Join(',', ucs);
        }

        private string CheckStringValue(string? strValue)
        {
            if (strValue == null)
                strValue = "";
            string newStringValue = strValue.Trim();
            if (newStringValue != "" && !newStringValue.EndsWith(','))
                newStringValue += ',';
            return newStringValue;
        }

        public string prompt { get; set; } = "";
        public string? image { get; set; } = null;
        public int n_samples { get; set; } = 1;
        public int steps { get; set; } = 50;
        [JsonConverter(typeof(StringEnumConverter))]
        public Sampling sampler { get; set; } = Sampling.plms;
        public bool fixed_code { get; set; } = false;
        public float ddim_eta { get; set; } = 0.0f;
        public int height { get; set; } = 512;
        public int width { get; set; } = 512;
        public int latent_channels { get; set; } = 4;
        public int downsampling_factor { get; set; } = 8;
        public float scale { get; set; } = 7.0f;
        public float? dynamic_threshold { get; set; } = null;
        public int? seed { get; set; } = null;
        public float temp { get; set; } = 1.0f;
        public int top_k { get; set; } = 256;
        public int grid_size { get; set; } = 4;
        public bool advanced { get; set; } = false;
        public int? stage_two_seed { get; set; } = null;
        public float strength { get; set; } = 0.69f;
        public float noise { get; set; } = 0.667f;
        public bool mitigate { get; set; } = false;
        public string? module { get; set; } = null;
        public IEnumerable<Masker>? masks { get; set; } = null;
        public string? uc { get; set; } = null;
    }
    public class Masker
    {
        public int seed { get; set; }
        public string? mask { get; set; }
    }
    public enum Sampling
    {
        k_euler_ancestral,
        k_euler,
        k_lms,
        plms,
        ddim,
    }
}
