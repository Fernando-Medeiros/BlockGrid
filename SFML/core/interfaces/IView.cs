namespace SFMLGame.core.interfaces;

public interface IView
{
    void Build();
    void Event();
    void Dispose();
    void Render(RenderWindow window);
}

public interface IHud : IView
{
    void VisibilityChanged();

    event Action<object?>? OnClicked;
}