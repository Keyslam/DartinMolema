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

    private Match Match { get; }

    private ImGuiExtensions.FireOnce SelectFirstDartInput;

    public MatchInputScreen(Match match, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.RuleEngine = this.DependencyContainer.MakeRuleEngine(match);

        this.DartInputs = new DartInput[this.RuleEngine.ThrowsPerTurn];
        for (int i = 0; i < DartInputs.Length; i++)
            this.DartInputs[i] = new DartInput();

        this.Match = match;

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

        foreach (var player in RuleEngine.Players)
        {
            PlayerThrows(player);
            ImGui.Spacing();
        }

        RemainingPointsModal();
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

            foreach (var player in RuleEngine.Players)
            {
                var matchStatistic = RuleEngine.GetPlayerMatchStatistic(player);
                var setStatistic = RuleEngine.GetPlayerSetStatistic(player);
                var legStatistic = RuleEngine.GetPlayerLegStatistic(player);

                ImGui.TableNextColumn();
                ImGui.Text(player.FullName);

                ImGui.TableNextColumn();
                ImGui.Text("0");

                ImGui.TableNextColumn();
                ImGui.Text(matchStatistic.SetsWon.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(setStatistic.LegsWon.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(legStatistic.RemainingPoints.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(matchStatistic.OneEighties.ToString());
            }

            ImGui.EndTable();
        }
        ImGui.EndDisabled();
    }

    private void CurrentlyPlaying()
    {
        ImGui.Text($"Currently playing: {RuleEngine.CurrentPlayer.FullName}");
    }

    private void DartInputFields()
    {
        var done = false;

        for (int i = 0; i < this.RuleEngine.ThrowsPerTurn; i++)
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
        if (ImGui.BeginTable($"{player.FullName} Throws", (int)this.RuleEngine.ThrowsPerTurn + 2, ImGuiTableFlags.Borders))
        {
            ImGui.TableSetupColumn("Turn");
            for (int i = 0; i < this.RuleEngine.ThrowsPerTurn; i++)
                ImGui.TableSetupColumn($"Dart {i + 1}");
            ImGui.TableSetupColumn($"Remaining");
            ImGui.TableHeadersRow();

            var turns = this.RuleEngine.GetPlayerTurns(player.Id);
            var remainingPoints = this.RuleEngine.ScoreToWin;

            for (int i = 0; i < Math.Max(10, turns.Count); i++)
            {
                var turn = i < turns.Count ? turns[i] : null;

                ImGui.TableNextColumn();
                ImGui.Text($"Turn {i + 1}");

                for (int j = 0; j < RuleEngine.ThrowsPerTurn; j++)
                {
                    Throw? @throw = null;

                    if (turn != null)
                        @throw = turn.Throws[j];

                    ImGui.TableNextColumn();

                    if (@throw != null)
                        ImGui.Text($"{this.MapThrowKindToPrefix(@throw.Kind)}{@throw.ThrownValue.ToString()}");
                }

                ImGui.TableNextColumn();
                if (turn != null)
                {
                    remainingPoints -= turn.Score;
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
            var throws = new List<(ThrowKind throwKind, int value)>();
            foreach (var dartInput in DartInputs)
                throws.Add(new(dartInput.ThrowKind, dartInput.Value));

            var remainingPoints = this.RuleEngine.GetRemainingPointsAfterTurn(throws);

            if (remainingPoints == 0)
            {
                ImGuiExtensions.CenterText("This turn ends the leg.");
                ImGuiExtensions.CenterText($"The winner is {this.RuleEngine.CurrentPlayer.FullName}");
            }
            else
            {
                ImGuiExtensions.CenterText($"The remaining points after this turn is {remainingPoints}.");
            }

            ImGuiExtensions.CenterText("Is this correct?");

            ImGuiExtensions.EndDialogModal("No", "Yes", () =>
            {
                var matchEnded = this.RuleEngine.PlayTurn(throws);

                for (int i = 0; i < DartInputs.Length; i++)
                    this.DartInputs[i] = new DartInput();

                SelectFirstDartInput.MakeActive();

                if (matchEnded)
                {
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