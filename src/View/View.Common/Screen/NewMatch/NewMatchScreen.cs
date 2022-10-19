using App.Builders;
using App.Core;
using App.Repository;
using ImGuiNET;
using App.Models;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace App.View;

internal class NewMatchScreen : Screen
{
	private IPlayerRepository PlayerRepository { get; }
	private MatchBuilder MatchBuilder { get; }

	private IReadOnlyList<Player> Players { get; set; }

	private Player[] SelectedPlayers { get; set; }

	private string PlayerName { get; set; }
	private string PlayerCountry { get; set; }
	private int AddPlayerIndex { get; set; }

	private int SetsToWinInput { get; set; }
	private int LegsToWinInput { get; set; }

	private int DayInput { get; set; }
	private int MonthInput { get; set; }
	private int YearInput { get; set; }
	private int HourInput { get; set; }
	private int MinuteInput { get; set; }

	private int ScoreToWinSelectedIdx;
	private string[] Scores;

	public NewMatchScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.PlayerRepository = dependencyContainer.GetPlayerRepository();
		this.MatchBuilder = new MatchBuilder();

		this.Players = this.PlayerRepository.ReadAll().ToList().OrderBy(x => x.FullName).ToList();

		this.SelectedPlayers = new Player[2];

		this.PlayerName = String.Empty;
		this.PlayerCountry = String.Empty;

		this.SetsToWinInput = (int)this.MatchBuilder.SetsToWin;
		this.LegsToWinInput = (int)this.MatchBuilder.LegsToWin;

		this.DayInput = this.MatchBuilder.Date.Day;
		this.MonthInput = this.MatchBuilder.Date.Month;
		this.YearInput = this.MatchBuilder.Date.Year;
		this.HourInput = this.MatchBuilder.Date.Hour;
		this.MinuteInput = this.MatchBuilder.Date.Minute;

