using System.Numerics;
using System.Text;
using App.Core;
using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class PlayerOverviewScreen : Screen
{
	private IPlayerRepository PlayerRepository { get; }

	private IReadOnlyList<Match> Matches { get; }
	private string[] MatchTitles { get; }

	private Player Player { get; }

	private int SelectedMatch { get; set; }

	public PlayerOverviewScreen(Player player, DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();

		var matchRepository = dependencyContainer.GetMatchRepository();

		this.Matches = player.PlayedMatches
			.Select(matchId => matchRepository.Read(matchId)!)
			.ToList();

		this.MatchTitles = new string[this.Matches.Count];
		for (var i = 0; i < this.Matches.Count; i++)
			this.MatchTitles[i] = this.Matches[i].Name;

		this.Player = player;
		this.SelectedMatch = 0;
	}

	public override void Update()
	{
		ImGui.Text($"Player Overview: {Player.FullName} from {Player.Country}");

		ImGuiExtensions.Spacing(5);

		if (ImGui.TreeNodeEx("Statistics", ImGuiTreeNodeFlags.DefaultOpen))
		{
			if (ImGui.TreeNodeEx("Matches", ImGuiTreeNodeFlags.DefaultOpen))
			{
				ImGui.Text($"Matches Played: {this.Matches.Count()}");
				ImGui.Text($"Matches Won: {this.Player.WonMatches.Count}");
				ImGui.Text($"Matches Lost: {this.Player.LostMatches.Count}");

				ImGui.TreePop();
			}

			if (ImGui.TreeNodeEx("Throws", ImGuiTreeNodeFlags.DefaultOpen))
			{
				ImGui.Text($"Average score: {((int)this.Player.Statistic.AverageTurnScore)}");
				ImGui.Text($"9 darters: {this.Player.Statistic.Ninedarters}");
				ImGui.Text($"180's: {this.Player.Statistic.OneEighties}");

				ImGui.TreePop();
			}

			ImGui.TreePop();
		}

		ImGui.NewLine();

		ImGui.Text("Matches");

		var selectedMatch = this.SelectedMatch;
		if (ImGui.ListBox("", ref selectedMatch, this.MatchTitles, this.MatchTitles.Count(), 20))
		{
			this.SelectedMatch = selectedMatch;
			this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(this.Matches[this.SelectedMatch]));
		}

		ImGui.NewLine();

		if (ImGuiExtensions.Button("Back", new Vector2(120, 0)))
			this.ScreenNavigator.Pop();
	}
}