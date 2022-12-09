using System.Text;
using System.Text.RegularExpressions;
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
        public int icon { get; set; }
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
    private static readonly string[] KNOWN_RARITIES = new[] { "R", "SR", "UR", "PMR", "HR" };

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
            ("画像", e => $"https://cdn.primagi.jp/assets/images/item/{e.Chapter ?? e.ChapterName}/{e.Id}.png"),
        };

        foreach (var cg in items.GroupBy(e => e.ChapterName ?? e.Chapter))
        {
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, cg.Key + ".md"), FileMode.Create), Encoding.UTF8, 4096);

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
                    Icon = e.icon,
                    Release = e.release,
                });
            }
        }
        var unlisted = JsonConvert.DeserializeObject<PrimagiDataSet>(await File.ReadAllTextAsync(Path.Combine(directory.Parent!.FullName, "unlisted", "unlistedItems.json")))!;

        var rawRepo = new Uri("https://raw.githubusercontent.com/pgrho/primagi-items/master/unlisted/img/");

        foreach (var uc in unlisted.Coordinations)
        {
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
    }
}

internal static class ShopWriter
{
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

    private static string[] prefs = new[]
    {
        "北海道",
        "青森県",
        "岩手県",
        "宮城県",
        "秋田県",
        "山形県",
        "福島県",
        "茨城県",
        "栃木県",
        "群馬県",
        "埼玉県",
        "千葉県",
        "東京都",
        "神奈川県",
        "新潟県",
        "富山県",
        "石川県",
        "福井県",
        "山梨県",
        "長野県",
        "岐阜県",
        "静岡県",
        "愛知県",
        "三重県",
        "滋賀県",
        "京都府",
        "大阪府",
        "兵庫県",
        "奈良県",
        "和歌山県",
        "鳥取県",
        "島根県",
        "岡山県",
        "広島県",
        "山口県",
        "徳島県",
        "香川県",
        "愛媛県",
        "高知県",
        "福岡県",
        "佐賀県",
        "長崎県",
        "熊本県",
        "大分県",
        "宮崎県",
        "鹿児島県",
        "沖縄県"
    };

    public static async Task GenerateAsync(HttpClient http, DirectoryInfo directory, PrimagiDataSet ds)
    {
        var items = new List<Item>();
        for (var pi = 0; pi < prefs.Length; pi++)
        {
            var p = prefs[pi];
            var url = $"https://cdnprimagiimg01.blob.core.windows.net/primagi/data/json/shop/{pi + 1}.json";

            var res = await http.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();
            var jd = JObject.Parse(json);

            foreach (var jp in jd.Properties())
            {
                items.Add(new Item
                {
                    Id = jp.Name,
                    Name = (jp.Value as JObject)?.Property("Name")?.Value?.Value<string>(),
                    Prefecture = p,
                    Address = (jp.Value as JObject)?.Property("Address")?.Value?.Value<string>(),
                    Latitude = (jp.Value as JObject)?.Property("Latitude")?.Value?.Value<double?>(),
                    Longitude = (jp.Value as JObject)?.Property("Longitude")?.Value?.Value<double?>(),
                    ShopGroup = (jp.Value as JObject)?.Property("ShopGroup")?.Value?.Value<string>(),
                });
            }

            Console.WriteLine("Downloaded: {0} ({1})", p, url);
        }

        for (var i = 0; i < 2; i++)
        {
            var sep = i == 0 ? '\t' : ',';
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, i == 0 ? "shops.tsv" : "shops.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

            sw.Write(nameof(Item.Id));
            sw.Write(sep);
            sw.Write(nameof(Item.Prefecture));
            sw.Write(sep);
            sw.Write(nameof(Item.Category));
            sw.Write(sep);
            sw.Write(nameof(Item.Name));
            sw.Write(sep);
            sw.Write(nameof(Item.Address));
            sw.Write(sep);
            sw.Write(nameof(Item.Latitude));
            sw.Write(sep);
            sw.Write(nameof(Item.Longitude));
            sw.Write(sep);
            sw.Write(nameof(Item.ShopGroup));
            sw.Write(sep);
            sw.WriteLine(nameof(Item.FullAddress));

            foreach (var e in items)
            {
                sw.Write(e.Id);
                sw.Write(sep);
                sw.Write(e.Prefecture);
                sw.Write(sep);
                sw.Write(e.Category);
                sw.Write(sep);
                sw.Write(e.Name);
                sw.Write(sep);
                sw.Write(e.Address);
                sw.Write(sep);
                sw.Write(e.Latitude);
                sw.Write(sep);
                sw.Write(e.Longitude);
                sw.Write(sep);
                sw.Write(e.ShopGroup);
                sw.Write(sep);
                sw.WriteLine(e.FullAddress);
            }
        }

        foreach (var jp in items)
        {
            ds.Shops.Add(new()
            {
                Id = jp.Id,
                Name = jp.Name,
                Prefecture = jp.Prefecture,
                Address = jp.Address,
                Latitude = jp.Latitude,
                Longitude = jp.Longitude,
                ShopGroup = jp.ShopGroup,
            });
        }
    }
}