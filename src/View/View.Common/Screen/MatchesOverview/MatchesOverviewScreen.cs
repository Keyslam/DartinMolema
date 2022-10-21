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

	private Dictionary<Guid, string> MatchTitles { get; }

	private string SearchInput { get; set; } = "";

	private int SelectedIndex { get; set; } = 0;

	public MatchesOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.MatchRepository = dependencyContainer.GetMatchRepository();
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();

		var matchNames = this.MatchRepository.ReadAllNames();
		this.MatchTitles = new Dictionary<Guid, string>();
		foreach (var (id, name) in matchNames)
		{
			this.MatchTitles[id] = name;
		}
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

		if (ImGui.BeginChild("Matches", new Vector2(0, 250), true, ImGuiWindowFlags.HorizontalScrollbar))
		{
			var i = 0;
			foreach (var (id, name) in this.MatchTitles)
			{
				if (!name.ToLower().Contains(SearchInput)) continue;

				if (ImGui.Selectable(name, i == SelectedIndex))
				{
					SelectedIndex = i;
					var match = MatchRepository.Read(id)!;
					this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(match));
				}

				i++;
			}

			ImGui.EndChild();
		}

		ImGuiExtensions.Spacing(3);

		if (ImGuiExtensions.Button("Back", new Vector2(120, 0)))
			this.ScreenNavigator.PopToRoot();
	}
}