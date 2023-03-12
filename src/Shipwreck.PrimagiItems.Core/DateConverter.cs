using Newtonsoft.Json.Converters;

namespace Shipwreck.PrimagiItems;

public sealed class DateConverter : IsoDateTimeConverter
{
    public DateConverter()
    {
        base.DateTimeFormat = "yyyy-MM-dd";
    }
}