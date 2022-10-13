#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Leg
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("winner_id")]
    public Guid WinnerId { get; set; }

    [JsonProperty("turns")]
    public Dictionary<Guid, List<Turn>> Turns { get; set; }

    [JsonProperty("statistics")]
    public Dictionary<Guid, PlayerLegStatistic> Statistics { get; set; }

    public Leg() { }

    public Leg(App.Models.Leg leg)
    {
        this.Id = leg.Id;
        this.WinnerId = leg.WinnerId;

        this.Turns = new Dictionary<Guid, List<Turn>>();
        foreach (var (guid, turns) in leg.Turns)
        {
            var turn = turns.Select(turn => new Turn(turn)).ToList();
            this.Turns.Add(guid, turn);
        }

        this.Statistics = new Dictionary<Guid, PlayerLegStatistic>();
        foreach (var (guid, statistics) in leg.Statistics)
        {
            var statistic = new PlayerLegStatistic(statistics);
            this.Statistics.Add(guid, statistic);
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

        leg.Statistics = new Dictionary<Guid, App.Models.PlayerLegStatistic>();
        foreach (var (guid, statistics) in this.Statistics)
        {
            var statistic = statistics.ToReal();
            leg.Statistics.Add(guid, statistic);
        }

        return leg;
    }
}