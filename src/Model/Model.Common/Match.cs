#pragma warning disable 8618

namespace App.Models;

public class Match
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public List<Guid> Players { get; set; }
    public Guid WinnerId { get; set; }
    public int SetsToWin { get; set; }
    public int LegsToWin { get; set; }
    public int ScoreToWin { get; set; }
    public int ThrowsPerTurn { get; set; }
    public List<Set> Sets { get; set; }
    public Dictionary<Guid, PlayerMatchStatistic> Statistics { get; set; }
}