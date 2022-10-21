using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerStatisticDTO
{
	[JsonProperty("a")]
	public double AverageTurnScore { get; set; }

	[JsonProperty("b")]
	public int PlayedTurns { get; set; }

	[JsonProperty("c")]
	public int OneEighties { get; set; }

	[JsonProperty("d")]
	public int Ninedarters { get; set; }

#pragma warning disable 8618
	public PlayerStatisticDTO() { }
#pragma warning restore 8618

	public PlayerStatisticDTO(PlayerStatistic playerStatistic)
	{
		this.AverageTurnScore = playerStatistic.AverageTurnScore;
		this.PlayedTurns = playerStatistic.PlayedTurns;
		this.OneEighties = playerStatistic.OneEighties;
		this.Ninedarters = playerStatistic.Ninedarters;
	}

	public PlayerStatistic ToReal()
	{
		var playerStatistic = new PlayerStatistic(
			this.AverageTurnScore,
			this.PlayedTurns,
			this.OneEighties,
			this.Ninedarters
		);

		return playerStatistic;
	}
}