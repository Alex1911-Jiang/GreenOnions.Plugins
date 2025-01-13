namespace GreenOnions.NT.PictureSearcher.Models.TraceMoe
{
    internal class TraceMoeResult
    {
        public Anilist anilist { get; set; }
        public string filename { get; set; }
        public int? episode { get; set; }
        public float from { get; set; }
        public float to { get; set; }
        public float similarity { get; set; }
        public string video { get; set; }
        public string image { get; set; }
    }
}
