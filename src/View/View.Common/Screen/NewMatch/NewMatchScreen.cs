using App.Builders;
using App.Core;
using App.Repository;
using ImGuiNET;
using App.Models;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace App.View;

internal class NewMatchScreen : Screen
{
    private IPlayerRepository PlayerRepository { get; }
    private MatchBuilder MatchBuilder { get; }

    private IReadOnlyList<Player> players;

    private Player[] selectedPlayers;

    private string playerName;
    private int addPlayerIndex;

    private int SetsToWinInput;
    private int LegsToWinInput;
    private int ScoreToWinInput;

    private int dayInput;
    private int monthInput;
    private int yearInput;
    private int hourInput;
    private int minuteInput;

    public NewMatchScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
    {
        this.PlayerRepository = dependencyContainer.GetPlayerRepository();
        this.MatchBuilder = new MatchBuilder();

        this.players = this.PlayerRepository.ReadAll().ToList().OrderBy(x => x.FullName).ToList();

        this.selectedPlayers = new Player[2];

        this.playerName = String.Empty;

        this.SetsToWinInput = (int)this.MatchBuilder.SetsToWin;
        this.LegsToWinInput = (int)this.MatchBuilder.LegsToWin;
        this.ScoreToWinInput = (int)this.MatchBuilder.ScoreToWin;

        this.dayInput = this.MatchBuilder.Date.Day;
        this.monthInput = this.MatchBuilder.Date.Month;
        this.yearInput = this.MatchBuilder.Date.Year;
        this.hourInput = this.MatchBuilder.Date.Hour;
        this.minuteInput = this.MatchBuilder.Date.Minute;
    }

