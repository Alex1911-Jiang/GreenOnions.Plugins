namespace GreenOnions.NT.PictureSearcher.Models.Soutubot
{
    public class SoutubotDatum
    {
        public string source { get; set; }
        public int page { get; set; }
        public string title { get; set; }
        public string language { get; set; }
        public string pagePath { get; set; }
        public string subjectPath { get; set; }
        public string previewImageUrl { get; set; }
        public double similarity { get; set; }
    }
}
