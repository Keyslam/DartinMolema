#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Leg
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("winner_id")]
    public Guid? WinnerId { get; set; }

    [JsonProperty("turns")]
    public Dictionary<Guid, List<Turn>> Turns { get; set; }

    public Leg() { }

    public Leg(App.Models.Leg leg)
    {
        this.Id = leg.Id;
        this.WinnerId = leg.WinnerId;

        this.Turns = new Dictionary<Guid, List<Turn>>();

        foreach (var (guid, turn) in leg.Turns)
        {
            var turns = turn.Select(turn => new Turn(turn)).ToList();
            this.Turns.Add(guid, turns);
        }
    }

    public App.Models.Leg ToReal()
    {
        var leg = new App.Models.Leg();

        leg.Id = this.Id;
        leg.WinnerId = this.WinnerId;

        leg.Turns = new Dictionary<Guid, List<App.Models.Turn>>();

        foreach (var (guid, turn) in this.Turns)
        {
            var turns = turn.Select(turn => turn.ToReal()).ToList();
            leg.Turns.Add(guid, turns);
        }

        return leg;
    }
}