namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class BrandViewModel : EnumViewModel
{
    public BrandViewModel(IndexPageViewModel page, Brand model)
        : base(page, model)
    {
        ImageUrl = model.ImageUrl;
    }

    public string? ImageUrl { get; }

    protected override void OnIsSelectedChanged()
    {
        Page.UpdateAllBrandsSelected();
        base.OnIsSelectedChanged();
    }
}
