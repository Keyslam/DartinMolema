namespace App.Models;

public class MatchRules
{
	public SetRules SetRules { get; }
	public int SetsToWin { get; }

	public MatchRules(SetRules setRules, int setsToWin)
	{
		this.SetRules = setRules;
		this.SetsToWin = setsToWin;
	}
}