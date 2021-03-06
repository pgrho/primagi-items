using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;

partial class Program
{
    static string[] prefs = new[]
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

    static async Task Main()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var items = new List<Item>();

        using var hc = new HttpClient();
        for (var pi = 0; pi < prefs.Length; pi++)
        {
            var p = prefs[pi];
            var url = $"https://cdnprimagiimg01.blob.core.windows.net/primagi/data/json/shop/{pi + 1}.json";

            var res = await hc.GetAsync(url);
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
                });
            }

            Console.WriteLine("Downloaded: {0} ({1})", p, url);
        }

        var di = new DirectoryInfo(Path.Combine(GetRepositoryPath(), "output"));
        if (!di.Exists)
        {
            di.Create();
        }

        for (var i = 0; i < 2; i++)
        {
            var sep = i == 0 ? '\t' : ',';
            using var sw = new StreamWriter(new FileStream(Path.Combine(di.FullName, i == 0 ? "shops.tsv" : "shops.csv"), FileMode.Create), i == 0 ? Encoding.UTF8 : Encoding.GetEncoding(932), 4096);

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
                sw.WriteLine(e.FullAddress);
            }
        }
    }

    private static string GetRepositoryPath([CallerFilePath] string filePath = "")
        => Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(filePath)))!;
}
