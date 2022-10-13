#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerMatchStatistic
{
	[JsonProperty("one_eighties")]
	public int OneEighties { get; set; }

	[JsonProperty("nine_darters")]
	public int Ninedarters { get; set; }

	[JsonProperty("sets_played")]
	public int SetsPlayed { get; set; }

	[JsonProperty("sets_won")]
	public int SetsWon { get; set; }

	public PlayerMatchStatistic() { }

	public PlayerMatchStatistic(App.Models.PlayerMatchStatistic playerMatchStatistic)
	{
		this.OneEighties = playerMatchStatistic.OneEighties;
		this.Ninedarters = playerMatchStatistic.Ninedarters;
		this.SetsPlayed = playerMatchStatistic.SetsPlayed;
		this.SetsWon = playerMatchStatistic.SetsWon;
	}

	public App.Models.PlayerMatchStatistic ToReal()
	{
		var playerMatchStatistic = new App.Models.PlayerMatchStatistic();

		playerMatchStatistic.OneEighties = this.OneEighties;
		playerMatchStatistic.Ninedarters = this.Ninedarters;
		playerMatchStatistic.SetsPlayed = this.SetsPlayed;
		playerMatchStatistic.SetsWon = this.SetsWon;

		return playerMatchStatistic;
	}
}