using App.Models;
using Newtonsoft.Json;

namespace App.Repository.Caching;

public class MatchCachingDecorator : IMatchRepository
{
	private IMatchRepository MatchRepository { get; }
	private List<MatchMetadata> MatchMetadatas { get; }

	public MatchCachingDecorator(IMatchRepository matchRepository)
	{
		this.MatchRepository = matchRepository;
		this.MatchMetadatas = new List<MatchMetadata>();

		if (File.Exists(GetFileName()))
		{
			var serializedObject = File.ReadAllText(GetFileName());
			var matchMetadataDtos = JsonConvert.DeserializeObject<List<MatchMetadataDTO>>(serializedObject)!;

			foreach (var matchMetadataDTO in matchMetadataDtos)
			{
				var matchMetaData = matchMetadataDTO.ToReal();
				this.MatchMetadatas.Add(matchMetaData);
			}
		}
	}

	public Match? Read(Guid id) => MatchRepository.Read(id);
	public IReadOnlyList<Match> ReadAll() => MatchRepository.ReadAll();

	public void Save(Match t)
	{
		MatchRepository.Save(t);

		var index = -1;
		var matchMetaData = this.MatchMetadatas.Where(x => x.Id == t.Id).FirstOrDefault();
		if (matchMetaData != null)
			index = this.MatchMetadatas.IndexOf(matchMetaData);

		if (index == -1)
		{
			this.MatchMetadatas.Add(new MatchMetadata(
				t.Id,
				t.Name,
				t.IsDone
			));
		}
		else
		{
			this.MatchMetadatas[index].Id = t.Id;
			this.MatchMetadatas[index].Name = t.Name;
			this.MatchMetadatas[index].IsDone = t.IsDone;
		}


		this.SaveAll();
	}

	public IReadOnlyList<MatchMetadata> ReadAllMetadata()
	{
		return this.MatchMetadatas;
	}

	private void SaveAll()
	{
		var matchMetaDataDTOs = new List<MatchMetadataDTO>();

		foreach (var matchMetadata in this.MatchMetadatas)
		{
			var matchMetaDataDTO = new MatchMetadataDTO(matchMetadata);
			matchMetaDataDTOs.Add(matchMetaDataDTO);
		}

		var serializedObject = JsonConvert.SerializeObject(matchMetaDataDTOs);
		File.WriteAllText(this.GetFileName(), serializedObject);
	}

	private string GetFileName()
	{
		return $"{Environment.CurrentDirectory}/Data/MatchMetadata.json";
	}
}
