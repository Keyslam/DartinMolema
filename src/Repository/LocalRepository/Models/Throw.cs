#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Throw
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public int ThrownValue { get; set; }

	[JsonProperty("c")]
	public int AssignedValue { get; set; }

	[JsonProperty("d")]
	public App.Models.ThrowKind Kind { get; set; }

	public Throw() { }

	public Throw(App.Models.Throw @throw)
	{
		this.Id = @throw.Id;
		this.ThrownValue = @throw.ThrownValue;
		this.AssignedValue = @throw.AssignedValue;
		this.Kind = @throw.Kind;
	}

	public App.Models.Throw ToReal()
	{
		var @throw = new App.Models.Throw();

		@throw.Id = this.Id;
		@throw.ThrownValue = this.ThrownValue;
		@throw.AssignedValue = this.AssignedValue;
		@throw.Kind = this.Kind;

		return @throw;
	}
}