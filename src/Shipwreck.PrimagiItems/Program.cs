using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Shipwreck.PrimagiItems;

class Program
{
    private const string src = "https://cdnprimagiimg01.blob.core.windows.net/primagi/assets/data/item.json";

    private static readonly string[] KNOWN_GENRES = new[] { "カジュアル", "スタイリッシュ", "ライブ" };
    private static readonly string[] KNOWN_CATEGORIES = new[] { "トップス", "ワンピ", "ボトムス", "シューズ", "アクセ" };
    private static readonly string[] KNOWN_RARITIES = new[] { "R", "SR", "UR", "PMR" };

    private static readonly string?[] KNOWN_BRANDS = new[] {
    "LOVELY MELODY",
    "VIVID STAR",
    "Radiant Abyss",
    "Eternal Revue",
    null,
    null,
    "SHINING DIVA",
    "PrismStone",
};

    private static readonly string[] KNOWN_COLORS = new[] { "ちゃ", "あか", "ピンク", "オレンジ", "きいろ", "みどり", "みずいろ", "あお", "むらさき", "くろ", "しろ", "シルバー", "ゴールド", };

    private static readonly string?[] KNOWN_SUBCATEGORIES = new[] {
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
        "エレガント",
    };

    static async Task Main()
    {
        using var hc = new HttpClient();
        var res = await hc.GetAsync(src);

        var json = await res.Content.ReadAsStringAsync();
        var jd = JArray.Parse(json);
        var items = jd.ToObject<List<Item>>()!;

        var fe = (JObject)jd[0];

        var props = fe.Properties().Select(e => e.Name).ToList();

        var di = new DirectoryInfo(Path.Combine(GetRepositoryPath(), "output"));
        if (!di.Exists)
        {
            di.Create();
        }

        static Dictionary<int, string> toDictionary(string?[] array)
            => array.Select((e, i) => (e, i)).Where(t => t.e != null).ToDictionary(t => t.i + 1, t => t.e!);

        var rarities = toDictionary(KNOWN_RARITIES);
        var genres = toDictionary(KNOWN_GENRES);
        var categories = toDictionary(KNOWN_CATEGORIES);
        var brands = toDictionary(KNOWN_BRANDS);
        var colors = toDictionary(KNOWN_COLORS);
        var subcategories = toDictionary(KNOWN_SUBCATEGORIES);

        static string getValue(Dictionary<int, string> dic, int no, string? errorMessage = null)
        {
            if (!dic.TryGetValue(no, out var s))
            {
                s = "error#" + no;
                dic[no] = s;

                if (errorMessage != null)
                {
                    Console.Error?.WriteLine(errorMessage);
                }
            }
            return s;
        }


        using (var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, "items.tsv"), FileMode.Create), Encoding.UTF8, 4096))
        {
            foreach (var p in props)
            {
                sw.Write(p);
                sw.Write('\t');
            }
            sw.Write("RarityName\t");
            sw.Write("GenreName\t");
            sw.Write("CategoryName\t");
            sw.Write("BrandName\t");
            sw.Write("ColorName\t");
            sw.Write("SubcategoryName");
            sw.WriteLine();

            for (var i = 0; i < items.Count; i++)
            {
                var obj = (JObject)jd[i];
                var item = items[i];

                foreach (var p in props)
                {
                    obj.TryGetValue(p, out var jp);
                    sw.Write(jp);
                    sw.Write('\t');
                }

                string getValueWithError(Dictionary<int, string> dic, int no, string errorKey)
                    => getValue(dic, no, $"{item.SealId}: {item.CoordinationName} {errorKey}={no}");

                sw.Write(getValueWithError(rarities, item.Rarity, nameof(item.Rarity)));
                sw.Write('\t');
                sw.Write(getValueWithError(genres, item.Genre, nameof(item.Genre)));
                sw.Write('\t');
                sw.Write(getValueWithError(categories, item.Category, nameof(item.Category)));
                sw.Write('\t');
                sw.Write(getValueWithError(brands, item.Brand, nameof(item.Brand)));
                sw.Write('\t');
                sw.Write(getValueWithError(colors, item.Color, nameof(item.Color)));
                sw.Write('\t');
                sw.Write(getValueWithError(subcategories, item.SubCategory, nameof(item.SubCategory)));

                sw.WriteLine();
            }
        }

        var cols = new (string, Func<Item, object>)[]
        {
            ("ID", e => e.Id),
            ("刻印", e => e.SealId),
            ("レアリティ", e => getValue(rarities, e.Rarity)),
            ("コーデ", e => e.CoordinationName),
            ("ジャンル", e => getValue(genres, e.Genre)),
            ("部位", e => getValue(categories, e.Category)),
            ("ブランド", e => getValue(brands, e.Brand)),
            ("色", e => getValue(colors, e.Color)),
            ("テイスト", e => getValue(subcategories, e.SubCategory)),
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
        => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(filePath)))!;
}
