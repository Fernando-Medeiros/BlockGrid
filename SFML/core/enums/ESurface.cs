namespace SFMLGame.core.enums;

public enum ESurface : byte
{
    House,
    Grass,
    Road,
    Desert,
    Water,
}

public readonly struct SurfaceAccess
{
    public static IReadOnlyList<ESurface> Surfaces { get; } = [
        ESurface.Water, ESurface.House
        ];

    public static bool ItsBlocked(ESurface surface) => Surfaces.Contains(surface);
}
