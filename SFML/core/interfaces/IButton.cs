namespace SFMLGame.core.interfaces;

public interface IButton
{
    event EventHandler<EventArgs>? OnClicked;
    event EventHandler<EventArgs>? OnSelected;

    void Dispose();
    void LoadEvents();
    void Draw(RenderWindow window);
}
