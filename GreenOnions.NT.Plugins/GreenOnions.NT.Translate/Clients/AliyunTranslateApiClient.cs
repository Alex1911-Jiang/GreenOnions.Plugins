using AlibabaCloud.SDK.Alimt20181012;
using AlibabaCloud.SDK.Alimt20181012.Models;
using AlibabaCloud.TeaUtil.Models;

namespace GreenOnions.NT.Translate.Clients
{
    internal class AliyunTranslateApiClient : BaseTranslateClient
    {
        internal AliyunTranslateApiClient(Config config) : base(config)
        {
        }

        internal override Dictionary<string, string> LanguageCodes { get; } = new()
        {
            {"阿布哈兹语","ab"},
            {"阿尔巴尼亚语","sq"},
            {"阿肯语","ak"},
            {"阿拉伯语","ar"},
            {"阿拉贡语","an"},
            {"阿姆哈拉语","am"},
            {"阿萨姆语","as"},
            {"阿塞拜疆语","az"},
            {"阿斯图里亚斯语","ast"},
            {"阿兹特克语","nch"},
            {"埃维语","ee"},
            {"艾马拉语","ay"},
            {"爱尔兰语","ga"},
            {"爱沙尼亚语","et"},
            {"奥杰布瓦语","oj"},
            {"奥克语","oc"},
            {"奥里亚语","or"},
            {"奥罗莫语","om"},
            {"奥塞梯语","os"},
            {"巴布亚皮钦语","tpi"},
            {"巴什基尔语","ba"},
            {"巴斯克语","eu"},
            {"白俄罗斯语","be"},
            {"柏柏尔语","ber"},
            {"班巴拉语","bm"},
            {"邦阿西楠语","pag"},
            {"保加利亚语","bg"},
            {"北萨米语","se"},
            {"本巴语","bem"},
            {"比林语","byn"},
            {"比斯拉马语","bi"},
            {"俾路支语","bal"},
            {"冰岛语","is"},
            {"波兰语","pl"},
            {"波斯尼亚语","bs"},
            {"波斯语","fa"},
            {"博杰普尔语","bho"},
            {"布列塔尼语","br"},
            {"查莫罗语","ch"},
            {"查瓦卡诺语","cbk"},
            {"楚瓦什语","cv"},
            {"聪加语","ts"},
            {"鞑靼语","tt"},
            {"丹麦语","da"},
            {"掸语","shn"},
            {"德顿语","tet"},
            {"德语","de"},
            {"低地德语","nds"},
            {"低地苏格兰语","sco"},
            {"迪维西语","dv"},
            {"侗语","kdx"},
            {"杜順語","dtp"},
            {"俄语","ru"},
            {"法罗语","fo"},
            {"法语","fr"},
            {"梵语","sa"},
            {"菲律宾语","fil"},
            {"斐济语","fj"},
            {"芬兰语","fi"},
            {"弗留利语","fur"},
            {"富尔语","fvr"},
            {"刚果语","kg"},
            {"高棉语","km"},
            {"格雷罗纳瓦特尔语","ngu"},
            {"格陵兰语","kl"},
            {"格鲁吉亚语","ka"},
            {"格罗宁根方言","gos"},
            {"古吉拉特语","gu"},
            {"瓜拉尼语","gn"},
            {"哈萨克语","kk"},
            {"海地克里奥尔语","ht"},
            {"韩语","ko"},
            {"豪萨语","ha"},
            {"荷兰语","nl"},
            {"黑山语","cnr"},
            {"胡帕语","hup"},
            {"基里巴斯语","gil"},
            {"基隆迪语","rn"},
            {"基切语","quc"},
            {"吉尔吉斯斯坦语","ky"},
            {"加利西亚语","gl"},
            {"加泰罗尼亚语","ca"},
            {"捷克语","cs"},
            {"卡拜尔语","kab"},
            {"卡纳达语","kn"},
            {"卡努里语","kr"},
            {"卡舒比语","csb"},
            {"卡西语","kha"},
            {"康沃尔语","kw"},
            {"科萨语","xh"},
            {"科西嘉语","co"},
            {"克里克语","mus"},
            {"克里米亚鞑靼语","crh"},
            {"克林贡语","tlh"},
            {"克罗地亚语","hbs"},
            {"克丘亚语","qu"},
            {"克什米尔语","ks"},
            {"库尔德语","ku"},
            {"拉丁语","la"},
            {"拉特加莱语","ltg"},
            {"拉脱维亚语","lv"},
            {"老挝语","lo"},
            {"立陶宛语","lt"},
            {"林堡语","li"},
            {"林加拉语","ln"},
            {"卢干达语","lg"},
            {"卢森堡语","lb"},
            {"卢森尼亚语","rue"},
            {"卢旺达语","rw"},
            {"罗马尼亚语","ro"},
            {"罗曼什语","rm"},
            {"罗姆语","rom"},
            {"逻辑语","jbo"},
            {"马达加斯加语","mg"},
            {"马恩语","gv"},
            {"马耳他语","mt"},
            {"马拉地语","mr"},
            {"马拉雅拉姆语","ml"},
            {"马来语","ms"},
            {"马里语（俄罗斯）","chm"},
            {"马其顿语","mk"},
            {"马绍尔语","mh"},
            {"玛雅语","kek"},
            {"迈蒂利语","mai"},
            {"毛里求斯克里奥尔语","mfe"},
            {"毛利语","mi"},
            {"蒙古语","mn"},
            {"孟加拉语","bn"},
            {"缅甸语","my"},
            {"苗语","hmn"},
            {"姆班杜语","umb"},
            {"纳瓦霍语","nv"},
            {"南非语","af"},
            {"尼泊尔语","ne"},
            {"纽埃语","niu"},
            {"挪威语","no"},
            {"帕姆语","pmn"},
            {"帕皮阿门托语","pap"},
            {"旁遮普语","pa"},
            {"葡萄牙语","pt"},
            {"普什图语","ps"},
            {"齐切瓦语","ny"},
            {"契维语","tw"},
            {"切罗基语","chr"},
            {"日语","ja"},
            {"瑞典语","sv"},
            {"萨摩亚语","sm"},
            {"桑戈语","sg"},
            {"僧伽罗语","si"},
            {"上索布语","hsb"},
            {"世界语","eo"},
            {"斯洛文尼亚语","sl"},
            {"斯瓦希里语","sw"},
            {"索马里语","so"},
            {"斯洛伐克语","sk"},
            {"他加禄语","tl"},
            {"塔吉克语","tg"},
            {"塔希提语","ty"},
            {"泰卢固语","te"},
            {"泰米尔语","ta"},
            {"泰语","th"},
            {"汤加语（汤加群岛）","to"},
            {"汤加语（赞比亚）","toi"},
            {"提格雷尼亚语","ti"},
            {"图瓦卢语","tvl"},
            {"图瓦语","tyv"},
            {"土耳其语","tr"},
            {"土库曼语","tk"},
            {"瓦隆语","wa"},
            {"瓦瑞语（菲律宾）","war"},
            {"威尔士语","cy"},
            {"文达语","ve"},
            {"沃拉普克语","vo"},
            {"沃洛夫语","wo"},
            {"乌德穆尔特语","udm"},
            {"乌尔都语","ur"},
            {"乌孜别克语","uz"},
            {"西班牙语","es"},
            {"西方国际语","ie"},
            {"西弗里斯兰语","fy"},
            {"西里西亚语","szl"},
            {"希伯来语","he"},
            {"希利盖农语","hil"},
            {"夏威夷语","haw"},
            {"现代希腊语","el"},
            {"新共同语言","lfn"},
            {"信德语","sd"},
            {"匈牙利语","hu"},
            {"修纳语","sn"},
            {"宿务语","ceb"},
            {"叙利亚语","syr"},
            {"巽他语","su"},
            {"亚美尼亚语","hy"},
            {"亚齐语","ace"},
            {"伊班语","iba"},
            {"伊博语","ig"},
            {"伊多语","io"},
            {"伊洛卡诺语","ilo"},
            {"伊努克提图特语","iu"},
            {"意大利语","it"},
            {"意第绪语","yi"},
            {"因特语","ia"},
            {"印地语","hi"},
            {"印度尼西亚语","id"},
            {"印古什语","inh"},
            {"英语","en"},
            {"约鲁巴语","yo"},
            {"越南语","vi"},
            {"扎扎其语","zza"},
            {"爪哇语","jv"},
            {"中文","zh"},
            {"中文繁体","zh-tw"},
            {"中文粤语","yue"},
            {"祖鲁语","zu"},
        };

