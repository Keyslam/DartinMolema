#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Throw
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("thrown_value")]
    public uint ThrownValue { get; set; }

    [JsonProperty("assigned_value")]
    public uint AssignedValue { get; set; }

    [JsonProperty("kind")]
    public ThrowKind Kind { get; set; }
}