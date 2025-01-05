namespace GreenOnions.NT.HPictures.Models.Lolicon
{
    internal class LoliconData
    {
        public int pid { get; set; }
        public int p { get; set; }
        public int uid { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public bool r18 { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string[] tags { get; set; }
        public string ext { get; set; }
        public int aiType { get; set; }
        public long uploadDate { get; set; }
        public LoliconUrls urls { get; set; }
    }
}
