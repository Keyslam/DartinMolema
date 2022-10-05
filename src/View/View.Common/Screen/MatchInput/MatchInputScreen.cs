using System.Text;
using System.Data;
using App.Core;
using App.Models;
using App.View.MatchInput;
using ImGuiNET;
using System.Numerics;
using App.GameRuler;

namespace App.View;

internal class MatchInputScreen : Screen
{
    private RuleEngine RuleEngine { get; }
    private DartInput[] DartInputs { get; }

    public MatchInputScreen(Match match, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.RuleEngine = this.DependencyContainer.MakeRuleEngine(match);

        this.DartInputs = new DartInput[this.RuleEngine.ThrowsPerTurn];
        for (int i = 0; i < DartInputs.Length; i++)
            this.DartInputs[i] = new DartInput();

        this.RuleEngine.StartSet();
        this.RuleEngine.StartLeg();
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

        foreach (var player in RuleEngine.Players)
        {
            PlayerThrows(player);
            ImGui.Spacing();
        }

        RemainingPointsModal();
        EndOfLegModal();
    }

    private void Header()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Match Input : ");
        for (int i = 0; i < RuleEngine.Players.Count; i++)
        {
            var player = RuleEngine.Players[i];

            if (i != 0)
                stringBuilder.Append(" vs ");

            stringBuilder.Append(player.FullName);
        }

        ImGui.Text(stringBuilder.ToString());
    }


    private void MatchInfo()
    {
        ImGui.Text($"First to {this.RuleEngine.SetsToWin} sets wins the match");
        ImGui.Text($"First to {this.RuleEngine.LegsToWin} legs wins the set");
    }

    private int SelectedIndex = 0;

    private void MatchStatistics()
    {
        for (int i = 0; i < 10; i++)
        {
            if (ImGui.Selectable(i.ToString(), i == SelectedIndex))
                SelectedIndex = i;
        }

        ImGui.PushAllowKeyboardFocus(false);
        if (ImGui.BeginTable($"Match Statistics", 6, ImGuiTableFlags.Borders))
        {
            ImGui.TableSetupColumn("Name");
            ImGui.TableSetupColumn("Average");
            ImGui.TableSetupColumn("Sets");
            ImGui.TableSetupColumn("Legs");
            ImGui.TableSetupColumn("Remaining points");
            ImGui.TableSetupColumn("180's");
            ImGui.TableHeadersRow();

            foreach (var player in RuleEngine.Players)
            {
                var statistic = RuleEngine.PlayerStatistics[player];

                ImGui.TableNextColumn();
                ImGui.Text(player.FullName);

                ImGui.TableNextColumn();
                ImGui.Text(statistic.AveragePoints.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(statistic.SetsWon.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(statistic.LegsWon.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(statistic.RemainingPoints.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(statistic.Tripledarts.ToString());
            }

            ImGui.EndTable();
        }
        ImGui.PopAllowKeyboardFocus();
    }

    private void CurrentlyPlaying()
    {
        ImGui.Text($"Currently playing: {RuleEngine.CurrentPlayer.FullName}");
    }

    private void DartInputFields()
    {
        for (int i = 0; i < this.RuleEngine.ThrowsPerTurn; i++)
        {
            var dartInput = DartInputs[i];

            var input = dartInput.Input;

            if (ImGui.InputText($"Dart {i + 1}", ref input, 3))
                dartInput.Input = input;
        }

        ImGui.Spacing();

        if (ImGuiExtensions.Button("Enter"))
        {
            var throws = new List<(ThrowKind throwKind, uint value)>();
            foreach (var dartInput in DartInputs)
                throws.Add(new(dartInput.ThrowKind, dartInput.Value));

            this.RuleEngine.PlayTurn(throws);

            ImGui.OpenPopup("Remaining points");

            for (int i = 0; i < DartInputs.Length; i++)
                this.DartInputs[i] = new DartInput();
        }
        ImGui.SameLine();
    }

    private void PlayerThrows(Player player)
    {
        ImGui.Text($"{player.FullName} Throws");
        ImGuiExtensions.Spacing(1);

        if (ImGui.BeginTable($"{player.FullName} Throws", (int)this.RuleEngine.ThrowsPerTurn + 2, ImGuiTableFlags.Borders))
        {
            ImGui.TableSetupColumn("Turn");
            for (int i = 0; i < this.RuleEngine.ThrowsPerTurn; i++)
                ImGui.TableSetupColumn($"Dart {i + 1}");
            ImGui.TableSetupColumn($"Remaining");
            ImGui.TableHeadersRow();

            var turns = this.RuleEngine.GetCurrentTurns(player.Id);
            for (int i = 0; i < Math.Max(10, turns.Count); i++)
            {
                var turn = i < turns.Count ? turns[i] : null;

                ImGui.TableNextColumn();
                ImGui.Text($"Turn {i + 1}");

                for (int j = 0; j < RuleEngine.ThrowsPerTurn; j++)
                {
                    Throw? throww = null;

                    if (turn != null)
                        throww = turn.Throws[j];

                    ImGui.TableNextColumn();

                    if (throww != null)
                        ImGui.Text(throww.ThrownValue.ToString());
                }

                ImGui.TableNextColumn();
                if (turn != null)
                    ImGui.Text(this.RuleEngine.ScoreToWin.ToString());
            }

            ImGui.EndTable();
        }
    }

    private void RemainingPointsModal()
    {
        if (ImGuiExtensions.BeginDialogModal("Remaining points"))
        {

            ImGuiExtensions.CenterText($"The remaining points after this turn is TODO.");
            ImGuiExtensions.CenterText("Is this correct?");

            ImGuiExtensions.EndDialogModal("No", "Yes", () =>
            {

            }, () =>
            {

            });
        }

    }

    private void EndOfLegModal()
    {
        if (ImGuiExtensions.BeginDialogModal("End of turn"))
        {
            ImGuiExtensions.CenterText("This turn ends the leg.");
            ImGuiExtensions.CenterText($"The winner is TODO");
            ImGuiExtensions.CenterText("Is this correct?");

            ImGuiExtensions.EndDialogModal("No", "Yes", () =>
            {

            }, () =>
            {

            });
        }
    }
}