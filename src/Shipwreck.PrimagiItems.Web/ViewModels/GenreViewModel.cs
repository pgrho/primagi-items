namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class GenreViewModel : EnumViewModel
{
    public GenreViewModel(IndexPageViewModel page, Genre model)
        : base(page, model)
    {
    }

    protected override void OnIsSelectedChanged()
    {
        Page.UpdateAllGenresSelected();
        base.OnIsSelectedChanged();
    }
}