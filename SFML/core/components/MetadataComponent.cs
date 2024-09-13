
namespace SFMLGame.core.components;

public sealed class MetadataComponent : IMetadataComponent
{
    private EDirection _direction = EDirection.Left;

    public EDirection GetDirection() => _direction;

    public void DirectionTo(object? keyCode)
    {
        if (keyCode is Key.Q || keyCode is Key.E)
        {
            var index = keyCode is Key.E ? ((sbyte)_direction) + 1 : ((sbyte)_direction) + -1;

            index = index > 7 ? 0 : index < 0 ? 7 : index;

            _direction = Enum.GetValues<EDirection>().ElementAt(index);
        }
    }

    public INode2D? ResolveDirection(IBody2D body, object? keyCode)
    {
        return (body.Metadata?.GetDirection(), keyCode) switch
        {
            (EDirection.Top, Key.W) => body.Node?.Get(EDirection.Top),
            (EDirection.Top, Key.A) => body.Node?.Get(EDirection.Left),
            (EDirection.Top, Key.D) => body.Node?.Get(EDirection.Right),
            (EDirection.Top, Key.S) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Bottom, Key.W) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Bottom, Key.A) => body.Node?.Get(EDirection.Left),
            (EDirection.Bottom, Key.D) => body.Node?.Get(EDirection.Right),
            (EDirection.Bottom, Key.S) => body.Node?.Get(EDirection.Top),
            (EDirection.Left, Key.W) => body.Node?.Get(EDirection.Left),
            (EDirection.Left, Key.A) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Left, Key.D) => body.Node?.Get(EDirection.Top),
            (EDirection.Left, Key.S) => body.Node?.Get(EDirection.Right),
            (EDirection.Right, Key.W) => body.Node?.Get(EDirection.Right),
            (EDirection.Right, Key.A) => body.Node?.Get(EDirection.Top),
            (EDirection.Right, Key.D) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Right, Key.S) => body.Node?.Get(EDirection.Left),
            (EDirection.TopRight, Key.W) => body.Node?.Get(EDirection.TopRight),
            (EDirection.TopRight, Key.A) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.TopRight, Key.D) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.TopRight, Key.S) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.TopLeft, Key.W) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.TopLeft, Key.A) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.TopLeft, Key.D) => body.Node?.Get(EDirection.TopRight),
            (EDirection.TopLeft, Key.S) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomRight, Key.W) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomRight, Key.A) => body.Node?.Get(EDirection.TopRight),
            (EDirection.BottomRight, Key.D) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.BottomRight, Key.S) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.BottomLeft, Key.W) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.BottomLeft, Key.A) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.BottomLeft, Key.D) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomLeft, Key.S) => body.Node?.Get(EDirection.TopRight),
            _ => null
        };
    }
}