using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems.Json;

public abstract class EnumBase
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public int Key { get; set; }
    public string? Value { get; set; }
}
