using App.Builders;
using App.Core;
using App.Repository;
using App.Models;
using App.GameRuler;
using ImGuiNET;

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

	private bool showPlayerMessage;
	private bool showMatchMessage;
	private String playerMessage;
	private String matchMessage;

	private Random random = new Random(0);

	public TestdataGeneratorScreen(DependencyContainer dependencyContainer) : base(dependencyContainer)
	{
		this.playerRepository = dependencyContainer.GetPlayerRepository();
		this.matchRepository = dependencyContainer.GetMatchRepository();

		this.players = this.playerRepository.ReadAll().ToList();

		this.names = new List<String>() { "Brian", "George", "William", "Daniel", "Ryan", "Bryan", "Cody", "Mike", "Wayne", "Timothy", "Michael", "Justin", "Christopher", "Derek", "Christopher", "Nicholas", "Brandon", "Kenneth", "Richard", "Zachary", "Jeremy", "Christopher", "Jon", "Gerald", "Richard", "Brandon", "Ryan", "Adam", "John", "Charles", "Matthew", "Jason", "Eric", "Jesse", "Robert", "John", "Matthew", "Matthew", "Timothy", "John", "Mark", "Steven", "Miguel", "Howard", "Tyler", "Caleb", "Brian", "Anthony", "Jacob", "Mark", "Amber", "Berber", "Nick", "Nyk", "Wouter", "Gert", "Corrie", "Karel", "Martin", "Daniel", "Eric", "Erik", "Dirk", "Noah", "Chris", "Edwin", "Eddy", "Mark", "Markiplier", "Joel", "Layton", "Xander", "Hammond" };
		this.surnames = new List<String>() { "Deleon", "Whitaker", "Mcgrath", "Sullivan", "Ward", "Evans", "Lee", "Reeves", "Richards", "Henry", "Rivera", "Clark", "Espinoza", "Hernandez", "Haynes", "Smith", "Barnes", "Robertson", "Stewart", "Smith", "Pham", "Haley", "Terry", "White", "Lewis", "Mccarty", "Spears", "Wells", "Scott", "Cunningham", "Aguirre", "Harris", "Jimenez", "Nguyen", "Harrison", "Lee", "Osborne", "Matthews", "Howard", "Ball", "Martin", "Porter", "Edwards", "Johnson", "Gardner", "Riley", "Chung", "Walter", "Frank", "Holmes", "Bakker", "Makkermaat", "Brokjes", "Owen", "Riley", "Farmer", "Crawford", "Woodward", "Flores", "Li", "Brooks", "Watson", "Waters", "Kirk", "Hale", "Poggers", "King", "Sherman", "Howells", "Warner", "Flynn", "Torress" };
		this.matches = new List<Match>();

		this.possibleValues = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 25 };

		this.showPlayerMessage = false;
		this.showMatchMessage = false;
	}

	public override void Update()
	{
		ImGui.SliderInt("Players to Generate", ref this.playersToGenerateInput, 0, 1000);

		ImGui.SliderInt("Matches to Generate", ref this.matchesToGenerateInput, 0, 10000);

		if (ImGuiExtensions.Button("Generate Testdata"))
		{
			GenerateTestData(this.playersToGenerateInput, this.matchesToGenerateInput);
		}

		if (showPlayerMessage)
		{
			ImGui.Text(this.playerMessage);
		}

		if (showMatchMessage)
		{
			ImGui.Text(this.matchMessage);
		}
	}

	public void GenerateTestData(int playerCount, int matchCount)
	{
		showPlayerMessage = playerCount > 0;
		showMatchMessage = matchCount > 0;

		if (GeneratePlayers(playerCount))
			playerMessage = "Players have been generated";
		else
			playerMessage = "Out of possible name combinations";

		if (GenerateMatches(matchCount))
			matchMessage = "Matches have been generated";
		else
			matchMessage = "Error while generating matches";
	}

	public bool GeneratePlayers(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Console.WriteLine($"Generating player {i}");
			var (success, fullName) = GetNewName();

			Player player = new Player()
			{
				Id = Guid.NewGuid(),
				FullName = fullName,
				PlayedGames = new List<Guid>(),
				WonGames = new List<Guid>(),
				LostGames = new List<Guid>(),
				Statistic = new PlayerStatistic()
				{
					AverageTurnScore = 0,
					Ninedarters = 0,
					OneEighties = 0,
					PlayedTurns = 0,
				}
			};

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

	public bool GenerateMatches(int matchCount)
	{
		var matchRepositoryMock = new MatchRepositoryMock(matchRepository);
		var playerRepositoryMock = new PlayerRepositoryMock(playerRepository);

		var batches = 100;
		var batchSize = matchCount / batches;

		for (int i = 0; i < batches; i++)
		{
			Console.WriteLine($"Generating batch {i}");
			List<(Match, RuleEngine)> matches = new List<(Match, RuleEngine)>();
			for (int j = 0; j < batchSize; j++)
			{
				Match match = GenerateMatch();
				var ruleEngine = new RuleEngine(match, matchRepositoryMock, playerRepositoryMock);

				matches.Add((match, ruleEngine));
			}

			var result = Parallel.For(0, batchSize, (j, state) =>
			{
				var (match, ruleEngine) = matches[j];
				playTurns(ruleEngine, match);
			});

			Console.WriteLine($"Done generating batch {i}");

			Console.WriteLine($"Writing batch {i}");
			foreach (var (match, ruleEngine) in matches)
			{
				foreach (var player in ruleEngine.Players)
					playerRepository.Save(player);

				matchRepository.Save(match);
			}
			Console.WriteLine($"Done writing batch {i}");
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

		DateTime date = new DateTime(2000, 1, 1);
		date.AddMinutes(random.Next(0, (int)(DateTime.Now - date).TotalMinutes));
		matchBuilder.Date = date;

		var playerOneIndex = random.Next(0, players.Count());
		var playerTwoIndex = -1;

		while (playerTwoIndex == -1 || playerTwoIndex == playerOneIndex)
			playerTwoIndex = random.Next(0, players.Count());

		matchBuilder.AddPlayer(players.ElementAt(playerOneIndex));
		matchBuilder.AddPlayer(players.ElementAt(playerTwoIndex));

		return matchBuilder.Build(playerRepository);
	}

	public void playTurns(RuleEngine ruleEngine, Match match)
	{
		while (ruleEngine.GetCurrentPlayerLegStatistic().RemainingPoints != 0)
		{
			List<(ThrowKind throwKind, int value)> throws = new List<(ThrowKind throwKind, int value)>();
			int remainingPoints = ruleEngine.GetCurrentPlayerLegStatistic().RemainingPoints;

			for (int i = 0; i < match.ThrowsPerTurn; i++)
			{
				throws.Add(makeThrow(ruleEngine, remainingPoints));
				remainingPoints = ruleEngine.GetRemainingPointsAfterTurn(throws);

				if (remainingPoints == 0)
					break;
			}

			if (ruleEngine.PlayTurn(throws))
				return;
		}
	}

	public (ThrowKind, int) makeThrow(RuleEngine ruleEngine, int remainingPoints)
	{
		Random random = new Random();

		if ((remainingPoints <= 40) && remainingPoints % 2 == 0 && random.Next(0, 100) < 100)
			return (ThrowKind.Double, remainingPoints / 2);

		if ((remainingPoints == 50) && remainingPoints % 2 == 0 && random.Next(0, 100) < 10)
			return (ThrowKind.InnerBull, remainingPoints / 2);


		int value = possibleValues.ElementAt(random.Next(0, possibleValues.Count()));

		if (value == 0)
			return (ThrowKind.None, value);

		if (value == 25)
			return (random.Next(0, 100) > 33 ? ThrowKind.OuterBull : ThrowKind.InnerBull, value);

		ThrowKind throwKind = random.Next(0, 100) > 50 ? ThrowKind.Single : random.Next(0, 50) > 20 ? ThrowKind.Double : ThrowKind.Triple;

		return (throwKind, value);
	}
}