namespace Shipwreck.PrimagiItems.Web.ViewModels;

public sealed class CategoryViewModel : EnumViewModel
{
    public CategoryViewModel(IndexPageViewModel page, Category model)
        : base(page, model)
    {
    }
    protected override void OnIsSelectedChanged()
    {
        Page.UpdateAllCategoriesSelected();
        base.OnIsSelectedChanged();
    }
}
