using System.Diagnostics;
using System.Text;

namespace Shipwreck.PrimagiItems;

internal sealed class HttpDownloader : IDisposable
{
    private readonly HttpClient _HttpClient;
    private readonly DirectoryInfo _Directory;
    private readonly string _ListPath;
    private readonly SortedDictionary<string, (int id, int statusCode, DateTimeOffset? lastModified)> _List;

    public HttpDownloader()
    {
        _HttpClient = new HttpClient();
        _Directory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), typeof(HttpDownloader).FullName!));
        if (!_Directory.Exists)
        {
            _Directory.Create();
        }
        _ListPath = Path.Combine(_Directory.FullName, "list.tsv");
        _List = new();
        if (File.Exists(_ListPath))
        {
            using (var sr = new StreamReader(_ListPath, Encoding.UTF8))
            {
                for (var l = sr.ReadLine(); l != null; l = sr.ReadLine())
                {
                    var fields = l.Split('\t');
                    if (fields.Length >= 3
                        && !string.IsNullOrEmpty(fields[0])
                        && int.TryParse(fields[1], out var st))
                    {
                        _List[fields[0]] = (_List.Count + 1, st, DateTimeOffset.TryParse(fields[2], out var dt) ? dt : null);
                    }
                }
            }
        }
    }

    public async Task<Stream> GetAsync(string url)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        if (_List.TryGetValue(url, out var tp)
            && tp.statusCode == 200
            && tp.lastModified != null
            && File.Exists(Path.Combine(_Directory.FullName, tp.id + ".dat")))
        {
            req.Headers.IfModifiedSince = tp.lastModified;
        }

        Console.WriteLine("Get {0}", url);
        var sw = Stopwatch.StartNew();
        var r = await _HttpClient.GetAsync(url).ConfigureAwait(false);
        sw.Stop();
        Console.WriteLine("Get {0}: {1:D} {1:G} in {2:#,0}ms", url, r.StatusCode, sw.ElapsedMilliseconds);

        if (r.StatusCode == System.Net.HttpStatusCode.NotModified)
        {
            return new FileStream(Path.Combine(_Directory.FullName, tp.id + ".dat"), FileMode.Open, FileAccess.Read);
        }

        tp = (tp.id > 0 ? tp.id : _List.Count + 1, (int)r.StatusCode, r.Content?.Headers.LastModified);
        _List[url] = tp;

        var hs = await r.Content.ReadAsStreamAsync().ConfigureAwait(false);

        if (tp.statusCode == 200
            && tp.lastModified != null)
        {
            using (hs)
            {
                var fs = new FileStream(Path.Combine(_Directory.FullName, tp.id + ".dat"), FileMode.Create);
                await hs.CopyToAsync(fs).ConfigureAwait(false);
                await fs.FlushAsync().ConfigureAwait(false);

                fs.Position = 0;

                return fs;
            }
        }

        return hs;
    }

    public async Task<string> GetStringAsync(string url)
    {
        using var s = await GetAsync(url).ConfigureAwait(false);
        using var sr = new StreamReader(s, Encoding.UTF8);

        return await sr.ReadToEndAsync().ConfigureAwait(false);
    }

    public void Dispose()
    {
        using (var sw = new StreamWriter(_ListPath, false, Encoding.UTF8))
        {
            foreach (var kv in _List.OrderBy(e => e.Value.id))
            {
                sw.Write(kv.Key);
                sw.Write('\t');
                sw.Write(kv.Value.statusCode);
                sw.Write('\t');
                sw.WriteLine(kv.Value.lastModified?.ToString("O"));
            }
        }

        _HttpClient.Dispose();
    }
}