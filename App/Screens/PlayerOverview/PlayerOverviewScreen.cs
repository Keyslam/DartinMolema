using App.Core;
using App.Models;

namespace App.Screens;

// User stories:
// M12. Gegevens inzien
// C2. Speciale situaties signaleren
// C3. Grafieken

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