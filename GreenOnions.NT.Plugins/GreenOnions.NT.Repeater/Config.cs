using YamlDotNet.Serialization;

namespace GreenOnions.NT.Repeater
{
    internal class Config
    {
        [YamlMember(Description = "群号和对应的复读配置（群号填0将会应用到所有群）")]
        public Dictionary<uint, RepeatSetting> RepeatSettings = new Dictionary<uint, RepeatSetting>()
        {
            {
                550398174, new RepeatSetting
                {
                    Enabled = true,
                    Remark = "葱葱机器人群",
                    RandomProbability = 0.01,
                    SuccessiveRepeatCount = 3,
                    RepeatImage = true,
                    HorizontalMirrorImageProbability = 0.5,
                    VerticalMirrorImageProbability = 0.5,
                    RewindGifProbability = 0.5,
                    ReplaceWords = new Dictionary<string, string>
                    {
                        { "我","你" },
                        { "I'm","You're" },
                        { "I am","You are" },
                        { "me","you" },
                    },
                    IgnoreWords = ["撅"],
                }
            },
        };
    }

    internal class RepeatSetting
    {
        [YamlMember(Description = "启用复读")]
        public bool Enabled { get; set; }

        [YamlMember(Description = "备注（无意义，仅用于帮助用户记忆配置）")]
        public string Remark { get; set; }

        [YamlMember(Description = "随机复读概率（0.01为1%）")]
        public double RandomProbability { get; set; }

        [YamlMember(Description = "连续复读次数")]
        public int SuccessiveRepeatCount { get; set; }

        [YamlMember(Description = "复读图片")]
        public bool RepeatImage { get; set; }

        [YamlMember(Description = "水平镜像复读图片概率（0.5为50%）")]
        public double HorizontalMirrorImageProbability { get; set; }

        [YamlMember(Description = "水平镜像复读图片概率（0.5为50%）")]
        public double VerticalMirrorImageProbability { get; set; }

        [YamlMember(Description = "倒放Gif概率（0.5为50%）")]
        public double RewindGifProbability { get; set; } = 0.5;

        [YamlMember(Description = "复读时的替换词")]
        public Dictionary<string, string> ReplaceWords { get; set; } = new Dictionary<string, string>();

        [YamlMember(Description = "忽略词（句子中只要包含以下任意词语则不会复读）")]
        public HashSet<string> IgnoreWords { get; set; } = new HashSet<string>();
    }
}
