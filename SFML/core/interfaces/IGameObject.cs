﻿namespace SFMLGame.core.interfaces;

public interface IGameObject
{
    void Dispose();
    void LoadContent();
    void LoadEvents();
    void Draw(RenderWindow window);
}
