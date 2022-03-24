using Shipwreck.PrimagiItems.Web.Pages;

namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class IndexPageViewModel : FrameworkPageViewModel
{
    public IndexPageViewModel(FrameworkPageBase page)
        : base(page)
    {
        Brands = new();
        Rarities = new();
        Categories = new();
        Colors = new();
        Genres = new();
        SubCategories = new();
        Coordinations = new();
        Filtered = new();
    }

    public HttpClient Http => ((IndexPage)Page).Http!;

    public BulkUpdateableCollection<BrandViewModel> Brands { get; }
    public BulkUpdateableCollection<RarityViewModel> Rarities { get; }
    public BulkUpdateableCollection<CategoryViewModel> Categories { get; }
    public BulkUpdateableCollection<ColorViewModel> Colors { get; }
    public BulkUpdateableCollection<GenreViewModel> Genres { get; }
    public BulkUpdateableCollection<SubCategoryViewModel> SubCategories { get; }
    public BulkUpdateableCollection<CoordinationViewModel> Coordinations { get; }
    public BulkUpdateableCollection<CoordinationViewModel> Filtered { get; }

    protected override async Task InitializeDataAsync()
    {
        using (var s = await Http.GetStreamAsync("items.json"))
        {
            var ds = await PrimagiDataSet.ParseAsync(s);

            Brands.Set(ds.Brands.Select(e => new BrandViewModel(this, e)));
            Rarities.Set(ds.Rarities.OrderByDescending(e => e.Key).Select(e => new RarityViewModel(this, e)));
            Categories.Set(ds.Categories.Select(e => new CategoryViewModel(this, e)));
            Colors.Set(ds.Colors.Select(e => new ColorViewModel(this, e)));
            Genres.Set(ds.Genres.Select(e => new GenreViewModel(this, e)));
            SubCategories.Set(ds.SubCategories.Select(e => new SubCategoryViewModel(this, e)));

            Coordinations.Set(
                ds.Coordinations
                    .OrderBy(e => e.Chapter)
                    .ThenBy(e => e.DirectoryNumber)
                    .Select(e => new CoordinationViewModel(this, e)));

            UpdateFiltered();
        }
    }

    internal bool SuppressUpdate { get; private set; }

    internal void UpdateFiltered()
    {
        if (SuppressUpdate)
        {
            return;
        }

        var fs = Coordinations.Where(e => e.InvalidateIsVisible()).ToList();

        if (!fs.SequenceEqual(Filtered))
        {
            Filtered.Set(fs);
        }
    }
}