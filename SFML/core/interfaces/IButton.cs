﻿namespace SFMLGame.core.interfaces;

public interface IButton
{
    void Enabled(bool value);

    void Event();
    void Dispose();
    void Render(RenderWindow window);

    event Action<object?>? OnClicked;
    event Action<object?>? OnFocus;
}
