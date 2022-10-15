namespace App.Models;

public class PlayerMatchStatistic
{
	public int OneEighties { get; set; }
	public int Ninedarters { get; set; }
	public int AverageScore { get; set; }

	public int SetsPlayed { get; set; }
	public int SetsWon { get; set; }
	public int SetsLost => SetsPlayed - SetsWon;
}