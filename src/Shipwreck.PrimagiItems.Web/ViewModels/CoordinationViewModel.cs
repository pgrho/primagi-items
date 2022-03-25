using System.Collections.ObjectModel;

namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class CoordinationViewModel : ObservableModel
{
    private readonly Coordination _Model;

    internal CoordinationViewModel(IndexPageViewModel page, Coordination model)
    {
        Page = page;
        Items = Array.AsReadOnly(model.Items.Select(e => new CoordinationItemViewModel(this, e)).ToArray());
        _Model = model;

        UpdateIsHidden();
    }

    public IndexPageViewModel Page { get; }
    public ReadOnlyCollection<CoordinationItemViewModel> Items { get; }
     
    #region Chapter

    private ChapterViewModel? _Chapter;

    public ChapterViewModel? Chapter => _Chapter ??= Page.Chapters.FirstOrDefault(e => e.Key == _Model.ChapterId);

    #endregion Chapter
     
    public string? ChapterId => _Model.ChapterId;

    public string? ChapterDisplayName
        => _Model.Chapter?.DisplayName ?? _Model.ChapterId;

    public int DirectoryNumber => _Model.DirectoryNumber;
    public string? Name => _Model.Name;

    #region IsHidden

    private bool _IsHidden;

    public bool IsHidden
    {
        get => _IsHidden;
        private set => SetProperty(ref _IsHidden, value);
    }

    internal void UpdateIsHidden()
        => IsHidden = Page.HidesSpoiler && _Model.Items.All(e => !e.IsShowItem);

    #endregion IsHidden

    public string DisplayName => (IsHidden ? null : _Model.Name) ?? "????";

    #region Brand

    private BrandViewModel? _Brand;

    public BrandViewModel? Brand
        => _Brand
        ??= (Items.FirstOrDefault()?.Brand is BrandViewModel b && Items.All(e => e.Brand == b) ? b : null);

    #endregion Brand

    #region Rarity

    private RarityViewModel? _Rarity;

    public RarityViewModel? Rarity
        => _Rarity
        ??= (Items.FirstOrDefault()?.Rarity is RarityViewModel r && Items.All(e => e.Rarity == r) ? r : null);

    #endregion Rarity

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
        var v = false;
        foreach (var e in Items)
        {
            v |= e.InvalidateIsVisible();
        }
        IsVisible = v;
        return _IsVisible;
    }
}