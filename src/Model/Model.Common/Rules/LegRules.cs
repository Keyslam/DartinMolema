namespace App.Models;

public class LegRules
{
	public TurnRules TurnRules { get; }
	public int TargetScore { get; }

	public LegRules(TurnRules turnRules, int targetScore)
	{
		this.TurnRules = turnRules;
		this.TargetScore = targetScore;
	}
}