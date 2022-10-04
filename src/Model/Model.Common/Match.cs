#pragma warning disable 8618

namespace App.Models;

public class Match
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<Guid> Players { get; set; }
    public Guid? WinnerId { get; set; }
    public uint SetsToWin { get; set; }
    public uint LegsToWin { get; set; }
    public uint ScoreToWin { get; set; }
    public uint ThrowsPerTurn { get; set; }
    public List<Set> Sets { get; set; }
}