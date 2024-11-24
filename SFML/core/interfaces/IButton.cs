namespace SFMLGame.core.interfaces;

public interface IButton
{
    event Action<object?>? OnClicked;
    event Action<object?>? OnFocused;
    event Action<object?>? OnChanged;

    bool Equal(object? value);
    void Activated(bool value);
    float GetPosition(EDirection direction);

    void Event();
    void Render(RenderWindow window);
    void Dispose();
}
