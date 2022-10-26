using Newtonsoft.Json;

namespace App.Repository.Caching;

internal class MatchMetadataDTO
{
	[JsonProperty("a")]
	public Guid Id { get; set; }

	[JsonProperty("b")]
	public string Name { get; set; }

	[JsonProperty("c")]
	public bool IsDone { get; set; }

#pragma warning disable 8618
	public MatchMetadataDTO() { }
#pragma warning restore 8618

	public MatchMetadataDTO(MatchMetadata matchMetadata)
	{
		this.Id = matchMetadata.Id;
		this.Name = matchMetadata.Name;
		this.IsDone = matchMetadata.IsDone;
	}

	public MatchMetadata ToReal()
	{
		return new MatchMetadata(
			this.Id,
			this.Name,
			this.IsDone
		);
	}
}