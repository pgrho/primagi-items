using System.Text.RegularExpressions;

namespace Shipwreck.PrimagiItems;

public sealed class Chapter : KeyedItem<string>
{
    public string? DisplayName
        => Key == null ? null
        : Regex.Match(Key, "^P(\\d+)$") is var m && m.Success ? m.Result("第$1章")
        : Key;
}
