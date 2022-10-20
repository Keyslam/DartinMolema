using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class LegRulesDTO
{
	[JsonProperty("a")]
	public TurnRulesDTO TurnRules { get; set; }

	[JsonProperty("b")]
	public int TargetScore { get; set; }

#pragma warning disable 8618
	public LegRulesDTO() { }
#pragma warning restore 8618

	public LegRulesDTO(LegRules legRules)
	{
		this.TurnRules = new TurnRulesDTO(legRules.TurnRules);
		this.TargetScore = legRules.TargetScore;
	}

	public LegRules ToReal()
	{
		var leg = new LegRules(
			this.TurnRules.ToReal(),
			this.TargetScore
		);

		return leg;
	}
}