namespace SFMLGame.core.interfaces;

public interface INode2D
{
    Canva Canva { get; }
    IBody2D? Body { get; }
    EOpacity Opacity { get; }
    Surface2D Surface { get; }
    Position2D Position { get; }
    Dictionary<EDirection, INode2D?> Navigation { get; }

    void SetBody(IBody2D? body);
    void SetOpacity(EOpacity opacity);

    INode2D? Get(object? keyCode);
    INode2D? Get(EDirection direction);
}