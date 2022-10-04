namespace App.View;

internal class ScreenNavigator
{
    private Stack<Screen> Screens { get; }

    public ScreenNavigator()
    {
        this.Screens = new Stack<Screen>();
    }

    public void Push(Screen screen)
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


    public Screen GetTopScreen()
    {
        return this.Screens.Peek();
    }
}