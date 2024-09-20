﻿namespace SFMLGame.core.interfaces;

public interface INode2D
{
    IBody2D? Body { get; }
    EOpacity Opacity { get; }
    Position2D Position2D { get; }
    IList<IGameItem> GameItems { get; }

    void SetBody(IBody2D? body);
    void SetOpacity(EOpacity opacity);
    void Draw(RenderWindow window);
    void Dispose();

    INode2D? Get(params EDirection[] directions);
}