using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems.Json;

public sealed class Genre : EnumBase
{
    #region Images

    private List<GenreColorImage>? _Images;

    [JsonIgnore]
    public IList<GenreColorImage> Images
    {
        get
        {
            if (_Images == null)
            {
                _Images = new();
                if (DataSet != null)
                {
                    foreach (var c in DataSet.Colors)
                    {
                        _Images.Add(new()
                        {
                            DataSet = DataSet,
                            GenreIndex = Key,
                            ColorIndex = c.Key
                        });
                    }
                }
            }
            return _Images;
        }
    }

    #endregion Images
}