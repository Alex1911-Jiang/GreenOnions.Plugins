# GreenOnions.Plugins
### [GreenOnions](https://github.com/Alex1911-Jiang/GreenOnions) 的各种插件

GreenOnions.KanCollectionTimeAnnouncer #舰C语音报时插件<br>
GreenOnions.RandomLocalPictures #随机本地图片插件<br>
GreenOnions.Replier #自定义回复插件 [查看示例](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/tree/main/GreenOnions.Replier)<br>
GreenOnions.TicTacToe_Windows #井字棋插件(仅限Windows) [查看示例](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/tree/main/GreenOnions.TicTacToe_Windows)<br>
GreenOnions.NovelAiClient #私服 NovelAi 插件 [查看示例](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/tree/main/GreenOnions.NovelAiClient)<br>
GreenOnions.GuessTheSong #听歌猜曲名小游戏插件（且已移植给葱葱小助手） [查看示例](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/tree/main/GreenOnions.GuessTheSong)<br>
GreenOnions.CustomHttpApiInvoker #自定义 API 客户端插件 [查看示例](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/blob/main/GreenOnions.CustomHttpApiInvoker) （自己有色图库的，或者什么随身API之类的，可以用它）<br>
~~GreenOnions.GPT3Client #GPT3 聊天插件，使用的是 API，需要自备 OpenAI 账号和 Api-Key~~（还能用，但不再维护了，建议改用 ChatGPT 插件）<br>
GreenOnions.NodeHttpClient #Node.js 插件，用于替代系统访问网络，需要有 Node.js 环境（建议只在发生SSL错误时安装，且不要和 Python 插件一起安装）<br>
GreenOnions.PythonHttpClient #Python 插件，用于替代系统访问网络，需要有 Python3 环境（建议只在发生SSL错误时安装，且不要和 Node.js 插件一起安装）<br>
GreenOnions.ChatGPTClient #ChatGPT 聊天插件，用户需自备 OpenAI Api-Key 和魔法上网，支持聊天保持上下文<br>
GreenOnions.AnimeTrace #通过 [animetrace](https://ai.animedb.cn/) 搜索角色名称的插件 [查看说明](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/tree/main/GreenOnions.AnimeTrace)<br>

#### 使用方法:
下载 [Release](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/releases) 解压到同名文件夹中, 把插件**文件夹**放在 GreenOnions\net6.0-windows\Plugins 或 GreenOnions\net6.0\Plugins 下<br>
启动 GreenOnions 后插件列表中有列出即为加载成功<br>

#### 开发插件:
如果你想自己开发一个 GreenOnions 的插件, 请看 [插件开发说明](https://github.com/Alex1911-Jiang/GreenOnions.Plugins/blob/main/Develop_ReadMe.md)
