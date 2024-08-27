namespace SFMLGame.Core.Interfaces;

public interface INode2D
{
    Canva Canva { get; }
    IBody2D? Body { get; }
    Surface2D Surface { get; }
    Position2D Position { get; }

    void SetBody(IBody2D? body);
    void SetOpacity(Opacity opacity);

    INode2D? Get(object? keyCode);
    INode2D? Get(Direction direction);
    void Mount(IReadOnlyList<IList<INode2D>> nodes);
}