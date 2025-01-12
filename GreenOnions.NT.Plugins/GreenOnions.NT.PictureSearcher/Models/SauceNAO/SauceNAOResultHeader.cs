namespace GreenOnions.NT.PictureSearcher.Models.SauceNAO
{
    public class SauceNAOResultHeader
    {
        public double similarity { get; set; }
        public string thumbnail { get; set; }
        public int index_id { get; set; }
        public string index_name { get; set; }
        public int dupes { get; set; }
        public int hidden { get; set; }
    }
}
