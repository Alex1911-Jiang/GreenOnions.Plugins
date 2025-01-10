using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Message;
using Yande.re.Api;

namespace GreenOnions.NT.HPictures.Clients
{
    internal static class Yande_re_Client
    {
        private static YandeClient? _api = null;
        private static string _lastTag = string.Empty;
        internal static async IAsyncEnumerable<MessageBuilder> CreateMessage(string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            if (_api is null || _lastTag != keyword)
                _api = await YandeClient.CreateNew(true, false, keyword, config.UseProxy ? commonConfig.ProxyUrl : null);
            _lastTag = keyword;

            LogHelper.LogMessage($"在Yande.re查找{num}张{(r18 ? "R-18" : "")}的{keyword}色图");

            await foreach (var item in Base_Yande_re_Client.CreateMessage(_api, keyword, num, r18, config, commonConfig, context, chain))
                yield return item;
        }
    }
}
