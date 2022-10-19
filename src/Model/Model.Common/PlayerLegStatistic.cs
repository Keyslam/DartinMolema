namespace App.Models;

public class PlayerLegStatistic
{
	public int OneEighties { get; set; }
	public int AverageTurnScore { get; set; }
	public int PlayedTurns { get; set; }
	public bool IsNineDarter { get; set; }

	public PlayerLegStatistic()
	{
		this.OneEighties = 0;
		this.IsNineDarter = false;

		this.AverageTurnScore = 0;
		this.PlayedTurns = 0;
	}

	public PlayerLegStatistic(int oneEighties, bool isNineDarter, int averageScore, int playedTurns)
	{
		this.OneEighties = oneEighties;
		this.AverageTurnScore = averageScore;
		this.PlayedTurns = playedTurns;
		this.IsNineDarter = isNineDarter;
	}

	public void PlayTurn(int points)
	{
		if (points == 180)
			this.OneEighties++;

		this.AverageTurnScore = ((this.AverageTurnScore * this.PlayedTurns) + points) / (this.PlayedTurns + 1);

		this.PlayedTurns++;
	}
}