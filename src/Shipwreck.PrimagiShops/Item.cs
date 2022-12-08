using System.Text.RegularExpressions;

class Item
{
    private static readonly Regex _PrismStone = new Regex("^プリズムストーン");
    private static readonly Regex _Namco = new Regex("^namco");
    private static readonly Regex _Mori = new Regex("^(モーリーファンタジー|PALO)");
    private static readonly Regex _Taito = new Regex("^(タイトーステーション|ハロータイトー|Hey)");
    private static readonly Regex _Bic = new Regex("ビックカメラ");
    private static readonly Regex _Yodobashi = new Regex("^ヨドバシカメラ");
    private static readonly Regex _ItoYokado = new Regex("^イトーヨーカドー");

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Prefecture { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ShopGroup { get; set; }

    public string? FullAddress
        => Prefecture + Address;

    public string? Category
        => Name == null ? "その他"
        : _PrismStone.IsMatch(Name) ? "プリズムストーン"
        : _Namco.IsMatch(Name) ? "namco"
        : _Mori.IsMatch(Name) ? "モーリーファンタジー・PALO"
        : _Taito.IsMatch(Name) ? "タイトー"
        : _Bic.IsMatch(Name) ? "ビックカメラ"
        : _Yodobashi.IsMatch(Name) ? "ヨドバシカメラ"
        : _ItoYokado.IsMatch(Name) ? "イトーヨーカドー"
        : "その他";
}
