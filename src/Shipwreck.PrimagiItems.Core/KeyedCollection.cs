namespace Shipwreck.PrimagiItems;

internal sealed class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>
    where TKey : notnull
    where TItem : KeyedItem<TKey>
{
    private readonly PrimagiDataSet _DataSet;

    public KeyedCollection(PrimagiDataSet dataSet)
    {
        _DataSet = dataSet;
    }

    protected override TKey GetKeyForItem(TItem item)
        => item.Key!;

    protected override void InsertItem(int index, TItem item)
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

    protected override void SetItem(int index, TItem item)
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