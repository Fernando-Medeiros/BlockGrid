namespace SFMLGame.common;

public static class Is
{
    public static readonly Random random = new();

    public static bool Type<T>(object? x) => x is T;
    public static bool Null(object? x) => x is null;
}