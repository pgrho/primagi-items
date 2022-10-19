using System.Text.RegularExpressions;

namespace Shipwreck.PrimagiItems;

public sealed class Chapter : KeyedItem<string>
{
    public string? DisplayName
        => Key == null ? null
        : Key == "UNKNOWN" ? "その他"
        : Year is int y && No is int v ? y switch
        {
            1 => null,
            2 => "スタジオ",
            _ => $"{y}年目"
        } + $"第{v}章"
        : Key;

    public int? Year
        => Key == null ? null
        : Regex.Match(Key, "^P(?:\\d+)$") is var m && m.Success ? 1
        : Regex.Match(Key, "^(\\d)+P(?:\\d+)$") is var m2 && m2.Success ? int.Parse(m2.Groups[1].Value)
        : null;

    public int? No
        => Key == null ? null
        : Regex.Match(Key, "^(?:\\d)*P(\\d+)$") is var m2 && m2.Success ? int.Parse(m2.Groups[1].Value)
        : null;
}