using System.Text;
using System.Data;
using App.Core;
using App.Models;
using App.View.MatchInput;
using ImGuiNET;
using System.Numerics;

namespace App.View;

internal class MatchInputScreen : Screen
{
	private DartInput[] DartInputs { get; }

	private Match Match { get; }
	private List<Player> Players { get; }

	private ImGuiExtensions.FireOnce SelectFirstDartInput { get; }

	public MatchInputScreen(Match match, DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.DartInputs = new DartInput[match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn];
		for (int i = 0; i < DartInputs.Length; i++)
			this.DartInputs[i] = new DartInput();

		this.Match = match;

		this.Players = this.Match.Players
			.Select(playerId => dependencyContainer.GetPlayerRepository().Read(playerId)!)
			.ToList();

		this.SelectFirstDartInput = new ImGuiExtensions.FireOnce(true);
	}

	public override void Update()
	{
		Layout();
	}

	private void Layout()
	{
		Header();
		ImGuiExtensions.Spacing(5);

		ImGui.Columns(2);

		MatchInfo();
		ImGuiExtensions.Spacing(3);

		MatchStatistics();
		ImGuiExtensions.Spacing(3);

		CurrentlyPlaying();
		ImGuiExtensions.Spacing(3);

		DartInputFields();

		ImGui.NextColumn();

		foreach (var player in this.Players)
		{
			PlayerThrows(player);
			ImGui.Spacing();
		}

		RemainingPointsModal();
	}

	private void Header()
	{
		ImGui.Text($"Match Input : {this.Match.Name}");
	}


	private void MatchInfo()
	{
		ImGui.Text($"First to {this.Match.MatchRules.SetsToWin} sets wins the match");
		ImGui.Text($"First to {this.Match.MatchRules.SetRules.LegsToWin} legs wins the set");
	}

	private int SelectedIndex = 0;

	private void MatchStatistics()
	{
		ImGui.BeginDisabled();
		if (ImGui.BeginTable($"Match Statistics", 6, ImGuiTableFlags.Borders))
		{
			ImGui.TableSetupColumn("Name");
			ImGui.TableSetupColumn("Average");
			ImGui.TableSetupColumn("Sets");
			ImGui.TableSetupColumn("Legs");
			ImGui.TableSetupColumn("Remaining points");
			ImGui.TableSetupColumn("180's");
			ImGui.TableHeadersRow();

			foreach (var player in this.Players)
			{
				var playerIndex = this.Players.IndexOf(player);
				var matchStatistic = this.Match.Statistics[playerIndex];
				var setStatistic = this.Match.CurrentSet.Statistics[playerIndex];
				var legStatistic = this.Match.CurrentSet.CurrentLeg.Statistics[playerIndex];

				var points = this.Match.CurrentSet.CurrentLeg.Points[playerIndex];
				var remainingPoints = this.Match.MatchRules.SetRules.LegRules.TargetScore - points;

				ImGui.TableNextColumn();
				ImGui.Text(player.FullName);

				ImGui.TableNextColumn();
				ImGui.Text(((int)legStatistic.AverageTurnScore).ToString());

				ImGui.TableNextColumn();
				ImGui.Text(matchStatistic.SetsWon.ToString());

				ImGui.TableNextColumn();
				ImGui.Text(setStatistic.LegsWon.ToString());

				ImGui.TableNextColumn();
				ImGui.Text(remainingPoints.ToString());

				ImGui.TableNextColumn();
				ImGui.Text(matchStatistic.OneEighties.ToString());
			}

			ImGui.EndTable();
		}
		ImGui.EndDisabled();
	}

	private void CurrentlyPlaying()
	{
		var playerIndex = this.Match.CurrentSet.CurrentPlayerIndex;
		var player = this.Players[playerIndex];
		ImGui.Text($"Currently playing: {player.FullName}");
	}

	private void DartInputFields()
	{
		var done = false;

		for (int i = 0; i < this.Match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn; i++)
		{
			var dartInput = DartInputs[i];

			var input = dartInput.Input;

			if (i == 0 && SelectFirstDartInput.Consume())
				ImGui.SetKeyboardFocusHere();

			if (ImGui.InputText($"Dart {i + 1}", ref input, 3, ImGuiInputTextFlags.AutoSelectAll))
			{
				dartInput.Input = input;
			}

			if (ImGui.IsItemDeactivated() && ImGui.IsKeyPressed(ImGuiKey.Enter))
				done = true;
		}

		ImGui.Spacing();

		if (ImGuiExtensions.Button("Enter", new Vector2(120, 0)))
			done = true;

		ImGui.SameLine();

		if (ImGuiExtensions.Button("Exit", new Vector2(120, 0)))
			ScreenNavigator.PopToRoot();

		if (done)
			ImGui.OpenPopup("Remaining points");

		ImGui.SameLine();
	}

