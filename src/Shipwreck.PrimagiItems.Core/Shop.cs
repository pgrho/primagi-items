using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class Shop
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Prefecture { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ShopGroup { get; set; }
}
