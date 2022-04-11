using System.Runtime.Serialization;

namespace Shipwreck.PrimagiItems.Browser.Models;

[DataContract]
public class Character
{
    [DataMember, DefaultValue(null)]
    public string? CardId { get; set; }

    [DataMember, DefaultValue(null)]
    public string? PlayerName { get; set; }

    [DataMember, DefaultValue((byte)0)]
    public byte BirthMonth { get; set; }

    [DataMember, DefaultValue((byte)0)]
    public byte BirthDay { get; set; }
}