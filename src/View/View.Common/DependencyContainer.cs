using App.GameRuler;
using App.Models;
using App.Repository;

namespace App.View;

internal class DependencyContainer
{
	private ScreenNavigator ScreenNavigator { get; }
	private IMatchRepository MatchRepository { get; }
	private IPlayerRepository PlayerRepository { get; }

	public DependencyContainer()
	{
		this.ScreenNavigator = new ScreenNavigator();
		this.MatchRepository = new App.Repository.LocalRepository.MatchRepository();
		this.PlayerRepository = new App.Repository.LocalRepository.PlayerRepository();
	}

	public ScreenNavigator GetScreenNavigator()
	{
		return this.ScreenNavigator;
	}

	public IMatchRepository GetMatchRepository()
	{
		return this.MatchRepository;
	}

	public IPlayerRepository GetPlayerRepository()
	{
		return this.PlayerRepository;
	}

	public RuleEngine MakeRuleEngine(Match match)
	{
		return new RuleEngine(match, this.GetMatchRepository(), this.GetPlayerRepository());
	}

	public MainScreen MakeMainScreen()
	{
		return new MainScreen(this);
	}

	public MatchInputScreen MakeMatchInputScreen(Match match)
	{
		return new MatchInputScreen(match, this);
	}

	public MatchOverviewScreen MakeMatchOverviewScreen(Match match)
	{
		return new MatchOverviewScreen(match, this);
	}

	public MatchesOverviewScreen MakeMatchesOverviewScreen()
	{
		return new MatchesOverviewScreen(this);
	}

	public NewMatchScreen MakeNewMatchScreen()
	{
		return new NewMatchScreen(this);
	}

	public PlayerOverviewScreen MakePlayerOverviewScreen(Player player)
	{
		return new PlayerOverviewScreen(player, this);
	}

	public PlayersOverviewScreen MakePlayersOverviewScreen()
	{
		return new PlayersOverviewScreen(this);
	}
}