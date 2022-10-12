#pragma warning disable 8618

using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Player
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

    [JsonProperty("played_games")]
    public List<Guid> PlayedGames { get; set; }

    [JsonProperty("won_games")]
    public List<Guid> WonGames { get; set; }

    [JsonProperty("lost_games")]
    public List<Guid> LostGames { get; set; }

    [JsonProperty("statistic")]
    public PlayerStatistic Statistic { get; set; }

    public Player() { }

    public Player(App.Models.Player player)
    {
        this.Id = player.Id;
        this.FullName = player.FullName;
        this.PlayedGames = player.PlayedGames;
        this.WonGames = player.WonGames;
        this.LostGames = player.LostGames;
        this.Statistic = player.Statistic;
    }

    public App.Models.Player ToReal()
    {
        var player = new App.Models.Player();

        player.Id = this.Id;
        player.FullName = this.FullName;
        player.PlayedGames = this.PlayedGames;
        player.WonGames = this.WonGames;
        player.LostGames = this.LostGames;
        player.Statistic = this.Statistic;

        return player;
    }
}