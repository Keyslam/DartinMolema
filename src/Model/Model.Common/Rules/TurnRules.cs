namespace App.Models;

public class TurnRules
{
	public int ThrowsPerTurn { get; }

	public TurnRules(int throwsPerTurn)
	{
		this.ThrowsPerTurn = throwsPerTurn;
	}
}