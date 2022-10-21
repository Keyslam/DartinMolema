using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerMatchStatisticDTO
{
	[JsonProperty("a")]
	public int OneEighties { get; set; }

	[JsonProperty("b")]
	public int Ninedarters { get; set; }

	[JsonProperty("c")]
	public double AverageScore { get; set; }

	[JsonProperty("d")]
	public int PlayedTurns { get; set; }

	[JsonProperty("e")]
	public int SetsPlayed { get; set; }

	[JsonProperty("f")]
	public int SetsWon { get; set; }

#pragma warning disable 8618
	public PlayerMatchStatisticDTO() { }
#pragma warning restore 8618

	public PlayerMatchStatisticDTO(PlayerMatchStatistic playerMatchStatistic)
	{
		this.OneEighties = playerMatchStatistic.OneEighties;
		this.Ninedarters = playerMatchStatistic.Ninedarters;
		this.AverageScore = playerMatchStatistic.AverageScore;
		this.PlayedTurns = playerMatchStatistic.PlayedTurns;

		this.SetsPlayed = playerMatchStatistic.SetsPlayed;
		this.SetsWon = playerMatchStatistic.SetsWon;
	}

	public PlayerMatchStatistic ToReal()
	{
		var playerMatchStatistic = new PlayerMatchStatistic(
			this.OneEighties,
			this.Ninedarters,
			this.AverageScore,
			this.PlayedTurns,
			this.SetsPlayed,
			this.SetsWon
		);

		return playerMatchStatistic;
	}
}