#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Leg
{
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[JsonProperty("winner_id")]
	public Guid? WinnerId { get; set; }

	[JsonProperty("turns")]
	public Dictionary<Guid, List<Turn>> Turns { get; set; }
}