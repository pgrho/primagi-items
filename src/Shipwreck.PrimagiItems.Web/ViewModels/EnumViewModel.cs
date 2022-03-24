namespace Shipwreck.PrimagiItems.Web.ViewModels;

public abstract class EnumViewModel : ObservableModel
{
    protected EnumViewModel(IndexPageViewModel page, EnumBase model)
    {
        Page = page;
        Key = model.Key;
        Value = model.Value ?? model.Key.ToString("'#'0");
    }

    public IndexPageViewModel Page { get; }

    public int Key { get; }

    public string Value { get; }

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

    protected virtual void OnIsSelectedChanged()
        => Page.UpdateFiltered();

    #endregion IsSelected
}