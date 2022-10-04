namespace App.Screens;

public class ScreenNavigator
{
    private Stack<IScreen> Screens { get; }

    public ScreenNavigator()
    {
        this.Screens = new Stack<IScreen>();
    }

    public void Push(IScreen screen)
    {
        this.Screens.Push(screen);
    }

    public void Pop()
    {
        this.Screens.Pop();
    }

    public void PopToRoot()
    {
        while (this.Screens.Count > 1)
            this.Pop();
    }


    public IScreen GetTopScreen()
    {
        return this.Screens.Peek();
    }
}