	private void PlayerThrows(Player player)
	{
		ImGui.Text($"{player.FullName} Throws");
		ImGuiExtensions.Spacing(1);

		ImGui.BeginDisabled();
		if (ImGui.BeginTable($"{player.FullName} Throws", (int)this.Match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn + 3, ImGuiTableFlags.Borders))
		{
			ImGui.TableSetupColumn("Turn");
			for (int i = 0; i < this.Match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn; i++)
				ImGui.TableSetupColumn($"Dart {i + 1}");
			ImGui.TableSetupColumn($"Score");
			ImGui.TableSetupColumn($"Remaining");
			ImGui.TableHeadersRow();

			var playerIndex = this.Players.IndexOf(player);
			var turns = this.Match.CurrentSet.CurrentLeg.Turns[playerIndex];


			var remainingPoints = this.Match.MatchRules.SetRules.LegRules.TargetScore;

			for (int i = 0; i < Math.Max(10, turns.Count); i++)
			{
				var turn = i < turns.Count ? turns[i] : null;

				ImGui.TableNextColumn();
				ImGui.Text($"Turn {i + 1}");

				for (int j = 0; j < this.Match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn; j++)
				{
					Throw? @throw = null;

					if (turn != null)
						@throw = turn.Throws[j];

					ImGui.TableNextColumn();

					if (@throw != null)
						ImGui.Text($"{this.MapThrowKindToPrefix(@throw.Kind)}{@throw.ValueRegion.ToString()}");
				}

				ImGui.TableNextColumn();
				if (turn != null)
				{
					ImGui.Text(turn.AssignedPoints.ToString());
				}

				ImGui.TableNextColumn();
				if (turn != null)
				{
					remainingPoints -= turn.AssignedPoints;
					ImGui.Text(remainingPoints.ToString());
				}
			}

			ImGui.EndTable();
		}
		ImGui.EndDisabled();
	}

	private void RemainingPointsModal()
	{
		if (ImGuiExtensions.BeginDialogModal("Remaining points"))
		{
			var throws = new List<Throw>();
			foreach (var dartInput in DartInputs)
				throws.Add(new Throw(dartInput.Value, dartInput.ThrowKind));
			var turn = new Turn(throws);

			var remainingPoints = this.Match.GetRemainingPointsAfterTurn(turn);

			if (remainingPoints == 0)
			{
				ImGuiExtensions.CenterText("This turn ends the leg.");
				var currentPlayerIndex = this.Match.CurrentSet.CurrentPlayerIndex;
				var player = this.Players[currentPlayerIndex];
				ImGuiExtensions.CenterText($"The winner is {player.FullName}");
			}
			else
			{
				ImGuiExtensions.CenterText($"The remaining points after this turn is {remainingPoints}.");
			}

			ImGuiExtensions.CenterText("Is this correct?");

			ImGuiExtensions.EndDialogModal("No", "Yes", () =>
			{
				this.Match.PlayTurn(turn);

				for (int i = 0; i < DartInputs.Length; i++)
					this.DartInputs[i] = new DartInput();

				SelectFirstDartInput.MakeActive();

				this.DependencyContainer.GetMatchRepository().Save(this.Match);

				if (this.Match.IsDone)
				{
					foreach (var player in this.Players)
					{
						player.PlayMatch(this.Match);
						this.DependencyContainer.GetPlayerRepository().Save(player);
					}

					ScreenNavigator.PopToRoot();
					ScreenNavigator.Push(DependencyContainer.MakeMatchOverviewScreen(this.Match));
				}
			}, () =>
			{
				SelectFirstDartInput.MakeActive();
			});
		}

	}

	private string MapThrowKindToPrefix(ThrowKind throwKind)
	{
		switch (throwKind)
		{
			case ThrowKind.None:
				return "";
			case ThrowKind.Foul:
				return "";
			case ThrowKind.Single:
				return "";
			case ThrowKind.Double:
				return "D";
			case ThrowKind.Triple:
				return "T";
			case ThrowKind.InnerBull:
				return "IB ";
			case ThrowKind.OuterBull:
				return "OB ";
			default:
				return "";
		}
	}
}