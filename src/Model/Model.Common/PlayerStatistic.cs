namespace App.Models;

public class PlayerStatistic
{
	public double AverageTurnScore { get; set; }
	public int PlayedTurns { get; set; }
	public int OneEighties { get; set; }
	public int Ninedarters { get; set; }

	public PlayerStatistic()
	{
		this.AverageTurnScore = 0;
		this.PlayedTurns = 0;
		this.OneEighties = 0;
		this.Ninedarters = 0;
	}

	public PlayerStatistic(double averageTurnScore, int playedTurns, int oneEighties, int ninedarters)
	{
		this.AverageTurnScore = averageTurnScore;
		this.PlayedTurns = playedTurns;
		this.OneEighties = oneEighties;
		this.Ninedarters = ninedarters;
	}
}