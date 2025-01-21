using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.NT.Translate.Clients
{
    internal class BaiduTranslateApiClient : BaseTranslateClient
    {
        internal BaiduTranslateApiClient(Config config) : base(config)
        {
        }

        internal override Dictionary<string, string> LanguageCodes => new()
        {
            { "自动检测", "auto" },
            { "中文", "zh" },
            { "简体中文", "zh" },
            { "英文", "en" },
            { "粤文", "yue" },
            { "文言文", "wyw" },
            { "日文", "jp" },
            { "韩文", "kor" },
            { "法文", "fra" },
            { "西班牙文", "spa" },
            { "泰文", "th" },
            { "阿拉伯文", "ara" },
            { "俄文", "ru" },
            { "葡萄牙文", "pt" },
            { "德文", "de" },
            { "意大利文", "it" },
            { "希腊文", "el" },
            { "荷兰文", "nl" },
            { "波兰文", "	pl" },
            { "保加利亚文", "bul" },
            { "爱沙尼亚文", "est" },
            { "丹麦文", "dan" },
            { "芬兰文", "fin" },
            { "捷克文", "cs" },
            { "罗马尼亚文", "rom" },
            { "斯洛文尼亚文", "slo" },
            { "瑞典文", "swe" },
            { "匈牙利文", "hu" },
            { "繁体中文", "cht" },
            { "越南文", "vie" },
        };


        internal override async Task<string> TranslateToChinese(string text)
        {
            return await Translate(text, "auto", "zh");
        }

        internal override async Task<string> TranslateTo(string text, string toLanguageChineseName)
        {
            string to = ChineseToCode(toLanguageChineseName);
            return await Translate(text, "auto", to);
        }

        internal override async Task<string> TranslateFromTo(string text, string fromLanguageChineseName, string toLanguageChineseName)
        {
            string from = ChineseToCode(fromLanguageChineseName);
            string to = ChineseToCode(toLanguageChineseName);
            return await Translate(text, from, to);
        }

        private async Task<string> Translate(string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(Config.AppId) || string.IsNullOrWhiteSpace(Config.AppKey))
                throw new Exception("未配置令牌，无法翻译");

            List<int> lstSalt = new List<int>(10);
            int code = Guid.NewGuid().GetHashCode();
            Random rdm = new Random(code);
            for (int i = 0; i < 10; i++)
                lstSalt.Add(rdm.Next(0, 10));
            string salt = string.Join("", lstSalt);
            string signStr = $"{Config.AppId}{text}{salt}{Config.AppKey}";
            MD5 signMD5 = MD5.Create();
            byte[] signBytes = signMD5.ComputeHash(Encoding.UTF8.GetBytes(signStr));
            StringBuilder sbSign = new StringBuilder();
            for (int i = 0; i < signBytes.Length; i++)
                sbSign.Append(signBytes[i].ToString("x2"));
            string sign = sbSign.ToString();
            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(text, Encoding.UTF8, "application/x-www-form-urlencoded"), "q" },
                { new StringContent(from, Encoding.UTF8, "application/x-www-form-urlencoded"), "from" },
                { new StringContent(to, Encoding.UTF8, "application/x-www-form-urlencoded"), "to" },
                { new StringContent(Config.AppId, Encoding.UTF8, "application/x-www-form-urlencoded"), "appid" },
                { new StringContent(salt, Encoding.UTF8, "application/x-www-form-urlencoded"), "salt" },
                { new StringContent(sign, Encoding.UTF8, "application/x-www-form-urlencoded"), "sign" }
            };
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = Config.UseProxy };
            using HttpClient client = new HttpClient(handler);
            var tranResp = await client.PostAsync("http://api.fanyi.baidu.com/api/trans/vip/translate", content);
            var resultJson = await tranResp.Content.ReadAsStringAsync();
            JToken jResult = JsonConvert.DeserializeObject<JToken>(resultJson);
            if (jResult["trans_result"] is null)
                throw new Exception($"百度翻译API返回错误，错误代码：{jResult["error_code"]}，错误内容：{jResult["error_msg"]}");
            return jResult["trans_result"][0]["dst"].ToString();
        }
    }
}
