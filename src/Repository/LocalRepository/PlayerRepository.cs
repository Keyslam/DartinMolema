using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository;

internal class PlayerRepository : PlayerRepository
{
	public void Save(Player player)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var serializedObject = JsonConvert.SerializeObject(player);
		var filepath = this.PlayerToFilepath(player);

		File.WriteAllText(filepath, serializedObject);
	}

	public Player? Read(Guid id)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = IdToFilepath(id);

		return ReadFromFile(filepath);
	}

	private Player? ReadFromFile(string filepath)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var serializedObject = File.ReadAllText(filepath);
		var player = JsonConvert.DeserializeObject<Player>(serializedObject);

		return player;
	}

	public IReadOnlyList<Player> ReadAll()
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var files = Directory.GetFiles(this.GetBaseDirectory());
		var players = new List<Player>();

		foreach (var file in files)
			players.Add(this.ReadFromFile(file)!);

		return players;
	}

	public void Delete(Guid id)
	{
		throw new NotImplementedException();
	}

	private string GetBaseDirectory()
	{
		return $"{Environment.CurrentDirectory}/Data/Players";
	}

	private string PlayerToFilepath(Player player)
	{
		return this.IdToFilepath(player.Id);
	}

	private string IdToFilepath(Guid id)
	{
		return $"{this.GetBaseDirectory()}/{id}.json";
	}
}