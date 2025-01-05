using System.Text.RegularExpressions;

namespace GreenOnions.NT.HPictures.Helpers
{
    public static class RegexMatchHelper
    {
        public static (string keyword, int num, bool r18) ExtractParameter(Match matchHPictureCmd)
        {
            string keyword = matchHPictureCmd.ExtractKeyword();
            int num = matchHPictureCmd.ExtractNum();
            bool bR18 = matchHPictureCmd.Groups["r18"].Success;
            return (keyword, num, bR18);
        }
        private static string ExtractKeyword(this Match matchHPictureCmd)
        {
            if (matchHPictureCmd.Groups["关键词"].Success)
                return matchHPictureCmd.Groups["关键词"].Value;
            return string.Empty;
        }
        private static int ExtractNum(this Match matchHPictureCmd)
        {
            if (!matchHPictureCmd.Groups["数量"].Success)
                return 1;

            if (string.IsNullOrWhiteSpace(matchHPictureCmd.Groups["数量"].Value))
                return 1;

            if (int.TryParse(matchHPictureCmd.Groups["数量"].Value, out int iNum))
                return iNum;

            long lNum = matchHPictureCmd.Groups["数量"].Value.ChineseToNumber();

            if (lNum > 20)
                lNum = 20;

            return (int)lNum;
        }

    }
}
