using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace Shipwreck.PrimagiItems.Browser.Models;

[DataContract]
public class BrowserDataContext
{
    #region Characters

    private List<Character>? _Characters;

    [DataMember]
    public IList<Character> Characters
    {
        get => _Characters ??= new List<Character>();
        set
        {
            if (value != _Characters)
            {
                _Characters?.Clear();
                if (value != null)
                {
                    foreach (var e in value)
                    {
                        Characters.Add(e);
                    }
                }
            }
        }
    }

    #endregion Characters

    public static BrowserDataContext Load(Stream stream)
    {
        using (var sr = new StreamReader(stream, encoding: Encoding.UTF8, leaveOpen: true))
        {
            return Parse(sr);
        }
    }

    public static BrowserDataContext Parse(TextReader reader)
    {
        using (var jtr = new JsonTextReader(reader)
        {
            CloseInput = false
        })
        {
            return new JsonSerializer().Deserialize<BrowserDataContext>(jtr) ?? throw new ArgumentException();
        }
    }

    public static BrowserDataContext Parse(string json)
        => JsonConvert.DeserializeObject<BrowserDataContext>(json) ?? throw new ArgumentException();

    public void Save(Stream stream)
    {
        using (var sw = new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: true))
        {
            Save(sw);
        }
    }

    public void Save(TextWriter writer)
        => new JsonSerializer().Serialize(writer, this);
}