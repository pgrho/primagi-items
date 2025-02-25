using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class CoordinationItem
{
    [JsonIgnore]
    public Coordination? Coordination { get; set; }

    [JsonIgnore]
    public PrimagiDataSet? DataSet => Coordination?.DataSet;

    public int Id { get; set; }
    public string? ModelName { get; set; }
    public string? SealId { get; set; }

    private string? _Name;

    public string? Name
    {
        get => _Name ?? Coordination?.Name;
        set => _Name = value;
    }

    public bool ShouldSerializeName()
        => DataSet?.IgnoreCalculatedProperties != true
        || _Name != null && _Name != Coordination?.Name;

    public int Watcha { get; set; }

    #region Genre

    private int _GenreIndex;
    private Genre? _Genre;

    [JsonProperty("genre")]
    public int GenreIndex
    {
        get => _GenreIndex;
        set
        {
            if (value != _GenreIndex)
            {
                _GenreIndex = value;
                _Genre = null;
            }
        }
    }

    [JsonIgnore]
    public Genre? Genre
    {
        get => _Genre ??= DataSet?.GetGenre(GenreIndex);
        set
        {
            _Genre = value;
            _GenreIndex = value?.Key ?? 0;
        }
    }

    #endregion Genre

    #region Brand

    private int _BrandIndex;
    private Brand? _Brand;

    [JsonProperty("brand")]
    public int BrandIndex
    {
        get => _BrandIndex;
        set
        {
            if (value != _BrandIndex)
            {
                _BrandIndex = value;
                _Brand = null;
            }
        }
    }

    [JsonIgnore]
    public Brand? Brand
    {
        get => _Brand ??= DataSet?.GetBrand(BrandIndex);
        set
        {
            _Brand = value;
            _BrandIndex = value?.Key ?? 0;
        }
    }

    #endregion Brand

    #region Color

    private int _ColorIndex;
    private Color? _Color;

    [JsonProperty("color")]
    public int ColorIndex
    {
        get => _ColorIndex;
        set
        {
            if (value != _ColorIndex)
            {
                _ColorIndex = value;
                _Color = null;
            }
        }
    }

    [JsonIgnore]
    public Color? Color
    {
        get => _Color ??= DataSet?.GetColor(ColorIndex);
        set
        {
            _Color = value;
            _ColorIndex = value?.Key ?? 0;
        }
    }

    #endregion Color

    #region Rarity

    private int _RarityIndex;
    private Rarity? _Rarity;

    [JsonProperty("rarity")]
    public int RarityIndex
    {
        get => _RarityIndex;
        set
        {
            if (value != _RarityIndex)
            {
                _RarityIndex = value;
                _Rarity = null;
            }
        }
    }

    [JsonIgnore]
    public Rarity? Rarity
    {
        get => _Rarity ??= DataSet?.GetRarity(RarityIndex);
        set
        {
            _Rarity = value;
            _RarityIndex = value?.Key ?? 0;
        }
    }

    #endregion Rarity

    #region Category

    private int _CategoryIndex;
    private Category? _Category;

    [JsonProperty("category")]
    public int CategoryIndex
    {
        get => _CategoryIndex;
        set
        {
            if (value != _CategoryIndex)
            {
                _CategoryIndex = value;
                _Category = null;
            }
        }
    }

    [JsonIgnore]
    public Category? Category
    {
        get => _Category ??= DataSet?.GetCategory(CategoryIndex);
        set
        {
            _Category = value;
            _CategoryIndex = value?.Key ?? 0;
        }
    }

    #endregion Category

    #region SubCategory

    private int _SubCategoryIndex;
    private SubCategory? _SubCategory;

    [JsonProperty("subCategory")]
    public int SubCategoryIndex
    {
        get => _SubCategoryIndex;
        set
        {
            if (value != _SubCategoryIndex)
            {
                _SubCategoryIndex = value;
                _SubCategory = null;
            }
        }
    }

    [JsonIgnore]
    public SubCategory? SubCategory
    {
        get => _SubCategory ??= DataSet?.GetSubCategory(SubCategoryIndex);
        set
        {
            _SubCategory = value;
            _SubCategoryIndex = value?.Key ?? 0;
        }
    }

    #endregion SubCategory

    public bool IsShowItem { get; set; }

    [JsonIgnore]
    public int Icon { get; set; }

    [JsonIgnore]
    public int Release { get; set; }

    #region ImageUrl

    private string? _ImageUrl;

    public string? ImageUrl
    {
        get => _ImageUrl ??= GetDefaultImageUrl();
        set => _ImageUrl = value;
    }

    private string? GetDefaultImageUrl()
        => Coordination?.ChapterId == null ? null
        : $"https://www.takaratomy-arts.co.jp/specials/primagi/assets/images/item/{Coordination?.ChapterId}/{Id}.png";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true
        || (_ImageUrl != null && _ImageUrl != GetDefaultImageUrl());

    #endregion ImageUrl

    public bool Aipri { get; set; }
}