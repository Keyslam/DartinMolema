using App.Models;

namespace App;

public class MatchBuilder
{
	public Match Result { get; private set; }

	public MatchBuilder()
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

	public MatchBuilder AddPlayer(Player player)
	{
		this.Result.Players.Add(player.Id);
		return this;
	}

	public MatchBuilder AddPlayer(Guid playerId)
	{
		this.Result.Players.Add(playerId);
		return this;
	}

	public MatchBuilder SetSetsToWin(uint setsTowin)
	{
		this.Result.SetsToWin = setsTowin;
		return this;
	}

	public MatchBuilder SetLegsToWin(uint legsTowin)
	{
		this.Result.LegsToWin = legsTowin;
		return this;
	}

	public MatchBuilder SetScoreToWin(uint scoreToWin)
	{
		this.Result.ScoreToWin = scoreToWin;
		return this;
	}

	public MatchBuilder SetThrowsPerTurn(uint throwsPerTurn)
	{
		this.Result.ThrowsPerTurn = throwsPerTurn;
		return this;
	}
}