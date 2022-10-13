using App.Core;
using App.Models;

namespace App.View;

internal class MatchOverviewScreen : Screen
{
    public MatchOverviewScreen(Match match, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {

    }

    public override void Update()
    {
        if (ImGuiExtensions.Button("Ok"))
            this.ScreenNavigator.PopToRoot();
    }
}