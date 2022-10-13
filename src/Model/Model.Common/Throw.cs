#pragma warning disable 8618

namespace App.Models;

public class Throw
{
    public Guid Id { get; set; }
    public uint ThrownValue { get; set; }
    public uint AssignedValue { get; set; }
    public ThrowKind Kind { get; set; }
}