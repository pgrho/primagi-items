using Newtonsoft.Json;
using System.Text;

namespace Shipwreck.PrimagiItems;

public sealed class PrimagiDataSet
{
    [JsonIgnore]
    public bool IgnoreCalculatedProperties { get; set; }

    #region Genres

    private EnumBaseCollection<Genre>? _Genres;

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

    private EnumBaseCollection<Brand>? _Brands;

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

    private EnumBaseCollection<Color>? _Colors;

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

    private EnumBaseCollection<Rarity>? _Rarities;

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

    private EnumBaseCollection<Category>? _Categories;

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

    private EnumBaseCollection<SubCategory>? _SubCategories;

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

    public static PrimagiDataSet Parse(Stream stream)
    {
        using (var sr = new StreamReader(stream, Encoding.UTF8, leaveOpen: true))
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
        using (var sr = new StreamReader(stream, Encoding.UTF8, leaveOpen: true))
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