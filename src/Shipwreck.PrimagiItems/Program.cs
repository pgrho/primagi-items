using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shipwreck.PrimagiItems;

internal class Program
{
    private static async Task Main()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using var hc = new HttpClient();
        var di = new DirectoryInfo(Path.Combine(GetRepositoryPath(), "output"));
        if (!di.Exists)
        {
            di.Create();
        }
        var ds = new PrimagiDataSet();
        await ItemWriter.GenerateAsync(hc, di, ds);
        await CatchCopyWriter.GenerateAsync(hc, di, ds);
        await PartsWriter.GenerateAsync(hc, di, ds);
        await ShopWriter.GenerateAsync(hc, di, ds);

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