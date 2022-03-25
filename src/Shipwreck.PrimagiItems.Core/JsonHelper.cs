namespace Shipwreck.PrimagiItems;

internal static class JsonHelper
{
    public static KeyedCollection<TKey, TItem> GetOrCreate<TKey, TItem>(this PrimagiDataSet dataSet, ref KeyedCollection<TKey, TItem>? field)
    where TKey : notnull
    where TItem : KeyedItem<TKey>
        => field ??= new(dataSet);

    public static void Set<TKey, TItem>(this PrimagiDataSet dataSet, ref KeyedCollection<TKey, TItem>? field, IEnumerable<TItem>? value)
         where TKey : notnull
    where TItem : KeyedItem<TKey>
    {
        if (value != field)
        {
            field?.Clear();
            if (value != null)
            {
                foreach (var v in value)
                {
                    dataSet.GetOrCreate(ref field).Add(v);
                }
            }
        }
    }
}