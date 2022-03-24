namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class SubCategoryViewModel : EnumViewModel
{
    public SubCategoryViewModel(IndexPageViewModel page, SubCategory model)
        : base(page, model)
    {
        ImageUrl = model.ImageUrl;
    }

    public string? ImageUrl { get; }
}
