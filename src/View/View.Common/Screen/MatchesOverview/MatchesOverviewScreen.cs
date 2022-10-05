using System.Numerics;
using System.Text;
using App.Core;
using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class MatchesOverviewScreen : Screen
{

    private readonly IMatchRepository matchRepository;
    private readonly IPlayerRepository playerRepository;

    private IReadOnlyList<Match> matches;
    private Dictionary<Match, string> matchTitles;

    private string searchInput = "";

    private int selectedIndex = 0;

    public MatchesOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.matchRepository = dependencyContainer.GetMatchRepository();
        this.playerRepository = dependencyContainer.GetPlayerRepository();

        this.matches = matchRepository.ReadAll();
        this.matchTitles = new Dictionary<Match, string>();
        foreach (var match in matches)
            this.matchTitles[match] = MakeMatchTitle(match);
    }

    public override void Update()
    {
        ImGui.Text("Matches Overview");

        ImGuiExtensions.Spacing(5);

        if (ImGui.InputText("Searchbar", ref searchInput, 255))
            searchInput = searchInput.ToLower();

        ImGuiExtensions.Spacing(3);

        if (ImGui.BeginChild("Matches", new Vector2(0, 250), true, ImGuiWindowFlags.HorizontalScrollbar))
        {
            for (int i = 0; i < matches.Count; i++)
            {
                string matchTitle = matchTitles[matches[i]];

                if (!matchTitle.ToLower().Contains(searchInput)) continue;

                if (ImGui.Selectable(matchTitle, i == selectedIndex))
                {
                    selectedIndex = i;
                    this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(matches[i]));
                }
            }

            ImGui.EndChild();
        }

        ImGuiExtensions.Spacing(3);

        if (ImGuiExtensions.Button("Back"))
            this.ScreenNavigator.PopToRoot();

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