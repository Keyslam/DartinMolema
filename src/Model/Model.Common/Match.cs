namespace App.Models;

public class Match
{
	public Guid Id { get; }
	public string Name { get; }

	public DateTime Date { get; set; }

	public List<Reference<Player>> Players { get; set; }
	public int WinnerIndex { get; set; }

	public MatchRules MatchRules { get; }

	public List<Set> Sets { get; set; }
	public List<PlayerMatchStatistic> Statistics { get; set; }

	public bool IsDone => this.WinnerIndex != -1;
	public Set CurrentSet => this.Sets.Last();

	public Match(string name, DateTime date, IEnumerable<Reference<Player>> players, MatchRules matchRules)
	{
		this.Id = Guid.NewGuid();
		this.Name = name;
		this.Date = date;
		this.Players = new List<Reference<Player>>(players);
		this.WinnerIndex = -1;
		this.MatchRules = matchRules;
		this.Sets = new List<Set>();
		this.Statistics = new List<PlayerMatchStatistic>();

		this.Sets.Add(new Set(this.Players.Count));
	}

	public Match(Guid id, string name, DateTime date, IEnumerable<Reference<Player>> players, int winnerIndex, MatchRules matchRules, IEnumerable<Set> sets, List<PlayerMatchStatistic> statistics)
	{
		this.Id = id;
		this.Name = name;
		this.Date = date;
		this.Players = new List<Reference<Player>>(players);
		this.WinnerIndex = winnerIndex;
		this.MatchRules = matchRules;
		this.Sets = new List<Set>(sets);
		this.Statistics = new List<PlayerMatchStatistic>(statistics);
	}

	public void PlayTurn(Turn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Match is already done");

		var currentPlayerIndex = this.CurrentSet.CurrentPlayerIndex;

		this.CurrentSet.PlayTurn(this.MatchRules.SetRules, turn);
		var points = turn.AssignedPoints;

		if (this.CurrentSet.IsDone)
		{
			foreach (var statistic in this.Statistics)
				statistic.PlaySet(currentPlayerIndex == CurrentSet.WinnerIndex);

			var isMatchWon = this.IsMatchWon(this.MatchRules.SetsToWin);

			if (isMatchWon)
			{
				this.WinnerIndex = currentPlayerIndex;
			}
			else
				this.Sets.Add(new Set(this.Players.Count));
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

// for (var i = 0; i< this.Statistics.Count; i++)
// 				{
// 					var statistic = this.Statistics[i];

// 					foreach (var set in this.Sets)
// 					{
// 	foreach (var leg in set.Legs)
// 	{
// 		foreach (var turnn in leg.Turns[i])
// 		{
// 			statistic.AverageScore = ((statistic.AverageTurnScore * statistic.PlayedTurns) + points) / (statistic.PlayedTurns + 1);
// 		}
// 	}
// }
// }