using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class TurnDTO
{
	[JsonProperty("a")]
	public List<ThrowDTO> Throws { get; set; }

	[JsonProperty("b")]
	public int PlayedByPlayerIndex { get; set; }


#pragma warning disable 8618
	public TurnDTO() { }
#pragma warning restore 8618

	public TurnDTO(ITurn turn)
	{
		this.Throws = turn.Throws
			.Select(@throw => new ThrowDTO(@throw))
			.ToList();
	}

	public ITurn ToReal()
	{
		var throws = this.Throws
			.Select(@throw => @throw.ToReal());

		var turn = new Turn(
			throws
		);

		return turn;
	}
}