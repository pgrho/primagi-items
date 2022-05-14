using System.IO.Compression;
using Newtonsoft.Json;

namespace Shipwreck.PrimagiItems.Web.Models;

internal sealed class LocalStorageUserDataAccessor : IUserDataAccessor
{
    private readonly IJSRuntime _JS;

    public LocalStorageUserDataAccessor(IJSRuntime js)
    {
        _JS = js;
    }

    public async Task<UserData?> ReadAsync(DateTimeOffset? ifModifiedSince)
    {
        try
        {
            var rawData = await _JS.InvokeAsync<string>("__readLocalStorage", typeof(UserData).FullName);
            if (!string.IsNullOrEmpty(rawData))
            {
                UserData? ud = null;
                if (rawData[0] == '{')
                {
                    ud = JsonConvert.DeserializeObject<UserData>(rawData);
                }
                else
                {
                    var data = Convert.FromBase64String(rawData);
                    try
                    {
                        using (var ms = new MemoryStream(data))
                        using (var gs = new GZipStream(ms, CompressionMode.Decompress))
                        using (var sr = new StreamReader(gs, Encoding.UTF8))
                        using (var jr = new JsonTextReader(sr))
                        {
                            ud = new JsonSerializer().Deserialize<UserData>(jr);
                        }
                    }
                    catch
                    {
                    }
                }

                if (ud != null)
                {
                    if (ud.LastUpdated <= ifModifiedSince)
                    {
                        return null;
                    }
                    return ud;
                }
            }
        }
        catch { }
        return new UserData();
    }

    public async Task WriteAsync(UserData userData)
    {
        userData ??= new UserData();
        userData.LastUpdated = DateTimeOffset.Now;

        using (var ms = new MemoryStream())
        {
            using (var gs = new GZipStream(ms, CompressionMode.Compress, true))
            using (var sw = new StreamWriter(gs, Encoding.UTF8))
            using (var jw = new JsonTextWriter(sw))
            {
                new JsonSerializer().Serialize(jw, userData);
            }
            await _JS.InvokeAsync<string>(
                "__writeLocalStorage",
                typeof(UserData).FullName,
                Convert.ToBase64String(ms.ToArray()));
        }
    }
}