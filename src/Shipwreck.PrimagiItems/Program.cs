using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shipwreck.PrimagiItems;

internal class Program
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
        "ELECTRO REMIX",
        "Cherry Sugar",
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
        "デジタル",
    };

    private static async Task Main()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using var hc = new HttpClient();
        var res = await hc.GetAsync(src);

        var json = await res.Content.ReadAsStringAsync();

        Console.WriteLine("Downloaded: {0}", src);

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

        for (var i = 0; i < 2; i++)
        {
            var sep = i == 0 ? '\t' : ',';
            using var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, i == 0 ? "items.tsv" : "items.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

            foreach (var p in props)
            {
                sw.Write(p);
                sw.Write(sep);
            }
            sw.Write("RarityName");
            sw.Write(sep);
            sw.Write("GenreName");
            sw.Write(sep);
            sw.Write("CategoryName");
            sw.Write(sep);
            sw.Write("BrandName");
            sw.Write(sep);
            sw.Write("ColorName");
            sw.Write(sep);
            sw.Write("SubcategoryName");
            sw.WriteLine();

            for (var j = 0; j < items.Count; j++)
            {
                var obj = (JObject)jd[j];
                var item = items[j];

                foreach (var p in props)
                {
                    obj.TryGetValue(p, out var jp);
                    sw.Write(jp);
                    sw.Write(sep);
                }

                string getValueWithError(Dictionary<int, string> dic, int no, string errorKey)
                    => getValue(dic, no, $"{item.SealId}: {item.CoordinationName} {errorKey}={no}");

                sw.Write(getValueWithError(rarities, item.Rarity, nameof(item.Rarity)));
                sw.Write(sep);
                sw.Write(getValueWithError(genres, item.Genre, nameof(item.Genre)));
                sw.Write(sep);
                sw.Write(getValueWithError(categories, item.Category, nameof(item.Category)));
                sw.Write(sep);
                sw.Write(getValueWithError(brands, item.Brand, nameof(item.Brand)));
                sw.Write(sep);
                sw.Write(getValueWithError(colors, item.Color, nameof(item.Color)));
                sw.Write(sep);
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

        var ds = new PrimagiDataSet();
        foreach (var kv in genres.OrderBy(e => e.Key))
        {
            ds.Genres.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }
        foreach (var kv in brands.OrderBy(e => e.Key))
        {
            ds.Brands.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }
        foreach (var kv in colors.OrderBy(e => e.Key))
        {
            ds.Colors.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }
        foreach (var kv in rarities.OrderBy(e => e.Key))
        {
            ds.Rarities.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }
        foreach (var kv in categories.OrderBy(e => e.Key))
        {
            ds.Categories.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }
        foreach (var kv in subcategories.OrderBy(e => e.Key))
        {
            ds.SubCategories.Add(new()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            });
        }

        foreach (var cg in items.GroupBy(e => new
        {
            e.CoordinationName,
            e.collection,
            e.directoryNumber,
        }))
        {
            var gf = cg.FirstOrDefault(e => !string.IsNullOrEmpty(e.span)) ?? cg.First();
            var coord = new Coordination
            {
                Name = cg.Key.CoordinationName,
                Collection = cg.Key.collection,
                DirectoryNumber = cg.Key.directoryNumber,

                Chapter = gf.Chapter,
                IsShow = gf.isShow,
                HasMainImage = gf.hasMainImage,
                Kinds = gf.kinds,
                Span = gf.span,
                Order = gf.order ?? int.MaxValue,
            };
            ds.Coordinations.Add(coord);

            foreach (var e in cg)
            {
                coord.Items.Add(new()
                {
                    Id = e.Id,
                    ModelName = e.modelName,
                    SealId = e.SealId,
                    Watcha = e.Watcha,
                    GenreIndex = e.Genre,
                    BrandIndex = e.Brand,
                    ColorIndex = e.Color,
                    RarityIndex = e.Rarity,
                    CategoryIndex = e.Category,
                    SubCategoryIndex = e.SubCategory,
                    IsShowItem = e.isShowItem,
                    Icon = e.icon,
                    Release = e.release,
                });
            }
        }
        var jss = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        using (var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, "items.json"), FileMode.Create), Encoding.UTF8, 4096))
        {
            await sw.WriteLineAsync(
                JsonConvert.SerializeObject(ds,
                Formatting.Indented, jss));
        }
        ds.IgnoreCalculatedProperties = true;
        using (var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, "items.raw.json"), FileMode.Create), Encoding.UTF8, 4096))
        {
            await sw.WriteLineAsync(
                JsonConvert.SerializeObject(ds,
                Formatting.None, jss));
        }
    }

    private static string GetRepositoryPath([CallerFilePath] string filePath = "")
        => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(filePath)))!;
}