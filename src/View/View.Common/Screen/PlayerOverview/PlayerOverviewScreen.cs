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

	private IReadOnlyList<Match> PlayedMatches { get; }
	private IReadOnlyList<Match> Matches { get; }

	private Player Player { get; }

	private int SelectedMatch { get; set; }

	public PlayerOverviewScreen(Player player, DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();

		var matchRepository = dependencyContainer.GetMatchRepository();

		this.PlayedMatches = player.PlayedMatches
			.Select(matchId => matchRepository.Read(matchId)!)
			.ToList();

		var playingMatches = player.PlayingMatches
			.Select(matchId => matchRepository.Read(matchId)!)
			.ToList();

		this.Matches = Enumerable.Union(this.PlayedMatches, playingMatches).ToList();

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
				ImGui.Text($"Matches Played: {this.PlayedMatches.Count()}");
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

		ImGui.SetCursorPosX(17);
		ImGui.Text("Name");
		ImGui.SameLine(500);
		ImGui.Text("Done?");
		if (ImGui.BeginChild("Matches", new Vector2(0, 250), true, ImGuiWindowFlags.HorizontalScrollbar))
		{
			var i = 0;
			foreach (var match in this.Matches)
			{
				if (ImGui.Selectable(match.Name, i == this.SelectedMatch))
				{
					this.SelectedMatch = i;
					this.ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(match));
				}
				ImGui.SameLine(500);
				if (match.IsDone)
					ImGui.Text("Yes");
				else
					ImGui.Text("No");

				i++;
			}

			ImGui.EndChild();
		}

		ImGui.NewLine();

		if (ImGuiExtensions.Button("Back", new Vector2(120, 0)))
			this.ScreenNavigator.Pop();
	}
}