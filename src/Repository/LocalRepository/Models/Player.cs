#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Player
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public string FullName { get; set; }

	[JsonProperty("c")]
	public string Country { get; set; }

	[JsonProperty("d")]
	public List<Guid> PlayedGames { get; set; }

	[JsonProperty("e")]
	public List<Guid> WonGames { get; set; }

	[JsonProperty("f")]
	public List<Guid> LostGames { get; set; }

	[JsonProperty("g")]
	public PlayerStatistic Statistic { get; set; }

	public Player() { }

	public Player(App.Models.Player player)
	{
		this.Id = player.Id;
		this.FullName = player.FullName;
		this.Country = player.Country;
		this.PlayedGames = player.PlayedGames;
		this.WonGames = player.WonGames;
		this.LostGames = player.LostGames;
		this.Statistic = new PlayerStatistic(player.Statistic);
	}

	public App.Models.Player ToReal()
	{
		var player = new App.Models.Player();

		player.Id = this.Id;
		player.FullName = this.FullName;
		player.Country = this.Country;
		player.PlayedGames = this.PlayedGames;
		player.WonGames = this.WonGames;
		player.LostGames = this.LostGames;
		player.Statistic = this.Statistic.ToReal();

		return player;
	}
}