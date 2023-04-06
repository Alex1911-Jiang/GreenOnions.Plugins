接入 [animetrace](https://ai.animedb.cn/) 搜动漫/游戏角色的插件

应该没几个人用吧，懒得做设计器了（

配置文件说明：

```Json
{
  "StartSearchCommand": "<机器人名称>搜角色",  //使用配置文件中所选的模型搜索的命令
  "StartSearchInAnimeCommand": "<机器人名称>搜动漫角色",  //使用默认动漫模型搜索的命令
  "StartSearchInGameCommand": "<机器人名称>搜游戏角色",  //使用默认游戏模型搜索的命令
  "SearchStartReply": "请发送图片",  //进入搜索模式等待图片时的回复语
  "SearchErrortReply": "搜索失败QAQ\r\n(<错误信息>)",  //搜索失败回复语
  "model": 0,  //选择使用的模型，和网站上的顺序对应（0=动漫模型，1=公测动漫模型no1，2=高级动漫模型，3=GalGame模型）
  "force_one": 1  //是否为多结果模式，0=单结果，1=多结果
}
```
