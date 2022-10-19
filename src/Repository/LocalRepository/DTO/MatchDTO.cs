using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class MatchDTO
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public string Name { get; set; }

	[JsonProperty("c")]
	public DateTime Date { get; set; }

	[JsonProperty("d")]
	public List<Guid> Players { get; set; }

	[JsonProperty("e")]
	public int WinnerIndex { get; set; }

	[JsonProperty("f")]
	public MatchRulesDTO MatchRules { get; set; }

	[JsonProperty("g")]
	public List<SetDTO> Sets { get; set; }

	[JsonProperty("h")]
	public List<PlayerMatchStatisticDTO> Statistics { get; set; }

#pragma warning disable 8618
	public MatchDTO() { }
#pragma warning restore 8618

	public MatchDTO(Match match)
	{
		this.Id = match.Id;
		this.Name = match.Name;
		this.Date = match.Date;
		this.Players = match.Players.Select(player => player.Id).ToList();
		this.WinnerIndex = match.WinnerIndex;
		this.MatchRules = new MatchRulesDTO(match.MatchRules);
		this.Sets = match.Sets.Select(set => new SetDTO(set)).ToList();

		this.Statistics = match.Statistics
			.Select(x => new PlayerMatchStatisticDTO(x))
			.ToList();
	}

	public Match ToReal()
	{
		var players = this.Players
			.Select(playerId => new Reference<Player>(playerId));

		var sets = this.Sets
			.Select(set => set.ToReal());

		var statistics = this.Statistics
			.Select(x => x.ToReal())
			.ToList();

		var match = new Match(
			this.Id,
			this.Name,
			this.Date,
			players,
			this.WinnerIndex,
			this.MatchRules.ToReal(),
			sets,
			statistics
		);

		return match;
	}
}