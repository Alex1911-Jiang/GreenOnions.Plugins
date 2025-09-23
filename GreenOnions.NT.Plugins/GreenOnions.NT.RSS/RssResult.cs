using System.Text;

namespace GreenOnions.NT.RSS
{
    internal class RssResult
    {
        public static bool operator ==(RssResult left, RssResult right) => left.InnerTitle == right.InnerTitle;
        public static bool operator !=(RssResult left, RssResult right) => left.InnerTitle != right.InnerTitle;

        public override bool Equals(object? obj)
        {
            if (obj is not RssResult theOther)
                return false;
            return InnerTitle == theOther.InnerTitle;
        }

        public override int GetHashCode()
        {
            int hashCode = InnerTitle.GetHashCode();
            return hashCode;
        }

        internal string? Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        internal string? Title { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        internal string InnerTitle { get; set; }
        /// <summary>
        /// 正文文字部分
        /// </summary>
        internal StringBuilder Text { get; set; } = new StringBuilder();
        /// <summary>
        /// 正文图片地址
        /// </summary>
        internal List<string> ImageUrls { get; set; } = new List<string>();
        /// <summary>
        /// 正文视频地址
        /// </summary>
        internal List<string> VideoUrls { get; set; } = new List<string>();
        /// <summary>
        /// 正文嵌套内容地址
        /// </summary>
        internal List<string> IFrameUrls { get; set; } = new List<string>();
        /// <summary>
        /// 作者名
        /// </summary>
        internal string? Author { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        internal DateTime? PubDate { get; set; } = null;
        /// <summary>
        /// 原文地址
        /// </summary>
        internal string? Link { get; set; }
    }
}
