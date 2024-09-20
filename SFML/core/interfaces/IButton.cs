﻿namespace SFMLGame.core.interfaces;

public interface IButton
{
    event Action<object?>? OnClicked;
    event Action<object?>? OnSelected;

    void Dispose();
    void LoadEvents();
    void Draw(RenderWindow window);
}