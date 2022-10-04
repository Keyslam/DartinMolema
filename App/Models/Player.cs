#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Models;

public class Player
{
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[JsonProperty("full_name")]
	public string FullName { get; set; }

	[JsonProperty("played_games")]
	public List<Guid> PlayedGames { get; set; }

	[JsonProperty("wins")]
	public uint Wins { get; set; }

	[JsonProperty("lossess")]
	public uint Lossess { get; set; }

	[JsonProperty("tripledarts")]
	public uint Tripledarts { get; set; }

	[JsonProperty("ninedarters")]
	public uint Ninedarters { get; set; }
}