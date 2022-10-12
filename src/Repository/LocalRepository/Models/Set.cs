#pragma warning disable 8618

using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Set
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("winner_id")]
    public Guid WinnerId { get; set; }

    [JsonProperty("legs")]
    public List<Leg> Legs { get; set; }

    [JsonProperty("statistics")]
    public Dictionary<Guid, PlayerSetStatistic> Statistics { get; set; }

    public Set() { }

    public Set(App.Models.Set set)
    {
        this.Id = set.Id;
        this.WinnerId = set.WinnerId;
        this.Legs = set.Legs.Select(leg => new Leg(leg)).ToList();
        this.Statistics = set.Statistics;
    }

    public App.Models.Set ToReal()
    {
        var set = new App.Models.Set();

        set.Id = this.Id;
        set.WinnerId = this.WinnerId;
        set.Legs = this.Legs.Select(leg => leg.ToReal()).ToList();
        set.Statistics = this.Statistics;

        return set;
    }
}