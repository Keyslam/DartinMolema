#pragma warning disable 8618

namespace App.Models;

public class Player
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public List<Guid> PlayedGames { get; set; }
    public uint Wins { get; set; }
    public uint Lossess { get; set; }
    public uint Tripledarts { get; set; }
    public uint Ninedarters { get; set; }
}