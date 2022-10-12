using System.Linq;
using App.Models;
using App.Repository;

namespace App.GameRuler;

public class RuleEngine
{
    private IMatchRepository MatchRepository { get; }
    private IPlayerRepository PlayerRepository { get; }
    private Match Match { get; }

    public Set CurrentSet { get; private set; }
    public Leg CurrentLeg { get; private set; }
    public Player CurrentPlayer { get; private set; }

    public IList<Player> Players { get; }

    public int SetsToWin
    {
        get => this.Match.SetsToWin;
    }

    public int LegsToWin
    {
        get => this.Match.LegsToWin;
    }

    public int ScoreToWin
    {
        get => this.Match.ScoreToWin;
    }

    public int ThrowsPerTurn
    {
        get => this.Match.ThrowsPerTurn;
    }

#pragma warning disable 8618
    public RuleEngine(Match match, IMatchRepository matchRepository, IPlayerRepository playerRepository)
    {
        this.Match = match;

        this.MatchRepository = matchRepository;
        this.PlayerRepository = playerRepository;

        this.Players = this.Match.Players
            .Select(playerId => PlayerRepository.Read(playerId)!)
            .Where(player => player != null)
            .ToList();

        foreach (var player in this.Players)
        {
            this.Match.Statistics.Add(player.Id, new PlayerMatchStatistic()
            {
                OneEighties = 0,
                Ninedarters = 0,
                SetsWon = 0,
            });

            player.PlayedGames.Add(this.Match.Id);

            this.PlayerRepository.Save(player);
        }

        this.CurrentPlayer = this.Players[0];

        this.StartNewSet();

        this.MatchRepository.Save(this.Match);
    }
#pragma warning restore 8618

    public void StartNewSet()
    {
        var statistics = new Dictionary<Guid, PlayerSetStatistic>();

        foreach (var player in this.Players)
        {
            statistics.Add(player.Id, new PlayerSetStatistic()
            {
                OneEighties = 0,
                Ninedarters = 0,
                LegsWon = 0,
                AverageScore = 0,
            });
        }

        this.CurrentSet = new Set()
        {
            Id = Guid.NewGuid(),
            WinnerId = Guid.Empty,
            Legs = new List<Leg>(),
            Statistics = statistics,
        };

        this.Match.Sets.Add(this.CurrentSet);

        this.StartNewLeg();

        this.MatchRepository.Save(this.Match);
    }

    public void StartNewLeg()
    {
        var turns = new Dictionary<Guid, List<Turn>>();
        foreach (var player in this.Players)
            turns.Add(player.Id, new List<Turn>());

        var statistics = new Dictionary<Guid, PlayerLegStatistic>();
        foreach (var player in this.Players)
        {
            statistics.Add(player.Id, new PlayerLegStatistic()
            {
                AverageTurnScore = 0,
                OneEighties = 0,
                RemainingPoints = Match.ScoreToWin
            });
        }

        this.CurrentLeg = new Leg()
        {
            Id = Guid.NewGuid(),
            WinnerId = Guid.Empty,
            Turns = turns,
            Statistics = statistics,
        };

        this.CurrentSet.Legs.Add(this.CurrentLeg);

        this.MatchRepository.Save(this.Match);
    }

    public int GetRemainingPointsAfterTurn(List<(ThrowKind throwKind, int value)> throws)
    {
        var totalPoins = throws
            .Select(@throw => CalculateThrowScore(@throw.throwKind, @throw.value))
            .Sum();

        var remainingPoints = this.GetCurrentPlayerLegStatistic().RemainingPoints;

        var remainingPointsAfterTurn = remainingPoints - totalPoins;

        if (remainingPointsAfterTurn == 0)
        {
            var lastThrow = throws.Last();
            var lastThrowCountsAsDouble = this.DoesThrowCountAsDouble(lastThrow.throwKind);

            if (!lastThrowCountsAsDouble)
                return remainingPoints;
        }

        if (remainingPoints < 0)
            return remainingPoints;

        return remainingPointsAfterTurn;
    }

