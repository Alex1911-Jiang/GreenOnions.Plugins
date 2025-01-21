using System.Net;
using System.Text;
using AnimeTrace_Net_SDK;
using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace GreenOnions.NT.PictureSearcher.Clients
{
    internal class AnimeTraceSearcher
    {
        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl, int modelIndex)
        {
            if (modelIndex >= config.EnabledAnimeTraceModels.Length)
            {
                LogHelper.LogError($"配置错误，AnimeTrace搜索次数{modelIndex + 1}大于启用的模型数量{config.EnabledAnimeTraceModels}");
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "AnimeTrace").Replace("<错误信息>", "配置错误，AnimeTrace搜索次数大于启用的模型数量"));
                return 0;
            }

            string model = config.EnabledAnimeTraceModels[modelIndex];

            try
            {
                return await Search(commonConfig, config, context, chain, imageUrl, model);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, $"AnimeTrace({model}模型)搜图发生异常");
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "AnimeTrace").Replace("<错误信息>", ex.Message));
                return 0;
            }
        }

        public static async Task<double> Search(ICommonConfig commonConfig, Config config, BotContext context, MessageChain chain, string imageUrl, string model)
        {
            using HttpClientHandler httpClientHandler = new HttpClientHandler { UseProxy = config.UseProxy };
            using HttpClient httpClient = new HttpClient(httpClientHandler);

            Stream img = await httpClient.GetStreamAsync(imageUrl);

            AnimeTraceClient client = new AnimeTraceClient(model, httpClient);
            AnimeTraceResult result = await client.RecognizeAsync(img, true);

            foreach (var item in result.Data)
            {
                Console.WriteLine($"动画名称：{item.CartoonName}");
                Console.WriteLine($"准确度：{item.AccPercent}");
                Console.WriteLine($"角色名称：" + item.Name);
                Console.WriteLine();
            }

            if (!result.Success)
            {
                await chain.ReplyAsync(config.SearchErrorReply.Replace("<搜索类型>", "AnimeTrace").Replace("<错误信息>", result.Message));
                return 0;
            }

            if (result.Data.Length == 0)
            {
                await chain.ReplyAsync(config.SearchNoResultReply.Replace("<搜索类型>", "AnimeTrace"));
                return 0;
            }

            StringBuilder sb = new StringBuilder();

            double similarity = 0;
            foreach (var item in result.Data)
            {
                if (item.AccPercent > similarity)
                    similarity = item.AccPercent;
                similarity = item.AccPercent;

                sb.AppendLine($"动画名称：{item.CartoonName}");
                sb.AppendLine($"相似度：{similarity * 100:0.00}%");
                sb.AppendLine($"角色名称：" + item.Name);
                sb.AppendLine();
            }
            sb.AppendLine("(AnimeTrace)");

            LogHelper.LogMessage($"执行AnimeTrace {model.ToString()}搜索成功");

            MessageBuilder msg;
            if (chain.GroupUin is null)
                msg = MessageBuilder.Friend(chain.FriendUin);
            else
                msg = MessageBuilder.Group(chain.GroupUin.Value).Forward(chain);
            msg = msg.Text(sb.ToString());

            await context.SendMessage(msg.Build());

            return similarity * 100;
        }
    }
}
