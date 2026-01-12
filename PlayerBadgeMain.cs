using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabAPI_PlayerBadge;

/// <summary>
/// 无限子弹 插件。
/// </summary>
public class PlayerBadgeMain : Plugin {
    /// <summary>
    /// 单例模式。
    /// </summary>
    public static PlayerBadgeMain Singleton { get; private set; }

    /// <summary>
    /// 插件名称。
    /// </summary>
    public override string Name => "Player Badge Plugin";
    /// <summary>
    /// 插件描述。
    /// </summary>
    public override string Description => "Award a badge when a player joins";
    /// <summary>
    /// 插件作者。
    /// </summary>
    public override string Author => "TASA-Ed Studio";
    /// <summary>
    /// 插件版本。
    /// </summary>
    public override Version Version => new(1, 0, 0, 0);

    /// <summary>
    /// 需要的 LabApi 版本。
    /// </summary>
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    /// <summary>
    /// 自定义事件处理器。
    /// </summary>
    public PlayerBadgeEvent Events { get; private set; }

    /// <summary>
    /// 徽章缓存。
    /// </summary>
    public Dictionary<string, ListType> BadgeCache { get; set; }

    // 加载配置时
    public override void LoadConfigs()
    {
        base.LoadConfigs();
        var config = this.LoadConfig<PlayerBadgeConfig>("configs.yml") ?? new PlayerBadgeConfig();
        BadgeCache = config.PlayerBadgeList
            .ToDictionary(x => x.SteamId64, x => x);
    }

    // 启用插件。
    public override void Enable() {
        Singleton = this;
        Events = new PlayerBadgeEvent();
        CustomHandlersManager.RegisterEventsHandler(Events);
    }

    // 禁用插件。
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(Events);
        Events = null;
        BadgeCache = null;
        Singleton = null;
    }
}

/// <summary>
/// 插件配置类。
/// </summary>
public class PlayerBadgeConfig {
    // 所有配置必须为可 set
    /// <summary>
    /// 玩家徽章列表。如 [new ListType { SteamId64, BadgeName, BadgeColor }]。
    /// </summary>
    public ListType[] PlayerBadgeList { get; set; } = [new() {
        SteamId64 = "7656@steam",
        BadgeName = "徽章名称",
        BadgeColor = "red"
    }];
}

public class ListType {
    /// <summary>
    /// 玩家 SteamID64。
    /// </summary>
    public string SteamId64 { get; set; }
    /// <summary>
    /// 徽章名称。
    /// </summary>
    public string BadgeName { get; set; }
    /// <summary>
    /// 徽章颜色。
    /// </summary>
    public string BadgeColor { get; set; }
}