    public bool PlayTurn(List<(ThrowKind throwKind, int value)> throws)
    {
        var matchStatistic = this.GetCurrentPlayerMatchStatistic();
        var setStatistic = this.GetCurrentPlayerSetStatistic();
        var legStatistic = this.GetCurrentPlayerLegStatistic();

        var previousRemainingPoints = legStatistic.RemainingPoints;
        var newRemainingPoints = this.GetRemainingPointsAfterTurn(throws);

        var turnScore = previousRemainingPoints - newRemainingPoints;

        var turn = new Turn()
        {
            Id = Guid.NewGuid(),
            Score = turnScore,
            Throws = new List<Throw>(),
        };

        foreach (var throwData in throws)
        {
            var @throw = new Throw()
            {
                Id = Guid.NewGuid(),
                Kind = throwData.throwKind,
                ThrownValue = throwData.value,
                AssignedValue = previousRemainingPoints == newRemainingPoints ? 0 : throwData.value,
            };

            turn.Throws.Add(@throw);
        }

        var turns = this.CurrentLeg.Turns[this.CurrentPlayer.Id];
        turns.Add(turn);

        UpdateStatistics(newRemainingPoints, turnScore);

        legStatistic.RemainingPoints = newRemainingPoints;
        legStatistic.PlayedTurns += 1;
        CurrentPlayer.Statistic.AverageTurnScore = CalculateAverageIteratively(CurrentPlayer.Statistic.AverageTurnScore, turnScore, CurrentPlayer.Statistic.PlayedTurns);
        CurrentPlayer.Statistic.PlayedTurns++;

        var didMatchEnd = false;
        if (newRemainingPoints == 0)
            didMatchEnd = EndLeg();

        this.MatchRepository.Save(this.Match);
        this.PlayerRepository.Save(this.CurrentPlayer);

        var indexOf = this.Players.IndexOf(this.CurrentPlayer);
        var nextIndex = indexOf == this.Players.Count - 1 ? 0 : indexOf + 1;
        this.CurrentPlayer = this.Players[nextIndex];

        return didMatchEnd;
    }

    public PlayerMatchStatistic GetCurrentPlayerMatchStatistic() => this.GetPlayerMatchStatistic(this.CurrentPlayer);
    public PlayerMatchStatistic GetPlayerMatchStatistic(Player player) => this.GetPlayerMatchStatistic(player.Id);
    public PlayerMatchStatistic GetPlayerMatchStatistic(Guid playerId) => this.Match.Statistics[playerId];

    public PlayerSetStatistic GetCurrentPlayerSetStatistic() => this.GetPlayerSetStatistic(this.CurrentPlayer);
    public PlayerSetStatistic GetPlayerSetStatistic(Player player) => this.GetPlayerSetStatistic(player.Id);
    public PlayerSetStatistic GetPlayerSetStatistic(Guid playerId) => this.CurrentSet.Statistics[playerId];

    public PlayerLegStatistic GetCurrentPlayerLegStatistic() => this.GetPlayerLegStatistic(this.CurrentPlayer);
    public PlayerLegStatistic GetPlayerLegStatistic(Player player) => this.GetPlayerLegStatistic(player.Id);
    public PlayerLegStatistic GetPlayerLegStatistic(Guid playerId) => this.CurrentLeg.Statistics[playerId];

    public List<Turn> GetCurrentPlayerTurns() => this.GetPlayerTurns(this.CurrentPlayer);
    public List<Turn> GetPlayerTurns(Player player) => this.GetPlayerTurns(player.Id);
    public List<Turn> GetPlayerTurns(Guid playerId) => this.CurrentLeg.Turns[playerId];

    private int CalculateThrowScore(ThrowKind throwKind, int value)
    {
        switch (throwKind)
        {
            case ThrowKind.None:
                return 0;
            case ThrowKind.Foul:
                return 0;
            case ThrowKind.Single:
                return value;
            case ThrowKind.Double:
                return value * 2;
            case ThrowKind.Triple:
                return value * 3;
            case ThrowKind.OuterBull:
                return value * 1;
            case ThrowKind.InnerBull:
                return value * 2;
            default:
                return 0;
        }
    }

