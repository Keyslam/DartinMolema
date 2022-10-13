#pragma warning disable 8618

using App.Models;
using Newtonsoft.Json;

namespace App.Repository.LocalRepository.Models;

internal class Throw
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("thrown_value")]
    public uint ThrownValue { get; set; }

    [JsonProperty("assigned_value")]
    public uint AssignedValue { get; set; }

    [JsonProperty("kind")]
    public ThrowKind Kind { get; set; }

    public Throw() { }

    public Throw(App.Models.Throw throww)
    {
        this.Id = throww.Id;
        this.ThrownValue = throww.ThrownValue;
        this.AssignedValue = throww.AssignedValue;
        this.Kind = throww.Kind;
    }

    public App.Models.Throw ToReal()
    {
        var throww = new App.Models.Throw();

        throww.Id = this.Id;
        throww.ThrownValue = this.ThrownValue;
        throww.AssignedValue = this.AssignedValue;
        throww.Kind = this.Kind;

        return throww;
    }
}