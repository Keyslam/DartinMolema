#pragma warning disable 8618

using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

public class PlayerStatistic
{
    [JsonProperty("average_turn_score")]
    public int AverageTurnScore { get; set; }

    [JsonProperty("played_turns")]
    public int PlayedTurns { get; set; }

    [JsonProperty("one_eighties")]
    public int OneEighties { get; set; }

    [JsonProperty("nine_darters")]
    public int Ninedarters { get; set; }

    public PlayerStatistic() { }

    public PlayerStatistic(App.Models.PlayerStatistic playerStatistic)
    {
        this.AverageTurnScore = playerStatistic.AverageTurnScore;
        this.PlayedTurns = playerStatistic.PlayedTurns;
        this.OneEighties = playerStatistic.OneEighties;
        this.Ninedarters = playerStatistic.Ninedarters;
    }

    public App.Models.PlayerStatistic ToReal()
    {
        var playerStatistic = new App.Models.PlayerStatistic();

        playerStatistic.AverageTurnScore = this.AverageTurnScore;
        playerStatistic.PlayedTurns = this.PlayedTurns;
        playerStatistic.OneEighties = this.OneEighties;
        playerStatistic.Ninedarters = this.Ninedarters;

        return playerStatistic;
    }
}