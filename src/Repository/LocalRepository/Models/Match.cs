#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Match
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

    public Match() { }

    public Match(App.Models.Match match)
    {
        this.Id = match.Id;
        this.Date = match.Date;
        this.Players = match.Players;
        this.WinnerId = match.WinnerId;
        this.SetsToWin = match.SetsToWin;
        this.LegsToWin = match.LegsToWin;
        this.ScoreToWin = match.ScoreToWin;
        this.ThrowsPerTurn = match.ThrowsPerTurn;
        this.Sets = match.Sets.Select(set => new Set(set)).ToList();
    }


    public App.Models.Match ToReal()
    {
        var match = new App.Models.Match();

        match.Id = this.Id;
        match.Date = this.Date;
        match.Players = this.Players;
        match.WinnerId = this.WinnerId;
        match.SetsToWin = this.SetsToWin;
        match.LegsToWin = this.LegsToWin;
        match.ScoreToWin = this.ScoreToWin;
        match.ThrowsPerTurn = this.ThrowsPerTurn;

        return match;
    }
}