using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class ThrowRulesDTO
{
	[JsonProperty("a")]
	public SetRulesDTO SetRules { get; set; }

	[JsonProperty("b")]
	public int SetsToWin { get; set; }

#pragma warning disable 8618
	public ThrowRulesDTO() { }
#pragma warning restore 8618

	public ThrowRulesDTO(MatchRules matchRules)
	{
		this.SetRules = new SetRulesDTO(matchRules.SetRules);
		this.SetsToWin = matchRules.SetsToWin;
	}

	public MatchRules ToReal()
	{
		var match = new MatchRules(
			this.SetRules.ToReal(),
			this.SetsToWin
		);

		return match;
	}
}