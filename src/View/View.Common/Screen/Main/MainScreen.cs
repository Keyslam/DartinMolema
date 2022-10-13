using System.Numerics;
using App.Core;

namespace App.View;

internal class MainScreen : Screen
{
	public MainScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
	}

	public override void Update()
	{
		if (ImGuiExtensions.Button("New Game", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakeNewMatchScreen());

		if (ImGuiExtensions.Button("Players Overview", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakePlayersOverviewScreen());

		if (ImGuiExtensions.Button("Matches Overview", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchesOverviewScreen());
	}
}