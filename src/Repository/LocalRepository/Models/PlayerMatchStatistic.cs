#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerMatchStatistic
{
	[JsonProperty("a")]
	public int OneEighties { get; set; }

	[JsonProperty("b")]
	public int Ninedarters { get; set; }

	[JsonProperty("c")]
	public int AverageScore { get; set; }

	[JsonProperty("d")]
	public int SetsPlayed { get; set; }

	[JsonProperty("e")]
	public int SetsWon { get; set; }

	public PlayerMatchStatistic() { }

	public PlayerMatchStatistic(App.Models.PlayerMatchStatistic playerMatchStatistic)
	{
		this.OneEighties = playerMatchStatistic.OneEighties;
		this.Ninedarters = playerMatchStatistic.Ninedarters;
		this.AverageScore = playerMatchStatistic.AverageScore;
		this.SetsPlayed = playerMatchStatistic.SetsPlayed;
		this.SetsWon = playerMatchStatistic.SetsWon;
	}

	public App.Models.PlayerMatchStatistic ToReal()
	{
		var playerMatchStatistic = new App.Models.PlayerMatchStatistic();

		playerMatchStatistic.OneEighties = this.OneEighties;
		playerMatchStatistic.Ninedarters = this.Ninedarters;
		playerMatchStatistic.AverageScore = this.AverageScore;
		playerMatchStatistic.SetsPlayed = this.SetsPlayed;
		playerMatchStatistic.SetsWon = this.SetsWon;

		return playerMatchStatistic;
	}
}