namespace Shipwreck.PrimagiItems.Web.Models;

public sealed class UserCoordinationItem
{
    [DefaultValue(null)]
    public string? SealId { get; set; }

    [DefaultValue(0)]
    public int PosessionCount { get; set; }

    [DefaultValue(0)]
    public int ListingCount { get; set; }

    [DefaultValue(0)]
    public int TradingCount { get; set; }

    [DefaultValue(null)]
    public string? Remarks { get; set; }
}