    private bool DoesThrowCountAsDouble(ThrowKind throwKind)
    {
        if (throwKind == ThrowKind.Double)
            return true;

        if (throwKind == ThrowKind.InnerBull)
            return true;

        return false;
    }

    private bool EndLeg()
    {
        CurrentLeg.WinnerId = CurrentPlayer.Id;

        foreach (var player in this.Players)
        {
            var isWinningPlayer = player == this.CurrentPlayer;

            var matchStatistic = this.GetPlayerMatchStatistic(player);
            var setStatistic = this.GetPlayerSetStatistic(player);
            var legStatistic = this.GetPlayerLegStatistic(player);

            setStatistic.AverageScore = CalculateAverageIteratively(setStatistic.AverageScore, legStatistic.AverageTurnScore, setStatistic.LegsPlayed);
            setStatistic.LegsPlayed += 1;

            if (player == CurrentPlayer)
                setStatistic.LegsWon += 1;
        }

        var currentPlayerSetStatistic = this.GetCurrentPlayerSetStatistic();
        var isSetWon = currentPlayerSetStatistic.LegsWon == this.Match.LegsToWin;

        var didMatchEnd = false;
        if (isSetWon)
            didMatchEnd = EndSet();
        else
            StartNewLeg();

        return didMatchEnd;
    }

    private bool EndSet()
    {
        CurrentSet.WinnerId = CurrentPlayer.Id;

        foreach (var player in this.Players)
        {
            var isWinningPlayer = player == this.CurrentPlayer;

            var matchStatistic = this.GetPlayerMatchStatistic(player);
            var setStatistic = this.GetPlayerSetStatistic(player);
            var legStatistic = this.GetPlayerLegStatistic(player);

            matchStatistic.SetsPlayed += 1;

            if (player == CurrentPlayer)
                matchStatistic.SetsWon += 1;
        }

        var currentPlayerMatchStatistic = this.GetCurrentPlayerMatchStatistic();
        var isMatchWon = currentPlayerMatchStatistic.SetsWon == this.Match.SetsToWin;

        if (isMatchWon)
        {
            EndMatch();
            return true;
        }
        else
            StartNewSet();

        return false;
    }

    private void EndMatch()
    {
        foreach (var player in this.Players)
        {
            var isWinner = player == this.CurrentPlayer;

            if (isWinner)
                player.WonGames.Add(this.Match.Id);
            else
                player.LostGames.Add(this.Match.Id);

            this.PlayerRepository.Save(player);
        }
        this.Match.WinnerId = this.CurrentPlayer.Id;
    }

    private int CalculateAverageIteratively(int previousAverage, int addedValue, int totalRecords)
    {
        return ((previousAverage * totalRecords) + addedValue) / (totalRecords + 1);
    }

    private void UpdateStatistics(int newRemainingPoints, int turnScore)
    {
        var matchStatistic = this.GetCurrentPlayerMatchStatistic();
        var setStatistic = this.GetCurrentPlayerSetStatistic();
        var legStatistic = this.GetCurrentPlayerLegStatistic();

        legStatistic.AverageTurnScore = this.CalculateAverageIteratively(legStatistic.AverageTurnScore, turnScore, legStatistic.PlayedTurns);

        var IsOneEighty = turnScore == 180;
        if (IsOneEighty)
        {
            legStatistic.OneEighties += 1;
            setStatistic.OneEighties += 1;
            matchStatistic.OneEighties += 1;
            CurrentPlayer.Statistic.OneEighties++;
        }

        var IsNineDarter = newRemainingPoints == 0 && legStatistic.PlayedTurns == 3;
        if (IsNineDarter)
        {
            legStatistic.IsNineDarter = true;
            setStatistic.Ninedarters += 1;
            matchStatistic.Ninedarters += 1;
            CurrentPlayer.Statistic.Ninedarters++;
        }
    }
}