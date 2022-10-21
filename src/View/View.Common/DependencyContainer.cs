using App.Models;
using App.Repository;
using App.Repository.Caching;
using App.Repository.LocalRepository;

namespace App.View;

internal class DependencyContainer
{
	private ScreenNavigator ScreenNavigator { get; }

	private IMatchRepository MatchRepository { get; }
	private IPlayerRepository PlayerRepository { get; }

	public DependencyContainer()
	{
		this.ScreenNavigator = new ScreenNavigator();

		this.MatchRepository = new MatchCachingDecorator(new LocalMatchRepository());
		this.PlayerRepository = new LocalPlayerRepository();
	}

	public ScreenNavigator GetScreenNavigator() => this.ScreenNavigator;

	public IMatchRepository GetMatchRepository() => this.MatchRepository;
	public IPlayerRepository GetPlayerRepository() => this.PlayerRepository;

	public MainScreen MakeMainScreen() => new MainScreen(this);
	public MatchInputScreen MakeMatchInputScreen(Match match) => new MatchInputScreen(match, this);
	public MatchOverviewScreen MakeMatchOverviewScreen(Match match) => new MatchOverviewScreen(match, this);
	public MatchesOverviewScreen MakeMatchesOverviewScreen() => new MatchesOverviewScreen(this);
	public NewMatchScreen MakeNewMatchScreen() => new NewMatchScreen(this);
	public PlayerOverviewScreen MakePlayerOverviewScreen(Player player) => new PlayerOverviewScreen(player, this);
	public PlayersOverviewScreen MakePlayersOverviewScreen() => new PlayersOverviewScreen(this);
	public TestdataGeneratorScreen MakeNewTestdataGeneratorScreen() => new TestdataGeneratorScreen(this);
}