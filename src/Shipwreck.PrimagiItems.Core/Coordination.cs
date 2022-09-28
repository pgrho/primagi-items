using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems;

public sealed class Coordination
{
    [JsonIgnore]
    public PrimagiDataSet? DataSet { get; set; }

    public int Collection { get; set; }
    public string? Name { get; set; }
    public string? Kinds { get; set; }
    public int DirectoryNumber { get; set; }
    public bool IsShow { get; set; }
    public bool HasMainImage { get; set; }

    #region Span

    private string? _Span;
    private string? _SpanEventName;
    private DateTime? _SpanStart;
    private DateTime? _SpanEnd;
    private int? _SpanFriendPoint;

    private string? _ParsedSpanEventName;
    private DateTime? _ParsedSpanStart;
    private DateTime? _ParsedSpanEnd;
    private int? _ParsedSpanFriendPoint;

    public string? Span
    {
        get => _Span;
        set
        {
            value = string.IsNullOrEmpty(value) ? null : value;
            if (value != _Span)
            {
                _Span = value;

                _ParsedSpanStart = null;
                _ParsedSpanEnd = null;
                _ParsedSpanEventName = null;
                _ParsedSpanFriendPoint = null;
            }
        }
    }

    private static Regex? _YMD_YMD;
    private static Regex? _YM_MD_MD;
    private static Regex? _YM;

    private Coordination ParseSpan()
    {
        if (_ParsedSpanEventName != null)
        {
            return this;
        }

        var value = _Span ?? string.Empty;

        static string createYmd(char g)
            => @$"(?<{g}y>\d{{1,4}})年(?<{g}m>\d{{1,2}})[年月日](?<{g}d>\d{{1,2}})[年月日](\s*[(（][日月火水木金土][）)])?";

        static string createYmdOrMd(char g)
            => "("
            + @$"(?<{g}y>\d{{1,4}})年(?<{g}m>\d{{1,2}})[年月日](?<{g}d>\d{{1,2}})[年月日](\s*[(（][日月火水木金土][）)])?"
            + "|"
            + @$"(?<{g}m>\d{{1,2}})月(?<{g}d>\d{{1,2}})[年月日](\s*[(（][日月火水木金土][）)])?"
            + ")";

        static string createYm(char g)
            => @$"(?<{g}y>\d{{1,4}})年(?<{g}m>\d{{1,2}})月";

        static string createYmOrYmd(char g)
            => @$"(?<{g}y>\d{{1,4}})年(?<{g}m>\d{{1,2}})月((?<{g}d>\d{{1,2}})日(\s*[(（][日月火水木金土][）)])?)?";

        if (Regex.Match(value, "フレンドポイント\\s+(\\d+)ポイント$") is var m0 && m0.Success)
        {
            _ParsedSpanStart = null;
            _ParsedSpanEnd = null;
            _ParsedSpanEventName = string.Empty;
            _ParsedSpanFriendPoint = int.Parse(m0.Groups[1].Value);
        }
        else if ((_YM_MD_MD ??= new Regex(
            "^" + createYm('c') + "\\s*(?<en>[^\\s\\d]+)\\s*"
            + "(" + createYmdOrMd('s') + "\\s*[〜～]\\s*" + createYmdOrMd('e')
            + "|[(（]" + createYmdOrMd('s') + "\\s*[〜～]\\s*" + createYmdOrMd('e') + "[）)])"
            + "\\s*$")).Match(value) is var m2
                && m2.Success
                && DateTime.TryParse(m2.Result(
                    m2.Groups["sy"].Success ? "${sy}-${sm}-${sd}"
                    : "${cy}-${sm}-${sd}"), out var sd2)
                && DateTime.TryParse(m2.Result(
                    m2.Groups["ey"].Success ? "${ey}-${em}-${ed}"
                    : m2.Groups["sy"].Success ? "${sy}-${em}-${ed}"
                    : "${cy}-${em}-${ed}"), out var ed2))
        {
            _ParsedSpanStart = sd2.Date;
            _ParsedSpanEnd = ed2.Date;
            _ParsedSpanEventName = m2.Groups["en"].Value;
            _ParsedSpanFriendPoint = null;
        }
        else if ((_YMD_YMD ??= new Regex(createYmd('s') + "\\s*[〜～]\\s*" + createYmdOrMd('e') + "\\s*$")).Match(value) is var m1 && m1.Success
                && DateTime.TryParse(m1.Result("${sy}-${sm}-${sd}"), out var sd1)
                && DateTime.TryParse(m1.Result(m1.Groups["ey"].Success ? "${ey}-${em}-${ed}" : "${sy}-${em}-${ed}"), out var ed1))
        {
            _ParsedSpanStart = sd1.Date;
            _ParsedSpanEnd = ed1.Date;
            _ParsedSpanEventName = value.Substring(0, m1.Index).Trim();
            _ParsedSpanFriendPoint = null;
        }
        else if ((_YM ??= new Regex("^" + createYmOrYmd('s') + "(?<is>～|発売|\\s)?")).Match(value) is var m3 && m3.Success
                && DateTime.TryParse(m3.Result(m3.Groups["sd"].Success ? "${sy}-${sm}-${sd}" : "${sy}-${sm}-1"), out var sd3))
        {
            _ParsedSpanStart = sd3.Date;
            _ParsedSpanEnd = string.IsNullOrWhiteSpace(m3.Groups["is"].Value) ? sd3.Date.AddMonths(1).AddDays(-1) : null;
            _ParsedSpanEventName = value.Substring(m3.Length).Trim();
            _ParsedSpanFriendPoint = null;
        }
        else
        {
            _ParsedSpanStart = null;
            _ParsedSpanEnd = null;
            _ParsedSpanEventName = value;
            _ParsedSpanFriendPoint = null;
        }

        return this;
    }

