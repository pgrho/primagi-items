namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class ColorViewModel : EnumViewModel
{
    public ColorViewModel(IndexPageViewModel page, Color model)
        : base(page, model)
    {
    }

    protected override void OnIsSelectedChanged()
    {
        Page.UpdateAllColorsSelected();
        base.OnIsSelectedChanged();
    }
}