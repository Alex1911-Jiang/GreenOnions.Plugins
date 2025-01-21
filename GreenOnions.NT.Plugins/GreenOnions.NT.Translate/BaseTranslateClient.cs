namespace GreenOnions.NT.Translate
{
    internal abstract class BaseTranslateClient
    {
        internal Config Config { get; }

        internal BaseTranslateClient(Config config)
        {
            Config = config;
        }

        internal abstract Dictionary<string, string> LanguageCodes { get; }

        internal abstract Task<string> TranslateToChinese( string text);

        internal abstract Task<string> TranslateTo(string text, string toLanguageChineseName);

        internal abstract Task<string> TranslateFromTo(string text, string fromLanguageChineseName, string toLanguageChineseName);

        internal string ChineseToCode(string languageName)
        {
            languageName = languageName.Replace("语", "文");
            if (LanguageCodes.TryGetValue(languageName, out string? code))
                return code;
            return languageName;
        }
    }
}
