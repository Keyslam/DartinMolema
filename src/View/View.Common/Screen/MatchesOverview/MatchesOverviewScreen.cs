using System.Numerics;
using System.Text;
using App.Core;
using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class MatchesOverviewScreen : Screen
{
	private IMatchRepository MatchRepository { get; }
	private IPlayerRepository PlayerRepository { get; }

	private IReadOnlyList<MatchMetadata> MatchMetaDatas { get; }

	private string SearchInput { get; set; } = "";

	private int SelectedIndex { get; set; } = 0;

	public MatchesOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.MatchRepository = dependencyContainer.GetMatchRepository();
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();

		this.MatchMetaDatas = this.MatchRepository.ReadAllMetadata();
	}

	public override void Update()
	{
		ImGui.Text("Matches Overview");

		ImGuiExtensions.Spacing(5);

		var searchInput = this.SearchInput;
		if (ImGui.InputText("Searchbar", ref searchInput, 255))
			SearchInput = SearchInput.ToLower();
		this.SearchInput = searchInput;

		ImGuiExtensions.Spacing(3);

		ImGui.SetCursorPosX(17);
		ImGui.Text("Name");
		ImGui.SameLine(500);
		ImGui.Text("Done?");
		if (ImGui.BeginChild("Matches", new Vector2(0, 250), true, ImGuiWindowFlags.HorizontalScrollbar))
		{
			var i = 0;
			foreach (var matchMetaData in this.MatchMetaDatas)
			{
				if (!matchMetaData.Name.ToLower().Contains(SearchInput)) continue;

				if (ImGui.Selectable(matchMetaData.Name, i == SelectedIndex))
				{
					SelectedIndex = i;
					var match = MatchRepository.Read(matchMetaData.Id)!;

					this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(match));
				}
				ImGui.SameLine(500);
				if (matchMetaData.IsDone)
					ImGui.Text("Yes");
				else
					ImGui.Text("No");

				i++;
			}

			ImGui.EndChild();
		}

		ImGuiExtensions.Spacing(3);

		if (ImGuiExtensions.Button("Back", new Vector2(120, 0)))
			this.ScreenNavigator.PopToRoot();
	}
}