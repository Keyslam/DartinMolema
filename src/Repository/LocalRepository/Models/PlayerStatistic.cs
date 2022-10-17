#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerStatistic
{
	[JsonProperty("a")]
	public int AverageTurnScore { get; set; }

	[JsonProperty("b")]
	public int PlayedTurns { get; set; }

	[JsonProperty("c")]
	public int OneEighties { get; set; }

	[JsonProperty("d")]
	public int Ninedarters { get; set; }

	public PlayerStatistic() { }

	public PlayerStatistic(App.Models.PlayerStatistic playerStatistic)
	{
		this.AverageTurnScore = playerStatistic.AverageTurnScore;
		this.PlayedTurns = playerStatistic.PlayedTurns;
		this.OneEighties = playerStatistic.OneEighties;
		this.Ninedarters = playerStatistic.Ninedarters;
	}

	public App.Models.PlayerStatistic ToReal()
	{
		var playerStatistic = new App.Models.PlayerStatistic();

		playerStatistic.AverageTurnScore = this.AverageTurnScore;
		playerStatistic.PlayedTurns = this.PlayedTurns;
		playerStatistic.OneEighties = this.OneEighties;
		playerStatistic.Ninedarters = this.Ninedarters;

		return playerStatistic;
	}
}