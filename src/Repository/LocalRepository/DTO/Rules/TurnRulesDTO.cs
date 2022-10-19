using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class TurnRulesDTO
{
	[JsonProperty("a")]
	public int ThrowsPerTurn { get; set; }

#pragma warning disable 8618
	public TurnRulesDTO() { }
#pragma warning restore 8618

	public TurnRulesDTO(TurnRules turnRules)
	{
		this.ThrowsPerTurn = turnRules.ThrowsPerTurn;
	}

	public TurnRules ToReal()
	{
		var turnRules = new TurnRules(
			this.ThrowsPerTurn
		);

		return turnRules;
	}
}