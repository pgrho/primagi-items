namespace Shipwreck.PrimagiItems;

#if NETSTANDARD2_0

internal static class NetStandardHelper
{
    public static bool TryGetValue<TKey, TValue>(this System.Collections.ObjectModel.KeyedCollection<TKey, TValue> collection, TKey key, out TValue? result)
        where TKey : notnull
    {
        if (collection.Contains(key))
        {
            result = collection[key];
            return true;
        }
        result = default;
        return false;
    }
}

#endif