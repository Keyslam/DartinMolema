#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerSetStatistic
{
    [JsonProperty("one_eighties")]
    public int OneEighties { get; set; }

    [JsonProperty("nine_darters")]
    public int Ninedarters { get; set; }

    [JsonProperty("legs_played")]
    public int LegsPlayed { get; set; }

    [JsonProperty("legs_won")]
    public int LegsWon { get; set; }

    public PlayerSetStatistic() { }

    public PlayerSetStatistic(App.Models.PlayerSetStatistic playerSetStatistic)
    {
        this.OneEighties = playerSetStatistic.OneEighties;
        this.Ninedarters = playerSetStatistic.Ninedarters;
        this.LegsPlayed = playerSetStatistic.LegsPlayed;
        this.LegsWon = playerSetStatistic.LegsWon;
    }

    public App.Models.PlayerSetStatistic ToReal()
    {
        var playerSetStatistic = new App.Models.PlayerSetStatistic();

        playerSetStatistic.OneEighties = this.OneEighties;
        playerSetStatistic.Ninedarters = this.Ninedarters;
        playerSetStatistic.LegsPlayed = this.LegsPlayed;
        playerSetStatistic.LegsWon = this.LegsWon;

        return playerSetStatistic;
    }
}