        internal override async Task<string> TranslateToChinese(string text)
        {
            Client aliyunClient = CreateAliyunClient();
            string from = await DetectLanguage(aliyunClient, text);
            return await Translate(aliyunClient, text, from, "zh");
        }

        internal override async Task<string> TranslateTo(string text, string toLanguageChineseName)
        {
            Client aliyunClient = CreateAliyunClient();
            string from = await DetectLanguage(aliyunClient, text);
            string to = ChineseToCode(toLanguageChineseName);
            return await Translate(aliyunClient, text, from, to);
        }

        internal override async Task<string> TranslateFromTo(string text, string fromLanguageChineseName, string toLanguageChineseName)
        {
            Client aliyunClient = CreateAliyunClient();
            string from = ChineseToCode(fromLanguageChineseName);
            string to = ChineseToCode(toLanguageChineseName);
            return await Translate(aliyunClient, text, from, to);
        }

        private async Task<string> DetectLanguage(Client aliyunClient, string text)
        {
            GetDetectLanguageRequest getDetectLanguageRequest = new GetDetectLanguageRequest
            {
                SourceText = text,
            };
            RuntimeOptions runtime = new RuntimeOptions();
            var resp = await aliyunClient.GetDetectLanguageWithOptionsAsync(getDetectLanguageRequest, runtime);
            return resp.Body.DetectedLanguage;
        }

        private Client CreateAliyunClient()
        {
            AlibabaCloud.OpenApiClient.Models.Config aliyunConfig = new AlibabaCloud.OpenApiClient.Models.Config
            {
                AccessKeyId = Config.AppId,
                AccessKeySecret = Config.AppKey,
            };
            aliyunConfig.Endpoint = "mt.cn-hangzhou.aliyuncs.com";
            Client aliyunClient = new Client(aliyunConfig);
            return aliyunClient;
        }

        private async Task<string> Translate(Client aliyunClient, string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(Config.AppId) || string.IsNullOrWhiteSpace(Config.AppKey))
                throw new Exception("未配置令牌，无法翻译");

            TranslateGeneralRequest translateGeneralRequest = new TranslateGeneralRequest
            {
                FormatType = "text",
                SourceLanguage = from,
                TargetLanguage = to,
                SourceText = text,
                Scene = "general",
            };
            RuntimeOptions runtime = new RuntimeOptions();
            TranslateGeneralResponse response = await aliyunClient.TranslateGeneralWithOptionsAsync(translateGeneralRequest, runtime);
            return response.Body.Data.Translated;
        }
    }
}
