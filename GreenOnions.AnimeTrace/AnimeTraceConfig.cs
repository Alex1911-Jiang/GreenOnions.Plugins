namespace GreenOnions.AnimeTrace
{
    public class AnimeTraceConfig
    {
        public string StartSearchCommand { get; set; } = "<机器人名称>搜角色";
        public string StartSearchInAnimeCommand { get; set; } = "<机器人名称>搜动漫角色";
        public string StartSearchInGameCommand { get; set; } = "<机器人名称>搜游戏角色";
        public string SearchStartReply { get; set; } = "请发送图片";
        public string SearchErrortReply { get; set; } = "搜索失败QAQ\r\n(<错误信息>)";

        public models model { get; set; } = models.anime;
        public int force_one { get; set; } = 1;
    }

    public enum models
    {
        anime,
        anime_model_lovelive,
        pre_stable,
        game,
    }
}
