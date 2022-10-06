#pragma warning disable 8618

namespace App.Models;

public class Leg
{
    public Guid Id { get; set; }
    public Guid WinnerId { get; set; }
    public Dictionary<Guid, List<Turn>> Turns { get; set; }
}