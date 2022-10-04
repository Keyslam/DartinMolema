#pragma warning disable 8618

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

    [JsonProperty("wins")]
    public uint Wins { get; set; }

    [JsonProperty("lossess")]
    public uint Lossess { get; set; }

    [JsonProperty("tripledarts")]
    public uint Tripledarts { get; set; }

    [JsonProperty("ninedarters")]
    public uint Ninedarters { get; set; }

    public Player() { }

    public Player(App.Models.Player player)
    {
        this.Id = player.Id;
        this.FullName = player.FullName;
        this.PlayedGames = player.PlayedGames;
        this.Wins = player.Wins;
        this.Lossess = player.Lossess;
        this.Tripledarts = player.Tripledarts;
        this.Ninedarters = player.Ninedarters;
    }

    public App.Models.Player ToReal()
    {
        var player = new App.Models.Player();

        player.Id = this.Id;
        player.FullName = this.FullName;
        player.PlayedGames = this.PlayedGames;
        player.Wins = this.Wins;
        player.Lossess = this.Lossess;
        player.Tripledarts = this.Tripledarts;
        player.Ninedarters = this.Ninedarters;

        return player;
    }
}