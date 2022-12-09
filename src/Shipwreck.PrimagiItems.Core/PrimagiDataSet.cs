using System.Text;
using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class PrimagiDataSet
{
    [JsonIgnore]
    public bool IgnoreCalculatedProperties { get; set; }

    #region Chapters

    private KeyedCollection<string, Chapter>? _Chapters;

    public IList<Chapter> Chapters
    {
        get
        {
            if (_Chapters == null)
            {
                _Chapters = new KeyedCollection<string, Chapter>(this);
                if (_Coordinations != null)
                {
                    foreach (var cid in _Coordinations.Select(e => e.ChapterId).Distinct())
                    {
                        GetChapter(cid);
                    }
                }
            }
            return _Chapters;
        }
        set => this.Set(ref _Chapters, value);
    }

    public bool ShouldSerializeChapters()
        => !IgnoreCalculatedProperties;

    internal Chapter? GetChapter(string? key)
    {
        if (key == null)
        {
            return null;
        }
        Chapter? g;
        if (!((KeyedCollection<string, Chapter>)Chapters).TryGetValue(key, out g))
        {
            Chapters.Add(g = new Chapter() { Key = key });
        }

        return g;
    }

    internal void EnsureChapter(string? key)
    {
        if (_Chapters != null)
        {
            GetChapter(key);
        }
    }

    #endregion Chapters

    #region Genres

    private KeyedCollection<int, Genre>? _Genres;

    public IList<Genre> Genres
    {
        get => this.GetOrCreate(ref _Genres);
        set => this.Set(ref _Genres, value);
    }

    internal Genre? GetGenre(int id)
        => _Genres != null
        && _Genres.TryGetValue(id, out var g) ? g : null;

    #endregion Genres

    #region Brands

    private KeyedCollection<int, Brand>? _Brands;

    public IList<Brand> Brands
    {
        get => this.GetOrCreate(ref _Brands);
        set => this.Set(ref _Brands, value);
    }

    internal Brand? GetBrand(int id)
        => _Brands != null
        && _Brands.TryGetValue(id, out var g) ? g : null;

    #endregion Brands

    #region Colors

    private KeyedCollection<int, Color>? _Colors;

    public IList<Color> Colors
    {
        get => this.GetOrCreate(ref _Colors);
        set => this.Set(ref _Colors, value);
    }

    internal Color? GetColor(int id)
        => _Colors != null
        && _Colors.TryGetValue(id, out var g) ? g : null;

    #endregion Colors

    #region Rarities

    private KeyedCollection<int, Rarity>? _Rarities;

    public IList<Rarity> Rarities
    {
        get => this.GetOrCreate(ref _Rarities);
        set => this.Set(ref _Rarities, value);
    }

    internal Rarity? GetRarity(int id)
        => _Rarities != null
        && _Rarities.TryGetValue(id, out var g) ? g : null;

    #endregion Rarities

    #region Categories

    private KeyedCollection<int, Category>? _Categories;

    public IList<Category> Categories
    {
        get => this.GetOrCreate(ref _Categories);
        set => this.Set(ref _Categories, value);
    }

    internal Category? GetCategory(int id)
        => _Categories != null
        && _Categories.TryGetValue(id, out var g) ? g : null;

    #endregion Categories

    #region SubCategories

    private KeyedCollection<int, SubCategory>? _SubCategories;

    public IList<SubCategory> SubCategories
    {
        get => this.GetOrCreate(ref _SubCategories);
        set => this.Set(ref _SubCategories, value);
    }

    internal SubCategory? GetSubCategory(int id)
        => _SubCategories != null
        && _SubCategories.TryGetValue(id, out var g) ? g : null;

    #endregion SubCategories

    #region Coordinations

    private CoordinationCollection? _Coordinations;

    public IList<Coordination> Coordinations
    {
        get => _Coordinations ??= new(this);
        set
        {
            if (value != _Coordinations)
            {
                _Coordinations?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Coordinations.Add(e);
                    }
                }
            }
        }
    }

    #endregion Coordinations

    #region PartCategories

    private KeyedCollection<int, PartCategory>? _PartCategories;

    public IList<PartCategory> PartCategories
    {
        get => this.GetOrCreate(ref _PartCategories);
        set => this.Set(ref _PartCategories, value);
    }

    internal PartCategory? GetPartCategory(int id)
        => _PartCategories != null
        && _PartCategories.TryGetValue(id, out var g) ? g : null;

    #endregion PartCategories

    #region PartGetTypes

    private KeyedCollection<int, PartGetType>? _PartGetTypes;

    public IList<PartGetType> PartGetTypes
    {
        get => this.GetOrCreate(ref _PartGetTypes);
        set => this.Set(ref _PartGetTypes, value);
    }

    internal PartGetType? GetPartGetType(int id)
        => _PartGetTypes != null
        && _PartGetTypes.TryGetValue(id, out var g) ? g : null;

    #endregion PartGetTypes

    #region Parts

    private PartCollection? _Parts;

    public IList<Part> Parts
    {
        get => _Parts ??= new(this);
        set
        {
            if (value != _Parts)
            {
                _Parts?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Parts.Add(e);
                    }
                }
            }
        }
    }

    #endregion Parts

    #region Shops

    private ShopCollection? _Shops;

    public IList<Shop> Shops
    {
        get => _Shops ??= new(this);
        set
        {
            if (value != _Shops)
            {
                _Shops?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Shops.Add(e);
                    }
                }
            }
        }
    }

    #endregion Shops

    public static PrimagiDataSet Parse(Stream stream)
    {
        using (var sr = new StreamReader(stream, Encoding.UTF8, false, -1, true))
        {
            return Parse(sr);
        }
    }

    public static PrimagiDataSet Parse(TextReader reader)
    {
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<PrimagiDataSet>(json)!;
    }

    public static Task<PrimagiDataSet> ParseAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using (var sr = new StreamReader(stream, Encoding.UTF8, false, -1, true))
        {
            return ParseAsync(sr, cancellationToken);
        }
    }

    public static async Task<PrimagiDataSet> ParseAsync(TextReader reader, CancellationToken cancellationToken = default)
    {
        var json = await reader.ReadToEndAsync().ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        return JsonConvert.DeserializeObject<PrimagiDataSet>(json)!;
    }
}