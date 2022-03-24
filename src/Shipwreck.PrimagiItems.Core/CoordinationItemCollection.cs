using System.Collections.ObjectModel;

namespace Shipwreck.PrimagiItems;

internal sealed class CoordinationItemCollection : Collection<CoordinationItem>
{
    private readonly Coordination _Coordination;

    public CoordinationItemCollection(Coordination coordination)
    {
        _Coordination = coordination;
    }

    protected override void InsertItem(int index, CoordinationItem item)
    {
        if ((item ?? throw new ArgumentNullException(nameof(item))).Coordination != null)
        {
            throw new InvalidOperationException();
        }
        item.Coordination = _Coordination;
        base.InsertItem(index, item);
    }

    protected override void ClearItems()
    {
        foreach (var e in this)
        {
            e.Coordination = null;
        }
        base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
        var e = this[index];
        e.Coordination = null;
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, CoordinationItem item)
    {
        var old = this[index];
        if (old != item)
        {
            if ((item ?? throw new ArgumentNullException(nameof(item))).Coordination != null)
            {
                throw new InvalidOperationException();
            }
            old.Coordination = null;
            item.Coordination = _Coordination;
            base.SetItem(index, item);
        }
    }
}
