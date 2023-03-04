using GreenOnions.Interface;
using GreenOnions.Interface.Configs;
using Newtonsoft.Json;

namespace GreenOnions.RandomLocalPictures
{
    public class PicutreSender : IMessagePlugin, IPluginSetting
    {
        private Dictionary<string, SourcesInfo> _cmdToConfig = new Dictionary<string, SourcesInfo>();
        public string Name => "本地随机色图";

        public string Description => "通过命令随机发送本地指定目录的图片";

        public void OnConnected(long selfId, IGreenOnionsApi api)
        {

        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath, IBotConfig botConfig)
        {
            string configPath = Path.Combine(pluginPath, "config.json");
            List<SourcesInfo> sourcesInfos;
            if (File.Exists(configPath))
            {
                sourcesInfos = JsonConvert.DeserializeObject<List<SourcesInfo>>(File.ReadAllText(configPath))!;
            }
            else
            {
                //配置文件例子
                sourcesInfos = new List<SourcesInfo>
                {
                    new SourcesInfo()
                    {
                        Cmds = new [] { "setu", "色图来" },
                        PictureSourcePath = @"C:\图库1",
                        SearchItemDirect = true,
                    },
                    new SourcesInfo()
                    {
                        Cmds = new [] { "本地第二个图库命令" },
                        PictureSourcePath = @"D:\图库2",
                        SearchItemDirect = false,
                    }
                };
            }
            File.WriteAllText(configPath, JsonConvert.SerializeObject(sourcesInfos, Formatting.Indented));
            CreateCmdToPathDic(sourcesInfos);
        }

        private void CreateCmdToPathDic(List<SourcesInfo> sourcesInfos)
        {
            if (sourcesInfos != null)
            {
                for (int i = 0; i < sourcesInfos.Count; i++)
                {
                    for (int j = 0; j < sourcesInfos[i].Cmds.Length; j++)
                    {
                        if (_cmdToConfig.ContainsKey(sourcesInfos[i].Cmds[j]))
                            _cmdToConfig[sourcesInfos[i].Cmds[j]] = sourcesInfos[i];
                        else
                            _cmdToConfig.Add(sourcesInfos[i].Cmds[j], sourcesInfos[i]);
                    }
                }
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.FirstOrDefault() is GreenOnionsTextMessage textMsg)
            {
                if (_cmdToConfig.ContainsKey(textMsg.Text))
                {
                    if (!Directory.Exists(_cmdToConfig[textMsg.Text].PictureSourcePath))
                    {
                        Response($"图库<{textMsg.Text}>不存在");
                        return true;
                    }
                    string[] imgs = Directory.GetFiles(_cmdToConfig[textMsg.Text].PictureSourcePath, "*", _cmdToConfig[textMsg.Text].SearchItemDirect ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    Response(new GreenOnionsMessages(new GreenOnionsImageMessage(imgs[rdm.Next(0, imgs.Length)])) { Reply = false });
                    return true;
                }
            }
            return false;
        }

        public void Setting()
        {
            throw new Exception("请进入插件目录修改config.json配置文件");
        }

        public struct SourcesInfo
        {
            public string[] Cmds { get; set; }
            public string PictureSourcePath { get; set; }
            public bool SearchItemDirect { get; set; }
        }
    }
}