namespace App.Models;

public class PlayerSetStatistic
{
	public int OneEighties { get; set; }
	public int Ninedarters { get; set; }
	public double AverageScore { get; set; }
	public int PlayedTurns { get; set; }

	public int LegsPlayed { get; set; }
	public int LegsWon { get; set; }
	public int LegsLost => LegsPlayed - LegsWon;

	public PlayerSetStatistic()
	{
		this.OneEighties = 0;
		this.Ninedarters = 0;
		this.AverageScore = 0;
		this.PlayedTurns = 0;

		this.LegsPlayed = 0;
		this.LegsWon = 0;
	}

	public PlayerSetStatistic(int oneEighties, int nineDarters, double averageScore, int playedTurns, int legsPlayed, int legsWon)
	{
		this.OneEighties = oneEighties;
		this.Ninedarters = nineDarters;
		this.AverageScore = averageScore;
		this.PlayedTurns = playedTurns;

		this.LegsPlayed = legsPlayed;
		this.LegsWon = legsWon;
	}

	public void PlayTurn(int points)
	{
		if (points == 180)
			this.OneEighties++;

		this.AverageScore = ((this.AverageScore * this.PlayedTurns) + points) / (this.PlayedTurns + 1);

		this.PlayedTurns++;
	}

	public void PlayLeg(bool won, bool isNineDarter)
	{
		this.LegsPlayed++;

		if (won)
			this.LegsWon++;

		if (isNineDarter)
			this.Ninedarters++;
	}
}