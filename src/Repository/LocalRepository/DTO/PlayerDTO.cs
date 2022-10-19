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
	public List<Guid> PlayedMatches { get; set; }

	[JsonProperty("d")]
	public List<Guid> WonMatches { get; set; }

	[JsonProperty("e")]
	public List<Guid> LostMatches { get; set; }

	[JsonProperty("f")]
	public PlayerStatisticDTO Statistic { get; set; }

#pragma warning disable 8618
	public PlayerDTO() { }
#pragma warning restore 8618

	public PlayerDTO(Player player)
	{
		this.Id = player.Id;
		this.FullName = player.FullName;
		this.PlayedMatches = player.PlayedMatches
			.Select(match => match.Id)
			.ToList();
		this.WonMatches = player.WonMatches
			.Select(match => match.Id)
			.ToList();
		this.LostMatches = player.LostMatches
			.Select(match => match.Id)
			.ToList();
		this.Statistic = new PlayerStatisticDTO(player.Statistic);
	}

	public Player ToReal()
	{
		var playedMatches = this.PlayedMatches
			.Select(matchId => new Reference<Match>(matchId));

		var wonMatches = this.WonMatches
			.Select(matchId => new Reference<Match>(matchId));

		var lostMatches = this.LostMatches
			.Select(matchId => new Reference<Match>(matchId));

		var statistic = this.Statistic.ToReal();

		var player = new Player(
			this.Id,
			this.FullName,
			playedMatches,
			wonMatches,
			lostMatches,
			statistic
		);

		return player;
	}
}