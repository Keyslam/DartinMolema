using App.Builders;
using App.Core;
using App.Repository;
using App.Models;
using ImGuiNET;
using System.Globalization;

namespace App.View;

internal class TestdataGeneratorScreen : Screen
{
	private class MatchRepositoryMock : IMatchRepository
	{
		private IMatchRepository Reader { get; }

		public MatchRepositoryMock(IMatchRepository reader)
		{
			this.Reader = reader;
		}

		public Match? Read(Guid id) => Reader.Read(id);
		public IReadOnlyList<Match> ReadAll() => Reader.ReadAll();
		public IReadOnlyList<(Guid, string)> ReadAllNames() => Reader.ReadAllNames();

		public void Save(Match t) { }
	}

	private class PlayerRepositoryMock : IPlayerRepository
	{
		private IPlayerRepository Reader { get; }

		public PlayerRepositoryMock(IPlayerRepository reader)
		{
			this.Reader = reader;
		}

		public Player? Read(Guid id) => Reader.Read(id);
		public IReadOnlyList<Player> ReadAll() => Reader.ReadAll();

		public void Save(Player t) { }
	}

	private IPlayerRepository playerRepository { get; }
	private IMatchRepository matchRepository { get; }

	private List<Player> players;

	private List<String> names;
	private List<String> surnames;
	private List<Match> matches;

	private int playersToGenerateInput;
	private int matchesToGenerateInput;

	private List<int> possibleValues;

	private Random random = new Random(0);

	private bool Generated { get; set; } = false;

	public TestdataGeneratorScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.playerRepository = dependencyContainer.GetPlayerRepository();
		this.matchRepository = dependencyContainer.GetMatchRepository();

		this.players = this.playerRepository.ReadAll().ToList();

		this.names = new List<String>() { "Brian", "George", "William", "Daniel", "Ryan", "Bryan", "Cody", "Mike", "Wayne", "Timothy", "Michael", "Justin", "Christopher", "Derek", "Christopher", "Nicholas", "Brandon", "Kenneth", "Richard", "Zachary", "Jeremy", "Christopher", "Jon", "Gerald", "Richard", "Brandon", "Ryan", "Adam", "John", "Charles", "Matthew", "Jason", "Eric", "Jesse", "Robert", "John", "Matthew", "Matthew", "Timothy", "John", "Mark", "Steven", "Miguel", "Howard", "Tyler", "Caleb", "Brian", "Anthony", "Jacob", "Mark", "Amber", "Berber", "Nick", "Nyk", "Wouter", "Gert", "Corrie", "Karel", "Martin", "Daniel", "Eric", "Erik", "Dirk", "Noah", "Chris", "Edwin", "Eddy", "Mark", "Markiplier", "Joel", "Layton", "Xander", "Hammond" };
		this.surnames = new List<String>() { "Deleon", "Whitaker", "Mcgrath", "Sullivan", "Ward", "Evans", "Lee", "Reeves", "Richards", "Henry", "Rivera", "Clark", "Espinoza", "Hernandez", "Haynes", "Smith", "Barnes", "Robertson", "Stewart", "Smith", "Pham", "Haley", "Terry", "White", "Lewis", "Mccarty", "Spears", "Wells", "Scott", "Cunningham", "Aguirre", "Harris", "Jimenez", "Nguyen", "Harrison", "Lee", "Osborne", "Matthews", "Howard", "Ball", "Martin", "Porter", "Edwards", "Johnson", "Gardner", "Riley", "Chung", "Walter", "Frank", "Holmes", "Bakker", "Makkermaat", "Brokjes", "Owen", "Riley", "Farmer", "Crawford", "Woodward", "Flores", "Li", "Brooks", "Watson", "Waters", "Kirk", "Hale", "Poggers", "King", "Sherman", "Howells", "Warner", "Flynn", "Torress" };
		this.matches = new List<Match>();

