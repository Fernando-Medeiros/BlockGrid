namespace SFMLGame.core;

public sealed record Position2D(byte Row, byte Column, float X, float Y)
{
    public (float X, float Y) TopLeft => (X, Y);
    public (byte Row, byte Column) Matrix => (Row, Column);

    public static Position2D Empty => new(0, 0, 0, 0);

    public static implicit operator Vector2f(Position2D postion) => new(postion.X, postion.Y);
};

public sealed record Rect(float X, float Y, float Width, float Height)
{
    public (float X, float Y) Center => (X + (Width / 2), Y + (Height / 2));

    public static Rect Empty => new(0, 0, 0, 0);

    public static implicit operator Vector2f(Rect rect) => new(rect.X, rect.Y);
};

public sealed record MouseDTO(EMouse Button, int X, int Y)
{
    public static implicit operator Vector2f(MouseDTO mouse) => new(mouse.X, mouse.Y);
}

public sealed record LoggerDTO(ELogger Guide, string Message);

public sealed record StatusDTO(string Name, int Level, int Hp, int MaxHp, int Mp, int MaxMp, int Exp, int MaxExp);
