using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace GreenOnions.KanCollectionTimeAnnouncerWindows
{
    public static class MoeGirlHelper
    {
        /// <summary>
        /// 请求萌娘百科获取舰娘名称列表
        /// </summary>
        /// <returns></returns>
        public static async Task<List<string>> GetKanGrilNameList()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(@"https://zh.moegirl.org.cn/舰队Collection/图鉴/舰娘");
                string html = await response.Content.ReadAsStringAsync() + "</body></html>";

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //按图鉴编号排列的Table
                var tables = doc.DocumentNode.SelectNodes(@"/html/body/template[@id='MOE_SKIN_TEMPLATE_BODYCONTENT']/div[@id='mw-content-text']/div[@class='mw-parser-output']/table[@class='wikitable']");

                if (tables == null)
                {
                    return null;  //滑动验证
                }

                HashSet<string> collectionNames = new HashSet<string>();
                foreach (HtmlNode table in tables)
                {
                    foreach (HtmlNode tr in table.SelectNodes("tbody/tr"))
                    {
                        foreach (string title in tr.SelectNodes("td/a").Select(a => a.Attributes["title"].Value))
                        {
                            if (title.Contains("舰队Collection:"))  //排除联动的舰娘
                                collectionNames.Add(title.Substring("舰队Collection:".Length));
                        }
                    }
                }

                return collectionNames.ToList();
            }
        }
    }
}
