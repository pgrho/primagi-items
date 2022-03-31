namespace Shipwreck.PrimagiItems.Web.Models;

public sealed class UserCoordinationItem
{
    public string? SealId { get; set; }

    public int Level { get; set; }

    public int OtherPosesssionCount { get; set; }
    public int ListingCount { get; set; }
    public int DesiredCount { get; set; }
    public string? Remarks { get; set; }
}