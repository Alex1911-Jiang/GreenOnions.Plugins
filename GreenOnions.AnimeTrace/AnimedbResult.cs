namespace GreenOnions.AnimeTrace
{
    public class AnimedbResult
    {
        public int code { get; set; }
        public DataObject[]? data { get; set; }
    }

    public class DataObject
    {
        public double[]? box { get; set; }
        #region -- 单个 --
        public string? name { get; set; }
        public string? cartoonname { get; set; }
        public double? acc_percen { get; set; }
        #endregion -- 单个 --
        #region -- 多个 --
        public CharObject[]? @char { get; set; }
        #endregion -- 多个 --
        public string? box_id { get; set; }

    }

    public class CharObject
    {
        public string? name { get; set; }
        public string? cartoonname { get; set; }
        public double acc { get; set; }
    }
}
