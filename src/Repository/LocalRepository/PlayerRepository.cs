using App.Repository.LocalRepository.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository;

public class PlayerRepository : IPlayerRepository
{
    public void Save(App.Models.Player player)
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var filepath = this.PlayerToFilepath(player);

        var jsonPlayer = new Player(player);
        var serializedObject = JsonConvert.SerializeObject(jsonPlayer);

        File.WriteAllText(filepath, serializedObject);
    }

    public App.Models.Player? Read(Guid id)
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var filepath = IdToFilepath(id);

        return ReadFromFile(filepath);
    }

    private App.Models.Player? ReadFromFile(string filepath)
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var serializedObject = File.ReadAllText(filepath);
        var jsonPlayer = JsonConvert.DeserializeObject<Player>(serializedObject);

        if (jsonPlayer == null)
            return null;

        var player = jsonPlayer.ToReal();

        return player;
    }

    public IReadOnlyList<App.Models.Player> ReadAll()
    {
        Directory.CreateDirectory(this.GetBaseDirectory());

        var files = Directory.GetFiles(this.GetBaseDirectory());
        var players = new List<App.Models.Player>();

        foreach (var file in files)
        {
            var player = this.ReadFromFile(file);

            if (player == null)
                continue;

            players.Add(player);
        }

        return players;
    }

    private string GetBaseDirectory()
    {
        return $"{Environment.CurrentDirectory}/Data/Players";
    }

    private string PlayerToFilepath(App.Models.Player player)
    {
        return this.IdToFilepath(player.Id);
    }

    private string IdToFilepath(Guid id)
    {
        return $"{this.GetBaseDirectory()}/{id}.json";
    }
}