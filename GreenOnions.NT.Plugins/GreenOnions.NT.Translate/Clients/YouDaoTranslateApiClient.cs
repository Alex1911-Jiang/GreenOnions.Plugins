using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.NT.Translate.Clients
{
    internal class YouDaoTranslateApiClient : BaseTranslateClient
    {
        internal YouDaoTranslateApiClient(Config config) : base(config)
        {
        }

        internal override Dictionary<string, string> LanguageCodes { get; } = new()
        {
            { "自动识别", "auto" },
            { "中文", "zh-CHS" },
            { "简体中文", "zh-CHS" },
            { "繁体中文", "zh-CHT" },
            { "英文", "en" },
            { "日文", "ja" },
            { "韩文", "ko" },
            { "法文", "fr" },
            { "西班牙文", "es" },
            { "葡萄牙文", "pt" },
            { "意大利文", "it" },
            { "俄文", "ru" },
            { "越南文", "vi" },
            { "德文", "de" },
            { "阿拉伯文", "ar" },
            { "印尼文", "id" },
            { "南非荷兰文", "af" },
            { "波斯尼亚文", "bs" },
            { "保加利亚文", "bg" },
            { "粤文", "yue" },
            { "加泰隆文", "ca" },
            { "克罗地亚文", "hr" },
            { "捷克文", "cs" },
            { "丹麦文", "da" },
            { "荷兰文", "nl" },
            { "爱沙尼亚文", "et" },
            { "斐济文", "fj" },
            { "芬兰文", "fi" },
            { "希腊文", "el" },
            { "海地克里奥尔文", "ht" },
            { "希伯来文", "he" },
            { "印地文", "hi" },
            { "白苗文", "mww" },
            { "匈牙利文", "hu" },
            { "斯瓦希里文", "sw" },
            { "克林贡文", "tlh" },
            { "拉脱维亚文", "lv" },
            { "立陶宛文", "lt" },
            { "马来文", "ms" },
            { "马耳他文", "mt" },
            { "挪威文", "no" },
            { "波斯文", "fa" },
            { "波兰文", "pl" },
            { "克雷塔罗奥托米文", "otq" },
            { "罗马尼亚文", "ro" },
            { "塞尔维亚文(西里尔文)", "sr-Cyrl" },
            { "塞尔维亚文(拉丁文)", "sr-Latn" },
            { "斯洛伐克文", "sk" },
            { "斯洛文尼亚文", "sl" },
            { "瑞典文", "sv" },
            { "塔希提文", "ty" },
            { "泰文", "th" },
            { "汤加文", "to" },
            { "土耳其文", "tr" },
            { "乌克兰文", "uk" },
            { "乌尔都文", "ur" },
            { "威尔士文", "cy" },
            { "尤卡坦玛雅文", "yua" },
            { "阿尔巴尼亚文", "sq" },
            { "阿姆哈拉文", "am" },
            { "亚美尼亚文", "hy" },
            { "阿塞拜疆文", "az" },
            { "孟加拉文", "bn" },
            { "巴斯克文", "eu" },
            { "白俄罗斯文", "be" },
            { "宿务文", "ceb" },
            { "科西嘉文", "co" },
            { "世界文", "eo" },
            { "菲律宾文", "tl" },
            { "弗里西文", "fy" },
            { "加利西亚文", "gl" },
            { "格鲁吉亚文", "ka" },
            { "古吉拉特文", "gu" },
            { "豪萨文", "ha" },
            { "夏威夷文", "haw" },
            { "冰岛文", "is" },
            { "伊博文", "ig" },
            { "爱尔兰文", "ga" },
            { "爪哇文", "jw" },
            { "卡纳达文", "kn" },
            { "哈萨克文", "kk" },
            { "高棉文", "km" },
            { "库尔德文", "ku" },
            { "柯尔克孜文", "ky" },
            { "老挝文", "lo" },
            { "拉丁文", "la" },
            { "卢森堡文", "lb" },
            { "马其顿文", "mk" },
            { "马尔加什文", "mg" },
            { "马拉雅拉姆文", "ml" },
            { "毛利文", "mi" },
            { "马拉地文", "mr" },
            { "蒙古文", "mn" },
            { "缅甸文", "my" },
            { "尼泊尔文", "ne" },
            { "齐切瓦文", "ny" },
            { "普什图文", "ps" },
            { "旁遮普文", "pa" },
            { "萨摩亚文", "sm" },
            { "苏格兰盖尔文", "gd" },
            { "塞索托文", "st" },
            { "修纳文", "sn" },
            { "信德文", "sd" },
            { "僧伽罗文", "si" },
            { "索马里文", "so" },
            { "巽他文", "su" },
            { "塔吉克文", "tg" },
            { "泰米尔文", "ta" },
            { "泰卢固文", "te" },
            { "乌兹别克文", "uz" },
            { "南非科萨文", "xh" },
            { "意第绪文", "yi" },
            { "约鲁巴文", "yo" },
            { "南非祖鲁文", "zu" },
        };

        internal override async Task<string> TranslateToChinese(string text)
        {
            return await Translate(text, "auto", "zh-CHS");
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

            string appKey = Config.AppId;
            string appSecret = Config.AppKey;
            string salt = DateTime.Now.Millisecond.ToString();
            string curtime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string signStr = appKey + Truncate(text) + salt + curtime + appSecret;
            string sign = ComputeHash(signStr, SHA256.Create());
            Dictionary<string, string> dic = new()
            {
                { "from", from },
                { "to", to },
                { "signType", "v3" },
                { "curtime", curtime },
                { "q", System.Web.HttpUtility.UrlEncode(text) },
                { "appKey", appKey },
                { "salt", salt },
                { "sign", sign }
            };
            return await Post(Config, "https://openapi.youdao.com/api", dic);
        }

        private async Task<string> Post(Config config, string url, Dictionary<string, string> dic)
        {
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = config.UseProxy };
            using HttpClient client = new HttpClient(handler);
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (var item in dic)
                form.Add(new StringContent(item.Value, Encoding.UTF8, "application/x-www-form-urlencoded"), item.Key);
            request.Content = form;
            HttpResponseMessage resp = await client.SendAsync(request);
            string resultText = await resp.Content.ReadAsStringAsync();
            JToken jt = JsonConvert.DeserializeObject<JToken>(resultText);
            if (jt["translation"] is null)
                throw new Exception($"有道智云API返回错误，错误代码：{jt["errorCode"]}");
            return jt["translation"].First.ToString();
        }

        private string Truncate(string q)
        {
            int len = q.Length;
            return len <= 20 ? q : q.Substring(0, 10) + len + q.Substring(len - 10, 10);
        }

        private string ComputeHash(string input, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }
}
