using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerSetStatisticDTO
{
	[JsonProperty("a")]
	public int OneEighties { get; set; }

	[JsonProperty("b")]
	public int Ninedarters { get; set; }

	[JsonProperty("c")]
	public int AverageScore { get; set; }

	[JsonProperty("d")]
	public int PlayedTurns { get; set; }

	[JsonProperty("e")]
	public int LegsPlayed { get; set; }

	[JsonProperty("f")]
	public int LegsWon { get; set; }

#pragma warning disable 8618
	public PlayerSetStatisticDTO() { }
#pragma warning restore 8618

	public PlayerSetStatisticDTO(PlayerSetStatistic playerSetStatistic)
	{
		this.OneEighties = playerSetStatistic.OneEighties;
		this.Ninedarters = playerSetStatistic.Ninedarters;
		this.AverageScore = playerSetStatistic.AverageScore;
		this.PlayedTurns = playerSetStatistic.PlayedTurns;

		this.LegsPlayed = playerSetStatistic.LegsPlayed;
		this.LegsWon = playerSetStatistic.LegsWon;
	}

	public PlayerSetStatistic ToReal()
	{
		var playerSetStatistic = new PlayerSetStatistic(
			this.OneEighties,
			this.Ninedarters,
			this.AverageScore,
			this.PlayedTurns,
			this.LegsPlayed,
			this.LegsWon
		);

		return playerSetStatistic;
	}
}