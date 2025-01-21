using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace GreenOnions.NT.Translate.Clients
{
    internal class TencentTranslateApiClient : BaseTranslateClient
    {
        public TencentTranslateApiClient(Config config) : base(config)
        {
        }

        internal override Dictionary<string, string> LanguageCodes { get; } = new()
        {
            { "自动识别", "auto" },
            { "中文", "zh" },
            { "简体中文", "zh" },
            { "繁体中文", "zh-TW" },
            { "英文", "en" },
            { "日文", "ja" },
            { "韩文", "ko" },
            { "法文", "fr" },
            { "西班牙文", "es" },
            { "意大利文", "it" },
            { "德文", "de" },
            { "土耳其文", "tr" },
            { "俄文", "ru" },
            { "葡萄牙文", "pt" },
            { "越南文", "vi" },
            { "印尼文", "id" },
            { "泰文", "th" },
            { "马来西亚文", "ms" },
            { "阿拉伯文", "ar" },
            { "印地文", "hi" },
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

            Credential cred = new Credential
            {
                SecretId = Config.AppId,
                SecretKey = Config.AppKey
            };
            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = "tmt.tencentcloudapi.com";
            clientProfile.HttpProfile = httpProfile;

            TmtClient client = new TmtClient(cred, "ap-guangzhou", clientProfile);
            TextTranslateRequest req = new TextTranslateRequest();
            req.SourceText = text;
            req.Source = from;
            req.Target = to;
            req.ProjectId = 0;
            TextTranslateResponse resp = await client.TextTranslate(req);
            string strJson = AbstractModel.ToJsonString(resp);
            JToken jt = JsonConvert.DeserializeObject<JToken>(strJson);
            return jt["TargetText"].ToString();
        }
    }
}
