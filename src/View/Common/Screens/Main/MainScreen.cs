using App.Core;
using ImGuiNET;

namespace App.Screens;

public class MainScreen : IScreen
{
    public void Update(ScreenNavigator screenNavigator)
    {
        if (ImGuiExtensions.Button("New Game"))
            screenNavigator.Push(new NewMatchScreen());

        ImGui.Button("Player Overview");
        ImGui.Button("Match Overview");
        ImGui.Button("Import Match");
    }
}