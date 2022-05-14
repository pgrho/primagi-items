using Shipwreck.PrimagiItems.Web.Models;

namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class CoordinationItemViewModel : ObservableModel
{
    private readonly CoordinationItem _Model;

    private bool _IsUpdating;
    private int _OriginalPosessionCount;
    private int _OriginalListingCount;
    private int _OriginalTradingCount;
    private string _OriginalRemarks = string.Empty;

    internal CoordinationItemViewModel(CoordinationViewModel coordination, CoordinationItem model)
    {
        Coordination = coordination;
        _Model = model;
    }

    public CoordinationViewModel Coordination { get; }
    public IndexPageViewModel Page => Coordination.Page;

    public string CategoryName => _Model.Category?.Value ?? _Model.CategoryIndex.ToString("'category#'0");

    public string? Name => _Model.Name;
    public string? SealId => _Model.SealId;
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

    private GenreColorImage? _GenreColor;

    public GenreColorImage? GenreColor
        => _GenreColor ??= _Model.Genre?.Images.FirstOrDefault(e => e.ColorIndex == _Model.ColorIndex);

    #region IsVisible

    private bool _IsVisible = true;

    public bool IsVisible
    {
        get => _IsVisible;
        private set => SetProperty(ref _IsVisible, value);
    }

    #endregion IsVisible

    #region IsChanged

    private bool _IsChanged = true;

    public bool IsChanged
    {
        get => _IsChanged;
        private set
        {
            SetProperty(ref _IsChanged, value);
            if (!_IsUpdating)
            {
                Page.InvalidateChangedCount();
            }
        }
    }

    private void SetIsChanged()
        => IsChanged = _OriginalPosessionCount != PosessionCount
        || _OriginalTradingCount != TradingCount
        || _OriginalListingCount != ListingCount
        || _OriginalRemarks != Remarks;

    #endregion IsChanged

    #region UserData

    #region PosessionCount

    private int _PosessionCount;

    public int PosessionCount
    {
        get => _PosessionCount;
        set
        {
            if (SetProperty(ref _PosessionCount, value))
            {
                SetIsChanged();
            }
        }
    }

    #endregion PosessionCount

    #region ListingCount

    private int _ListingCount;

    public int ListingCount
    {
        get => _ListingCount;
        set
        {
            if (SetProperty(ref _ListingCount, value))
            {
                SetIsChanged();
            }
        }
    }

    #endregion ListingCount

    #region TradingCount

    private int _TradingCount;

    public int TradingCount
    {
        get => _TradingCount;
        set
        {
            if (SetProperty(ref _TradingCount, value))
            {
                SetIsChanged();
            }
        }
    }

    #endregion TradingCount

    #region Remarks

    private string _Remarks = string.Empty;

    public string Remarks
    {
        get => _Remarks;
        set
        {
            if (SetProperty(ref _Remarks, value))
            {
                SetIsChanged();
            }
        }
    }

    #endregion Remarks

    #endregion UserData

    internal bool HasValue()
        => PosessionCount != 0 || ListingCount != 0 || TradingCount != 0 || !string.IsNullOrEmpty(Remarks);

    internal bool InvalidateIsVisible()
    {
        IsVisible = Coordination.Chapter?.IsSelected == true
                && Brand?.IsSelected == true
                && Rarity?.IsSelected == true
                && Category?.IsSelected == true
                && Color?.IsSelected == true
                && Genre?.IsSelected == true
                && SubCategory?.IsSelected == true
                && ((Page.IsOwned && Page.IsListed && Page.IsDesired && Page.IsNotListed)
                || (Page.IsOwned && PosessionCount > 0)
                || (Page.IsListed && ListingCount > 0)
                || (Page.IsDesired && ListingCount < 0)
                || (Page.IsNotListed && ListingCount == 0 && TradingCount == 0));
        return _IsVisible;
    }

    public string GetTooltip()
        => $"{SealId} {Name} ({CategoryName}, {Coordination.Rarity?.Value}, {Brand?.Value}, {Genre?.Value}, {Color?.Value}, {SubCategory?.Value}, {_Model.Watcha}ワッチャ)";

    public void ApplyTool(bool altKey)
    {
        var t = Coordination.Page.SummaryTool;
        switch (t)
        {
            case IndexSummaryTool.IncrementPosessionCount:
            case IndexSummaryTool.DecrementPosessionCount:
                PosessionCount = Math.Min(Math.Max(0, PosessionCount + ((t == IndexSummaryTool.IncrementPosessionCount) != altKey ? 1 : -1)), 99);
                break;

            case IndexSummaryTool.ClearPosessionCount:
                PosessionCount = 0;
                break;

            case IndexSummaryTool.IncrementListingCount:
            case IndexSummaryTool.DecrementListingCount:
                ListingCount = Math.Min(Math.Max(-99, ListingCount + ((t == IndexSummaryTool.IncrementListingCount) != altKey ? 1 : -1)), 99);
                break;

            case IndexSummaryTool.ClearListingCount:
                ListingCount = 0;
                break;

            case IndexSummaryTool.IncrementTradingCount:
            case IndexSummaryTool.DecrementTradingCount:
                TradingCount = Math.Min(Math.Max(-99, TradingCount + ((t == IndexSummaryTool.IncrementTradingCount) != altKey ? 1 : -1)), 99);
                break;

            case IndexSummaryTool.ClearTradingCount:
                TradingCount = 0;
                break;
        }
    }

    public void Update(UserCoordinationItem? userData)
    {
        try
        {
            _IsUpdating = true;

            _OriginalPosessionCount = PosessionCount = userData?.PosessionCount ?? 0;
            _OriginalListingCount = ListingCount = userData?.ListingCount ?? 0;
            _OriginalTradingCount = TradingCount = userData?.TradingCount ?? 0;
            _OriginalRemarks = Remarks = userData?.Remarks ?? string.Empty;
        }
        finally
        {
            _IsUpdating = false;

            IsChanged = false;
        }
    }

    public void SetUnchanged()
    {
        try
        {
            _IsUpdating = true;

            _OriginalPosessionCount = PosessionCount;
            _OriginalListingCount = ListingCount;
            _OriginalTradingCount = TradingCount;
            _OriginalRemarks = Remarks;
        }
        finally
        {
            _IsUpdating = false;

            IsChanged = false;
        }
    }
}