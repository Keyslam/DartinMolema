namespace App.Models;

public interface ISet
{
	int WinnerIndex { get; }
	int PlayerCount { get; }
	int CurrentPlayerIndex { get; }

	List<ILeg> Legs { get; set; }
	List<PlayerSetStatistic> Statistics { get; set; }

	ILeg CurrentLeg { get; }
	bool IsDone { get; }

	void PlayTurn(SetRules setRules, ITurn turn);
}

public class Set : ISet
{
	public int WinnerIndex { get; set; }
	public int PlayerCount { get; }
	public List<ILeg> Legs { get; set; }

	public List<PlayerSetStatistic> Statistics { get; set; }

	public ILeg CurrentLeg => Legs.Last();
	public bool IsDone => this.WinnerIndex != -1;
	public int CurrentPlayerIndex => this.CurrentLeg.CurrentPlayerIndex;

	public Set(int playerCount)
	{
		this.WinnerIndex = -1;
		this.PlayerCount = playerCount;
		this.Legs = new List<ILeg>();
		this.Statistics = Enumerable.Repeat(new PlayerSetStatistic(), this.PlayerCount).ToList();

		this.Legs.Add(this.CreateLeg(this.PlayerCount));
	}

	public Set(int winnerIndex, int playerCount, IEnumerable<ILeg> legs, IEnumerable<PlayerSetStatistic> statistics)
	{
		this.WinnerIndex = winnerIndex;
		this.PlayerCount = playerCount;
		this.Legs = new List<ILeg>(legs);
		this.Statistics = new List<PlayerSetStatistic>(statistics);
	}

	public void PlayTurn(SetRules setRules, ITurn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Set is already done");

		var currentPlayerIndex = this.CurrentPlayerIndex;

		this.CurrentLeg.PlayTurn(setRules.LegRules, turn);
		var points = turn.AssignedPoints;

		this.Statistics[currentPlayerIndex].PlayTurn(points);

		if (this.CurrentLeg.IsDone)
		{
			foreach (var statistic in this.Statistics)
				statistic.PlayLeg(currentPlayerIndex == CurrentLeg.WinnerIndex);

			var isSetWon = this.IsSetWon(setRules.LegsToWin);

			if (isSetWon)
				this.WinnerIndex = currentPlayerIndex;
			else
			{
				this.Legs.Add(this.CreateLeg(this.PlayerCount));
			}
		}
	}

	private bool IsSetWon(int legsToWin)
	{
		var wonLegs = this.Legs
				.Where(leg => leg.WinnerIndex == this.CurrentLeg.WinnerIndex)
				.Count();

		return (wonLegs == legsToWin);
	}

	public virtual Leg CreateLeg(int playerCount)
	{
		return new Leg(playerCount);
	}
}