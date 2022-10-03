using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.Screens;

public class NewGameScreen : IScreen
{
    private RuleEngine RuleEngine { get; }
    private PlayerRepository PlayerRepository { get; }

    private int SetsToWinInput;
    private int LegsToWinInput;
    private int ScoreToWinInput;

    public NewGameScreen()
    {
        var matchFactory = new MatchFactory();
        var match = matchFactory.CreateDefault();


        this.PlayerRepository = new PlayerRepository();

        var players = this.PlayerRepository.ReadAll();
        foreach (var player in players)
        {
            match.Players.Add(player.Id);
        }

        this.RuleEngine = new RuleEngine(match);

        this.SetsToWinInput = (int)this.RuleEngine.SetsToWin;
        this.LegsToWinInput = (int)this.RuleEngine.LegsToWin;
        this.ScoreToWinInput = (int)this.RuleEngine.ScoreToWin;

    }

    public void Update(ScreenNavigator screenNavigator)
    {
        if (ImGui.InputInt("Sets to win", ref this.SetsToWinInput))
        {
            this.SetsToWinInput = Math.Clamp(this.SetsToWinInput, 1, 100);
            this.RuleEngine.SetsToWin = (uint)SetsToWinInput;
        }

        if (ImGui.InputInt("Legs to win", ref this.LegsToWinInput))
        {
            this.LegsToWinInput = Math.Clamp(this.LegsToWinInput, 1, 100);
            this.RuleEngine.LegsToWin = (uint)LegsToWinInput;
        }

        if (ImGui.InputInt("Score to win", ref this.ScoreToWinInput))
        {
            this.ScoreToWinInput = Math.Clamp(this.ScoreToWinInput, 1, 1000);
            this.RuleEngine.ScoreToWin = (uint)ScoreToWinInput;
        }

        if (ImGuiExtensions.Button("Start"))
        {
            screenNavigator.Push(new GameInput(this.RuleEngine));
        }
        ImGui.SameLine();
        if (ImGuiExtensions.Button("Cancel"))
            screenNavigator.Pop();
    }
}