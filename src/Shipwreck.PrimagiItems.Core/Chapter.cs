using System.ComponentModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shipwreck.PrimagiItems;

public sealed class Chapter : KeyedItem<string>
{
    public string? DisplayName
        => Key == null ? null
        : Key == "UNKNOWN" ? "その他"
        : Year is int y && No is int v ? y switch
        {
            1 => null,
            2 => "スタジオ",
            3 => "プリティーオールスター",
            _ => $"{y}年目"
        } + $"第{v}章"
        : Key;

    public int? Year
        => Key == null ? null
        : Regex.Match(Key, "^P(?:\\d+)$") is var m && m.Success ? 1
        : Regex.Match(Key, "^(\\d)+P(?:\\d+)$") is var m2 && m2.Success ? int.Parse(m2.Groups[1].Value)
        : null;

    public int? No
        => Key == null ? null
        : Regex.Match(Key, "^(?:\\d)*P(\\d+)$") is var m2 && m2.Success ? int.Parse(m2.Groups[1].Value)
        : null;

    #region MyRegion

    private DateTime? _Start;
    private DateTime? _End;

    [JsonConverter(typeof(DateConverter))]
    public DateTime? Start
    {
        get => ParseSpan()._Start is DateTime d && d < DateTime.MaxValue ? d : null;
        set => _Start = value;
    }

    [JsonConverter(typeof(DateConverter))]
    public DateTime? End
    {
        get => ParseSpan()._End is DateTime d && d > DateTime.MinValue ? d : null;
        set => _End = value;
    }

    internal Chapter ParseSpan()
    {
        if (_Start != null || DataSet == null)
        {
            return this;
        }

        var s = DateTime.MaxValue;
        var e = DateTime.MinValue;
        foreach (var c in DataSet.Coordinations)
        {
            if (c.ChapterId == Key && c.IsChapterCollection)
            {
                if (c.SpanStart < s)
                {
                    s = c.SpanStart.Value;
                }
                if (c.SpanEnd > e)
                {
                    e = c.SpanEnd.Value;
                }
            }
        }

        _Start = s;
        _End = e;

        return this;
    }

    public bool ShouldSerializeStart()
        => DataSet?.IgnoreCalculatedProperties != true;

    public bool ShouldSerializeEnd()
        => ShouldSerializeStart();

    #endregion MyRegion
}