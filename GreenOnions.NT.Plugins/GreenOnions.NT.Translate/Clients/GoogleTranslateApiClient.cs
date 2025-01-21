using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GreenOnions.NT.Translate.Clients
{
    internal class GoogleTranslateApiClient : BaseTranslateClient
    {
        internal GoogleTranslateApiClient(Config config) : base(config)
        {
        }

        internal override Dictionary<string, string> LanguageCodes { get; } = new()
        {
            { "检测语言", "auto" },
            { "阿尔巴尼亚文", "sq" },
            { "阿拉伯文", "ar" },
            { "阿姆哈拉文", "am" },
            { "阿萨姆文", "as" },
            { "阿塞拜疆文", "az" },
            { "埃维文", "ee" },
            { "艾马拉文", "ay" },
            { "爱尔兰文", "ga" },
            { "爱沙尼亚文", "et" },
            { "奥利亚文", "or" },
            { "奥罗莫文", "om" },
            { "巴斯克文", "eu" },
            { "白俄罗斯文", "be" },
            { "班巴拉文", "bm" },
            { "保加利亚文", "bg" },
            { "冰岛文", "is" },
            { "波兰文", "pl" },
            { "波斯尼亚文", "bs" },
            { "波斯文", "fa" },
            { "博杰普尔文", "bho" },
            { "布尔文(南非荷兰文)", "af" },
            { "鞑靼文", "tt" },
            { "丹麦文", "da" },
            { "德文", "de" },
            { "迪维希文", "dv" },
            { "蒂格尼亚文", "ti" },
            { "多格来文", "doi" },
            { "俄文", "ru" },
            { "法文", "fr" },
            { "梵文", "sa" },
            { "菲律宾文", "tl" },
            { "芬兰文", "fi" },
            { "弗里西文", "fy" },
            { "高棉文", "km" },
            { "格鲁吉亚文", "ka" },
            { "贡根文", "gom" },
            { "古吉拉特文", "gu" },
            { "瓜拉尼文", "gn" },
            { "哈萨克文", "kk" },
            { "海地克里奥尔文", "ht" },
            { "韩文", "ko" },
            { "豪萨文", "ha" },
            { "荷兰文", "nl" },
            { "吉尔吉斯文", "ky" },
            { "加利西亚文", "gl" },
            { "加泰罗尼亚文", "ca" },
            { "捷克文", "cs" },
            { "卡纳达文", "kn" },
            { "科西嘉文", "co" },
            { "克里奥尔文", "kri" },
            { "克罗地亚文", "hr" },
            { "克丘亚文", "qu" },
            { "库尔德文（库尔曼吉文）", "ku" },
            { "库尔德文（索拉尼）", "ckb" },
            { "拉丁文", "la" },
            { "拉脱维亚文", "lv" },
            { "老挝文", "lo" },
            { "立陶宛文", "lt" },
            { "林格拉文", "ln" },
            { "卢干达文", "lg" },
            { "卢森堡文", "lb" },
            { "卢旺达文", "rw" },
            { "罗马尼亚文", "ro" },
            { "马尔加什文", "mg" },
            { "马耳他文", "mt" },
            { "马拉地文", "mr" },
            { "马拉雅拉姆文", "ml" },
            { "马来文", "ms" },
            { "马其顿文", "mk" },
            { "迈蒂利文", "mai" },
            { "毛利文", "mi" },
            { "梅泰文（曼尼普尔文）", "mni-Mtei" },
            { "蒙古文", "mn" },
            { "孟加拉文", "bn" },
            { "米佐文", "lus" },
            { "缅甸文", "my" },
            { "苗文", "hmn" },
            { "南非科萨文", "xh" },
            { "南非祖鲁文", "zu" },
            { "尼泊尔文", "ne" },
            { "挪威文", "no" },
            { "旁遮普文", "pa" },
            { "葡萄牙文", "pt" },
            { "普什图文", "ps" },
            { "齐切瓦文", "ny" },
            { "契维文", "ak" },
            { "日文", "ja" },
            { "瑞典文", "sv" },
            { "萨摩亚文", "sm" },
            { "塞尔维亚文", "sr" },
            { "塞佩蒂文", "nso" },
            { "塞索托文", "st" },
            { "僧伽罗文", "si" },
            { "世界文", "eo" },
            { "斯洛伐克文", "sk" },
            { "斯洛文尼亚文", "sl" },
            { "斯瓦希里文", "sw" },
            { "苏格兰盖尔文", "gd" },
            { "宿务文", "ceb" },
            { "索马里文", "so" },
            { "塔吉克文", "tg" },
            { "泰卢固文", "te" },
            { "泰米尔文", "ta" },
            { "泰文", "th" },
            { "土耳其文", "tr" },
            { "土库曼文", "tk" },
            { "威尔士文", "cy" },
            { "维吾尔文", "ug" },
            { "乌尔都文", "ur" },
            { "乌克兰文", "uk" },
            { "乌兹别克文", "uz" },
            { "西班牙文", "es" },
            { "希伯来文", "iw" },
            { "希腊文", "el" },
            { "夏威夷文", "haw" },
            { "信德文", "sd" },
            { "匈牙利文", "hu" },
            { "修纳文", "sn" },
            { "亚美尼亚文", "hy" },
            { "伊博文", "ig" },
            { "伊洛卡诺文", "ilo" },
            { "意大利文", "it" },
            { "意第绪文", "yi" },
            { "印地文", "hi" },
            { "印尼巽他文", "su" },
            { "印尼文", "id" },
            { "印尼爪哇文", "jw" },
            { "英文", "en" },
            { "约鲁巴文", "yo" },
            { "越南文", "vi" },
            { "中文", "zh-Hans" },
            { "繁体中文", "zh-TW" },
            { "简体中文", "zh-CN" },
            { "宗加文", "ts" },
        };

        internal override async Task<string> TranslateToChinese(string text)
        {
            return await Translate(text, "auto", "zh-CN");
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
            using HttpClientHandler handler = new HttpClientHandler { UseProxy = Config.UseProxy };
            using HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("DotNetRuntime/8.0");
            var response = await client.GetAsync($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q={text}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{(int)response.StatusCode} {response.StatusCode}");
            string result = await response.Content.ReadAsStringAsync();
            JArray arr = JsonConvert.DeserializeObject<JArray>(result);
            StringBuilder sb = new StringBuilder();
            foreach (var jt in arr[0])
                sb.Append(jt[0]);
            return sb.ToString();
        }
    }
}
