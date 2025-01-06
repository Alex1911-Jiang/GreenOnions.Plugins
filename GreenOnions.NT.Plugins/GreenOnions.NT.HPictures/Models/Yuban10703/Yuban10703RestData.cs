namespace GreenOnions.NT.HPictures.Models.Yuban10703
{
    public class Yuban10703RestData
    {
        public Yuban10703Artwork artwork { get; set; }
        public Yuban10703Author author { get; set; }
        public int sanity_level { get; set; }
        public bool r18 { get; set; }
        public int page { get; set; }
        public DateTime create_date { get; set; }
        public Yuban10703Size size { get; set; }
        public string[] tags { get; set; }
        public Yuban10703Urls urls { get; set; }
    }
}
