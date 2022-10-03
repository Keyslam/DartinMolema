#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Match
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("players")]
    public List<Guid> Players { get; set; }

    [JsonProperty("winner_id")]
    public Guid? WinnerId { get; set; }

    [JsonProperty("sets_to_win")]
    public uint SetsToWin { get; set; }

    [JsonProperty("legs_to_win")]
    public uint LegsToWin { get; set; }

    [JsonProperty("score_to_win")]
    public uint ScoreToWin { get; set; }

    [JsonProperty("throws_per_turn")]
    public uint ThrowsPerTurn { get; set; }

    [JsonProperty("sets")]
    public List<Set> Sets { get; set; }
}