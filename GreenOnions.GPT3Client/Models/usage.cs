namespace GreenOnions.GPT3Client.Models
{
    public class usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
