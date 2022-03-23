using System.Collections.ObjectModel;

namespace Shipwreck.PrimagiItems.Json;

internal sealed class EnumBaseCollection<T> : KeyedCollection<int, T>
    where T : EnumBase
{
    private readonly PrimagiDataSet _DataSet;

    public EnumBaseCollection(PrimagiDataSet dataSet)
    {
        _DataSet = dataSet;
    }

    protected override int GetKeyForItem(T item)
        => item.Key;

    protected override void InsertItem(int index, T item)
    {
        if ((item ?? throw new ArgumentNullException(nameof(item))).DataSet != null)
        {
            throw new InvalidOperationException();
        }
        item.DataSet = _DataSet;
        base.InsertItem(index, item);
    }

    protected override void ClearItems()
    {
        foreach (var e in this)
        {
            e.DataSet = null;
        }
        base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
        var e = this[index];
        e.DataSet = null;
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, T item)
    {
        var old = this[index];
        if (old != item)
        {
            if ((item ?? throw new ArgumentNullException(nameof(item))).DataSet != null)
            {
                throw new InvalidOperationException();
            }
            old.DataSet = null;
            item.DataSet = _DataSet;
            base.SetItem(index, item);
        }
    }
}
