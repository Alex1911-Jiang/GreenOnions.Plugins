using YamlDotNet.Serialization;

namespace GreenOnions.NT.CryptocurrencyPrices
{
    internal class Config
    {
        [YamlMember(Description = "允许查询现货价格的群白名单")]
        public uint[] WhileGroups { get; set; } = [];

        [YamlMember(Description = "新币上线时推送通知的群号")]
        public uint[] NewSymbolNotifyGroups { get; set; } = [];

        [YamlMember(Description = "撤回时间，单位：秒")]
        public int RecallSecond { get; set; } = 30;

        [YamlMember(Description = "将中文替换为货币名称的列表")]
        public Dictionary<string, string> ReplaceSymbol { get; set; } = new()
        {
            { "特朗普", "TRUMP" },
            { "川普", "TRUMP" },
        };
    }
}
