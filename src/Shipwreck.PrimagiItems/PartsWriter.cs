using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Shipwreck.PrimagiItems;

internal static class PartsWriter
{
    class PartData
    {
        public string? Id { get; set; }
        public string? FileName { get; set; }
        public bool IsNew { get; set; }
        public string? Date { get; set; }

        public string? CategoryName { get; set; }
        public string? PartsName { get; set; }
        public string? Psd { get; set; }

        [JsonProperty("getType")]
        public string? Type { get; set; }

        public string? ModalText { get; set; }
        public string? Destination { get; set; }
        public string? Url { get; set; }
        public int CategoryId { get; set; }
        public int GetTypeId { get; set; }
    }

    private const string URL = "https://cdnprimagiimg01.blob.core.windows.net/primagi/assets/data/parts.json";

    public static async Task GenerateAsync(HttpClient http, DirectoryInfo directory, PrimagiDataSet ds)
    {
        var res = await http.GetAsync(URL);

        var json = await res.Content.ReadAsStringAsync();

        Console.WriteLine("Downloaded: {0}", URL);

        var jd = JArray.Parse(json);
        var rawitems = jd.ToObject<List<PartData>>()!;
        var items = rawitems.GroupBy(e => e.Id).Select(e => e.First()).ToList();

        static string? sanitize(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : Regex.Replace(s.Trim(), @"[\s\r\n]+", " ");

        foreach (var e in items)
        {
            e.PartsName = sanitize(e.PartsName);
            e.ModalText = sanitize(e.ModalText);
            e.Destination = sanitize(e.Destination);
            e.Url = sanitize(e.Url);
            e.CategoryName = sanitize(e.CategoryName);
            e.Type = sanitize(e.Type);
        }

        foreach (var g in items.GroupBy(e => e.CategoryId).OrderBy(e => e.Key))
        {
            ds.PartCategories.Add(new PartCategory
            {
                Key = g.Key,
                Value = g.FirstOrDefault(e => e.CategoryName != null)?.CategoryName
            });
        }
        foreach (var g in items.GroupBy(e => e.GetTypeId).OrderBy(e => e.Key))
        {
            ds.PartGetTypes.Add(new PartGetType
            {
                Key = g.Key,
                Value = g.FirstOrDefault(e => e.Type != null)?.Type
            });
        }

        var fe = (JObject)jd[0];

        var props = fe.Properties().Select(e => e.Name).ToList();

        var pageUrl = new Uri("https://primagi.jp/parts/");
        for (var i = 0; i < 2; i++)
        {
            var sep = i == 0 ? '\t' : ',';
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, i == 0 ? "parts.tsv" : "parts.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

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
            var cols = new (string, Func<PartData, object>)[]
            {
                ("", e => $"![](https://cdn.primagi.jp/assets/images/parts/thumb/img_{e.Id}.png \"{e.PartsName}\")"),
                ("ID", e => e.Id),
                ("名前", e => e.PartsName),
                ("取得条件", e => e.ModalText),
                ("画像", e => $"https://cdn.primagi.jp/assets/images/parts/thumb/img_{e.Id}.png"),
            };
            using var sw = new StreamWriter(new FileStream(Path.Combine(directory.FullName, "parts.md"), FileMode.Create), Encoding.UTF8, 4096);

            foreach (var cg in items.GroupBy(e => e.CategoryId))
            {
                sw.Write("## ");
                sw.WriteLine(cg.FirstOrDefault(e => e.CategoryName != null)?.CategoryName ?? ("#" + cg.Key.ToString()));
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
            ds.Parts.Add(new Part
            {
                Id = s.Id,
                File = int.TryParse(s.FileName, out var i) ? i : null,
                IsNew = s.IsNew,
                Date = DateTime.TryParseExact(s.Date, "yyyy.MM.dd", null, System.Globalization.DateTimeStyles.None, out var dt) ? dt : null,
                PartsName = s.PartsName,
                Psd = s.Psd,
                ModalText = s.ModalText,
                Destination = s.Destination,
                Url = string.IsNullOrEmpty(s.Url) ? null : new Uri(pageUrl, s.Url).ToString(),
                TypeId = s.GetTypeId,
                CategoryId = s.CategoryId,
            });
        }
    }
}
