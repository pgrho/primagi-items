namespace Shipwreck.PrimagiItems;

internal sealed class HttpDownloader : IDisposable
{
    private readonly HttpClient _HttpClient;

    public HttpDownloader()
    {
        _HttpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        var r = await _HttpClient.GetAsync(url).ConfigureAwait(false);

        Console.WriteLine("Get[{0}]: {1:D} {1:G}", url, r.StatusCode);

        return r;
    }

    public void Dispose()
        => _HttpClient.Dispose();
}