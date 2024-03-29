﻿using System.Collections.ObjectModel;

namespace Shipwreck.PrimagiItems;

internal sealed class CoordinationCollection : Collection<Coordination>
{
    private readonly PrimagiDataSet _DataSet;

    public CoordinationCollection(PrimagiDataSet dataSet)
    {
        _DataSet = dataSet;
    }

    protected override void InsertItem(int index, Coordination item)
    {
        if ((item ?? throw new ArgumentNullException(nameof(item))).DataSet != null)
        {
            throw new InvalidOperationException();
        }
        item.DataSet = _DataSet;
        base.InsertItem(index, item);

        _DataSet.EnsureChapter(item.ChapterId);
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

    protected override void SetItem(int index, Coordination item)
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

            _DataSet.EnsureChapter(item.ChapterId);
        }
    }
}