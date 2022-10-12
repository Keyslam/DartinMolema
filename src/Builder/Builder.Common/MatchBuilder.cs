using System.Text;
using App.Models;
using App.Repository;

namespace App.Builders;

public class MatchBuilder
{
    public DateTime Date { get; set; }
    public Guid WinnerId { get; set; }
    public int SetsToWin { get; set; }
    public int LegsToWin { get; set; }
    public int ScoreToWin { get; set; }
    public int ThrowsPerTurn { get; set; }

    private List<Guid> Players { get; set; }
    private List<Set> Sets { get; set; }

#pragma warning disable 8618
    public MatchBuilder()
    {
        this.Reset();
    }
#pragma warning restore 8618

    public void Reset()
    {
        this.Date = DateTime.Now;
        this.Players = new List<Guid>();
        this.WinnerId = Guid.Empty;
        this.SetsToWin = 5;
        this.LegsToWin = 5;
        this.ScoreToWin = 501;
        this.ThrowsPerTurn = 3;
        this.Sets = new List<Set>();
    }

    public MatchBuilder AddPlayer(Player player)
    {
        this.Players.Add(player.Id);
        return this;
    }

    public MatchBuilder AddPlayer(Guid playerId)
    {
        this.Players.Add(playerId);
        return this;
    }

    public Match Build(IPlayerRepository playerRepository)
    {
        var match = new Match();

        match.Id = Guid.NewGuid();
        match.Date = this.Date;
        match.Players = this.Players;
        match.WinnerId = this.WinnerId;
        match.SetsToWin = this.SetsToWin;
        match.LegsToWin = this.LegsToWin;
        match.ScoreToWin = this.ScoreToWin;
        match.ThrowsPerTurn = this.ThrowsPerTurn;
        match.Sets = this.Sets;
        match.Statistics = new Dictionary<Guid, PlayerMatchStatistic>();

        this.Reset();

        return match;
    }

    private string BuildName(Match match, IPlayerRepository playerRepository)
    {
        var titleBuilder = new StringBuilder();

        for (int i = 0; i < match.Players.Count; i++)
        {
            if (i != 0)
                titleBuilder.Append(" vs ");

            string playerName = playerRepository.Read(match.Players[i])?.FullName ?? "Unknown Player";
            titleBuilder.Append(playerName);
        }

        titleBuilder.Append(" | ");
        titleBuilder.Append(match.Date.ToString("dd-MM-yyyy HH:mm"));

        return titleBuilder.ToString();
    }
}