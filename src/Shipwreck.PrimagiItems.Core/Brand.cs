namespace Shipwreck.PrimagiItems;

public sealed class Brand : EnumBase
{
    public string? ImageUrl
        => Value == null ? null
        : $"https://www.takaratomy-arts.co.jp/specials/primagi/assets/images/item/common/ico_props_brand_{Key}_pc.png";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}