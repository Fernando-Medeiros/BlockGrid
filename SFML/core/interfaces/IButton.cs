namespace SFMLGame.core.interfaces;

public interface IButton
{
    event Action<object?>? OnClicked;
    event Action<object?>? OnFocused;
    event Action<object?>? OnChanged;

    bool Equal(object? value);
    void SetActivated(bool value);

    void Event();
    void Dispose();
    void Render(RenderWindow window);
}
