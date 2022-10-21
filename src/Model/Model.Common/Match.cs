namespace App.Models;

public class Match
{
	public Guid Id { get; }
	public string Name { get; }

	public DateTime Date { get; set; }

	public List<Guid> Players { get; set; }
	public int WinnerIndex { get; set; }

	public MatchRules MatchRules { get; }

	public List<Set> Sets { get; set; }
	public List<PlayerMatchStatistic> Statistics { get; set; }

	public bool IsDone => this.WinnerIndex != -1;
	public Set CurrentSet => this.Sets.Last();

	public Match(string name, DateTime date, IEnumerable<Guid> players, MatchRules matchRules)
	{
		this.Id = Guid.NewGuid();
		this.Name = name;
		this.Date = date;
		this.Players = new List<Guid>(players);
		this.WinnerIndex = -1;
		this.MatchRules = matchRules;
		this.Sets = new List<Set>();
		this.Statistics = Enumerable
			.Repeat(() => new PlayerMatchStatistic(), this.Players.Count)
			.Select(x => x())
			.ToList();

		this.Sets.Add(new Set(this.Players.Count, 0));
	}

	public Match(Guid id, string name, DateTime date, IEnumerable<Guid> players, int winnerIndex, MatchRules matchRules, IEnumerable<Set> sets, List<PlayerMatchStatistic> statistics)
	{
		this.Id = id;
		this.Name = name;
		this.Date = date;
		this.Players = new List<Guid>(players);
		this.WinnerIndex = winnerIndex;
		this.MatchRules = matchRules;
		this.Sets = new List<Set>(sets);
		this.Statistics = new List<PlayerMatchStatistic>(statistics);
	}

	public int GetRemainingPointsAfterTurn(Turn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Match is already done");

		return this.CurrentSet.GetRemainingPointsAfterTurn(this.MatchRules.SetRules, turn);
	}

	public void PlayTurn(Turn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Match is already done");

		var currentPlayerIndex = this.CurrentSet.CurrentPlayerIndex;

		this.CurrentSet.PlayTurn(this.MatchRules.SetRules, turn);
		var points = turn.AssignedPoints;

		this.Statistics[currentPlayerIndex].PlayTurn(points);

		if (this.CurrentSet.IsDone)
		{
			foreach (var statistic in this.Statistics)
			{
				var index = this.Statistics.IndexOf(statistic);

				var nineDarters = this.CurrentSet.Legs
					.Select(leg => leg.Statistics[index])
					.Where(statistic => statistic.IsNineDarter)
					.Count();

				statistic.PlaySet(index == CurrentSet.WinnerIndex, nineDarters);
			}

			var isMatchWon = this.IsMatchWon(this.MatchRules.SetsToWin);

			if (isMatchWon)
			{
				this.WinnerIndex = currentPlayerIndex;
			}
			else
			{
				var startingPlayerIndex = this.CurrentSet.StartingPlayerIndex;
				this.Sets.Add(new Set(this.Players.Count, startingPlayerIndex));
			}
		}
	}

	private bool IsMatchWon(int setToWin)
	{
		var wonLegs = this.Sets
				.Where(leg => leg.WinnerIndex == this.CurrentSet.WinnerIndex)
				.Count();

		return (wonLegs == setToWin);
	}
}

