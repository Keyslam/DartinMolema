#pragma warning disable 8618

namespace App.Models;

public class Turn
{
    public Guid Id { get; set; }
    public uint Score { get; set; }
    public List<Throw> Throws { get; set; }
}