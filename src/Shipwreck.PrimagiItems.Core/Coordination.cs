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

    #region Chapter

    private string? _ChapterId;
    private Chapter? _Chapter;

    [JsonProperty(nameof(Chapter))]
    public string? ChapterId
    {
        get => _ChapterId;
        set
        {
            if (value != _ChapterId)
            {
                _ChapterId = value;
                _Chapter = null;
            }
        }
    }

    [JsonIgnore]
    public Chapter? Chapter
    {
        get => _Chapter ??= DataSet?.GetChapter(ChapterId);
        set
        {
            _Chapter = value;
            _ChapterId = value?.Key;
        }
    }

    #endregion Chapter

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
        => ChapterId == null || !HasMainImage ? null
        : $"https://cdn.primagi.jp/assets/images/item/{ChapterId}/img_codination_{DirectoryNumber}_main.jpg";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}