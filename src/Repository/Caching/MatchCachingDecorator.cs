using App.Models;
using Newtonsoft.Json;

namespace App.Repository.Caching;

public class MatchCachingDecorator : IMatchRepository
{
	private IMatchRepository MatchRepository { get; }
	private Dictionary<Guid, string> MatchNames { get; }

	public MatchCachingDecorator(IMatchRepository matchRepository)
	{
		this.MatchRepository = matchRepository;

		if (File.Exists(GetFileName()))
		{
			var serializedObject = File.ReadAllText(GetFileName());
			this.MatchNames = JsonConvert.DeserializeObject<Dictionary<Guid, string>>(serializedObject)!;
		}
		else
		{
			this.MatchNames = new Dictionary<Guid, string>();
		}
	}

	public Match? Read(Guid id) => MatchRepository.Read(id);
	public IReadOnlyList<Match> ReadAll() => MatchRepository.ReadAll();

	public void Save(Match t)
	{
		MatchRepository.Save(t);

		this.MatchNames[t.Id] = t.Name;

		var serializedObject = JsonConvert.SerializeObject(this.MatchNames);
		File.WriteAllText(this.GetFileName(), serializedObject);
	}

	public IReadOnlyList<(Guid, string)> ReadAllNames()
	{
		return this.MatchNames.Select(x => (x.Key, x.Value)).ToList();
	}

	private string GetFileName()
	{
		return $"{Environment.CurrentDirectory}/Data/MatchNames.json";
	}
}
