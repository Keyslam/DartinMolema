using App.Core;
using App.Models;
using App.Repository;
using System.Numerics;
using ImGuiNET;

namespace App.View;

internal class PlayersOverviewScreen : Screen
{
	private IPlayerRepository PlayerRepository { get; }
	private IReadOnlyList<Player> Players { get; }

	private string SearchInput { get; set; } = "";

	private int SelectedIndex { get; set; } = 0;

	public PlayersOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();
		this.Players = PlayerRepository.ReadAll();
	}

	public override void Update()
	{
		ImGui.Text("Players Overview");

		ImGuiExtensions.Spacing(5);

		var searchInput = this.SearchInput;
		if (ImGui.InputText("Searchbar", ref searchInput, 255))
			SearchInput = searchInput.ToLower();

		ImGuiExtensions.Spacing(3);

		if (ImGui.BeginChild("Players", new Vector2(0, 250), true))
		{
			for (int i = 0; i < Players.Count; i++)
			{
				if (!Players[i].FullName.ToLower().Contains(SearchInput)) continue;

				if (ImGui.Selectable(Players[i].FullName, i == SelectedIndex))
				{
					SelectedIndex = i;
					this.ScreenNavigator.Push(DependencyContainer.MakePlayerOverviewScreen(Players[i]));
				}
			}

			ImGui.EndChild();
		}

		ImGuiExtensions.Spacing(3);

		if (ImGuiExtensions.Button("Back"))
			this.ScreenNavigator.PopToRoot();

	}

}