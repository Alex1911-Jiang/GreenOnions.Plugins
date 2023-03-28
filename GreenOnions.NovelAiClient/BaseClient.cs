namespace GreenOnions.NovelAiClient
{
    public abstract class BaseClient
    {
        protected string _host;
        public BaseClient(string host)
        {
            _host = host;
        }
    }
}
