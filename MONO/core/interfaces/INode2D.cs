namespace MONOGame.Core.Interfaces;

public interface INode2D
{
    Canva Canva { get; }
    Position2D Position { get; }
    Surface2D Surface { get; }
    IBody2D? Body { get; set; }

    INode2D? GetNode(object? keyCode);
    void Mount(IReadOnlyList<IList<INode2D>> nodes);
}