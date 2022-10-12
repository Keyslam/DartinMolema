#pragma warning disable 8618

namespace App.Models;

public class Set
{
    public Guid Id { get; set; }
    public Guid WinnerId { get; set; }
    public List<Leg> Legs { get; set; }
    public Dictionary<Guid, PlayerSetStatistic> Statistics { get; set; }
}