namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class ChapterViewModel : ObservableModel
{
    public ChapterViewModel(IndexPageViewModel page, Chapter model)
    {
        Page = page;
        Key = model.Key;
        Value = model.DisplayName ?? model.Key;
    }

    public IndexPageViewModel Page { get; }

    public string? Key { get; }

    public string? Value { get; }

    #region IsSelected

    private bool _IsSelected = true;

    public bool IsSelected
    {
        get => _IsSelected;
        set
        {
            if (SetProperty(ref _IsSelected, value))
            {
                OnIsSelectedChanged();
            }
        }
    }

    private void OnIsSelectedChanged()
    {
        Page.UpdateAllChaptersSelected();
        Page.UpdateFiltered();
    }

    #endregion IsSelected
}