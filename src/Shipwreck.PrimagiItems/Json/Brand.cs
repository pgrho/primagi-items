namespace Shipwreck.PrimagiItems.Json;

public sealed class Brand : EnumBase
{
    public string? ImageUrl
        => Value == null ? null
        : $"https://cdn.primagi.jp/assets/images/item/common/ico_props_brand_{Key}_pc.png";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}
