using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class SetRulesDTO
{
	[JsonProperty("a")]
	public LegRulesDTO LegRules { get; set; }

	[JsonProperty("b")]
	public int LegsToWin { get; set; }

#pragma warning disable 8618
	public SetRulesDTO() { }
#pragma warning restore 8618

	public SetRulesDTO(SetRules setRules)
	{
		this.LegRules = new LegRulesDTO(setRules.LegRules);
		this.LegsToWin = setRules.LegsToWin;
	}

	public SetRules ToReal()
	{
		var match = new SetRules(
			this.LegRules.ToReal(),
			this.LegsToWin
		);

		return match;
	}
}