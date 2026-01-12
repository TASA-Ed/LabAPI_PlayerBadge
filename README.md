# LabAPI_PlayerBadge

[![GitHub release](https://flat.badgen.net/github/release/TASA-Ed/LabAPI_SuicideCommand)](https://github.com/TASA-Ed/LabAPI_SuicideCommand/releases)
[![LabAPI Version](https://flat.badgen.net/static/LabAPI/v1.1.4)](https://github.com/northwood-studios/LabAPI)
[![License](https://flat.badgen.net/github/license/TASA-Ed/LabAPI_SuicideCommand/)](https://github.com/TASA-Ed/LabAPI_SuicideCommand/blob/master/LICENSE)

A LabAPI plugin that awards badges when players join.

一个 LabAPI 插件，当玩家加入时授予徽章。

## Use / 使用

Download LabAPI_PlayerBadge_x64.dll from Releases and place it in `%appdata%\SCP Secret Laboratory\LabAPI\plugins\<port>`.

从 Releases 中下载 LabAPI_PlayerBadge_x64.dll 并放入 `%appdata%\SCP Secret Laboratory\LabAPI\plugins\<port>`。

All done!

大功告成！

## Config / 配置

```yaml
# Badge list | 徽章列表
player_badge_list:
- player_id: PlayerId@steam # SteamId 64 or DiscordId | SteamId 64 或 DiscordId
  badge_name: 徽章名称 # Badge name | 徽章名称
  badge_color: red # Badge color | 徽章颜色
- player_id: PlayerId@discord
  badge_name: 徽章名称
  badge_color: red
```

`badge_color` must use only the following colors:

`badge_color` 只能使用下列颜色：

```csharp
private static readonly HashSet<string> Color = new(StringComparer.OrdinalIgnoreCase) {
    "pink", "red", "brown", "silver", "light_green", "crimson",
    "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta",
    "blue_green", "orange", "lime", "green", "emerald", "carmine",
    "nickel", "mint", "army_green", "pumpkin", "white"
};
```
