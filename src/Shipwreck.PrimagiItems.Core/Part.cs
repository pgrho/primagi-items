using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class Shop
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Prefecture { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ShopGroup { get; set; }
}
public sealed class Part
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public string? Id { get; set; }
    public int? File { get; set; }
    public bool IsNew { get; set; }
    public DateTime? Date { get; set; }

    public string? PartsName { get; set; }
    public string? Psd { get; set; }

    public string? ModalText { get; set; }
    public string? Destination { get; set; }
    public string? Url { get; set; }

    #region Type

    private int? _TypeId;
    private PartGetType? _Type;

    [JsonProperty("type")]
    public int? TypeId
    {
        get => _TypeId;
        set
        {
            if (value != _TypeId)
            {
                _TypeId = value;
                _Type = null;
            }
        }
    }

    [JsonIgnore]
    public PartGetType? Type
    {
        get => _Type ??= (TypeId == null ? null : DataSet?.GetPartGetType(TypeId.Value));
        set
        {
            _Type = value;
            _TypeId = value?.Key;
        }
    }

    #endregion Type

    #region Category

    private int? _CategoryId;
    private PartCategory? _Category;

    [JsonProperty("category")]
    public int? CategoryId
    {
        get => _CategoryId;
        set
        {
            if (value != _CategoryId)
            {
                _CategoryId = value;
                _Category = null;
            }
        }
    }

    [JsonIgnore]
    public PartCategory? Category
    {
        get => _Category ??= (CategoryId == null ? null : DataSet?.GetPartCategory(CategoryId.Value));
        set
        {
            _Category = value;
            _CategoryId = value?.Key;
        }
    }

    #endregion Category

    public string? ImageUrl
        => Id == null ? null
        : $"https://cdn.primagi.jp/assets/images/parts/thumb/img_{Id}.png";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}
