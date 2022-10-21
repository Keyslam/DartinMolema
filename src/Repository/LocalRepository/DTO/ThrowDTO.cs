using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class ThrowDTO
{
	[JsonProperty("a")]
	public int AssignedValue { get; set; }

	[JsonProperty("b")]
	public ThrowKind Kind { get; set; }

	[JsonProperty("c")]
	public bool IsValid { get; set; }

#pragma warning disable 8618
	public ThrowDTO() { }
#pragma warning restore 8618

	public ThrowDTO(Throw @throw)
	{
		this.AssignedValue = @throw.AssignedPoints;
		this.Kind = @throw.Kind;
		this.IsValid = @throw.IsValid;
	}

	public Throw ToReal()
	{
		var @throw = new Throw(
			this.AssignedValue,
			this.Kind,
			this.IsValid
		);

		return @throw;
	}
}