		this.ScoreToWinSelectedIdx = 0;
		this.Scores = new string[] { "501", "301" };
	}

	public override void Update()
	{
		DateTimeSelector();

		PlayerSelector("Player One", 0);
		PlayerSelector("Player Two", 1);

		bool openAddPlayer = true;
		if (ImGui.BeginPopupModal("Add Player", ref openAddPlayer, ImGuiWindowFlags.AlwaysAutoResize))
		{
			var playerName = this.PlayerName;
			ImGui.InputText("##Enter Name", ref playerName, 255);
			this.PlayerName = playerName;

			ImGui.SameLine(0, 20);
			ImGui.Text("Enter Name");

			RegexOptions options = RegexOptions.None;
			Regex regex = new Regex("[ ]{2,}", options);
			this.PlayerName = regex.Replace(this.PlayerName.Trim(), " ");
			this.PlayerName = Regex.Replace(this.PlayerName, @"^(?<cap>\w)|\b(?<cap>\w)(?=\w*$)", m => m.Groups["cap"].Value.ToUpper());


			if (ImGui.BeginCombo("##Country", this.PlayerCountry != String.Empty ? this.PlayerCountry : "Select Country"))
			{
				foreach (var country in GetCountryList())
				{
					if (country == this.PlayerCountry)
						continue;
					if (ImGui.Selectable(country))
					{
						this.PlayerCountry = country;
					};
				}
				ImGui.EndCombo();
			}

			if (ImGui.IsKeyPressed(ImGuiKey.Escape))
				ImGui.CloseCurrentPopup();

			bool nameInvalid = String.IsNullOrEmpty(this.PlayerName) || this.Players.ToList().Any(x => x.FullName.ToLower() == this.PlayerName.ToLower());
			bool countryInvalid = String.IsNullOrEmpty(this.PlayerCountry);

			if (nameInvalid || countryInvalid)
				ImGui.BeginDisabled();

			if (ImGuiExtensions.Button("Save"))
			{
				Player player = new Player()
				{
					Id = Guid.NewGuid(),
					FullName = this.PlayerName,
					Country = this.PlayerCountry,
					LostGames = new List<Guid>(),
					WonGames = new List<Guid>(),
					PlayedGames = new List<Guid>(),
					Statistic = new PlayerStatistic()
					{
						AverageTurnScore = 0,
						Ninedarters = 0,
						OneEighties = 0,
					}
				};

				if (!this.Players.ToList().Any(x => x.FullName == player.FullName))
				{
					this.PlayerRepository.Save(player);
					this.Players = this.PlayerRepository.ReadAll().ToList().OrderBy(x => x.FullName).ToList();
					SetSelectedPlayer(AddPlayerIndex, player);
					ImGui.CloseCurrentPopup();
				}
			}

			if (nameInvalid)
			{
				if (!String.IsNullOrEmpty(this.PlayerName))
				{
					var player = this.Players.ToList().Find(x => x.FullName.ToLower() == this.PlayerName.ToLower());
					var alreadySelected = player != null && this.SelectedPlayers.Any(x => x != null && x.Id == player.Id);

					ImGui.SameLine();
					ImGui.Text("Name already exists" + (alreadySelected ? " and selected" : ""));
					ImGui.EndDisabled();

					if (player != null && !alreadySelected)
					{
						ImGui.SameLine();
						if (ImGuiExtensions.Button("Select existing"))
						{
							SetSelectedPlayer(AddPlayerIndex, player);
							ImGui.CloseCurrentPopup();
						}
					}
				}
			}
			if (nameInvalid || countryInvalid)
				ImGui.EndDisabled();

			ImGui.EndPopup();
		}

		var setsToWinInput = this.SetsToWinInput;
		if (ImGui.InputInt("Sets to win match", ref setsToWinInput))
		{
			this.SetsToWinInput = Math.Clamp(setsToWinInput, 1, 100);
			this.MatchBuilder.SetsToWin = this.SetsToWinInput;
			Console.WriteLine(this.MatchBuilder.SetsToWin);
		}

		var legsToWinInput = this.LegsToWinInput;
		if (ImGui.InputInt("Legs to win set", ref legsToWinInput))
		{
			this.LegsToWinInput = Math.Clamp(legsToWinInput, 1, 100);
			this.MatchBuilder.LegsToWin = this.LegsToWinInput;
		}

		ImGui.Combo("Score to win leg", ref this.ScoreToWinSelectedIdx, this.Scores, this.Scores.Length);

		if (!InputValid())
			ImGui.BeginDisabled();
		if (ImGuiExtensions.Button("Start", new Vector2(120, 0)))
		{
			StartMatch();
		}
		if (!InputValid())
			ImGui.EndDisabled();

		ImGui.SameLine();
		if (ImGuiExtensions.Button("Cancel", new Vector2(120, 0)))
			this.ScreenNavigator.Pop();
	}

	private void PlayerSelector(string label, int playerIndex)
	{
		if (ImGui.BeginCombo("##" + label, this.SelectedPlayers[playerIndex] != null ? this.SelectedPlayers[playerIndex].FullName : "Select " + label))
		{
			foreach (var player in this.Players)
			{
				if (this.SelectedPlayers.Contains(player))
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
			this.PlayerName = String.Empty;
			this.PlayerCountry = String.Empty;
			this.AddPlayerIndex = playerIndex;
			ImGui.OpenPopup("Add Player");
		}
	}

	private void SetSelectedPlayer(int playerIndex, Player player)
	{
		if (this.SelectedPlayers[playerIndex] != null)
			this.MatchBuilder.RemovePlayer(this.SelectedPlayers[playerIndex]);
		this.SelectedPlayers[playerIndex] = player;
		this.MatchBuilder.AddPlayer(player.Id);
	}

	private void DateTimeSelector()
	{
		ImGui.Text("Day / Month / Year");
		ImGui.SameLine(0, 30);
		ImGui.Text("Hours / Minutes");

		ImGui.SetNextItemWidth(22);

		var dayInput = this.DayInput;
		if (ImGui.InputInt("##Day", ref dayInput, 0))
			this.DayInput = Math.Clamp(dayInput, 1, 31);

		ImGui.SameLine();
		ImGui.SetNextItemWidth(22);

		var monthInput = this.MonthInput;
		if (ImGui.InputInt("##Month", ref monthInput, 0))
			this.MonthInput = Math.Clamp(monthInput, 1, 12);

		ImGui.SameLine();
		ImGui.SetNextItemWidth(44);

		var yearInput = this.YearInput;
		if (ImGui.InputInt("##Year", ref yearInput, 0))
		{
			this.YearInput = Math.Clamp(yearInput, 1000, int.MaxValue);
		}

		ImGui.SameLine(0, 50);
		ImGui.SetNextItemWidth(22);

		var hourInput = this.HourInput;
		if (ImGui.InputInt("##Hours", ref hourInput, 0))
			this.HourInput = Math.Clamp(hourInput, 0, 23);

		ImGui.SameLine();
		ImGui.SetNextItemWidth(22);

		var minuteInput = this.MinuteInput;
		if (ImGui.InputInt("##Minutes", ref minuteInput, 0))
			this.MinuteInput = Math.Clamp(minuteInput, 0, 59);

		bool open = true;
		if (ImGui.BeginPopupModal("Date Invalid", ref open, ImGuiWindowFlags.AlwaysAutoResize))
		{
			ImGui.Text("Date is not valid");
			if (ImGui.IsKeyPressed(ImGuiKey.Escape))
				ImGui.CloseCurrentPopup();
			ImGui.EndPopup();
		}
	}

	private static List<string> GetCountryList()
	{
		List<string> CountryList = new List<string>();
		CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		foreach (var getCulture in getCultureInfo)
		{
			RegionInfo GetRegionInfo = new RegionInfo(getCulture.LCID);

			if (!(CountryList.Contains(GetRegionInfo.EnglishName)))
				CountryList.Add(GetRegionInfo.EnglishName);
		}
		CountryList.Sort();
		return CountryList;
	}

	private bool InputValid()
	{
		foreach (var player in SelectedPlayers)
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
			this.MatchBuilder.SetDate(new DateTime(this.YearInput, this.MonthInput, this.DayInput, this.HourInput, this.MinuteInput, 0));
		}
		catch
		{
			ImGui.OpenPopup("Date Invalid");
			return;
		}

		this.MatchBuilder.SetScoreToWin(Int32.Parse(this.Scores[this.ScoreToWinSelectedIdx]));

		var match = this.MatchBuilder.Build(this.DependencyContainer.GetPlayerRepository());
		this.ScreenNavigator.Push(this.DependencyContainer.MakeMatchInputScreen(match));
	}
}