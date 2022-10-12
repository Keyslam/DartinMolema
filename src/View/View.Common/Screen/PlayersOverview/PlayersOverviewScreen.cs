using App.Core;
using App.Models;
using App.Repository;
using System.Numerics;
using ImGuiNET;

namespace App.View;

internal class PlayersOverviewScreen : Screen
{

    private readonly IPlayerRepository playerRepository;
    private IReadOnlyList<Player> players;

    private string searchInput = "";

    private int selectedIndex = 0;

    public PlayersOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.playerRepository = dependencyContainer.GetPlayerRepository();
        this.players = playerRepository.ReadAll();
    }

    public override void Update()
    {
        ImGui.Text("Players Overview");

        ImGuiExtensions.Spacing(5);

        if (ImGui.InputText("Searchbar", ref searchInput, 255))
            searchInput = searchInput.ToLower();

        ImGuiExtensions.Spacing(3);

        if (ImGui.BeginChild("Players", new Vector2(0, 250), true))
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].FullName.ToLower().Contains(searchInput)) continue;

                if (ImGui.Selectable(players[i].FullName, i == selectedIndex))
                {
                    selectedIndex = i;
                    this.ScreenNavigator.Push(DependencyContainer.MakePlayerOverviewScreen(players[i]));
                }
            }

            ImGui.EndChild();
        }

        ImGuiExtensions.Spacing(3);

        if (ImGuiExtensions.Button("Back"))
            this.ScreenNavigator.PopToRoot();

    }

}