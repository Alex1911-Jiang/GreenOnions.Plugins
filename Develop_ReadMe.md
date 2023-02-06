## GreenOnions 插件开发指南

~~其实这个文档是怕自己忘了要配什么参数才写的~~

### 注意: 
GreenOnions 只能加载 .Net 框架的动态库作为插件, 但插件自身可以引用其他语言导出的的过程库

### 开发准备:
1.创建一个 .Net 框架的项目<br>
2.引用 GreenOnions.Interface<br>
3.新建一个类, 实现 IMessagePlugin 接口 (GreenOnions 会实例化所有实现了 IPlugin 接口的类)<br>
4.开始开发 (消息插件接口 IMessagePlugin 和基本插件接口 IPlugin 内有详细的方法和属性注释说明)<br>

### 提示: 
任何的插件项目都应被标记为启用动态加载的程序集 EnableDynamicLoading

```XML
  <PropertyGroup>
    <EnableDynamicLoading>true</EnableDynamicLoading>
  </PropertyGroup>
```

如果您的插件引用了 nuget 程序包, 您应当在生成插件的动态库后将 "插件目录\runtimes\" 下的所有适用于当前系统的库文件复制到 "插件目录" 并替换(没有当前框架版本时, 取最接近当前框架但小于当前框架的版本)

以下以 Microsoft.Data.SqlClient 为例子 (不应使用 System.Data.SqlClient):

```
//插件版本 .net 6.0
复制 "插件\bin\Debug\net6.0\runtimes\win\lib\netcoreapp3.1\Microsoft.Data.SqlClient.dll" 到 "插件\Microsoft.Data.SqlClient.dll"  //没有 .net 6.0 版本, 最接近的是 .net core 3.1, 就复制 3.1 的
复制 "插件\bin\Debug\net6.0\runtimes\win-x64\native\Microsoft.Data.SqlClient.SNI.dll" 到 "插件\Microsoft.Data.SqlClient.SNI.dll"  //根据系统复制对应的
```

特别的: <br>
Microsoft.Data.SqlClient 这个库还需要在 new SqlConnection() 前设置以下属性:
```C#
AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.UseSystemDefaultSecureProtocols", true);
```

