using System.ComponentModel;
using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class CatchCopy
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    #region Chapter

    private string? _ChapterId;
    private Chapter? _Chapter;

    [JsonProperty("chapter")]
    public string? ChapterId
    {
        get => _ChapterId;
        set
        {
            if (value != _ChapterId)
            {
                _ChapterId = value;
                _Chapter = null;
            }
        }
    }

    [JsonIgnore]
    public Chapter? Chapter
    {
        get => _Chapter ??= DataSet?.GetChapter(ChapterId);
        set
        {
            _Chapter = value;
            _ChapterId = value?.Key;
        }
    }

    #endregion Chapter

    public int Id { get; set; }
    public string? Conditions { get; set; }

    [DefaultValue(null)]
    public string? Url { get; set; }

    public string? Name { get; set; }

    #region Category

    private int? _CategoryId;
    private CatchCopyCategory? _Category;

    [JsonProperty("category")]
    public int? CategoryId
    {
        get => _CategoryId;
        set
        {
            if (value != _CategoryId)
            {
                _CategoryId = value;
                _Category = null;
            }
        }
    }

    [JsonIgnore]
    public CatchCopyCategory? Category
    {
        get => _Category ??= (CategoryId == null ? null : DataSet?.GetCatchCopyCategory(CategoryId.Value));
        set
        {
            _Category = value;
            _CategoryId = value?.Key;
        }
    }

    #endregion Category

    public string? ImageUrl
        => Id <= 0 ? null
        : $"https://cdn.primagi.jp/assets/images/copy/{Id}.png";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}