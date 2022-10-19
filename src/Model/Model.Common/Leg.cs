namespace App.Models;

public interface ILeg
{
	int WinnerIndex { get; }
	int PlayerCount { get; }
	int CurrentPlayerIndex { get; }

	List<int> Points { get; }
	IReadOnlyList<List<ITurn>> Turns { get; }
	IReadOnlyList<PlayerLegStatistic> Statistics { get; }

	ITurn CurrentTurn { get; }
	bool IsDone { get; }

	int GetRemainingPointsAfterTurn(LegRules legRules, ITurn turn);
	void PlayTurn(LegRules legRules, ITurn turn);
}

public class Leg : ILeg
{
	public int WinnerIndex { get; private set; }
	public int PlayerCount { get; }
	public int CurrentPlayerIndex { get; private set; }

	public List<int> Points { get; }
	public IReadOnlyList<List<ITurn>> Turns { get; }
	public IReadOnlyList<PlayerLegStatistic> Statistics { get; }

	public ITurn CurrentTurn => this.Turns[this.CurrentPlayerIndex].Last();
	public bool IsDone => this.WinnerIndex != -1;

	public Leg(int playerCount, int currentPlayerIndex)
	{
		this.WinnerIndex = -1;
		this.PlayerCount = playerCount;
		this.CurrentPlayerIndex = currentPlayerIndex;

		this.Points = Enumerable.Repeat(0, this.PlayerCount).ToList();

		this.Turns = Enumerable
			.Repeat(() => new List<ITurn>(), this.PlayerCount)
			.Select(x => x())
			.ToList();

		this.Statistics = Enumerable
			.Repeat(() => new PlayerLegStatistic(), this.PlayerCount)
			.Select(x => x())
			.ToList();
	}

	public Leg(int winnerIndex, int playerCount, int currentPlayerIndex, IEnumerable<int> points, IEnumerable<List<ITurn>> turns, IEnumerable<PlayerLegStatistic> statistics)
	{
		this.WinnerIndex = winnerIndex;
		this.PlayerCount = playerCount;
		this.CurrentPlayerIndex = currentPlayerIndex;
		this.Points = new List<int>(points);
		this.Turns = new List<List<ITurn>>(turns);
		this.Statistics = new List<PlayerLegStatistic>(statistics);
	}

	public int GetRemainingPointsAfterTurn(LegRules legRules, ITurn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Leg is already done");

		var isTurnValid = this.IsTurnValid(legRules, turn);

		if (!isTurnValid)
			return 0;

		var currentPoints = this.GetCurrentPlayerPoints();
		return legRules.TargetScore - (currentPoints + turn.ThrownPoints);
	}

	public void PlayTurn(LegRules legRules, ITurn turn)
	{
		if (this.IsDone)
			throw new InvalidOperationException("Leg is already done");

		this.Turns[this.CurrentPlayerIndex].Add(turn);

		turn.IsValid = this.IsTurnValid(legRules, turn);
		var thrownPoints = turn.AssignedPoints;

		var currentPoints = this.GetCurrentPlayerPoints();
		var newPoints = currentPoints + turn.AssignedPoints;

		this.Points[this.CurrentPlayerIndex] = newPoints;

		var isEndOfLeg = newPoints == legRules.TargetScore;

		this.UpdateStatistics(thrownPoints, isEndOfLeg);

		if (isEndOfLeg)
			this.EndLeg();
		else
			this.GoToNextPlayer();
	}

	private int GetCurrentPlayerPoints()
	{
		return this.Points[this.CurrentPlayerIndex];
	}

	private bool IsTurnValid(LegRules legRules, ITurn turn)
	{
		var thrownPoints = turn.ThrownPoints;
		var playerPoints = this.GetCurrentPlayerPoints();
		var newPoints = playerPoints + turn.ThrownPoints;

		// Turn is invalid if new points exceeds target points
		if (newPoints > legRules.TargetScore)
			return false;

		// Turn is invalid if it leaves 1 remaining point
		if (newPoints == legRules.TargetScore - 1)
			return false;

		// Turn is invalid if it ends the leg without being a valid last throw kind
		if (newPoints == legRules.TargetScore && !turn.IsValidLastTurn())
			return false;

		return true;
	}

	private void UpdateStatistics(int points, bool isEndOfLeg)
	{
		var playerLegStatistic = this.Statistics[this.CurrentPlayerIndex];
		playerLegStatistic.PlayTurn(points);

		if (isEndOfLeg)
		{
			if (this.Turns[this.CurrentPlayerIndex].Count == 3)
				playerLegStatistic.IsNineDarter = true;
		}
	}

	private void EndLeg()
	{
		this.WinnerIndex = this.CurrentPlayerIndex;
	}

	private void GoToNextPlayer()
	{
		this.CurrentPlayerIndex = (this.CurrentPlayerIndex + 1) % (this.PlayerCount);
	}
}