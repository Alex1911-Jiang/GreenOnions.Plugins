using GreenOnions.NT.Base;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace GreenOnions.NT.WakeOnLan;

public class WakeOnLanPlugin : IPlugin
{
    private Config? _config;
    private string? _pluginPath;
    private BotContext? _bot;
    private ICommonConfig? _commonConfig;
    public string Name => "魔术包开机";
    public string Description => "像局域网广播魔术包尝试唤醒设备";
    public void OnConfigUpdated(ICommonConfig commonConfig)
    {
        _commonConfig = commonConfig;
        if (_pluginPath is null)
            return;
        LoadConfig(_pluginPath);
    }
    private void LoadConfig(string pluginPath)
    {
        var configPath = Path.Combine(pluginPath, "config.yml");
        if (File.Exists(configPath))
        {
            string yamlConfig = File.ReadAllText(configPath);
            _config = YamlConvert.DeserializeObject<Config>(yamlConfig);
        }
        _config ??= new Config();
        File.WriteAllText(configPath, YamlConvert.SerializeObject(_config));
    }

    public void OnLoaded(string pluginPath, BotContext bot, ICommonConfig commonConfig)
    {
        _pluginPath = pluginPath;
        _bot = bot;
        _commonConfig = commonConfig;
        LoadConfig(_pluginPath);

        bot.Invoker.OnFriendMessageReceived -= OnFriendMessageReceived;
        bot.Invoker.OnFriendMessageReceived += OnFriendMessageReceived;
        
        bot.Invoker.OnGroupMessageReceived -= OnGroupMessageReceived;
        bot.Invoker.OnGroupMessageReceived += OnGroupMessageReceived;
    }

    private async void OnGroupMessageReceived(BotContext context, GroupMessageEvent e)
    {
        if (e.Chain.FriendUin == context.BotUin)
            return;
        
        if (!e.Chain.AllowUseIfDebug())
            return;

        if (!Requirement(e.Chain))
            return;
        await SendMagicPacket(e.Chain);
    }

    private async void OnFriendMessageReceived(BotContext context, FriendMessageEvent e)
    {
        if (e.Chain.FriendUin == context.BotUin)
            return;
        
        if (!e.Chain.AllowUseIfDebug())
            return;

        if (!Requirement(e.Chain))
            return;
        await SendMagicPacket(e.Chain);
    }

    private bool Requirement(MessageChain chain)
    {
        if (_commonConfig is null)
        {
            LogHelper.LogWarning("机器人配置为空");
            return false;
        }
        if (_config is null)
        {
            LogHelper.LogWarning($"{Name}插件配置为空");
            return false;
        }
        
        if (!_commonConfig.AdminQQ.Contains(chain.FriendUin)) 
            return false;
        
        foreach (var entity in chain)
        {
            if (entity is TextEntity textEntity)
            {
                if (textEntity.Text.Contains("魔术包开机", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private async Task SendMagicPacket(MessageChain chain)
    {
        var builder = chain.GroupUin is not null ? MessageBuilder.Group((uint)chain.GroupUin) : MessageBuilder.Friend(chain.FriendUin);
        builder.Add(new TextEntity("正在发送魔术包..."));
        _bot?.SendMessage(builder.Build());
        var result = await MagicPacketHelper.SendMagicPacket(_config?.Mac, _config?.Ip);
        builder = chain.GroupUin is not null ? MessageBuilder.Group((uint)chain.GroupUin) : MessageBuilder.Friend(chain.FriendUin);
        switch (result)
        {
            case WakeStatus.InvalidInput:
                builder.Add(new TextEntity("魔术包开机失败，MAC地址或IP地址无效。请检查配置。"));
                break;
            case WakeStatus.AlreadyAwake:
                builder.Add(new TextEntity("魔术包开机成功，但设备已经处于唤醒状态。"));
                break;
            case WakeStatus.WakeSuccessful:
                builder.Add(new TextEntity("魔术包开机成功！设备已唤醒。"));
                break;
            case WakeStatus.WakeTimeout:
                builder.Add(new TextEntity("魔术包开机超时。请检查设备是否支持唤醒功能，或网络连接是否正常。"));
                break;
            case WakeStatus.Error:
                builder.Add(new TextEntity("魔术包开机失败，发生未知错误。请检查配置或网络连接。"));
                break;
        }
        _bot?.SendMessage(builder.Build());
    }
}