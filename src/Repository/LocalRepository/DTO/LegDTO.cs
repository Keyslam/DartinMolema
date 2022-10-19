using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class LegDTO
{
	[JsonProperty("a")]
	public int WinnerIndex { get; set; }

	[JsonProperty("b")]
	public int PlayerCount { get; set; }

	[JsonProperty("c")]
	public int CurrentPlayerIndex { get; set; }

	[JsonProperty("d")]
	public List<int> Points { get; set; }

	[JsonProperty("e")]
	public List<List<TurnDTO>> Turns { get; set; }

	[JsonProperty("f")]
	public List<PlayerLegStatisticDTO> Statistics { get; set; }

#pragma warning disable 8618
	public LegDTO() { }
#pragma warning restore 8618

	public LegDTO(ILeg leg)
	{
		this.WinnerIndex = leg.WinnerIndex;
		this.PlayerCount = leg.PlayerCount;
		this.CurrentPlayerIndex = leg.CurrentPlayerIndex;

		this.Points = leg.Points
			.Select(x => x)
			.ToList();

		this.Turns = leg.Turns
			.Select(playerTurns => playerTurns
				.Select(turn => new TurnDTO(turn))
				.ToList())
			.ToList();

		this.Statistics = leg.Statistics
			.Select(statistics => new PlayerLegStatisticDTO(statistics))
			.ToList();
	}

	public Leg ToReal()
	{
		var points = this.Points
			.Select(x => x)
			.ToList();

		var turns = this.Turns
			.Select(playerTurns => playerTurns
				.Select(turn => turn.ToReal())
				.ToList())
			.ToList();

		var statistics = this.Statistics
			.Select(statistics => statistics.ToReal());

		var leg = new Leg(
			this.WinnerIndex,
			this.PlayerCount,
			this.CurrentPlayerIndex,
			points,
			turns,
			statistics
		);

		return leg;
	}
}