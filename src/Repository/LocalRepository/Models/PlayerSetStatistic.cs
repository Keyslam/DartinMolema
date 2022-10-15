#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerSetStatistic
{
	[JsonProperty("a")]
	public int OneEighties { get; set; }

	[JsonProperty("b")]
	public int Ninedarters { get; set; }

	[JsonProperty("c")]
	public int AverageScore { get; set; }

	[JsonProperty("d")]
	public int LegsPlayed { get; set; }

	[JsonProperty("e")]
	public int LegsWon { get; set; }

	public PlayerSetStatistic() { }

	public PlayerSetStatistic(App.Models.PlayerSetStatistic playerSetStatistic)
	{
		this.OneEighties = playerSetStatistic.OneEighties;
		this.Ninedarters = playerSetStatistic.Ninedarters;
		this.AverageScore = playerSetStatistic.AverageScore;
		this.LegsPlayed = playerSetStatistic.LegsPlayed;
		this.LegsWon = playerSetStatistic.LegsWon;
	}

	public App.Models.PlayerSetStatistic ToReal()
	{
		var playerSetStatistic = new App.Models.PlayerSetStatistic();

		playerSetStatistic.OneEighties = this.OneEighties;
		playerSetStatistic.Ninedarters = this.Ninedarters;
		playerSetStatistic.AverageScore = this.AverageScore;
		playerSetStatistic.LegsPlayed = this.LegsPlayed;
		playerSetStatistic.LegsWon = this.LegsWon;

		return playerSetStatistic;
	}
}