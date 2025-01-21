using System.Net;
using System.Text;
using GreenOnions.NT.Base;
using GreenOnions.NT.HPictures.Helpers;
using Lagrange.Core;
using Lagrange.Core.Message;
using Yande.re.Api;

namespace GreenOnions.NT.HPictures.Clients
{
    internal class Base_Yande_re_Client
    {
        internal static async IAsyncEnumerable<MessageBuilder> CreateMessage(BaseClient api, string keyword, int num, bool r18, Config config, ICommonConfig commonConfig, BotContext context, MessageChain chain)
        {
            int sendedCount = 0;
            List<PictureItem> pictures = api.PictureList.Where(p => p.Rating == (r18 ? Rating.Explicit : Rating.Safe)).ToList();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            while (sendedCount < num)
            {
                IL_CheckPictureCount:
                if (pictures.Count == 0)
                {
                    await api.GetNextPagePictureList();  //移动到下一页
                    pictures = api.PictureList.Where(p => p.Rating == (r18 ? Rating.Explicit : Rating.Safe)).ToList();
                    goto IL_CheckPictureCount;
                }

                int index = random.Next(pictures.Count);
                PictureItem item = pictures[index];

                pictures.Remove(item);

                StringBuilder sb = new StringBuilder();
                if (config.SendUrl)
                    sb.AppendLine($"https://{item.Host}/{item.ShowPageUrl}");
                if (config.SendTags && item.Tags is not null)
                    sb.AppendLine($"标签:{string.Join(',', item.Tags)}");

                HttpClientHandler httpClientHandler = new HttpClientHandler() { UseProxy = config.UseProxy };
                using HttpClient client = new HttpClient(httpClientHandler);

                byte[] img = await client.GetByteArrayAsync(await item.GetBigImgUrl());

                if (config.AntiShielding)
                    img = ImageHelper.AntiShielding(img);

                if (chain.GroupUin is null)
                    yield return MessageBuilder.Friend(chain.FriendUin).Text(sb.ToString()).Image(img);
                else
                    yield return MessageBuilder.Group(chain.GroupUin.Value).Forward(chain).Text(sb.ToString()).Image(img);
                sendedCount++;
            }
        }
    }
}
