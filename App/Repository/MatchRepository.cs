using App.Models;
using Newtonsoft.Json;

namespace App.Repository;

public class MatchRepository : IRepository<Match>
{
    public void Save(Match match)
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var serializedObject = JsonConvert.SerializeObject(match);
        var filepath = this.MatchToFilepath(match);
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
        var match = JsonConvert.DeserializeObject<Match>(serializedObject);

        return match;
    }

    public IReadOnlyList<Match> ReadAll()
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var files = Directory.GetFiles(this.GetBaseDirectory());
        var matches = new List<Match>();

        foreach (var file in files)
            matches.Add(this.ReadFromFile(file)!);

        return matches;
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