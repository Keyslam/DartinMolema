namespace App.View;

internal abstract class Screen
{
	protected DependencyContainer DependencyContainer { get; }
	protected ScreenNavigator ScreenNavigator { get; }

	public Screen(DependencyContainer dependencyContainer)
	{
		this.DependencyContainer = dependencyContainer;
		this.ScreenNavigator = dependencyContainer.GetScreenNavigator();
	}

	public abstract void Update();
}