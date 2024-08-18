namespace SFMLGame.Core.Interfaces;

public interface INode2D
{
    Canva Canva { get; }
    Position2D Position { get; }
    IBody2D? Body { get; set; }
    Surface2D Surface { get; set; }

    void Draw();
    void FadeTo(double opacity);
    void VisibilityTo(double opacity);
    INode2D? GetNode(object? key);
    void Mount(IReadOnlyList<IList<INode2D>> _nodes);
}