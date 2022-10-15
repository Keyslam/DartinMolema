#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Set
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public Guid WinnerId { get; set; }

	[JsonProperty("c")]
	public List<Leg> Legs { get; set; }

	[JsonProperty("d")]
	public Dictionary<Guid, PlayerSetStatistic> Statistics { get; set; }

	public Set() { }

	public Set(App.Models.Set set)
	{
		this.Id = set.Id;
		this.WinnerId = set.WinnerId;
		this.Legs = set.Legs.Select(leg => new Leg(leg)).ToList();
		this.Statistics = new Dictionary<Guid, PlayerSetStatistic>();
		foreach (var (guid, statistics) in set.Statistics)
		{
			var statistic = new PlayerSetStatistic(statistics);
			this.Statistics.Add(guid, statistic);
		}
	}

	public App.Models.Set ToReal()
	{
		var set = new App.Models.Set();

		set.Id = this.Id;
		set.WinnerId = this.WinnerId;
		set.Legs = this.Legs.Select(leg => leg.ToReal()).ToList();
		set.Statistics = new Dictionary<Guid, App.Models.PlayerSetStatistic>();
		foreach (var (guid, statistics) in this.Statistics)
		{
			var statistic = statistics.ToReal();
			set.Statistics.Add(guid, statistic);
		}

		return set;
	}
}