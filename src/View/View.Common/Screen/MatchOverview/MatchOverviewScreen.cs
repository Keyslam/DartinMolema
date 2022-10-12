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
                // ImGui.Text(this.Match.Sets[this.SelectedSet].Statistics[player.Id].AverageTurnScore.ToString());
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
                        ImGui.Text("NO WINNER");
                        ImGui.TableNextColumn();
                        ImGui.Text("NO WINNER");
                    }

                }

                ImGui.EndTable();
            }
        }

        ImGui.NextColumn();



        ImGui.Columns();

        if (ImGuiExtensions.Button("Ok"))
            this.ScreenNavigator.PopToRoot();


    }
}