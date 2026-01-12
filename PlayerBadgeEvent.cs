using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;

namespace LabAPI_PlayerBadge;

/// <summary>
/// 玩家徽章事件类。
/// </summary>
public class PlayerBadgeEvent : CustomEventsHandler {
    // 默认颜色
    private const string DefaultColor = "white";
    // 彩虹颜色
    private const string RainbowColor = "rainbow";
    // 支持的颜色列表
    private static readonly HashSet<string> Color = new(StringComparer.OrdinalIgnoreCase) {
        "pink", "red", "brown", "silver", "light_green", "crimson",
        "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta",
        "blue_green", "orange", "lime", "green", "emerald", "carmine",
        "nickel", "mint", "army_green", "pumpkin", "white",
        RainbowColor
    };

    // 彩虹颜色列表
    private static readonly string[] RainbowColors = [
        "red", "orange", "yellow", "lime", "green",
        "cyan", "aqua", "blue_green", "magenta", "pink"
    ];

    // 存储每个玩家的彩虹协程句柄
    private readonly Dictionary<string, CoroutineHandle> _rainbowCoroutines = new();

    // 玩家加入时触发
    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        // 检查玩家是否有徽章配置
        if (!PlayerBadgeMain.Singleton.BadgeCache.TryGetValue(ev.Player.UserId, out var badgeConfig))
            return;

        // 设置玩家的徽章名称和颜色
        ev.Player.GroupName = badgeConfig.BadgeName;

        if (string.IsNullOrEmpty(badgeConfig.BadgeColor)) {
            // 如果没有指定颜色，使用默认颜色
            Logger.Warn($"No color specified for player '{ev.Player.Nickname}'. Using default color '{DefaultColor}'.");
            ev.Player.GroupColor = DefaultColor;
        }
        // 处理彩虹颜色
        else if (badgeConfig.BadgeColor.Equals(RainbowColor, StringComparison.OrdinalIgnoreCase))
            StartRainbowEffect(ev.Player);
        // 处理普通颜色
        else if (Color.Contains(badgeConfig.BadgeColor))
            ev.Player.GroupColor = badgeConfig.BadgeColor;
        else {
            // 颜色无效，使用默认颜色
            Logger.Warn($"Invalid color '{badgeConfig.BadgeColor}' for player '{ev.Player.Nickname}'. Using default color '{DefaultColor}'.");
            ev.Player.GroupColor = DefaultColor;
        }
    }

    // 玩家离开时触发
    public override void OnPlayerLeft(PlayerLeftEventArgs ev) {
        // 玩家离开时停止彩虹效果，释放资源
        StopRainbowEffect(ev.Player.UserId);
    }

    private void StartRainbowEffect(Player player) {
        // 如果该玩家已有彩虹效果在运行，先停止
        StopRainbowEffect(player.UserId);

        // 启动新的彩虹协程
        var handle = Timing.RunCoroutine(RainbowCoroutine(player), Segment.Update);
        _rainbowCoroutines[player.UserId] = handle;
    }

    private void StopRainbowEffect(string userId) {
        // 停止并移除彩虹协程
        if (!_rainbowCoroutines.TryGetValue(userId, out var handle)) return;
        Timing.KillCoroutines(handle);
        _rainbowCoroutines.Remove(userId);
    }

    private IEnumerator<float> RainbowCoroutine(Player player) {
        var colorIndex = 0;

        var userId = player.UserId;

        while (true) {
            // 检查玩家是否仍然在线
            if (player.IsDestroyed) {
                StopRainbowEffect(userId);
                yield break;
            }

            // 检查玩家徽章配置是否仍为彩虹模式
            if (!PlayerBadgeMain.Singleton.BadgeCache.TryGetValue(userId, out var config) ||
                !config.BadgeColor.Equals(RainbowColor, StringComparison.OrdinalIgnoreCase)) {
                StopRainbowEffect(userId);
                yield break;
            }

            // 设置当前颜色
            player.GroupColor = RainbowColors[colorIndex];

            // 循环颜色索引
            colorIndex = (colorIndex + 1) % RainbowColors.Length;

            // 等待指定时间后切换下一个颜色
            yield return Timing.WaitForSeconds(PlayerBadgeMain.Singleton.RainbowInterval);
        }
    }

    /// <summary>
    /// 清理所有彩虹效果协程。
    /// </summary>
    public void CleanupAllRainbowEffects() {
        foreach (var handle in _rainbowCoroutines.Values)
            Timing.KillCoroutines(handle);
        
        _rainbowCoroutines.Clear();
    }
}

