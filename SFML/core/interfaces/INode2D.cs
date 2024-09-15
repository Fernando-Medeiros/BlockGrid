﻿namespace SFMLGame.core.interfaces;

public interface INode2D
{
    IBody2D? Body { get; }
    EOpacity Opacity { get; }
    Position2D Position2D { get; }
    IList<IGameItem> GameItems { get; }
    Dictionary<EDirection, INode2D?> Navigation { get; }

    void SetBody(IBody2D? body);
    void SetOpacity(EOpacity opacity);
    void Draw(RenderWindow window);

    INode2D? Get(EDirection direction);
    INode2D? Get(params EDirection[] directions);
}