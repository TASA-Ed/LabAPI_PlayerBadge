using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using System;
using System.Collections.Generic;

namespace LabAPI_PlayerBadge;

public class PlayerBadgeEvent : CustomEventsHandler {
    private const string DefaultColor = "white";
    private static readonly HashSet<string> Color = new(StringComparer.OrdinalIgnoreCase) {
        "pink", "red", "brown", "silver", "light_green", "crimson",
        "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta",
        "blue_green", "orange", "lime", "green", "emerald", "carmine",
        "nickel", "mint", "army_green", "pumpkin", "white"
    };

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        if (!PlayerBadgeMain.Singleton.BadgeCache.TryGetValue(ev.Player.UserId, out var badgeConfig))
            return;

        ev.Player.GroupName = badgeConfig.BadgeName;

        if (string.IsNullOrEmpty(badgeConfig.BadgeColor)) {
            Logger.Warn($"No color specified for player '{ev.Player.Nickname}'. Using default color 'red'.");
            ev.Player.GroupColor = DefaultColor;
        }
        else if (Color.Contains(badgeConfig.BadgeColor))
            ev.Player.GroupColor = badgeConfig.BadgeColor;
        else
        {
            Logger.Warn($"Invalid color '{badgeConfig.BadgeColor}' for player '{ev.Player.Nickname}'. Using default color 'red'.");
            ev.Player.GroupColor = DefaultColor;
        }
    }
}

