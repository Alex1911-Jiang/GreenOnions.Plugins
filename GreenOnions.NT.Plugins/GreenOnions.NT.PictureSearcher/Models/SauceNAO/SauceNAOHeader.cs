namespace GreenOnions.NT.PictureSearcher.Models.SauceNAO
{
    internal class SauceNAOHeader
    {
        public string user_id { get; set; }
        public string account_type { get; set; }
        public string short_limit { get; set; }
        public string long_limit { get; set; }
        public int long_remaining { get; set; }
        public int short_remaining { get; set; }
        public int status { get; set; }
        public int results_requested { get; set; }
        public Dictionary<int, SauceNAOIndex> index { get; set; }
        public string search_depth { get; set; }
        public double minimum_similarity { get; set; }
        public string query_image_display { get; set; }
        public string query_image { get; set; }
        public int results_returned { get; set; }
    }

}
