using System.Numerics;
using System.Text;
using App.Core;
using App.Models;
using App.Repository;
using ImGuiNET;

namespace App.View;

internal class MatchOverviewScreen : Screen
{
    private IMatchRepository MatchRepository { get; }
    private IPlayerRepository PlayerRepository { get; }
    private Match Match { get; }

    private List<Player> Players { get; }
    private Dictionary<Guid, Player> PlayersMap { get; }

    private Player? Winner { get; }

    private int SelectedSet { get; set; }

    private List<float[]> PlayerAHistogramDatas;
    private List<float[]> PlayerBHistogramDatas;

    private const int MaxTurnScore = 180;
    private const int ScoreStepSize = 20;
    private const float DiagramOffset = 30.0f;
    private const float DiagramHeight = 300.0f;
    private const float VerticalLabelHeight = 16.0f;

    public MatchOverviewScreen(Match match, DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.MatchRepository = dependencyContainer.GetMatchRepository();
        this.PlayerRepository = dependencyContainer.GetPlayerRepository();
        this.Match = match;

        this.Players = new List<Player>();
        this.PlayersMap = new Dictionary<Guid, Player>();
        foreach (var playerId in this.Match.Players)
        {
            var player = PlayerRepository.Read(playerId)!;

            this.Players.Add(player);
            this.PlayersMap.Add(playerId, player);
        }

        if (this.Match.WinnerId != Guid.Empty)
            this.Winner = PlayerRepository.Read(Match.WinnerId);

        PlayerAHistogramDatas = new List<float[]>();
        PlayerBHistogramDatas = new List<float[]>();

        foreach (var set in this.Match.Sets)
        {
            var playerAHistogramData = new float[set.Legs.Count * 3 - 1];
            var playerBHistogramData = new float[set.Legs.Count * 3 - 1];

            for (int i = 0; i < set.Legs.Count; i++)
            {
                var leg = set.Legs[i];

                var playerAAverage = leg.Statistics[this.Match.Players[0]].AverageTurnScore;
                var playerBAverage = leg.Statistics[this.Match.Players[1]].AverageTurnScore;

                playerAHistogramData[i * 3 + 0] = playerAAverage;
                playerBHistogramData[i * 3 + 1] = playerBAverage;
            }

            PlayerAHistogramDatas.Add(playerAHistogramData);
            PlayerBHistogramDatas.Add(playerBHistogramData);
        }

        this.SelectedSet = 0;
    }

