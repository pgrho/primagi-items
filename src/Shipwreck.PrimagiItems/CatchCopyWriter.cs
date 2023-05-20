using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Shipwreck.PrimagiItems;

internal static class CatchCopyWriter
{
    class CatchCopyData
    {
        public string? Chapter { get; set; }
        public int Id { get; set; }
        public int Category { get; set; }
        public string? Conditions { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
    }
    static readonly string[] categories =
    {
        "プリマジスタランクでゲット",
        "コーデでゲット",
        "リズムゲームでゲット",
        "マイキャラメイクでゲット",
        "イベント・キャンペーンでゲット",
        "期間限定でゲット",
        "プリマジスタジオランウェイでゲット",
        "プリマジフレンドカードでゲット",
        "プレミアムプランでゲット",
        "ミックスコーデコンテストでゲット",
    };

    private const string URL = "https://cdnprimagiimg01.blob.core.windows.net/primagi/data/json/copy/copy.json";

    public static async Task GenerateAsync(HttpDownloader http, DirectoryInfo directory, PrimagiDataSet ds)
    {
        var res = await http.GetAsync(URL);

        var json = await res.Content.ReadAsStringAsync();


        var jd = JArray.Parse(json);
        var rawitems = jd.ToObject<List<CatchCopyData>>()!;
        var items = rawitems.GroupBy(e => e.Id).Select(e => e.First()).OrderBy(e => e.Id).ThenBy(e => e.Category).ToList();

        static string? sanitize(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : Regex.Replace(s.Trim(), @"[\s\r\n]+", " ");

        foreach (var e in items)
        {
            e.Chapter = sanitize(e.Chapter);
            e.Conditions = sanitize(e.Conditions);
            e.Url = sanitize(e.Url);
            e.Name = sanitize(e.Name);
        }

        for (var i = 0; i < categories.Length; i++)
        {
            ds.CatchCopyCategories.Add(new()
            {
                Key = i + 1,
                Value = categories[i]
            });
        }

        var cats = ds.CatchCopyCategories.ToDictionary(e => e.Key, e => e.Value);
        string getCategoryName(int no)
        {
            if (!cats.TryGetValue(no, out var s))
            {
                s = "error#" + no;
                cats[no] = s;

                Console.Error?.WriteLine($"Invalid CatchCopy category: {no}");
            }
            return s;
        }
        var chaps = new Dictionary<string, (int y, int c)>();
        (int y, int c) parseChapters(string chapter)
        {
            if (!chaps.TryGetValue(chapter ?? string.Empty, out var r))
            {
                var m = Regex.Match(chapter ?? string.Empty, "^(\\d)期(\\d)章$");
                if (m.Success)
                {
                    r = (m.Groups[1].Value[0] - '0', m.Groups[2].Value[0] - '0');
                }
                else
                {
                    Console.Error?.WriteLine($"Invalid CatchCopy chapter: {chapter}");
                    r = (-1, -1);
                }
                chaps[chapter ?? string.Empty] = r;
            }
            return r;
        }

        var fe = (JObject)jd[0];

        var props = fe.Properties().Select(e => e.Name).ToList();

        var pageUrl = new Uri("https://primagi.jp/CatchCopys/");
        for (var i = 0; i < 2; i++)
        {
            var sep = i == 0 ? '\t' : ',';
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, i == 0 ? "copies.tsv" : "copies.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

            foreach (var p in props)
            {
                sw.Write(p);
                sw.Write(sep);
            }
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

                sw.WriteLine();
            }
        }

        {
            var cols = new (string, Func<CatchCopyData, object?>)[]
            {
                ("", e => $"![](https://cdn.primagi.jp/assets/images/copy/{e.Id}.png \"{e.Name}\")"),
                ("ID", e => e.Id),
                ("年", e => parseChapters(e.Chapter).y),
                ("章", e => parseChapters(e.Chapter).c),
                ("カテゴリー", e => getCategoryName(e.Category)),
                ("名前", e => e.Name),
                ("取得条件", e => e.Conditions),
                ("画像", e => $"https://cdn.primagi.jp/assets/images/copy/{e.Id}.png"),
            };
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, "copies.md"), FileMode.Create), Encoding.UTF8, 4096);

            foreach (var cg in items.GroupBy(e => e.Category).OrderBy(e => e.Key))
            {
                sw.Write("## ");
                sw.WriteLine(getCategoryName(cg.Key));
                sw.WriteLine();

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
        foreach (var s in items)
        {
            var chap = parseChapters(s.Chapter);
            ds.CatchCopies.Add(new CatchCopy
            {
                Id = s.Id,
                ChapterId = chap.y > 1 ? $"{chap.y}P{chap.c:D2}" : $"P{chap.c:D2}",
                CategoryId = s.Category,
                Conditions = s.Conditions,
                Url = s.Url,
                Name = s.Name,
            });
        }
    }
}
