using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shipwreck.PrimagiItems;

internal static class ItemWriter
{
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
        public int? icon { get; set; }
        public string? kinds { get; set; }
        public int? directoryNumber { get; set; }
        public bool isShow { get; set; }
        public bool hasMainImage { get; set; }
        public string? span { get; set; }
        public bool isShowItem { get; set; }
        public int? order { get; set; }
        public string? Chapter { get; set; }
        public string? ChapterName { get; set; }
    }

    private const string ITEM_URL = "https://cdnprimagiimg01.blob.core.windows.net/primagi/assets/data/item.json";
    private static readonly string[] KNOWN_GENRES = new[] { "カジュアル", "スタイリッシュ", "ライブ" };
    private static readonly string[] KNOWN_CATEGORIES = new[] { "トップス", "ワンピ", "ボトムス", "シューズ", "アクセ" };
    private static readonly string[] KNOWN_RARITIES = new[] { "R", "SR", "UR", "PMR", "HR", "OPR" };

    private static readonly string?[] KNOWN_BRANDS = new[] {
        "LOVELY MELODY",
        "VIVID STAR",
        "Radiant Abyss",
        "Eternal Revue",
        "ELECTRO REMIX",
        "Cherry Sugar",
        "SHINING DIVA",
        "PrismStone",
        "Princess Magic",
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

    public static async Task GenerateAsync(HttpClient http, DirectoryInfo directory, PrimagiDataSet ds)
    {
        var res = await http.GetAsync(ITEM_URL);

        var json = await res.Content.ReadAsStringAsync();

        Console.WriteLine("Downloaded: {0}", ITEM_URL);

        var jd = JArray.Parse(json);
        var rawitems = jd.ToObject<List<Item>>()!;
        var items = rawitems.GroupBy(e => e.SealId).Select(e => e.First()).ToList();

        var fe = (JObject)jd[0];

        var props = fe.Properties().Select(e => e.Name).ToList();

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
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, i == 0 ? "items.tsv" : "items.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

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
            var c = new Color()
            {
                Key = kv.Key,
                Value = kv.Value.Contains('#') ? null : kv.Value
            };
            ds.Colors.Add(c);
            try
            {
                if (c.Images.FirstOrDefault(e => e.GenreIndex == 3) is GenreColorImage gci)
                {
                    using var ir = await http.GetAsync(gci.ImageUrl).ConfigureAwait(false);
                    if (ir.IsSuccessStatusCode)
                    {
                        using var st = await ir.Content.ReadAsStreamAsync().ConfigureAwait(false);

                        if (Image.FromStream(st) is Bitmap bmp)
                        {
                            var cc = bmp.GetPixel(12, 22);

                            c.R = cc.R;
                            c.G = cc.G;
                            c.B = cc.B;
                        }
                    }
                }
            }
            catch { }
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
            e.collection,
            e.directoryNumber,
        }))
        {
            var gf = rawitems.FirstOrDefault(e => e.collection == cg.Key.collection && e.directoryNumber == cg.Key.directoryNumber && !string.IsNullOrEmpty(e.span)) ?? cg.First();
            var coord = new Coordination
            {
                Collection = cg.Key.collection,
                DirectoryNumber = cg.Key.directoryNumber ?? int.MaxValue,

                Name = gf.CoordinationName,
                ChapterId = gf.ChapterName ?? gf.Chapter,
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
                    Name = e.CoordinationName,
                    Watcha = e.Watcha,
                    GenreIndex = e.Genre,
                    BrandIndex = e.Brand,
                    ColorIndex = e.Color,
                    RarityIndex = e.Rarity,
                    CategoryIndex = e.Category,
                    SubCategoryIndex = e.SubCategory,
                    IsShowItem = e.isShowItem,
                    Icon = e.icon ?? 0,
                    Release = e.release,
                });
            }
        }
        var unlisted = JsonConvert.DeserializeObject<PrimagiDataSet>(await File.ReadAllTextAsync(Path.Combine(directory.Parent!.FullName, "unlisted", "unlistedItems.json")))!;

        var rawRepo = new Uri("https://raw.githubusercontent.com/pgrho/primagi-items/master/unlisted/img/");

        foreach (var uc in unlisted.Coordinations)
        {
            if (uc.Items.Count == 0)
            {
                var tc = ds.Coordinations.FirstOrDefault(e => e.ChapterId == uc.ChapterId && e.Name == uc.Name);

                if (tc != null)
                {
                    tc.WhichShowId = uc.WhichShowId;
                }

                continue;
            }

            var nc = new Coordination
            {
                Collection = uc.Collection,
                DirectoryNumber = uc.DirectoryNumber,

                Name = uc.Name,
                ChapterId = uc.ChapterId,
                IsShow = uc.IsShow,
                HasMainImage = uc.HasMainImage,
                Kinds = uc.Kinds,
                Span = uc.Span,
                Order = uc.Order,
            };
            foreach (var ui in uc.Items)
            {
                nc.Items.Add(new CoordinationItem
                {
                    Id = ui.Id,
                    ModelName = ui.ModelName,
                    SealId = ui.SealId,
                    Name = ui.Name,
                    Watcha = ui.Watcha,
                    GenreIndex = ui.GenreIndex,
                    BrandIndex = ui.BrandIndex,
                    ColorIndex = ui.ColorIndex,
                    RarityIndex = ui.RarityIndex,
                    CategoryIndex = ui.CategoryIndex,
                    SubCategoryIndex = ui.SubCategoryIndex,
                    IsShowItem = ui.IsShowItem,
                    Icon = ui.Icon,
                    Release = ui.Release,
                    ImageUrl = ui.ShouldSerializeImageUrl() ? new Uri(rawRepo, ui.ImageUrl).ToString() : null,
                });
            }
            ds.Coordinations.Add(nc);
        }

        var cols = new (string dislayName, Func<CoordinationItem, object?> getter)[]
        {
            ("ID", e => e.Id),
            ("刻印", e => e.SealId),
            ("レアリティ", e => e.Rarity?.Value),
            ("コーデ", e => e.Name),
            ("ジャンル", e => e.Genre?.Value),
            ("部位", e => e.Category?.Value),
            ("ブランド", e => e.Brand?.Value),
            ("色", e => e.Color?.Value),
            ("テイスト", e => e.SubCategory?.Value),
            ("ワッチャ", e => e.Watcha),
            ("画像", e => e.ImageUrl),
        };

        foreach (var cg in ds.Coordinations.GroupBy(e => e.ChapterId))
        {
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, cg.Key + ".md"), FileMode.Create), Encoding.UTF8, 4096);

            foreach (var kg in cg.GroupBy(e => new { e.Collection, e.DirectoryNumber }).GroupBy(e => e.First().Kinds))
            {
                sw.Write("## ");
                sw.WriteLine(kg.Key);

                foreach (var c in cols)
                {
                    sw.Write('|');
                    sw.Write(c.dislayName);
                }
                sw.WriteLine("|");

                foreach (var c in cols)
                {
                    sw.Write("|-");
                }
                sw.WriteLine("|");

                foreach (var e in kg.SelectMany(e => e).SelectMany(e => e.Items))
                {
                    foreach (var c in cols)
                    {
                        sw.Write('|');
                        sw.Write(c.getter(e));
                    }
                    sw.WriteLine("|");
                }

                sw.WriteLine();
            }
        }
    }
}