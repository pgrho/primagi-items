namespace Shipwreck.PrimagiItems;

class Item
{
    public int Id { get; set; }

    public string? CoordinationName { get; set; }
    public string? modelName { get; set; }
    public int Genre { get; set; }
    public int Brand { get; set; }
    public int Color { get; set; }
    public int Rarity { get; set; }
    public int Watcha { get; set; }
    public int Category { get; set; }
    public int SubCategory { get; set; }
    public string? SealId { get; set; }
    public int collection { get; set; }
    public int release { get; set; }
    public int icon { get; set; }
    public string? kinds { get; set; }
    public int? directoryNumber { get; set; }
    public bool isShow { get; set; }
    public bool hasMainImage { get; set; }
    public string? span { get; set; }
    public bool isShowItem { get; set; }
    public int? order { get; set; }
    public string? Chapter { get; set; }
}
