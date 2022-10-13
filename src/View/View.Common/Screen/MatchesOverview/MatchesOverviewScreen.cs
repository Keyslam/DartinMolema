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

	private IReadOnlyList<Match> Matches { get; }
	private Dictionary<Match, string> MatchTitles { get; }

	private string SearchInput { get; set; } = "";

	private int SelectedIndex { get; set; } = 0;

	public MatchesOverviewScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.MatchRepository = dependencyContainer.GetMatchRepository();
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();

		this.Matches = MatchRepository.ReadAll();
		this.MatchTitles = new Dictionary<Match, string>();
		foreach (var match in Matches)
			this.MatchTitles[match] = MakeMatchTitle(match);
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
			for (int i = 0; i < Matches.Count; i++)
			{
				string matchTitle = Matches[i].Name;

				if (!matchTitle.ToLower().Contains(SearchInput)) continue;

				if (ImGui.Selectable(matchTitle, i == SelectedIndex))
				{
					SelectedIndex = i;
					this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(Matches[i]));
				}
			}

			ImGui.EndChild();
		}

		ImGuiExtensions.Spacing(3);

		if (ImGuiExtensions.Button("Back", new Vector2(120, 0)))
			this.ScreenNavigator.PopToRoot();
	}

	private string MakeMatchTitle(Match match)
	{
		var titleBuilder = new StringBuilder();

		for (int i = 0; i < match.Players.Count; i++)
		{
			if (i != 0)
				titleBuilder.Append(" vs ");

			string playerName = this.PlayerRepository.Read(match.Players[i])?.FullName ?? "Unknown Player";
			titleBuilder.Append(playerName);
		}

		titleBuilder.Append(" | ");
		titleBuilder.Append(match.Date.ToString("dd-MM-yyyy HH:mm"));

		return titleBuilder.ToString();
	}

}