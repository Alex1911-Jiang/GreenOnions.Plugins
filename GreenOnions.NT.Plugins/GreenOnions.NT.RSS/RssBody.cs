using System.Text;

namespace GreenOnions.NT.RSS
{
    internal class RssBody
    {
        internal StringBuilder Text { get; } = new StringBuilder();
        internal List<string> ImageUrls { get; } = new List<string>();
        internal List<string> VideoUrls { get; } = new List<string>();
        internal List<string> IFrameUrls { get; } = new List<string>();
    }
}
