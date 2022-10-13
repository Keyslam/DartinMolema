using App.Repository.LocalRepository.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository;

public class MatchRepository : IMatchRepository
{
	public void Save(App.Models.Match match)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = this.MatchToFilepath(match);

		var jsonMatch = new Match(match);
		var serializedObject = JsonConvert.SerializeObject(jsonMatch);

		File.WriteAllText(filepath, serializedObject);
	}

	public App.Models.Match? Read(Guid id)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = IdToFilepath(id);

		return ReadFromFile(filepath);
	}

	private App.Models.Match? ReadFromFile(string filepath)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var serializedObject = File.ReadAllText(filepath);
		var jsonMatch = JsonConvert.DeserializeObject<Match>(serializedObject);

		if (jsonMatch == null)
			return null;

		var match = jsonMatch.ToReal();

		return match;
	}

	public IReadOnlyList<App.Models.Match> ReadAll()
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var files = Directory.GetFiles(this.GetBaseDirectory());
		var matches = new List<App.Models.Match>();

		foreach (var file in files)
		{
			var match = this.ReadFromFile(file);

			if (match == null)
				continue;

			matches.Add(match);
		}

		return matches;
	}

	private string GetBaseDirectory()
	{
		return $"{Environment.CurrentDirectory}/Data/Matches";
	}

	private string MatchToFilepath(App.Models.Match match)
	{
		return this.IdToFilepath(match.Id);
	}

	private string IdToFilepath(Guid id)
	{
		return $"{this.GetBaseDirectory()}/{id}.json";
	}
}