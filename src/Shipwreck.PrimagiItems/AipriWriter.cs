using System.Runtime.CompilerServices;

namespace Shipwreck.PrimagiItems;

internal static class AipriWriter
{
    public static async Task GenerateAsync(DirectoryInfo directory, PrimagiDataSet ds)
    {
        var ids = await GetSealIdsAsync();

        foreach (var c in ds.Coordinations)
        {
            foreach (var e in c.Items)
            {
                e.Aipri = ids.Contains(e.SealId!);
            }
        }
    }

    private static async Task<List<string>> GetSealIdsAsync([CallerFilePath] string path = "")
    {
        var p = Path.Combine(Path.GetDirectoryName(path)!, "aipri.txt");
        using var sr = new StreamReader(p);

        var l = new List<string>();

        for (var v = await sr.ReadLineAsync(); v != null; v = await sr.ReadLineAsync())
        {
            v = v.Trim();
            if (v.Length > 0)
            {
                l.Add(v);
            }
        }

        return l;
    }
}
