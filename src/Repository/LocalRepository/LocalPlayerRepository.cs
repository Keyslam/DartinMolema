using App.Models;
using App.Repository.LocalRepository.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository;

public class LocalPlayerRepository : IPlayerRepository
{
	public void Save(Player player)
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var filepath = this.PlayerToFilepath(player);

		var jsonPlayer = new PlayerDTO(player);
		var serializedObject = JsonConvert.SerializeObject(jsonPlayer);

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
		var jsonPlayer = JsonConvert.DeserializeObject<PlayerDTO>(serializedObject);

		if (jsonPlayer == null)
			return null;

		var player = jsonPlayer.ToReal();

		return player;
	}

	public IReadOnlyList<Player> ReadAll()
	{
		Directory.CreateDirectory(this.GetBaseDirectory());

		var files = Directory.GetFiles(this.GetBaseDirectory());
		var players = new List<Player>();

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

	private string PlayerToFilepath(Player player)
	{
		return this.IdToFilepath(player.Id);
	}

	private string IdToFilepath(Guid id)
	{
		return $"{this.GetBaseDirectory()}/{id}.json";
	}
}