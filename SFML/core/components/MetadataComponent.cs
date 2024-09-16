namespace SFMLGame.core.components;

public sealed class MetadataComponent : IMetadataComponent
{
    private EDirection Direction { get; set; } = EDirection.Left;

    public EDirection GetDirection() => Direction;

    public void DirectionTo(object? keyCode)
    {
        if (keyCode is Key.Q || keyCode is Key.E)
        {
            int offset = keyCode is Key.E ? 1 : -1;
            Direction = Factory.Resolve(Direction, offset);
        }
    }

    public INode2D? ResolveDirection(IBody2D body, object? keyCode)
    {
        if (Key.Movement.Contains(keyCode))
            return body.Node?.Get(Factory.Resolve(Direction, keyCode));
        return null;
    }
}