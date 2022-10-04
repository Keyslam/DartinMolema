using App.Core;
using App.Models;

namespace App.Screens;

public class PlayerOverviewScreen : IScreen
{
    public PlayerOverviewScreen(Player player)
    {

    }

    public void Update(ScreenNavigator screenNavigator)
    {
        if (ImGuiExtensions.Button("Back"))
            screenNavigator.PopToRoot();
    }
}