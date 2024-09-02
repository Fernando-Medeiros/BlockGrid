
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
            (EDirection.Top, Key.W or Key.Up) => body.Node?.Get(EDirection.Top),
            (EDirection.Top, Key.A or Key.Left) => body.Node?.Get(EDirection.Left),
            (EDirection.Top, Key.D or Key.Right) => body.Node?.Get(EDirection.Right),
            (EDirection.Top, Key.S or Key.Down) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Bottom, Key.W or Key.Up) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Bottom, Key.A or Key.Left) => body.Node?.Get(EDirection.Left),
            (EDirection.Bottom, Key.D or Key.Right) => body.Node?.Get(EDirection.Right),
            (EDirection.Bottom, Key.S or Key.Down) => body.Node?.Get(EDirection.Top),
            (EDirection.Left, Key.W or Key.Up) => body.Node?.Get(EDirection.Left),
            (EDirection.Left, Key.A or Key.Left) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Left, Key.D or Key.Right) => body.Node?.Get(EDirection.Top),
            (EDirection.Left, Key.S or Key.Down) => body.Node?.Get(EDirection.Right),
            (EDirection.Right, Key.W or Key.Up) => body.Node?.Get(EDirection.Right),
            (EDirection.Right, Key.A or Key.Left) => body.Node?.Get(EDirection.Top),
            (EDirection.Right, Key.D or Key.Right) => body.Node?.Get(EDirection.Bottom),
            (EDirection.Right, Key.S or Key.Down) => body.Node?.Get(EDirection.Left),
            (EDirection.TopRight, Key.W or Key.Up) => body.Node?.Get(EDirection.TopRight),
            (EDirection.TopRight, Key.A or Key.Left) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.TopRight, Key.D or Key.Right) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.TopRight, Key.S or Key.Down) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.TopLeft, Key.W or Key.Up) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.TopLeft, Key.A or Key.Left) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.TopLeft, Key.D or Key.Right) => body.Node?.Get(EDirection.TopRight),
            (EDirection.TopLeft, Key.S or Key.Down) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomRight, Key.W or Key.Up) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomRight, Key.A or Key.Left) => body.Node?.Get(EDirection.TopRight),
            (EDirection.BottomRight, Key.D or Key.Right) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.BottomRight, Key.S or Key.Down) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.BottomLeft, Key.W or Key.Up) => body.Node?.Get(EDirection.BottomLeft),
            (EDirection.BottomLeft, Key.A or Key.Left) => body.Node?.Get(EDirection.TopLeft),
            (EDirection.BottomLeft, Key.D or Key.Right) => body.Node?.Get(EDirection.BottomRight),
            (EDirection.BottomLeft, Key.S or Key.Down) => body.Node?.Get(EDirection.TopRight),
            _ => null
        };
    }
}