		this.possibleValues = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 25 };
	}

	public override void Update()
	{
		ImGui.SliderInt("Players to Generate", ref this.playersToGenerateInput, 2, 100);

		ImGui.SliderInt("Matches to Generate", ref this.matchesToGenerateInput, 1, 1000);

		if (ImGuiExtensions.Button("Generate Testdata"))
			GenerateTestData(this.playersToGenerateInput, this.matchesToGenerateInput);

		if (this.Generated)
			ImGui.Text("Data generated");

		if (ImGui.Button("Back"))
			this.DependencyContainer.GetScreenNavigator().Pop();
	}

	public void GenerateTestData(int playerCount, int matchCount)
	{
		GeneratePlayers(playerCount);
		GenerateMatches(matchCount);

		this.Generated = true;
	}

	public bool GeneratePlayers(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Console.WriteLine($"Generating player {i}");
			var (success, fullName) = GetNewName();
			var country = GetRandomCountry();

			Player player = new Player(fullName, country);

			players.Add(player);
			playerRepository.Save(player);
		};

		return true;
	}

	public (bool, String) GetNewName()
	{
		List<int> r1 = Enumerable.Range(0, names.Count()).OrderBy(n => random.NextDouble()).ToList();
		List<int> r2 = Enumerable.Range(0, surnames.Count()).OrderBy(n => random.NextDouble()).ToList();

		var name = "";
		var surname = "";

		for (int i = 0; i < names.Count(); i++)
		{
			name = names.ElementAt(r1[i]);
			for (int j = 0; j < surnames.Count(); j++)
			{
				surname = surnames.ElementAt(r2[j]);

				if (!players.Any(x => x.FullName == name + " " + surname))
					return (true, name + " " + surname);
			}
		}

		return (false, String.Empty);
	}

	private string GetRandomCountry()
	{
		Random random = new Random();
		List<string> CountryList = new List<string>();
		CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		foreach (var getCulture in getCultureInfo)
		{
			RegionInfo GetRegionInfo = new RegionInfo(getCulture.LCID);

			if (!(CountryList.Contains(GetRegionInfo.EnglishName)))
				CountryList.Add(GetRegionInfo.EnglishName);
		}
		return CountryList.OrderBy(n => random.NextDouble()).First();
	}

	public bool GenerateMatches(int matchCount)
	{
		var matchRepositoryMock = new MatchRepositoryMock(matchRepository);
		var playerRepositoryMock = new PlayerRepositoryMock(playerRepository);

		var batches = 100;
		var batchSize = matchCount / batches;

		for (int i = 0; i < batches; i++)
		{
			Console.WriteLine($"Generating batch {i}");
			List<Match> matches = new List<Match>();
			for (int j = 0; j < batchSize; j++)
			{
				Match match = GenerateMatch();
				matches.Add(match);
			}

			var result = Parallel.For(0, batchSize, (j, state) =>
			{
				var match = matches[j];
				PlayTurns(match);
			});

			foreach (var match in matches)
			{
				foreach (var playerId in match.Players)
				{
					var player = playerRepository.Read(playerId)!;
					player.PlayMatch(match);
					playerRepository.Save(player);
				}

				matchRepository.Save(match);
			}
		}

		return true;
	}

	public Match GenerateMatch()
	{
		Random random = new Random();
		MatchBuilder matchBuilder = new MatchBuilder();

		matchBuilder.SetsToWin = random.Next(1, 6);
		matchBuilder.LegsToWin = random.Next(1, 6);
		matchBuilder.ScoreToWin = random.Next(0, 2) == 1 ? 301 : 501;

		var startDate = new DateTime(2000, 1, 1);
		var endDate = DateTime.Now;

		TimeSpan timeSpan = endDate - startDate;
		TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
		DateTime date = startDate + newSpan;

		matchBuilder.Date = date;

		var playerOneIndex = random.Next(0, players.Count());
		var playerTwoIndex = -1;

		while (playerTwoIndex == -1 || playerTwoIndex == playerOneIndex)
			playerTwoIndex = random.Next(0, players.Count());

		matchBuilder.AddPlayer(players.ElementAt(playerOneIndex));
		matchBuilder.AddPlayer(players.ElementAt(playerTwoIndex));

		return matchBuilder.Build();
	}

	public void PlayTurns(Match match)
	{
		while (!match.IsDone)
		{
			List<(ThrowKind throwKind, int value)> throws = new List<(ThrowKind throwKind, int value)>();

			var points = match.CurrentSet.CurrentLeg.Points[match.CurrentSet.CurrentLeg.CurrentPlayerIndex];
			var remainingPoints = match.MatchRules.SetRules.LegRules.TargetScore - points;

			var turn = this.MakeTurn(match.MatchRules.SetRules.LegRules.TurnRules.ThrowsPerTurn, remainingPoints);
			match.PlayTurn(turn);
		}
	}

	public Turn MakeTurn(int maxThrowCount, int remainingPoints)
	{
		var throws = new List<Throw>();

		for (int i = 0; i < maxThrowCount; i++)
		{
			var @throw = this.MakeThrow(remainingPoints);
			remainingPoints -= @throw.GetThrownPoints();

			throws.Add(@throw);

			if (remainingPoints <= 0)
				break;
		}

		var turn = new Turn(throws);

		return turn;
	}

	public Throw MakeThrow(int remainingPoints)
	{
		Random random = new Random();

		if ((remainingPoints <= 40) && remainingPoints % 2 == 0 && random.Next(0, 100) < 100)
			return new Throw(remainingPoints / 2, ThrowKind.Double);

		if ((remainingPoints == 50) && remainingPoints % 2 == 0 && random.Next(0, 100) < 10)
			return new Throw(remainingPoints / 2, ThrowKind.InnerBull);

		int value = possibleValues.ElementAt(random.Next(0, possibleValues.Count()));

		if (value == 0)
			return new Throw(value, ThrowKind.Foul);

		if (value == 25)
			return new Throw(value, random.Next(0, 100) > 33 ? ThrowKind.OuterBull : ThrowKind.InnerBull);

		return new Throw(value, random.Next(0, 100) > 50 ? ThrowKind.Single : random.Next(0, 50) > 20 ? ThrowKind.Double : ThrowKind.Triple);
	}
}