    public override void Update()
    {
        ImGui.Text("Match Overview");
        ImGuiExtensions.Spacing(5);

        ImGui.Text($"First to {Match.SetsToWin} sets wins the match");
        ImGui.Text($"First to {Match.LegsToWin} legs wins the match");
        ImGuiExtensions.Spacing(3);

        if (this.Winner != null)
            ImGui.Text($"Winner: {this.Winner.FullName}");
        else
            ImGui.Text("Match still in progress");

        if (ImGui.BeginCombo("Sets", $"Set {this.SelectedSet + 1}"))
        {
            for (int i = 0; i < this.Match.Sets.Count; i++)
            {
                var set = this.Match.Sets[i];
                if (ImGui.Selectable($"Set {i + 1}", this.SelectedSet == i))
                    this.SelectedSet = i;
            }
            ImGui.EndCombo();
        }

        ImGuiExtensions.Spacing(3);

        ImGui.Columns(2);

        ImGui.Text("Match Specials");
        if (ImGui.BeginTable("Match Specials", 4))
        {
            ImGui.TableSetupColumn("Player");
            ImGui.TableSetupColumn("180's");
            ImGui.TableSetupColumn("9 Darters");
            ImGui.TableSetupColumn("Average Score");
            ImGui.TableHeadersRow();

            foreach (var player in this.Players)
            {
                ImGui.TableNextColumn();
                ImGui.Text(player.FullName);
                ImGui.TableNextColumn();
                ImGui.Text(player.Statistic.OneEighties.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(player.Statistic.Ninedarters.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(player.Statistic.AverageTurnScore.ToString());
            }
            ImGui.EndTable();
        }

        ImGui.NextColumn();

        ImGui.Text("Set Specials");
        if (ImGui.BeginTable("Set Specials", 4))
        {
            ImGui.TableSetupColumn("Player");
            ImGui.TableSetupColumn("180's");
            ImGui.TableSetupColumn("9 Darters");
            ImGui.TableSetupColumn("Average Score");
            ImGui.TableHeadersRow();

            foreach (var player in this.Players)
            {
                ImGui.TableNextColumn();
                ImGui.Text(player.FullName);
                ImGui.TableNextColumn();
                ImGui.Text(this.Match.Sets[this.SelectedSet].Statistics[player.Id].OneEighties.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(this.Match.Sets[this.SelectedSet].Statistics[player.Id].Ninedarters.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(this.Match.Sets[this.SelectedSet].Statistics[player.Id].AverageScore.ToString());
            }
            ImGui.EndTable();
        }

        ImGui.Columns();

        ImGuiExtensions.Spacing(3);

        ImGui.Columns(2);

        if (this.Match.Sets != null)
        {
            ImGui.Text("Legs");
            var selectedSet = this.Match.Sets[this.SelectedSet];
            if (ImGui.BeginTable("Legs", 3))
            {
                ImGui.TableSetupColumn("Leg");
                ImGui.TableSetupColumn("Winner");
                ImGui.TableSetupColumn("Aantal turns");
                ImGui.TableHeadersRow();

                for (int i = 0; i < selectedSet.Legs.Count; i++)
                {
                    var leg = selectedSet.Legs[i];

                    ImGui.TableNextColumn();
                    ImGui.Text($"{i + 1}");

                    ImGui.TableNextColumn();
                    if (leg.WinnerId != Guid.Empty)
                    {
                        var winnerName = this.PlayersMap[leg.WinnerId].FullName;
                        ImGui.Text($"{winnerName}");
                        ImGui.TableNextColumn();
                        ImGui.Text($"{leg.Turns[leg.WinnerId].Count()}");
                    }
                    else
                    {
                        ImGui.Text("-");
                        ImGui.TableNextColumn();
                        ImGui.Text("-");
                    }

                }

                ImGui.EndTable();
            }
        }

        ImGui.NextColumn();

        var currentPos = ImGui.GetCursorPos();
        var width = ImGui.GetContentRegionAvail().X;

        for (int i = 0; i < MaxTurnScore / (ScoreStepSize / 2); i++)
        {
            var y = (DiagramHeight / (MaxTurnScore / (ScoreStepSize / 2))) * i;
            var start = currentPos + new Vector2(DiagramOffset, y);
            var end = currentPos + new Vector2(width, y);

            if (i % 2 == 1)
            {
                start = start - new Vector2(DiagramOffset, 0);
            }

            ImGui.GetWindowDrawList().AddLine(start, end, Color(255, 255, 255, 40));
        }

        ImGui.SetCursorPos(currentPos + new Vector2(DiagramOffset, 0));

        ImGui.BeginDisabled();

        ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(1f, 1f, 1f, 0.1f));
        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, new System.Numerics.Vector4(1f, 0f, 0f, 1f));
        ImGui.PushStyleColor(ImGuiCol.TextDisabled, 0);

        ImGui.PlotHistogram("##playerAHistogram", ref PlayerAHistogramDatas[this.SelectedSet][0], PlayerAHistogramDatas[this.SelectedSet].Length, 0, "", 0, MaxTurnScore, new Vector2(width - DiagramOffset, DiagramHeight));

        ImGui.PopStyleColor(2);

        ImGui.SetCursorPos(currentPos + new Vector2(DiagramOffset, 0));

        ImGui.PushStyleColor(ImGuiCol.FrameBg, 0);

        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, new System.Numerics.Vector4(0f, 0f, 1f, 1f));

        ImGui.PlotHistogram("##playerBHistogram", ref PlayerBHistogramDatas[this.SelectedSet][0], PlayerBHistogramDatas[this.SelectedSet].Length, 0, "Average turn score per leg (score x leg)", 0, MaxTurnScore, new Vector2(width - DiagramOffset, DiagramHeight));

        ImGui.PopStyleColor(3);

        ImGui.EndDisabled();

        // Horizontal labels
        var numberOfLegs = this.Match.Sets![this.SelectedSet].Legs.Count();
        for (int i = 0; i < numberOfLegs; i++)
        {
            var horLabel = $"{i + 1}";
            var labelWidth = ImGui.CalcTextSize(horLabel).X;

            var horStepSize = (width - DiagramOffset) / PlayerAHistogramDatas[this.SelectedSet].Length;

            // Offset of first bar from start (4), every 3 bars there should be a new label, and we should skip 1 bar (i * 3 + 1), center text (labelWidth / 2)
            ImGui.SetCursorPos(currentPos + new Vector2(DiagramOffset + 4f + horStepSize * (i * 3 + 1) - labelWidth / 2f - i * 2, DiagramHeight + 2f));
            ImGui.Text(horLabel);
        }

        // Vertical labels
        for (int i = 0; i < MaxTurnScore / ScoreStepSize + 1; i++)
        {
            ImGui.SetCursorPos(currentPos + new Vector2(0, (DiagramHeight / (MaxTurnScore / ScoreStepSize)) * i - (VerticalLabelHeight / 2)));

            var label = (MaxTurnScore - (i * ScoreStepSize)).ToString();
            ImGuiExtensions.RightAlignText(label, DiagramOffset - 5.0f);
        }

        ImGui.Columns();

        if (ImGuiExtensions.Button("Back"))
            this.ScreenNavigator.PopToRoot();

        ImGui.SameLine();

        if (ImGuiExtensions.Button("Export"))
        {

        }
    }

    public static uint Color(byte r, byte g, byte b, byte a) { uint ret = a; ret <<= 8; ret += b; ret <<= 8; ret += g; ret <<= 8; ret += r; return ret; }
}