using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Shipwreck.PrimagiItems;

class Program
{
    private const string src = "https://cdnprimagiimg01.blob.core.windows.net/primagi/assets/data/item.json";

    private static readonly string[] genre = new[] { "カジュアル", "スタイリッシュ", "ライブ" };
    private static readonly string[] category = new[] { "トップス", "ワンピ", "ボトムス", "シューズ", "アクセ" };
    private static readonly string[] rarity = new[] { "R", "SR", "UR", "PMR" };

    private static readonly string?[] brand = new[] {
    "LOVELY MELODY",
    "VIVID STAR",
    "Radiant Abyss",
    "Eternal Revue",
    null,
    null,
    "SHINING DIVA",
    "PrismStone",
};

    private static readonly string[] color = new[] { "ちゃ", "あか", "ピンク", "オレンジ", "きいろ", "みどり", "みずいろ", "あお", "むらさき", "くろ", "しろ", "シルバー", "ゴールド", };

    private static readonly string?[] subcategory = new[] {
        null,
        "フラワー",
        "スター",
        "ハート",
        "ガーリー",
        "キュート",
        "チェック",
        "リボン",
        "クール",
        "ゴシック",
        "ジュエル",
        "スポーツ",
        "ダンス",
        "ポップ",
        "ミステリアス",
        "ラブリー",
        "スイーツ",
        "ベーシック",
        "アニマル",
    };

    static async Task Main()
    {
        using var hc = new HttpClient();
        var res = await hc.GetAsync(src);

        var json = await res.Content.ReadAsStringAsync();
        var jd = JArray.Parse(json);

        var fe = (JObject)jd[0];

        var props = fe.Properties().Select(e => e.Name).ToList();

        var di = new DirectoryInfo(Path.Combine(GetRepositoryPath(), "output"));
        if (!di.Exists)
        {
            di.Create();
        }

        using (var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, "items.tsv"), FileMode.Create), Encoding.UTF8, 4096))
        {
            foreach (var p in props)
            {
                sw.Write(p);
                sw.Write('\t');
            }
            sw.WriteLine();
            foreach (JObject obj in jd)
            {
                foreach (var p in props)
                {
                    obj.TryGetValue(p, out var jp);
                    sw.Write(jp);
                    sw.Write('\t');
                }
                sw.WriteLine();
            }
        }

        var items = jd.ToObject<List<Item>>()!;

        var cols = new (string, Func<Item, object>)[]
        {
            ("ID", e => e.Id),
            ("刻印", e => e.SealId),
            ("レアリティ", e => rarity.ElementAtOrDefault(e.Rarity - 1) ?? e.Rarity.ToString()),
            ("コーデ", e => e.CoordinationName),
            ("ジャンル", e => genre.ElementAtOrDefault(e.Genre - 1) ?? e.Genre.ToString()),
            ("部位", e => category.ElementAtOrDefault(e.Category - 1) ?? e.Category.ToString()),
            ("ブランド", e => brand.ElementAtOrDefault(e.Brand - 1) ?? e.Brand.ToString()),
            ("色", e => color.ElementAtOrDefault(e.Color - 1) ?? e.Color.ToString()),
            ("テイスト", e => subcategory.ElementAtOrDefault(e.SubCategory - 1) ?? e.SubCategory.ToString()),
            ("ワッチャ", e => e.Watcha),
            ("画像", e => $"https://cdn.primagi.jp/assets/images/item/{e.Chapter}/{e.Id}.png"),
        };

        foreach (var cg in items.GroupBy(e => e.Chapter))
        {
            using var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, cg.Key + ".md"), FileMode.Create), Encoding.UTF8, 4096);

            foreach (var c in cols)
            {
                sw.Write('|');
                sw.Write(c.Item1);
            }
            sw.WriteLine("|");

            foreach (var c in cols)
            {
                sw.Write("|-");
            }
            sw.WriteLine("|");

            foreach (var e in cg)
            {
                foreach (var c in cols)
                {
                    sw.Write('|');
                    sw.Write(c.Item2(e));
                }
                sw.WriteLine("|");
            }
        }
    }

    private static string GetRepositoryPath([CallerFilePath] string filePath = "")
        => Path.GetDirectoryName(Path.GetDirectoryName(filePath))!;
}
