using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class PlayerLegStatisticDTO
{
	[JsonProperty("a")]
	public int OneEighties { get; set; }

	[JsonProperty("b")]
	public bool IsNineDarter { get; set; }

	[JsonProperty("c")]
	public int AverageTurnScore { get; set; }

	[JsonProperty("d")]
	public int PlayedTurns { get; set; }

#pragma warning disable 8618
	public PlayerLegStatisticDTO() { }
#pragma warning restore 8618

	public PlayerLegStatisticDTO(PlayerLegStatistic playerLegStatistic)
	{
		this.OneEighties = playerLegStatistic.OneEighties;
		this.IsNineDarter = playerLegStatistic.IsNineDarter;
		this.AverageTurnScore = playerLegStatistic.AverageTurnScore;
		this.PlayedTurns = playerLegStatistic.PlayedTurns;
	}

	public PlayerLegStatistic ToReal()
	{
		var playerLegStatistic = new PlayerLegStatistic(
			this.OneEighties,
			this.IsNineDarter,
			this.AverageTurnScore,
			this.PlayedTurns
		);

		return playerLegStatistic;
	}
}