using App.Models;

namespace App;

public class MatchFactory
{
	public Match Result { get; private set; }

	public MatchFactory()
	{
		this.Reset();
	}

	public void Reset()
	{
		this.Result = new Match();

		this.Result.Id = Guid.NewGuid();
		this.Result.Date = DateTime.Now;
		this.Result.Players = new List<Guid>();
		this.Result.WinnerId = null;
		this.Result.SetsToWin = 5;
		this.Result.LegsToWin = 5;
		this.Result.ScoreToWin = 501;
		this.Result.ThrowsPerTurn = 3;
		this.Result.Sets = new List<Set>();
	}

	public MatchFactory AddPlayer(Player player)
	{
		this.Result.Players.Add(player.Id);
		return this;
	}

	public MatchFactory AddPlayer(Guid playerId)
	{
		this.Result.Players.Add(playerId);
		return this;
	}

	public MatchFactory SetSetsToWin(uint setsTowin)
	{
		this.Result.SetsToWin = setsTowin;
		return this;
	}

	public MatchFactory SetLegsToWin(uint legsTowin)
	{
		this.Result.LegsToWin = legsTowin;
		return this;
	}

	public MatchFactory SetScoreToWin(uint scoreToWin)
	{
		this.Result.ScoreToWin = scoreToWin;
		return this;
	}


	public Match CreateDefault()
	{
		var match = new Match();

		match.Id = Guid.NewGuid();
		match.Date = DateTime.Now;
		match.Players = new List<Guid>();
		match.WinnerId = null;
		match.SetsToWin = 5;
		match.LegsToWin = 5;
		match.ScoreToWin = 501;
		match.ThrowsPerTurn = 3;
		match.Sets = new List<Set>();

		return match;
	}
}