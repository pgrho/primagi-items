using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public abstract class KeyedItem<T>
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public T? Key { get; set; }
}
