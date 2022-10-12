#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Turn
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("throws")]
    public List<Throw> Throws { get; set; }

    public Turn() { }

    public Turn(App.Models.Turn turn)
    {
        this.Id = turn.Id;
        this.Score = turn.Score;
        this.Throws = turn.Throws.Select(@throw => new Throw(@throw)).ToList();
    }

    public App.Models.Turn ToReal()
    {
        var turn = new App.Models.Turn();

        turn.Id = this.Id;
        turn.Score = this.Score;
        turn.Throws = this.Throws.Select(@throw => @throw.ToReal()).ToList();

        return turn;
    }
}