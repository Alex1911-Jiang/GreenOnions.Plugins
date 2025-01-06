﻿using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Message;

namespace GreenOnions.NT.HPictures.Clients
{
    internal static class LoliconClient
    {
        internal static IAsyncEnumerable<MessageBuilder> CreateMessage(string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            return BaseLoliconClient.CreateMessage("Lolicon", "https://api.lolicon.app/setu/v2", keyword, num, r18, config, commonConfig, context, chain);
        }
    }
}
