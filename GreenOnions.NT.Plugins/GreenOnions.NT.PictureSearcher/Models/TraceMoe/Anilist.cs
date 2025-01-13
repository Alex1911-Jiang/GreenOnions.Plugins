namespace GreenOnions.NT.PictureSearcher.Models.TraceMoe
{
    internal class Anilist
    {
        public int id { get; set; }
        public int idMal { get; set; }
        public AnilistTitle title { get; set; }
        public string[] synonyms { get; set; }
        public bool isAdult { get; set; }
    }
}
