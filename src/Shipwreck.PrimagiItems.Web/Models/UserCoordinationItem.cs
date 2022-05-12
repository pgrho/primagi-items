namespace Shipwreck.PrimagiItems.Web.Models;

public sealed class UserCoordinationItem
{
    public string? SealId { get; set; }

    public int? PosessionCount { get; set; }

    public int ListingCount { get; set; }
    public int TradingCount { get; set; }
    public string? Remarks { get; set; }
}