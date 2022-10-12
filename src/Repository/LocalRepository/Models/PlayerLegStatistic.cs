#pragma warning disable 8618

using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class PlayerLegStatistic
{
    [JsonProperty("one_eighties")]
    public int OneEighties { get; set; }

    [JsonProperty("is_nine_darter")]
    public bool IsNineDarter { get; set; }

    [JsonProperty("average_turn_score")]
    public int AverageTurnScore { get; set; }

    [JsonProperty("remaining_points")]
    public int RemainingPoints { get; set; }

    [JsonProperty("played_turns")]
    public int PlayedTurns { get; set; }

    public PlayerLegStatistic() { }

    public PlayerLegStatistic(App.Models.PlayerLegStatistic playerLegStatistic)
    {
        this.OneEighties = playerLegStatistic.OneEighties;
        this.IsNineDarter = playerLegStatistic.IsNineDarter;
        this.AverageTurnScore = playerLegStatistic.AverageTurnScore;
        this.RemainingPoints = playerLegStatistic.RemainingPoints;
        this.PlayedTurns = playerLegStatistic.PlayedTurns;
    }

    public App.Models.PlayerLegStatistic ToReal()
    {
        var playerLegStatistic = new App.Models.PlayerLegStatistic();

        playerLegStatistic.OneEighties = this.OneEighties;
        playerLegStatistic.IsNineDarter = this.IsNineDarter;
        playerLegStatistic.AverageTurnScore = this.AverageTurnScore;
        playerLegStatistic.RemainingPoints = this.RemainingPoints;
        playerLegStatistic.PlayedTurns = this.PlayedTurns;

        return playerLegStatistic;
    }
}