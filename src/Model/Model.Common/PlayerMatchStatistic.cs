namespace App.Models;

public class PlayerMatchStatistic
{
	public int OneEighties { get; set; }
	public int Ninedarters { get; set; }
	public double AverageScore { get; set; }
	public int PlayedTurns { get; set; }

	public int SetsPlayed { get; set; }
	public int SetsWon { get; set; }
	public int SetsLost => SetsPlayed - SetsWon;

	public PlayerMatchStatistic()
	{
		this.OneEighties = 0;
		this.Ninedarters = 0;
		this.AverageScore = 0;
		this.PlayedTurns = 0;

		this.SetsPlayed = 0;
		this.SetsWon = 0;
	}

	public PlayerMatchStatistic(int oneEighties, int nineDarters, double averageScore, int playedTurns, int setsPlayed, int setsWon)
	{
		this.OneEighties = oneEighties;
		this.Ninedarters = nineDarters;
		this.AverageScore = averageScore;
		this.PlayedTurns = playedTurns;

		this.SetsPlayed = setsPlayed;
		this.SetsWon = setsWon;
	}

	public void PlayTurn(int points)
	{
		if (points == 180)
			this.OneEighties++;

		this.AverageScore = ((this.AverageScore * this.PlayedTurns) + points) / (this.PlayedTurns + 1);

		this.PlayedTurns++;
	}

	public void PlaySet(bool won, int nineDarters)
	{
		this.SetsPlayed++;

		if (won)
			this.SetsWon++;

		this.Ninedarters += nineDarters;
	}
}