    public string? SpanEventName
    {
        get => (_SpanEventName ?? ParseSpan()._ParsedSpanEventName) is string s && s.Length > 0 ? s : null;
        set => _SpanEventName = value;
    }

    public bool ShouldSerializeSpanEventName()
        => DataSet?.IgnoreCalculatedProperties != true && SpanEventName != null;

    public DateTime? SpanStart
    {
        get => (_SpanStart ?? ParseSpan()._ParsedSpanStart) is DateTime dt && dt > DateTime.MinValue ? dt : null;
        set => _SpanStart = value;
    }

    public bool ShouldSerializeSpanStart()
        => DataSet?.IgnoreCalculatedProperties != true && SpanStart != null;

    public DateTime? SpanEnd
    {
        get => (_SpanEnd ?? ParseSpan()._ParsedSpanEnd) is DateTime dt && dt < DateTime.MaxValue.AddDays(-2) ? dt : null;
        set => _SpanEnd = value;
    }

    public bool ShouldSerializeSpanEnd()
        => DataSet?.IgnoreCalculatedProperties != true && SpanEnd != null;

    public int? SpanFriendPoint
    {
        get => (_SpanFriendPoint ?? ParseSpan()._ParsedSpanFriendPoint) is int i && i > 0 ? i : null;
        set => _SpanFriendPoint = value;
    }

    public bool ShouldSerializeSpanFriendPoint()
        => DataSet?.IgnoreCalculatedProperties != true && SpanFriendPoint != null;

    #endregion Span

    public int Order { get; set; }

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

    #region Items

    private CoordinationItemCollection? _Items;

    public IList<CoordinationItem> Items
    {
        get => _Items ??= new(this);
        set
        {
            if (value != _Items)
            {
                _Items?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Items.Add(e);
                    }
                }
            }
        }
    }

    #endregion Items

    public string? ImageUrl
        => ChapterId == null || !HasMainImage ? null
        : $"https://cdn.primagi.jp/assets/images/item/{ChapterId}/img_codination_{DirectoryNumber}_main.jpg";

    public bool ShouldSerializeImageUrl()
        => DataSet?.IgnoreCalculatedProperties != true;
}