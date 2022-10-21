using App.Models;
using App.Repository.LocalRepository.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository;

public class LocalMatchRepository : IMatchRepository
{
	public void Save(Match match)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = this.MatchToFilepath(match);

		var jsonMatch = new MatchDTO(match);
		var serializedObject = JsonConvert.SerializeObject(jsonMatch);

		File.WriteAllText(filepath, serializedObject);
	}

	public Match? Read(Guid id)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = IdToFilepath(id);

		return ReadFromFile(filepath);
	}

	private Match? ReadFromFile(string filepath)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var serializedObject = File.ReadAllText(filepath);
		var jsonMatch = JsonConvert.DeserializeObject<MatchDTO>(serializedObject);

		if (jsonMatch == null)
			return null;

		var match = jsonMatch.ToReal();

		return match;
	}

	public IReadOnlyList<Match> ReadAll()
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var files = Directory.GetFiles(this.GetBaseDirectory());
		var matches = new List<Match>();

		foreach (var file in files)
		{
			var match = this.ReadFromFile(file);

			if (match == null)
				continue;

			matches.Add(match);
		}

		return matches;
	}

	public IReadOnlyList<(Guid, string)> ReadAllNames()
	{
		return this.ReadAll().Select(match => (match.Id, match.Name)).ToList();
	}

	private string GetBaseDirectory()
	{
		return $"{Environment.CurrentDirectory}/Data/Matches";
	}

	private string MatchToFilepath(Match match)
	{
		return this.IdToFilepath(match.Id);
	}

	private string IdToFilepath(Guid id)
	{
		return $"{this.GetBaseDirectory()}/{id}.json";
	}


}