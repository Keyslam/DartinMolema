#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Set {
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("winner_id")]
    public Guid? WinnerId { get; set; }

    [JsonProperty("legs")]
    public List<Leg> Legs { get; set; }
}