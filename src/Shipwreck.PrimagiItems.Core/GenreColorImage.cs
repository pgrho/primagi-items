using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class GenreColorImage
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public int GenreIndex { get; set; }
    public int ColorIndex { get; set; }

    public string? ImageUrl
        => $"https://cdn.primagi.jp/assets/images/item/common/ico_props_genre_color_{GenreIndex}_{ColorIndex}_pc.png";
}