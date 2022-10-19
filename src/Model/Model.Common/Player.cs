namespace App.Models;

public class Player
{
	public Guid Id { get; }
	public string FullName { get; }

	public List<Reference<Match>> PlayedMatches { get; set; }
	public List<Reference<Match>> WonMatches { get; set; }
	public List<Reference<Match>> LostMatches { get; set; }

	public PlayerStatistic Statistic { get; set; }

	public Player(string fullName)
	{
		this.Id = Guid.NewGuid();
		this.FullName = fullName;
		this.PlayedMatches = new List<Reference<Match>>();
		this.WonMatches = new List<Reference<Match>>();
		this.LostMatches = new List<Reference<Match>>();
		this.Statistic = new PlayerStatistic();
	}

	public Player(Guid id, string fullName, IEnumerable<Reference<Match>> playedMatches, IEnumerable<Reference<Match>> wonMatches, IEnumerable<Reference<Match>> lostMatches, PlayerStatistic statistic)
	{
		this.Id = id;
		this.FullName = fullName;
		this.PlayedMatches = new List<Reference<Match>>(playedMatches);
		this.WonMatches = new List<Reference<Match>>(wonMatches);
		this.LostMatches = new List<Reference<Match>>(lostMatches);
		this.Statistic = statistic;
	}
}