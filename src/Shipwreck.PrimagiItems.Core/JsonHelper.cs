namespace Shipwreck.PrimagiItems;

internal static class JsonHelper
{
    public static EnumBaseCollection<T> GetOrCreate<T>(this PrimagiDataSet dataSet, ref EnumBaseCollection<T>? field)
        where T : EnumBase
        => field ??= new(dataSet);

    public static void Set<T>(this PrimagiDataSet dataSet, ref EnumBaseCollection<T>? field, IEnumerable<T>? value)
        where T : EnumBase
    {
        if (value != field)
        {
            field?.Clear();
            if (value != null)
            {
                foreach (var v in value)
                {
                    dataSet.GetOrCreate<T>(ref field).Add(v);
                }
            }
        }
    }
}
