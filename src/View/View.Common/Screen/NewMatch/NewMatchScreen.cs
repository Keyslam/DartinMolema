using App.Builders;
using App.Core;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class NewMatchScreen : Screen
{
    private IPlayerRepository PlayerRepository { get; }
    private MatchBuilder MatchBuilder { get; }

    private int SetsToWinInput;
    private int LegsToWinInput;
    private int ScoreToWinInput;

    public NewMatchScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.PlayerRepository = dependencyContainer.GetPlayerRepository();
        this.MatchBuilder = new MatchBuilder();

        var players = this.PlayerRepository.ReadAll();
        foreach (var player in players)
            this.MatchBuilder.AddPlayer(player.Id);

        this.SetsToWinInput = (int)this.MatchBuilder.Result.SetsToWin;
        this.LegsToWinInput = (int)this.MatchBuilder.Result.LegsToWin;
        this.ScoreToWinInput = (int)this.MatchBuilder.Result.ScoreToWin;

    }

    public override void Update()
    {
        if (ImGui.InputInt("Sets to win", ref this.SetsToWinInput))
        {
            this.SetsToWinInput = Math.Clamp(this.SetsToWinInput, 1, 100);
            this.MatchBuilder.SetSetsToWin((uint)this.SetsToWinInput);
        }

        if (ImGui.InputInt("Legs to win", ref this.LegsToWinInput))
        {
            this.LegsToWinInput = Math.Clamp(this.LegsToWinInput, 1, 100);
            this.MatchBuilder.SetLegsToWin((uint)this.LegsToWinInput);
        }

        if (ImGui.InputInt("Score to win", ref this.ScoreToWinInput))
        {
            this.ScoreToWinInput = Math.Clamp(this.ScoreToWinInput, 1, 1000);
            this.MatchBuilder.SetScoreToWin((uint)this.ScoreToWinInput);
        }

        if (ImGuiExtensions.Button("Start"))
        {
            var match = MatchBuilder.Result;
            this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchInputScreen(match));
        }
        ImGui.SameLine();
        if (ImGuiExtensions.Button("Cancel"))
            this.ScreenNavigator.Pop();
    }
}