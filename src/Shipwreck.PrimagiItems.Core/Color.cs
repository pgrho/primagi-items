namespace Shipwreck.PrimagiItems;

public sealed class Color : EnumBase
{
    #region Images

    private List<GenreColorImage>? _Images;

    public IList<GenreColorImage> Images
    {
        get
        {
            if (_Images == null)
            {
                _Images = new();
                if (DataSet != null)
                {
                    foreach (var g in DataSet.Genres)
                    {
                        _Images.Add(new()
                        {
                            DataSet = DataSet,
                            GenreIndex = g.Key,
                            ColorIndex = Key
                        });
                    }
                }
            }
            return _Images;
        }
    }

    public bool ShouldSerializeImages()
        => DataSet?.IgnoreCalculatedProperties != true;

    #endregion Images
}