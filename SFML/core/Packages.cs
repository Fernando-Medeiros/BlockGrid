namespace SFMLGame.core;

public sealed record Position2D(byte Row, byte Column, float X, float Y)
{
    public static Position2D Empty => new(0, 0, 0, 0);

    public static implicit operator Vector2f(Position2D postion)
    {
        return new(postion.X, postion.Y);
    }
};

public sealed record MouseDTO(EMouse Button, int X, int Y);
public sealed record LoggerDTO(ELogger Guide, string Message);
public sealed record StatusDTO(string Name, int Level, int Hp, int MaxHp, int Mp, int MaxMp, int Exp, int MaxExp);
