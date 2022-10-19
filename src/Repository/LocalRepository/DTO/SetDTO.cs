using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class SetDTO
{
	[JsonProperty("a")]
	public int WinnerIndex { get; set; }

	[JsonProperty("b")]
	public int PlayerCount { get; set; }

	[JsonProperty("c")]
	public List<LegDTO> Legs { get; set; }

	[JsonProperty("d")]
	public List<PlayerSetStatisticDTO> Statistics { get; set; }

#pragma warning disable 8618
	public SetDTO() { }
#pragma warning restore 8618

	public SetDTO(Set set)
	{
		this.WinnerIndex = set.WinnerIndex;
		this.PlayerCount = set.PlayerCount;
		this.Legs = set.Legs.Select(leg => new LegDTO(leg)).ToList();

		this.Statistics = set.Statistics
			.Select(statistic => new PlayerSetStatisticDTO(statistic))
			.ToList();
	}

	public Set ToReal()
	{
		var legs = this.Legs.Select(leg => leg.ToReal()).ToList();

		var statistics = this.Statistics
			.Select(statistic => statistic.ToReal())
			.ToList();

		var set = new Set(
			this.WinnerIndex,
			this.PlayerCount,
			legs,
			statistics
		);

		return set;
	}
}