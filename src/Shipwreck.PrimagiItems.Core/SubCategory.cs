namespace Shipwreck.PrimagiItems;

public sealed class SubCategory : EnumBase
{
    public string? ImageUrl
        => Value == null ? null
        : $"https://cdn.primagi.jp/assets/images/item/common/ico_props_subCategory_{Key}_pc.png";
    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}