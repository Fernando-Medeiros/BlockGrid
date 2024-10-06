namespace SFMLGame.core.interfaces;

public interface IButton
{
    event Action<object?>? OnClicked;
    event Action<object?>? OnFocus;

    void IsEnabled(bool value);

    void Dispose();
    void LoadEvents();
    void Draw(RenderWindow window);
}
