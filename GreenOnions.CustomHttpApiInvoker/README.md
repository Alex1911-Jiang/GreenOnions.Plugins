# GreenOnions.CustomHttpApiInvoker
[GreenOnions](https://github.com/Alex1911-Jiang/GreenOnions)的自定义HttpApi客户端插件

可用QQ消息调用任意HttpApi，并提供自定义解析，@消息替换等功能<br>
不含任何业务功能和具体API地址，地址需要用户自己填写<br>

功能预览:

API列表：<br>
![API列表](https://user-images.githubusercontent.com/50268952/205450032-9833771e-fc99-41e7-9c5a-adb1ed498a6c.jpg)<br>
编辑请求：<br>
![流](https://user-images.githubusercontent.com/50268952/205450079-a5892042-b8d8-4286-b187-e8f8b2969f41.jpg)<br>
效果：<br>
![示范1](https://user-images.githubusercontent.com/50268952/205450093-e5889a36-7401-4f19-bdae-9ad4811d81d5.jpg)<br>

再补充一个调[Lolicon](https://api.lolicon.app/)API的例子：<br>
![lolicon配置](https://user-images.githubusercontent.com/50268952/205450166-7e3ba4b3-6516-4749-9a81-ff05dcef9bcc.jpg)<br>
![lolicon示范](https://user-images.githubusercontent.com/50268952/205450173-cda379eb-cb23-4b29-8b41-1637058baa31.jpg)<br>

参数说明：<br>
1. 在命令栏可以添加 (?\<参数\>表达式) 正则表达式提取子串式，用于提取命令中的参数，"\<参数\>" 这个子串名称是固定的，不可修改，但应当在字串名后面添加适当的表达式用于匹配合法的参数，例如 "(?\<参数\>.+)" 用于匹配命令后的任意长度任意文字<br>
2. 在地址栏、Header Value、Content Value 中可以添加 \<参数\> 标签，用于将第 1 条从 QQ 消息中提取到的实参替换此标签<br>
3. 解析表达式语法为方括号括住节点名称或序号，每个方括号访问一层节点，可以在方括号中插入特殊标签\<random\>从数组中随机获取一个（前提该节点是数组），例："[data][urls][\<random\>]"<br>
4. 解析表达式换行会重新对结果进行解析，可以在一个请求中写多行解析表达式，将多行不同的解析结果存放在一条QQ消息内返回<br>

食用方法: 下载Release并解压到GreenOnions\Plugins\下
