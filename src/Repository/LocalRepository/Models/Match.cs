#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Match
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public string Name { get; set; }

	[JsonProperty("c")]
	public DateTime Date { get; set; }

	[JsonProperty("d")]
	public List<Guid> Players { get; set; }

	[JsonProperty("e")]
	public Guid WinnerId { get; set; }

	[JsonProperty("f")]
	public int SetsToWin { get; set; }

	[JsonProperty("g")]
	public int LegsToWin { get; set; }

	[JsonProperty("h")]
	public int ScoreToWin { get; set; }

	[JsonProperty("i")]
	public int ThrowsPerTurn { get; set; }

	[JsonProperty("j")]
	public List<Set> Sets { get; set; }

	[JsonProperty("k")]
	public Dictionary<Guid, PlayerMatchStatistic> Statistics { get; set; }

	public Match() { }

	public Match(App.Models.Match match)
	{
		this.Id = match.Id;
		this.Name = match.Name;
		this.Date = match.Date;
		this.Players = match.Players;
		this.WinnerId = match.WinnerId;
		this.SetsToWin = match.SetsToWin;
		this.LegsToWin = match.LegsToWin;
		this.ScoreToWin = match.ScoreToWin;
		this.ThrowsPerTurn = match.ThrowsPerTurn;
		this.Sets = match.Sets.Select(set => new Set(set)).ToList();
		this.Statistics = new Dictionary<Guid, PlayerMatchStatistic>();
		foreach (var (guid, statistics) in match.Statistics)
		{
			var statistic = new PlayerMatchStatistic(statistics);
			this.Statistics.Add(guid, statistic);
		}
	}


	public App.Models.Match ToReal()
	{
		var match = new App.Models.Match();

		match.Id = this.Id;
		match.Name = this.Name;
		match.Date = this.Date;
		match.Players = this.Players;
		match.WinnerId = this.WinnerId;
		match.SetsToWin = this.SetsToWin;
		match.LegsToWin = this.LegsToWin;
		match.ScoreToWin = this.ScoreToWin;
		match.ThrowsPerTurn = this.ThrowsPerTurn;
		match.Statistics = new Dictionary<Guid, App.Models.PlayerMatchStatistic>();
		match.Sets = this.Sets.Select(set => set.ToReal()).ToList();
		foreach (var (guid, statistics) in this.Statistics)
		{
			var statistic = statistics.ToReal();
			match.Statistics.Add(guid, statistic);
		}

		return match;
	}
}