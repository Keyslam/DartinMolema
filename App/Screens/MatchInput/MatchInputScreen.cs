using App.Core;
using App.Models;
using App.Screens.MatchInput;
using ImGuiNET;

namespace App.Screens;

public class MatchInputScreen : IScreen
{
    private RuleEngine RuleEngine;
    private DartInput[] DartInputs;

    private bool modalOpen = false;

    public MatchInputScreen(Match match)
    {
        this.RuleEngine = new RuleEngine(match);

        this.DartInputs = new DartInput[this.RuleEngine.ThrowsPerTurn];
        for (int i = 0; i < DartInputs.Length; i++)
            this.DartInputs[i] = new DartInput();

        this.RuleEngine.StartSet();
        this.RuleEngine.StartLeg();
    }

    public void Update(ScreenNavigator screenNavigator)
    {
        ImGui.Text($"Player 1: {this.RuleEngine.Players[0].FullName}");
        ImGui.Text($"Player 2: {this.RuleEngine.Players[1].FullName}");
        ImGui.Spacing();
        ImGui.Text($"Current player turn: {this.RuleEngine.Players[0].FullName}");
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        for (int i = 0; i < this.RuleEngine.ThrowsPerTurn; i++)
        {
            var disabled = i > 0 && !DartInputs[i - 1].IsValid;
            var dartInput = DartInputs[i];

            if (disabled)
            {
                ImGui.BeginDisabled();
                dartInput.Input = string.Empty;
            }

            var input = dartInput.Input;

            if (ImGui.InputText($"Dart {i + 1}", ref input, 3))
                dartInput.Input = input;

            if (disabled)
                ImGui.EndDisabled();
        }

        ImGui.Spacing();

        if (ImGuiExtensions.Button("Enter"))
        {
            var throws = new List<(ThrowKind throwKind, uint value)>();
            foreach (var dartInput in DartInputs)
            {
                throws.Add(new(dartInput.ThrowKind, dartInput.Value));
            }
            this.RuleEngine.PlayTurn(throws);

            // ImGui.OpenPopup("Epic");
            // modalOpen = true;

            for (int i = 0; i < DartInputs.Length; i++)
                this.DartInputs[i] = new DartInput();
        }
        ImGui.SameLine();

        if (ImGuiExtensions.Button("Clear"))
        {
            for (int i = 0; i < DartInputs.Length; i++)
                this.DartInputs[i] = new DartInput();
        }

        if (ImGui.BeginPopupModal("Epic", ref modalOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove))
        {
            ImGui.Text("Nice");

            if (ImGuiExtensions.Button("Ok"))
            {
                modalOpen = false;
            }
            ImGui.EndPopup();
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        for (int i = 0; i < this.RuleEngine.Players.Count; i++)
        {
            var player = this.RuleEngine.Players[i];

            if (ImGui.BeginTable($"Player {i} table", (int)this.RuleEngine.ThrowsPerTurn + 2, ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("Turn");
                for (int j = 0; j < this.RuleEngine.ThrowsPerTurn; j++)
                    ImGui.TableSetupColumn($"Dart {j + 1}");
                ImGui.TableSetupColumn($"Remaining");
                ImGui.TableHeadersRow();

                var turns = this.RuleEngine.GetCurrentTurns(player.Id);
                for (int j = 0; j < turns.Count; j++)
                {
                    var turn = turns[j];

                    ImGui.TableNextColumn();
                    ImGui.Text($"Turn {j + 1}");

                    foreach (var throww in turn.Throws)
                    {
                        ImGui.TableNextColumn();
                        ImGui.Text($"{throww.ThrownValue}");
                    }

                    ImGui.TableNextColumn();
                    ImGui.Text(this.RuleEngine.ScoreToWin.ToString());
                }


                ImGui.EndTable();
            }

            // if (i != this.RuleEngine.Players.Count - 1)
            ImGui.SameLine();
        }
    }

    private bool AreDartInputsValid()
    {
        return false;
    }

}