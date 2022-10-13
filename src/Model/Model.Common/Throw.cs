#pragma warning disable 8618

namespace App.Models;

public class Throw
{
    public Guid Id { get; set; }
    public int ThrownValue { get; set; }
    public int AssignedValue { get; set; }
    public ThrowKind Kind { get; set; }
}