# GreenOnions.GuessTheSong
[GreenOnions](https://github.com/Alex1911-Jiang/GreenOnions)的听歌猜曲名游戏插件

对接的是网易云音乐平台

功能预览：（暴露假粉身份时刻）
![假粉](https://user-images.githubusercontent.com/50268952/203346042-8c90e8b2-e2c1-4b40-bd89-a42e2d1c5976.jpg)

配置文件说明：
![猜歌配置文件](https://user-images.githubusercontent.com/50268952/203346052-da3449b7-8739-45df-bc02-6c5155b9f4dc.jpg)

当 MusicIDAndAnswers 有值时进入自定义模式，忽略 SearchKeywords 属性，每次只在自定义的歌曲列表里随机一个
当 MusicIDAndAnswers 没有值时，使用 SearchKeywords 中的随机一个关键词进行搜索，在所有的搜索结果中随机一首歌曲，答案为网易云上该歌曲名称或翻译名称
当 MusicIDAndAnswers 和 SearchKeywords 都没有值时，该插件会自动停用
