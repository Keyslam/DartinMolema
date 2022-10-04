using App.Core;
using App.Models;

namespace App.Screens;

public class MatchOverviewScreen : IScreen
{
    public MatchOverviewScreen(Match match)
    {

    }

    public void Update(ScreenNavigator screenNavigator)
    {
        if (ImGuiExtensions.Button("Ok"))
            screenNavigator.PopToRoot();
    }
}