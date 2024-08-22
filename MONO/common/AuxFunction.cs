namespace MONOGame.Common;

public static class Is
{
    public static readonly Random random = new();

    public static bool Type<T>(object? x) => x is T;
    public static bool Not<T>(object? x) => x is not T;
    public static bool Null(object? x) => x is null;
    public static bool NotNull(object? x) => x is not null;
    public static bool Blocked(object? x) => x is Surface2D tile && SurfaceAccess.ItsBlocked(tile);
}