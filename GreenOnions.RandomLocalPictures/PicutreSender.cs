﻿using GreenOnions.Interface;
using Newtonsoft.Json;

namespace GreenOnions.RandomLocalPictures
{
    public class PicutreSender : IPlugin
    {
        private List<SourcesInfo> _sourcesInfos;
        public string Name => "本地随机色图";

        public string Description => "通过命令随机发送本地指定目录的图片";

        public void ConsoleSetting()
        {
            Console.WriteLine("本插件不通过运行中设置功能, 请手动修改插件目录下的config.json后重启机器人");
        }

        public void OnConnected(long selfId, Func<long, GreenOnionsMessages, Task<int>> SendFriendMessage, Func<long, GreenOnionsMessages, Task<int>> SendGroupMessage, Func<long, long, GreenOnionsMessages, Task<int>> SendTempMessage, Func<Task<List<GreenOnionsFriendInfo>>> GetFriendListAsync, Func<Task<List<GreenOnionsGroupInfo>>> GetGroupListAsync, Func<long, Task<List<long>>> GetMemberListAsync, Func<long, long, Task<GreenOnionsMemberInfo>> GetMemberInfoAsync)
        {

        }

        public void OnDisconnected()
        {

        }

        public void OnLoad(string pluginPath)
        {
            string configPath = Path.Combine(pluginPath, "config.json");
            if (File.Exists(configPath))
                _sourcesInfos = JsonConvert.DeserializeObject<List<SourcesInfo>>(File.ReadAllText(configPath))!;
            else
            {
                //配置文件例子
                _sourcesInfos = new List<SourcesInfo>();
                _sourcesInfos.Add(new SourcesInfo() 
                { 
                    Cmds = new List<string>() { "setu", "色图来" },
                    PictureSourcePath = @"C:\图库1" 
                });
                _sourcesInfos.Add(new SourcesInfo()
                {
                    Cmds = new List<string>() { "本地第二个图库命令" },
                    PictureSourcePath = @"D:\图库2"
                });
                File.WriteAllText(configPath, JsonConvert.SerializeObject(_sourcesInfos));
            }
        }

        public bool OnMessage(GreenOnionsMessages msgs, long? senderGroup, Action<GreenOnionsMessages> Response)
        {
            if (msgs.FirstOrDefault() is GreenOnionsTextMessage textMsg)
            {
                for (int i = 0; i < _sourcesInfos?.Count; i++)
                {
                    for (int j = 0; j < _sourcesInfos[i].Cmds?.Count; j++)
                    {
                        if (textMsg.Text == _sourcesInfos[i].Cmds[j])
                        {
                            if (Directory.Exists(_sourcesInfos[i].PictureSourcePath))
                            {
                                string[] imgs = Directory.GetFiles(_sourcesInfos[i].PictureSourcePath);
                                Random rdm = new Random(Guid.NewGuid().GetHashCode());
                                Response(new GreenOnionsMessages(new GreenOnionsImageMessage(imgs[rdm.Next(0, imgs.Length)])) { Reply = false});
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool WindowSetting()
        {
            return false;
        }

        public struct SourcesInfo
        {
            public List<string> Cmds { get; set; }
            public string PictureSourcePath { get; set; }
        }
    }
}