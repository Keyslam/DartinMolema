namespace App.Models;

public class Player
{

	public Guid Id { get; }
	public string FullName { get; }
	public string Country { get; set; }

	public List<Guid> PlayedMatches { get; set; }
	public List<Guid> WonMatches { get; set; }
	public List<Guid> LostMatches { get; set; }

	public PlayerStatistic Statistic { get; set; }

	public Player(string fullName, string country)
	{
		this.Id = Guid.NewGuid();
		this.FullName = fullName;
		this.Country = country;
		this.PlayedMatches = new List<Guid>();
		this.WonMatches = new List<Guid>();
		this.LostMatches = new List<Guid>();
		this.Statistic = new PlayerStatistic();
	}

	public Player(Guid id, string fullName, string country, IEnumerable<Guid> playedMatches, IEnumerable<Guid> wonMatches, IEnumerable<Guid> lostMatches, PlayerStatistic statistic)
	{
		this.Id = id;
		this.FullName = fullName;
		this.Country = country;
		this.PlayedMatches = new List<Guid>(playedMatches);
		this.WonMatches = new List<Guid>(wonMatches);
		this.LostMatches = new List<Guid>(lostMatches);
		this.Statistic = statistic;
	}

	public void PlayMatch(Match match)
	{
		var playerIndex = -1;
		for (var index = 0; index < match.Players.Count; index++)
		{
			if (match.Players[index] == this.Id)
			{
				playerIndex = index;
				break;
			}
		}

		var wonMatch = match.WinnerIndex == playerIndex;

		this.PlayedMatches.Add(match.Id);

		if (wonMatch)
			this.WonMatches.Add(match.Id);
		else
			this.LostMatches.Add(match.Id);

		foreach (var set in match.Sets)
		{
			foreach (var leg in set.Legs)
			{
				foreach (var turnn in leg.Turns[playerIndex])
				{
					this.Statistic.AverageTurnScore = (((this.Statistic.AverageTurnScore * this.Statistic.PlayedTurns) + turnn.AssignedPoints)) / (this.Statistic.PlayedTurns + 1);

					this.Statistic.PlayedTurns++;

					if (turnn.AssignedPoints == 180)
						this.Statistic.OneEighties++;
				}

				if (leg.Statistics[playerIndex].IsNineDarter)
					this.Statistic.Ninedarters++;
			}
		}
	}
}