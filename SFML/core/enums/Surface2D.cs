namespace SFMLGame.Core.Enums;

public enum Surface2D : byte
{
    House,
    Grass,
    Road,
    Desert,
    Water,
}

public readonly struct SurfaceAccess
{
    public static IReadOnlyList<Surface2D> Surfaces { get; } = [
        Surface2D.Water, Surface2D.House
        ];

    public static bool ItsBlocked(Surface2D surface) => Surfaces.Contains(surface);
}
