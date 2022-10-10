using App.Core;
using App.View;
using ImGuiNET;

namespace App.View;

internal class MainScreen : Screen
{
    public MainScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
    }

    public override void Update()
    {
        if (ImGuiExtensions.Button("New Game"))
            this.ScreenNavigator.Push(this.DependencyContainer.MakeNewMatchScreen());

        if (ImGuiExtensions.Button("Players Overview"))
            this.ScreenNavigator.Push(this.DependencyContainer.MakePlayersOverviewScreen());

        if (ImGuiExtensions.Button("Matches Overview"))
            this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchesOverviewScreen());

        ImGui.Button("Match Overview");
        ImGui.Button("Import Match");
    }
}