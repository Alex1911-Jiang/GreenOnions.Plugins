﻿namespace GreenOnions.NT.HPictures.Helpers
{
    public static class ChineseNumberHelper
    {
        /// <summary>
        /// 中文转数字
        /// </summary>
        public static long ChineseToNumber(this string src)
        {
            src = src.Replace("壹", "一")
                .Replace("贰", "二")
                .Replace("两", "二")
                .Replace("兩", "二")
                .Replace("叁", "三")
                .Replace("肆", "四")
                .Replace("伍", "五")
                .Replace("陆", "六")
                .Replace("陸", "六")
                .Replace("柒", "七")
                .Replace("捌", "八")
                .Replace("玖", "九")
                .Replace("拾", "十")
                .Replace("佰", "百")
                .Replace("仟", "千")
                .Replace("萬", "万");

            // 定义一个数组，用于接受分割字符串的结果
            string[] srcArr;
            // 定义计算结果
            long result = 0;
            // 如果字符串中包含'亿'则进行分割
            if (src.IndexOf("亿") != -1)
            {
                // 以字符'亿'分割源字符串
                srcArr = src.Split('亿');
                // 计算亿以上的数字
                result += Convert.ToInt64((srcArr[0].ConvertToNumber()) * Math.Pow(10, 8));
                // 如果剩余字符串中包括'万'，则再次进行分割
                if (src.IndexOf("万") != -1)
                {
                    // 以字符'万'分割源字符串
                    srcArr = srcArr[1].Split('万');
                    // 计算万以上亿以下的数字
                    result += Convert.ToInt64(srcArr[0].ConvertToNumber() * Math.Pow(10, 4)) + Convert.ToInt64(srcArr[1].ConvertToNumber());
                }
            }
            // 如果字符串中不包含字符'亿'
            else
            {
                // 如果源字符串中包括'万'，则以'万'字进行分割
                if (src.IndexOf("万") != -1)
                {
                    srcArr = src.Split('万');
                    result += Convert.ToInt64(srcArr[0].ConvertToNumber() * Math.Pow(10, 4)) + Convert.ToInt64(srcArr[1].ConvertToNumber());
                }
                else
                {
                    // 源文本为1万以下的数字
                    result += Convert.ToInt64(ConvertToNumber(src));
                }
            }
            return result;
        }

        /// <summary>
        /// 1万以内中文转数字
        /// </summary>
        private static long ConvertToNumber(this string src)
        {
            // 定义包含所有数字的字符串，用以判断字符是否为数字。
            string numberString = "零一二三四五六七八九十";
            // 定义单位字符串，用以判断字符是否为单位。
            string unitString = "零十百千";
            // 把数字字符串转换为char数组，方便截取。
            char[] charArr = src.Replace(" ", "").ToCharArray();
            // 返回结果
            int result = 0;
            // 如果源为空指针、空字符串、空白字符串 则返回0
            if (string.IsNullOrEmpty(src) || string.IsNullOrWhiteSpace(src))
            {
                return 0;
            }
            // 如果源的第一个字符不是数字 则返回0
            if (numberString.IndexOf(charArr[0]) == -1)
            {
                return 0;
            }
            if (src.Length > 1)  //至少两位才进入位数计算，避免单一个"十"字时无法识别或进入位数计算导致报错的问题
            {
                // 遍历字符数组
                for (int i = 0; i < charArr.Length; i++)
                {
                    // 遍历单位字符串
                    for (int j = 0; j < unitString.Length; j++)
                    {
                        // 如果字符为单位则进行计算
                        if (charArr[i] == unitString[j])
                        {
                            // 如果字符为非'零'字符，则计算出十位以上到万位以下数字的和
                            if (charArr[i] != '零')
                            {
                                result += Convert.ToInt32(int.Parse(numberString.IndexOf(charArr[i - 1]).ToString()) * Math.Pow(10, j));
                            }
                        }
                    }
                }
            }
            // 如果源文本末尾字符为'零'-'十'其中之一，则计算结果和个位数相加。
            if (numberString.IndexOf(charArr[charArr.Length - 1]) != -1)
            {
                result += numberString.IndexOf(charArr[charArr.Length - 1]);
            }
            // 返回计算结果。
            return result;
        }

        /// <summary>
        /// 数字转中文
        /// </summary>
        public static string NumberToChinese(this long number)
        {
            string res = string.Empty;
            string str = number.ToString();
            string schar = str.Substring(0, 1);
            switch (schar)
            {
                case "1":
                    res = "一";
                    break;
                case "2":
                    res = "二";
                    break;
                case "3":
                    res = "三";
                    break;
                case "4":
                    res = "四";
                    break;
                case "5":
                    res = "五";
                    break;
                case "6":
                    res = "六";
                    break;
                case "7":
                    res = "七";
                    break;
                case "8":
                    res = "八";
                    break;
                case "9":
                    res = "九";
                    break;
                default:
                    res = "零";
                    break;
            }
            if (str.Length > 1)
            {
                switch (str.Length)
                {
                    case 2:
                    case 6:
                        res += "十";
                        break;
                    case 3:
                    case 7:
                        res += "百";
                        break;
                    case 4:
                        res += "千";
                        break;
                    case 5:
                        res += "万";
                        break;
                    default:
                        res += "";
                        break;
                }
                res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
            }
            return res;
        }
    }
}
