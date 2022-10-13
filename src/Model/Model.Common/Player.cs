#pragma warning disable 8618

namespace App.Models;

public class Player
{
	public Guid Id { get; set; }
	public string FullName { get; set; }
	public List<Guid> PlayedGames { get; set; }
	public List<Guid> WonGames { get; set; }
	public List<Guid> LostGames { get; set; }
	public PlayerStatistic Statistic { get; set; }
}