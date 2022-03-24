using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class Coordination
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public int Collection { get; set; }
    public string? Name { get; set; }
    public string? Kinds { get; set; }
    public int DirectoryNumber { get; set; }
    public bool IsShow { get; set; }
    public bool HasMainImage { get; set; }
    public string? Span { get; set; }
    public int Order { get; set; }
    public string? Chapter { get; set; }

    #region Items

    private CoordinationItemCollection? _Items;

    public IList<CoordinationItem> Items
    {
        get => _Items ??= new(this);
        set
        {
            if (value != _Items)
            {
                _Items?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Items.Add(e);
                    }
                }
            }
        }
    }

    #endregion Items

    public string? ImageUrl
        => Chapter == null || !HasMainImage ? null
        : $"https://cdn.primagi.jp/assets/images/item/{Chapter}/img_codination_{DirectoryNumber}_main.jpg";
    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}