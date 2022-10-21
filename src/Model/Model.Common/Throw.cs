namespace App.Models;

public class Throw
{
	public int ValueRegion { get; set; }
	public ThrowKind Kind { get; }
	public bool IsValid { get; set; }

	public int AssignedPoints => this.IsValid ? this.GetThrownPoints() : 0;

	public Throw(int valueRegion, ThrowKind kind)
	{
		this.ValueRegion = valueRegion;
		this.Kind = kind;
		this.IsValid = false;
	}

	public Throw(int valueRegion, ThrowKind kind, bool isValid)
	{
		this.ValueRegion = valueRegion;
		this.Kind = kind;
		this.IsValid = isValid;
	}

	public int GetThrownPoints()
	{
		switch (this.Kind)
		{
			case ThrowKind.None:
				return 0;
			case ThrowKind.Foul:
				return 0;
			case ThrowKind.Single:
				return this.ValueRegion;
			case ThrowKind.Double:
				return this.ValueRegion * 2;
			case ThrowKind.Triple:
				return this.ValueRegion * 3;
			case ThrowKind.OuterBull:
				return this.ValueRegion;
			case ThrowKind.InnerBull:
				return this.ValueRegion * 2;
			default:
				return 0;
		}
	}

	public bool IsValidLastThrow()
	{
		switch (this.Kind)
		{
			case ThrowKind.Double:
			case ThrowKind.InnerBull:
				return true;
			default:
				return false;
		}
	}
}