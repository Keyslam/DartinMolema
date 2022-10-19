namespace App.Models;

public interface ITurn
{
	int ThrownPoints { get; }
	int AssignedPoints { get; }
	IReadOnlyList<Throw> Throws { get; }
	bool IsValid { get; set; }


	bool IsValidLastTurn();
}

public class Turn : ITurn
{
	public int ThrownPoints => this.Throws
		.Select(@throw => @throw.GetThrownPoints())
		.Sum();

	public int AssignedPoints => this.Throws
		.Select(@throw => @throw.AssignedPoints)
		.Sum();

	public IReadOnlyList<Throw> Throws { get; }

	private bool isValid = false;
	public bool IsValid
	{
		get => this.isValid;
		set
		{
			this.isValid = value;
			foreach (var @throw in this.Throws)
				@throw.IsValid = value;
		}
	}


	public Turn(IEnumerable<Throw> throws)
	{
		this.Throws = new List<Throw>(throws);
	}


	public bool IsValidLastTurn()
	{
		var lastThrow = this.Throws
			.Where(@throw => @throw.Kind != ThrowKind.None)
			.Last();

		return lastThrow.IsValidLastThrow();
	}
}