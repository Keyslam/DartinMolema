using System.Text;
using App.Core;
using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class PlayerOverviewScreen : Screen
{
    private readonly IPlayerRepository playerRepository;

    private IReadOnlyList<Match> matches;
    private string[] matchTitles;

    private Player player;

    private int selectedMatch;

    public PlayerOverviewScreen(Player player, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.playerRepository = dependencyContainer.GetPlayerRepository();

        this.matches = player.PlayedGames.Select(matchId => dependencyContainer.GetMatchRepository().Read(matchId)!).ToList();

        this.matchTitles = new string[this.matches.Count()];
        for (var i = 0; i < this.matches.Count(); ++i)
            this.matchTitles[i] = MakeMatchTitle(this.matches[i]);

        this.player = player;
        this.selectedMatch = 0;
    }

    public override void Update()
    {
        ImGui.Text($"Player Overview: {player.FullName}");

        ImGuiExtensions.Spacing(5);

        if (ImGui.TreeNodeEx("Statistics", ImGuiTreeNodeFlags.DefaultOpen))
        {
            if (ImGui.TreeNodeEx("Matches", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text($"Matches Played: {this.matches.Count()}");
                ImGui.Text($"Matches Won: {this.player.WonGames.Count}");
                ImGui.Text($"Matches Lost: {this.player.LostGames.Count}");

                ImGui.TreePop();
            }

            if (ImGui.TreeNodeEx("Throws", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text($"9 darters: {this.player.Statistic.Ninedarters}");
                ImGui.Text($"180's: {this.player.Statistic.OneEighties}");

                ImGui.TreePop();
            }

            ImGui.TreePop();
        }

        ImGui.NewLine();

        ImGui.Text("Matches");

        if (ImGui.ListBox("", ref this.selectedMatch, this.matchTitles, this.matchTitles.Count(), 20))
            this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(this.matches[this.selectedMatch]));

        ImGui.NewLine();

        if (ImGuiExtensions.Button("Back"))
            this.ScreenNavigator.Pop();
    }

    private string MakeMatchTitle(Match match)
    {
        var titleBuilder = new StringBuilder();

        for (int i = 0; i < match.Players.Count; i++)
        {
            if (i != 0)
                titleBuilder.Append(" vs ");

            string playerName = this.playerRepository.Read(match.Players[i])?.FullName ?? "Unknown Player";
            titleBuilder.Append(playerName);
        }

        titleBuilder.Append(" | ");
        titleBuilder.Append(match.Date.ToString("dd-MM-yyyy HH:mm"));

        return titleBuilder.ToString();
    }
}