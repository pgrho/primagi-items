namespace Shipwreck.PrimagiItems.Web.Models;

public sealed class UserData
{
    #region Items

    private List<UserCoordinationItem>? _Items;

    public IList<UserCoordinationItem> Items
    {
        get => _Items ??= new();
        set
        {
            if (value != _Items)
            {
                _Items?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Items.Add(e);
                    }
                }
            }
        }
    }

    #endregion Items
}