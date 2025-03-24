namespace GreenOnions.NT.PictureSearcher.Models.Soutubot
{
    public class SoutubotResult
    {
        public SoutubotDatum[] data { get; set; }
        public string id { get; set; }
        public float factor { get; set; }
        public string imageUrl { get; set; }
        public string searchOption { get; set; }
        public float executionTime { get; set; }
    }
}
