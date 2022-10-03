using App.Core;
using App.Models;

namespace App.Screens;

// User stories:
// M2. Match informatie einde
// S1. Match rapportage tonen
// C1. Match data exporteren
// C2. Speciale situaties signaleren
// C3. Grafieken

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