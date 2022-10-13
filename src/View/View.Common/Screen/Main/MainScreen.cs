using System.Numerics;
using App.Core;
using ImGuiNET;

namespace App.View;

internal class MainScreen : Screen
{
	private ImGuiExtensions.FireOnce SelectNewGame { get; }

	public MainScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.SelectNewGame = new ImGuiExtensions.FireOnce(true);
	}

	public override void Update()
	{
		if (SelectNewGame.Consume())
			ImGui.SetKeyboardFocusHere();
		if (ImGuiExtensions.Button("New Game", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakeNewMatchScreen());

		if (ImGuiExtensions.Button("Players Overview", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakePlayersOverviewScreen());

		if (ImGuiExtensions.Button("Matches Overview", new Vector2(120, 0)))
			this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchesOverviewScreen());
	}
}