#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Turn
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("score")]
    public uint Score { get; set; }

    [JsonProperty("throws")]
    public List<Throw> Throws { get; set; }
}