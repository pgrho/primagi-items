namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class RarityViewModel : EnumViewModel
{
    public RarityViewModel(IndexPageViewModel page, Rarity model)
        : base(page, model)
    {
        ImageUrl = model.ImageUrl;
    }

    public string? ImageUrl { get; }
    protected override void OnIsSelectedChanged()
    {
        Page.UpdateAllRaritiesSelected();
        base.OnIsSelectedChanged();
    }
}
