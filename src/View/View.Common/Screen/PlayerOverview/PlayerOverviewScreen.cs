using App.Core;
using App.Models;

namespace App.View;

internal class PlayerOverviewScreen : Screen
{
    public PlayerOverviewScreen(Player player, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {

    }

    public override void Update()
    {
        if (ImGuiExtensions.Button("Back"))
            this.ScreenNavigator.Pop();
    }
}