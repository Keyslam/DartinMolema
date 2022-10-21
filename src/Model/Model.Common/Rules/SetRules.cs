namespace App.Models;

public class SetRules
{
	public LegRules LegRules { get; }
	public int LegsToWin { get; }

	public SetRules(LegRules legRules, int legsToWin)
	{
		this.LegRules = legRules;
		this.LegsToWin = legsToWin;
	}
}