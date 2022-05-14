using Shipwreck.PrimagiItems.Web.Models;
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

    #region UserDataAccessor

    private IUserDataAccessor? _UserDataAccessor;

    internal IUserDataAccessor UserDataAccessor
        => _UserDataAccessor ??= new LocalStorageUserDataAccessor(Page.JS);

    #endregion UserDataAccessor

    #region Model

    private IndexPageMode _Mode = IndexPageMode.Summary;

    public IndexPageMode Mode
    {
        get => _Mode;
        set => SetProperty(ref _Mode, value);
    }

    #endregion Model

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

    #endregion IsCollapsed

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

    #region 所持数

    #region IsOwned

    public bool _IsOwned = true;

    public bool IsOwned
    {
        get => _IsOwned;
        set
        {
            if (SetProperty(ref _IsOwned, value))
            {
                UpdateFiltered();
            }
        }
    }

    #endregion IsOwned

    #region IsListed

    public bool _IsListed = true;

    public bool IsListed
    {
        get => _IsListed;
        set
        {
            if (SetProperty(ref _IsListed, value))
            {
                UpdateFiltered();
            }
        }
    }

    #endregion IsListed

    #region IsDesired

    public bool _IsDesired = true;

    public bool IsDesired
    {
        get => _IsDesired;
        set
        {
            if (SetProperty(ref _IsDesired, value))
            {
                UpdateFiltered();
            }
        }
    }

    #endregion IsDesired

    #region IsNotListed

    public bool _IsNotListed = true;

    public bool IsNotListed
    {
        get => _IsNotListed;
        set
        {
            if (SetProperty(ref _IsNotListed, value))
            {
                UpdateFiltered();
            }
        }
    }

    #endregion IsNotListed

    #endregion 所持数

    #region UserDataTask

    private Task<UserData>? _UserDataTask;

    private Task<UserData> UserDataTask
        => _UserDataTask ??= UserDataAccessor.ReadAsync(null).ContinueWith(t => t.Result ?? new UserData());

    #endregion UserDataTask

    #region SaveUserDataCommand

    private CommandViewModelBase? _SaveUserDataCommand;

    public CommandViewModelBase SaveUserDataCommand
        => _SaveUserDataCommand ??= CommandViewModel.CreateAsync(
            async () =>
            {
                var ud = new UserData();
                ud.Items = Coordinations.SelectMany(e => e.Items).Where(e => e.HasValue()).Select(e => new UserCoordinationItem
                {
                    SealId = e.SealId,
                    PosessionCount = e.PosessionCount,
                    ListingCount = e.ListingCount,
                    TradingCount = e.TradingCount,
                    Remarks = e.Remarks
                }).ToList();

                await UserDataAccessor.WriteAsync(ud);

                _UserDataTask = Task.FromResult(ud);
                foreach (var c in Coordinations)
                {
                    foreach (var e in c.Items)
                    {
                        e.SetUnchanged();
                    }
                }
                ChangedCount = 0;
            },
            title: "保存",
            icon: "fas fa-save",
            style: BorderStyle.Primary,
            badgeCountGetter: () => ChangedCount);

    #endregion SaveUserDataCommand

    public BulkUpdateableCollection<CoordinationViewModel> Coordinations { get; }
    public BulkUpdateableCollection<CoordinationViewModel> Filtered { get; }

    protected override async Task InitializeDataAsync()
    {
        await UserDataTask;

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

            SetUserData();

            UpdateFiltered();
        }
    }

    private async void SetUserData()
    {
        var ud = await UserDataTask;
        var udic = ud?.Items.Where(e => e.SealId != null).ToDictionary(e => e.SealId!) ?? new();
        foreach (var c in Coordinations)
        {
            foreach (var e in c.Items)
            {
                udic.TryGetValue(e.SealId ?? string.Empty, out var ue);
                e.Update(ue);
            }
        }
        ChangedCount = 0;
    }

    #region ChangedCount

    private int _ChangedCount;

    public int ChangedCount
    {
        get => _ChangedCount;
        internal set
        {
            if (SetProperty(ref _ChangedCount, value))
            {
                _SaveUserDataCommand?.Invalidate();
            }
        }
    }

    internal void InvalidateChangedCount()
        => ChangedCount = Coordinations.Sum(c => c.Items.Count(e => e.IsChanged));

    #endregion ChangedCount

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

    #region SummaryTool

    private IndexSummaryTool _SummaryTool = IndexSummaryTool.IncrementListingCount;

    public IndexSummaryTool SummaryTool
    {
        get => _SummaryTool;
        set
        {
            if (SetProperty(ref _SummaryTool, value))
            {
                _SelectIncrementPosessionCountCommand?.Invalidate();
                _SelectDecrementPosessionCountCommand?.Invalidate();
                _SelectClearPosessionCountCommand?.Invalidate();

                _SelectIncrementListingCountCommand?.Invalidate();
                _SelectDecrementListingCountCommand?.Invalidate();
                _SelectClearListingCountCommand?.Invalidate();

                _SelectIncrementTradingCountCommand?.Invalidate();
                _SelectDecrementTradingCountCommand?.Invalidate();
                _SelectClearTradingCountCommand?.Invalidate();
            }
        }
    }

    #region PosessionCount

    #region SelectIncrementPosessionCountCommand

    private CommandViewModelBase? _SelectIncrementPosessionCountCommand;

    public CommandViewModelBase SelectIncrementPosessionCountCommand
        => _SelectIncrementPosessionCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.IncrementPosessionCount,
            icon: "fas fa-plus",
            title: "所持数",
            styleGetter: () => SummaryTool == IndexSummaryTool.IncrementPosessionCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectIncrementPosessionCountCommand

    #region SelectDecrementPosessionCountCommand

    private CommandViewModelBase? _SelectDecrementPosessionCountCommand;

    public CommandViewModelBase SelectDecrementPosessionCountCommand
        => _SelectDecrementPosessionCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.DecrementPosessionCount,
            icon: "fas fa-minus",
            styleGetter: () => SummaryTool == IndexSummaryTool.DecrementPosessionCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectDecrementPosessionCountCommand

    #region SelectClearPosessionCountCommand

    private CommandViewModelBase? _SelectClearPosessionCountCommand;

    public CommandViewModelBase SelectClearPosessionCountCommand
        => _SelectClearPosessionCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.ClearPosessionCount,
            icon: "fas fa-times",
            styleGetter: () => SummaryTool == IndexSummaryTool.ClearPosessionCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectClearPosessionCountCommand

    #endregion PosessionCount

    #region ListingCount

    #region SelectIncrementListingCountCommand

    private CommandViewModelBase? _SelectIncrementListingCountCommand;

    public CommandViewModelBase SelectIncrementListingCountCommand
        => _SelectIncrementListingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.IncrementListingCount,
            icon: "fas fa-plus",
            title: "譲追加",
            styleGetter: () => SummaryTool == IndexSummaryTool.IncrementListingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectIncrementListingCountCommand

    #region SelectDecrementListingCountCommand

    private CommandViewModelBase? _SelectDecrementListingCountCommand;

    public CommandViewModelBase SelectDecrementListingCountCommand
        => _SelectDecrementListingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.DecrementListingCount,
            icon: "fas fa-minus",
            title: "求追加",
            styleGetter: () => SummaryTool == IndexSummaryTool.DecrementListingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectDecrementListingCountCommand

    #region SelectClearListingCountCommand

    private CommandViewModelBase? _SelectClearListingCountCommand;

    public CommandViewModelBase SelectClearListingCountCommand
        => _SelectClearListingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.ClearListingCount,
            icon: "fas fa-times",
            styleGetter: () => SummaryTool == IndexSummaryTool.ClearListingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectClearListingCountCommand

    #endregion ListingCount

    #region TradingCount

    #region SelectIncrementTradingCountCommand

    private CommandViewModelBase? _SelectIncrementTradingCountCommand;

    public CommandViewModelBase SelectIncrementTradingCountCommand
        => _SelectIncrementTradingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.IncrementTradingCount,
            icon: "fas fa-plus",
            title: "取引中",
            styleGetter: () => SummaryTool == IndexSummaryTool.IncrementTradingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectIncrementTradingCountCommand

    #region SelectDecrementTradingCountCommand

    private CommandViewModelBase? _SelectDecrementTradingCountCommand;

    public CommandViewModelBase SelectDecrementTradingCountCommand
        => _SelectDecrementTradingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.DecrementTradingCount,
            icon: "fas fa-minus",
            styleGetter: () => SummaryTool == IndexSummaryTool.DecrementTradingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectDecrementTradingCountCommand

    #region SelectClearTradingCountCommand

    private CommandViewModelBase? _SelectClearTradingCountCommand;

    public CommandViewModelBase SelectClearTradingCountCommand
        => _SelectClearTradingCountCommand ??= CommandViewModel.Create(
            () => SummaryTool = IndexSummaryTool.ClearTradingCount,
            icon: "fas fa-times",
            styleGetter: () => SummaryTool == IndexSummaryTool.ClearTradingCount ? BorderStyle.Primary : BorderStyle.OutlinePrimary);

    #endregion SelectClearTradingCountCommand

    #endregion TradingCount

    #endregion SummaryTool
}