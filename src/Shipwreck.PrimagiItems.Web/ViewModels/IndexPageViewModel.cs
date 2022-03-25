using Shipwreck.PrimagiItems.Web.Pages;

namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class IndexPageViewModel : FrameworkPageViewModel
{
    public IndexPageViewModel(FrameworkPageBase page)
        : base(page)
    {
        Chapters = new();
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

    #region IsCollapsed

    private bool? _IsCollapsed = true;

    public bool? IsCollapsed
    {
        get => _IsCollapsed;
        private set => SetProperty(ref _IsCollapsed, value);
    }

    public async void ToggleIsCollapsed()
    {
        if (_IsCollapsed == null)
        {
            return;
        }
        else if (_IsCollapsed == true)
        {
            IsCollapsed = null;
            await Task.Delay(350);
            IsCollapsed = false;
        }
        else
        {
            IsCollapsed = null;
            await Task.Delay(350);
            IsCollapsed = true;
        }
    }

    #endregion HidesSpoiler

    #region HidesSpoiler

    private bool _HidesSpoiler = true;

    public bool HidesSpoiler
    {
        get => _HidesSpoiler;
        set
        {
            if (SetProperty(ref _HidesSpoiler, value))
            {
                foreach (var c in Coordinations)
                {
                    c.UpdateIsHidden();
                }
                // TODO: UpdateFiltered();
            }
        }
    }

    #endregion HidesSpoiler

    #region Chapters

    private bool? _AllChaptersSelected = true;

    public BulkUpdateableCollection<ChapterViewModel> Chapters { get; }

    public bool? AllChaptersSelected
    {
        get => _AllChaptersSelected;
        set
        {
            if (SetProperty(ref _AllChaptersSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Chapters)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllChaptersSelected()
        => SetProperty(
            ref _AllChaptersSelected,
            Chapters.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Chapters.Count ? true
                : null
            : null, propertyName: nameof(Chapters));

    #endregion Chapters

    #region Brands

    private bool? _AllBrandsSelected = true;

    public BulkUpdateableCollection<BrandViewModel> Brands { get; }

    public bool? AllBrandsSelected
    {
        get => _AllBrandsSelected;
        set
        {
            if (SetProperty(ref _AllBrandsSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Brands)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllBrandsSelected()
        => SetProperty(
            ref _AllBrandsSelected,
            Brands.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Brands.Count ? true
                : null
            : null, propertyName: nameof(Brands));

    #endregion Brands

    #region Rarities

    private bool? _AllRaritiesSelected = true;

    public BulkUpdateableCollection<RarityViewModel> Rarities { get; }

    public bool? AllRaritiesSelected
    {
        get => _AllRaritiesSelected;
        set
        {
            if (SetProperty(ref _AllRaritiesSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Rarities)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllRaritiesSelected()
        => SetProperty(
            ref _AllRaritiesSelected,
            Rarities.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Rarities.Count ? true
                : null
            : null, propertyName: nameof(Rarities));

    #endregion Rarities

    #region Categories

    private bool? _AllCategoriesSelected = true;

    public BulkUpdateableCollection<CategoryViewModel> Categories { get; }

    public bool? AllCategoriesSelected
    {
        get => _AllCategoriesSelected;
        set
        {
            if (SetProperty(ref _AllCategoriesSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Categories)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllCategoriesSelected()
        => SetProperty(
            ref _AllCategoriesSelected,
            Categories.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Categories.Count ? true
                : null
            : null, propertyName: nameof(Categories));

    #endregion Categories

    #region Colors

    private bool? _AllColorsSelected = true;

    public BulkUpdateableCollection<ColorViewModel> Colors { get; }

    public bool? AllColorsSelected
    {
        get => _AllColorsSelected;
        set
        {
            if (SetProperty(ref _AllColorsSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Colors)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllColorsSelected()
        => SetProperty(
            ref _AllColorsSelected,
            Colors.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Colors.Count ? true
                : null
            : null, propertyName: nameof(Colors));

    #endregion Colors

    #region Genres

    private bool? _AllGenresSelected = true;

    public BulkUpdateableCollection<GenreViewModel> Genres { get; }

    public bool? AllGenresSelected
    {
        get => _AllGenresSelected;
        set
        {
            if (SetProperty(ref _AllGenresSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in Genres)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllGenresSelected()
        => SetProperty(
            ref _AllGenresSelected,
            Genres.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == Genres.Count ? true
                : null
            : null, propertyName: nameof(Genres));

    #endregion Genres

    #region SubCategories

    private bool? _AllSubCategoriesSelected = true;

    public BulkUpdateableCollection<SubCategoryViewModel> SubCategories { get; }

    public bool? AllSubCategoriesSelected
    {
        get => _AllSubCategoriesSelected;
        set
        {
            if (SetProperty(ref _AllSubCategoriesSelected, value))
            {
                if (value != null)
                {
                    try
                    {
                        SuppressUpdate = true;

                        foreach (var e in SubCategories)
                        {
                            e.IsSelected = value ?? false;
                        }
                    }
                    finally
                    {
                        SuppressUpdate = false;
                        UpdateFiltered();
                    }
                }
            }
        }
    }

    internal void UpdateAllSubCategoriesSelected()
        => SetProperty(
            ref _AllSubCategoriesSelected,
            SubCategories.Count(e => e.IsSelected) is var i
            ? i == 0 ? false
                : i == SubCategories.Count ? true
                : null
            : null, propertyName: nameof(SubCategories));

    #endregion SubCategories

    public BulkUpdateableCollection<CoordinationViewModel> Coordinations { get; }
    public BulkUpdateableCollection<CoordinationViewModel> Filtered { get; }

    protected override async Task InitializeDataAsync()
    {
        using (var s = await Http.GetStreamAsync("items.json"))
        {
            var ds = await PrimagiDataSet.ParseAsync(s);

            Chapters.Set(ds.Chapters.Select(e => new ChapterViewModel(this, e)));
            Brands.Set(ds.Brands.Select(e => new BrandViewModel(this, e)));
            Rarities.Set(ds.Rarities.OrderByDescending(e => e.Key).Select(e => new RarityViewModel(this, e)));
            Categories.Set(ds.Categories.Select(e => new CategoryViewModel(this, e)));
            Colors.Set(ds.Colors.Select(e => new ColorViewModel(this, e)));
            Genres.Set(ds.Genres.Select(e => new GenreViewModel(this, e)));
            SubCategories.Set(ds.SubCategories.Select(e => new SubCategoryViewModel(this, e)));

            Coordinations.Set(
                ds.Coordinations
                    .OrderBy(e => e.ChapterId)
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