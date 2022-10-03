using App.Models;
using App.Repository;

namespace App;

public class RuleEngine
{
    private MatchRepository MatchRepository { get; }
    private PlayerRepository PlayerRepository { get; }
    private Match Match { get; }

    private Set? CurrentSet { get; set; }
    private Leg? CurrentLeg { get; set; }

    public List<Player> Players { get; }

    public uint SetsToWin
    {
        get => this.Match.SetsToWin;
        set
        {
            this.Match.SetsToWin = value;
            this.MatchRepository.Save(this.Match);
        }
    }

    public uint LegsToWin
    {
        get => this.Match.LegsToWin;
        set
        {
            this.Match.LegsToWin = value;
            this.MatchRepository.Save(this.Match);
        }
    }

    public uint ScoreToWin
    {
        get => this.Match.ScoreToWin;
        set
        {
            this.Match.ScoreToWin = value;
            this.MatchRepository.Save(this.Match);
        }
    }

    public uint ThrowsPerTurn
    {
        get => this.Match.ThrowsPerTurn;
        set
        {
            this.Match.ThrowsPerTurn = value;
            this.MatchRepository.Save(this.Match);
        }
    }

    public RuleEngine(Match match)
    {
        this.Match = match;

        this.MatchRepository = new MatchRepository();
        this.PlayerRepository = new PlayerRepository();

        this.Players = new List<Player>();
        foreach (var playerId in this.Match.Players)
        {
            var player = PlayerRepository.Read(playerId)!;
            this.Players.Add(player);
        }

        this.MatchRepository.Save(this.Match);
    }

    public void StartSet()
    {
        this.CurrentSet = new Set();

        this.CurrentSet.Id = Guid.NewGuid();
        this.CurrentSet.WinnerId = null;
        this.CurrentSet.Legs = new List<Leg>();

        this.Match.Sets.Add(this.CurrentSet);

        this.MatchRepository.Save(this.Match);
    }

    public void EndSet()
    {

    }

    public void StartLeg()
    {
        if (this.CurrentSet == null)
            throw new InvalidOperationException();

        this.CurrentLeg = new Leg();

        this.CurrentLeg.Id = Guid.NewGuid();
        this.CurrentLeg.WinnerId = null;
        this.CurrentLeg.Turns = new List<Turn>();

        this.CurrentSet.Legs.Add(this.CurrentLeg);

        this.MatchRepository.Save(this.Match);
    }

    public void EndLeg()
    {

    }

    public bool PlayTurn(List<(ThrowKind throwKind, uint value)> throws)
    {
        if (this.CurrentLeg == null)
            throw new InvalidOperationException();

        var turn = new Turn();

        turn.Id = Guid.NewGuid();
        turn.Score = 0;
        turn.Throws = new List<Throw>();

        foreach (var throwData in throws)
        {
            var throww = new Throw();

            throww.Id = Guid.NewGuid();
            throww.Kind = throwData.throwKind;
            throww.ThrownValue = throwData.value;

            turn.Throws.Add(throww);
        }

        this.CurrentLeg.Turns.Add(turn);

        this.MatchRepository.Save(this.Match);

        return true;
    }

    public List<Turn> GetCurrentTurns()
    {
        if (this.CurrentLeg == null)
            throw new InvalidOperationException();

        return this.CurrentLeg.Turns;
    }
}