    public override void Update()
    {
        DateTimeSelector();

        PlayerSelector("Player One", 0);
        PlayerSelector("Player Two", 1);

        bool openAddPlayer = true;
        if (ImGui.BeginPopupModal("Add Player", ref openAddPlayer, ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.InputText("##Enter Name", ref this.playerName, 255);
            ImGui.SameLine(0, 20);
            ImGui.Text("Enter Name");

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            this.playerName = regex.Replace(this.playerName.Trim(), " ");
            this.playerName = Regex.Replace(this.playerName, @"^(?<cap>\w)|\b(?<cap>\w)(?=\w*$)", m => m.Groups["cap"].Value.ToUpper());

            if (ImGui.IsKeyPressed(ImGuiKey.Escape))
                ImGui.CloseCurrentPopup();

            bool nameInvalid = String.IsNullOrEmpty(this.playerName) || this.players.ToList().Any(x => x.FullName.ToLower() == this.playerName.ToLower());

            if (nameInvalid)
                ImGui.BeginDisabled();

            if (ImGuiExtensions.Button("Save"))
            {
                Player player = new Player();
                player.Id = Guid.NewGuid();
                player.FullName = this.playerName;

                if (!this.players.ToList().Any(x => x.FullName == player.FullName))
                {
                    this.PlayerRepository.Save(player);
                    this.players = this.PlayerRepository.ReadAll().ToList().OrderBy(x => x.FullName).ToList();
                    SetSelectedPlayer(addPlayerIndex, player);
                    ImGui.CloseCurrentPopup();
                }
            }

            if (nameInvalid)
            {
                if (!String.IsNullOrEmpty(this.playerName))
                {
                    var player = this.players.ToList().Find(x => x.FullName.ToLower() == this.playerName.ToLower());
                    var alreadySelected = player != null && this.selectedPlayers.Any(x => x != null && x.Id == player.Id);

                    ImGui.SameLine();
                    ImGui.Text("Name already exists" + (alreadySelected ? " and selected" : ""));
                    ImGui.EndDisabled();

                    if (player != null && !alreadySelected)
                    {
                        ImGui.SameLine();
                        if (ImGuiExtensions.Button("Select existing"))
                        {
                            SetSelectedPlayer(addPlayerIndex, player);
                            ImGui.CloseCurrentPopup();
                        }
                    }
                }
                ImGui.EndDisabled();
            }
            ImGui.EndPopup();
        }

        if (ImGui.InputInt("Sets to win", ref this.SetsToWinInput))
        {
            this.SetsToWinInput = Math.Clamp(this.SetsToWinInput, 1, 100);
            this.MatchBuilder.SetsToWin = this.SetsToWinInput;
        }

        if (ImGui.InputInt("Legs to win", ref this.LegsToWinInput))
        {
            this.LegsToWinInput = Math.Clamp(this.LegsToWinInput, 1, 100);
            this.MatchBuilder.LegsToWin = this.LegsToWinInput;
        }

        if (!InputValid())
            ImGui.BeginDisabled();
        if (ImGuiExtensions.Button("Start"))
        {
            StartMatch();
        }
        if (!InputValid())
            ImGui.EndDisabled();

        ImGui.SameLine();
        if (ImGuiExtensions.Button("Cancel"))
            this.ScreenNavigator.Pop();
    }

    private void PlayerSelector(string label, int playerIndex)
    {
        if (ImGui.BeginCombo("##" + label, this.selectedPlayers[playerIndex] != null ? this.selectedPlayers[playerIndex].FullName : "Select " + label))
        {
            foreach (var player in this.players)
            {
                if (this.selectedPlayers.Contains(player))
                    continue;
                if (ImGui.Selectable(player.FullName))
                {
                    SetSelectedPlayer(playerIndex, player);
                };
            }
            ImGui.EndCombo();
        }

        ImGui.SameLine(0, ImGui.GetStyle().ItemInnerSpacing.X);
        if (ImGuiExtensions.Button("+##" + label, new Vector2(ImGui.GetFrameHeight(), ImGui.GetFrameHeight())))
        {
            this.playerName = String.Empty;
            this.addPlayerIndex = playerIndex;
            ImGui.OpenPopup("Add Player");
        }
    }

    private void SetSelectedPlayer(int playerIndex, Player player)
    {
        if (this.selectedPlayers[playerIndex] != null)
            this.MatchBuilder.RemovePlayer(this.selectedPlayers[playerIndex]);
        this.selectedPlayers[playerIndex] = player;
        this.MatchBuilder.AddPlayer(player.Id);
    }

    private void DateTimeSelector()
    {
        ImGui.Text("Day / Month / Year");
        ImGui.SameLine(0, 30);
        ImGui.Text("Hours / Minutes");

        ImGui.SetNextItemWidth(22);
        if (ImGui.InputInt("##Day", ref this.dayInput, 0))
        {
            this.dayInput = Math.Clamp(this.dayInput, 1, 31);
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(22);
        if (ImGui.InputInt("##Month", ref this.monthInput, 0))
        {
            this.monthInput = Math.Clamp(this.monthInput, 1, 12);
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(44);
        if (ImGui.InputInt("##Year", ref this.yearInput, 0))
        {
            this.yearInput = Math.Clamp(this.yearInput, 1000, int.MaxValue);
        }

        ImGui.SameLine(0, 50);

        ImGui.SetNextItemWidth(22);
        if (ImGui.InputInt("##Hours", ref this.hourInput, 0))
        {
            this.hourInput = Math.Clamp(this.hourInput, 0, 23);
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(22);
        if (ImGui.InputInt("##Minutes", ref this.minuteInput, 0))
        {
            this.minuteInput = Math.Clamp(this.minuteInput, 0, 59);
        }

        bool open = true;
        if (ImGui.BeginPopupModal("Date Invalid", ref open, ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text("Date is not valid");
            if (ImGui.IsKeyPressed(ImGuiKey.Escape))
                ImGui.CloseCurrentPopup();
            ImGui.EndPopup();
        }
    }

    private bool InputValid()
    {
        foreach (var player in selectedPlayers)
        {
            if (player == null)
                return false;
        }
        return true;
    }

    private void StartMatch()
    {
        try
        {
            this.MatchBuilder.SetDate(new DateTime(this.yearInput, this.monthInput, this.dayInput, this.hourInput, this.minuteInput, 0));
        }
        catch
        {
            ImGui.OpenPopup("Date Invalid");
            return;
        }

        var match = this.MatchBuilder.Build(this.DependencyContainer.GetPlayerRepository());
        this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchInputScreen(match));
    }
}