namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class CoordinationItemViewModel : ObservableModel
{
    private readonly CoordinationItem _Model;

    internal CoordinationItemViewModel(CoordinationViewModel coordination, CoordinationItem model)
    {
        Coordination = coordination;
        _Model = model;
    }

    public CoordinationViewModel Coordination { get; }
    public IndexPageViewModel Page => Coordination.Page;

    public string CategoryName => _Model.Category?.Value ?? _Model.CategoryIndex.ToString("'category#'0");

    public string? ImageUrl => _Model.ImageUrl;

    #region Brand

    private BrandViewModel? _Brand;

    public BrandViewModel? Brand => _Brand ??= Page.Brands.FirstOrDefault(e => e.Key == _Model.BrandIndex);

    #endregion Brand

    #region Rarity

    private RarityViewModel? _Rarity;

    public RarityViewModel? Rarity => _Rarity ??= Page.Rarities.FirstOrDefault(e => e.Key == _Model.RarityIndex);

    #endregion Rarity

    #region Category

    private CategoryViewModel? _Category;

    public CategoryViewModel? Category => _Category ??= Page.Categories.FirstOrDefault(e => e.Key == _Model.CategoryIndex);

    #endregion Category

    #region Color

    private ColorViewModel? _Color;

    public ColorViewModel? Color => _Color ??= Page.Colors.FirstOrDefault(e => e.Key == _Model.ColorIndex);

    #endregion Color

    #region Genre

    private GenreViewModel? _Genre;

    public GenreViewModel? Genre => _Genre ??= Page.Genres.FirstOrDefault(e => e.Key == _Model.GenreIndex);

    #endregion Genre

    #region SubCategory

    private SubCategoryViewModel? _SubCategory;

    public SubCategoryViewModel? SubCategory => _SubCategory ??= Page.SubCategories.FirstOrDefault(e => e.Key == _Model.SubCategoryIndex);

    #endregion SubCategory

    #region IsVisible

    private bool _IsVisible = true;

    public bool IsVisible
    {
        get => _IsVisible;
        private set => SetProperty(ref _IsVisible, value);
    }

    #endregion IsVisible

    internal bool InvalidateIsVisible()
    {
        IsVisible = Brand?.IsSelected == true
                && Rarity?.IsSelected == true
                && Category?.IsSelected == true
                && Color?.IsSelected == true
                && Genre?.IsSelected == true
                && SubCategory?.IsSelected == true;
        return _IsVisible;
    }
}
