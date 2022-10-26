using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class PlayerDTO
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public string FullName { get; set; }

	[JsonProperty("c")]
	public string Country { get; set; }

	[JsonProperty("d")]
	public List<Guid> PlayedMatches { get; set; }

	[JsonProperty("e")]
	public List<Guid> WonMatches { get; set; }

	[JsonProperty("f")]
	public List<Guid> LostMatches { get; set; }

	[JsonProperty("g")]
	public List<Guid> PlayingMatches { get; set; }

	[JsonProperty("h")]
	public PlayerStatisticDTO Statistic { get; set; }

#pragma warning disable 8618
	public PlayerDTO() { }
#pragma warning restore 8618

	public PlayerDTO(Player player)
	{
		this.Id = player.Id;
		this.FullName = player.FullName;
		this.Country = player.Country;
		this.PlayedMatches = player.PlayedMatches
			.Select(match => match)
			.ToList();
		this.WonMatches = player.WonMatches
			.Select(match => match)
			.ToList();
		this.LostMatches = player.LostMatches
			.Select(match => match)
			.ToList();
		this.PlayingMatches = player.PlayingMatches
			.Select(match => match)
			.ToList();
		this.Statistic = new PlayerStatisticDTO(player.Statistic);
	}

	public Player ToReal()
	{
		var playedMatches = this.PlayedMatches
			.Select(matchId => matchId);

		var wonMatches = this.WonMatches
			.Select(matchId => matchId);

		var lostMatches = this.LostMatches
			.Select(matchId => matchId);

		var playingMatches = this.PlayingMatches
			.Select(matchId => matchId);

		var statistic = this.Statistic.ToReal();

		var player = new Player(
			this.Id,
			this.FullName,
			this.Country,
			playedMatches,
			wonMatches,
			lostMatches,
			playingMatches,
			statistic
		);

		return player;
	}
}