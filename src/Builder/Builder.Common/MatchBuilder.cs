using System.Text;
using App.Models;

namespace App.Builders;

public class MatchBuilder
{
	public DateTime Date { get; set; }
	public Guid WinnerId { get; set; }
	public int SetsToWin { get; set; }
	public int LegsToWin { get; set; }
	public int ScoreToWin { get; set; }
	public int ThrowsPerTurn { get; set; }

	private List<Player> Players { get; set; }

#pragma warning disable 8618
	public MatchBuilder()
	{
		this.Reset();
	}
#pragma warning restore 8618

	public void Reset()
	{
		this.Date = DateTime.Now;
		this.Players = new List<Player>();
		this.WinnerId = Guid.Empty;
		this.SetsToWin = 5;
		this.LegsToWin = 5;
		this.ScoreToWin = 501;
		this.ThrowsPerTurn = 3;
	}

	public MatchBuilder SetDate(DateTime date)
	{
		this.Date = date;
		return this;
	}

	public MatchBuilder SetScoreToWin(int scoreToWin)
	{
		this.ScoreToWin = scoreToWin;
		return this;
	}

	public MatchBuilder AddPlayer(Player player)
	{
		this.Players.Add(player);
		return this;
	}

	public MatchBuilder RemovePlayer(Player player)
	{
		this.Players.Remove(player);
		return this;
	}

	public Match Build()
	{
		var match = new Match(
			this.BuildName(this.Date, this.Players),
			this.Date,
			this.Players.Select(player => player.Id),
			new MatchRules(
				new SetRules(
					new LegRules(
						new TurnRules(
							this.ThrowsPerTurn
						),
						this.ScoreToWin
					),
					this.LegsToWin
				),
				this.SetsToWin
			)
		);

		this.Reset();

		return match;
	}

	private string BuildName(DateTime date, IEnumerable<Player> players)
	{
		var titleBuilder = new StringBuilder();

		foreach (var player in players)
		{
			if (players.First() != player)
				titleBuilder.Append(" vs ");

			titleBuilder.Append(player.FullName);
		}

		titleBuilder.Append(" | ");
		titleBuilder.Append(date.ToString("dd-MM-yyyy HH:mm"));

		return titleBuilder.ToString();
	}
}