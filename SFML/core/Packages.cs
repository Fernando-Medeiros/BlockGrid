namespace SFMLGame.core;

public sealed record Position2D(byte Row, byte Column, float X, float Y)
{
    public (float X, float Y) TopLeft => (X, Y);
    public (byte Row, byte Column) Matrix => (Row, Column);

    public static Position2D Empty => new(0, 0, 0, 0);

    public static implicit operator Vector2f(Position2D postion) => new(postion.X, postion.Y);
};

public sealed class Rect()
{
    #region Main Property
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public float Padding { get; private set; }
    #endregion

    #region Secondary Property
    public float WidthLeft => X + Padding;
    public float WidthRight => X + Width - Padding;
    public float WidthCenter => X + (Width / 2f) + (Padding / 2f);
    public float HeightTop => Y + Padding;
    public float HeightBottom => Y + Height - Padding;
    public float HeightCenter => Y + (Height / 2f) + (Padding / 2f);
    #endregion

    public Rect(float x, float y, float width, float height) : this()
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    #region Extension
    public static Rect Empty => new();

    public static implicit operator Vector2f(Rect rect) => new(rect.X, rect.Y);
    #endregion

    #region Builder
    public Rect WithSize(float width, float height, float padding)
    {
        Width = width;
        Height = height;
        Padding = padding;
        return this;
    }

    /// <summary>
    /// Default -> Center
    /// </summary>
    /// <returns><see cref="Rect"/> -> this</returns>
    public Rect WithAlignment(EDirection? alignment = null)
    {
        var width = App.Configuration.WindowWidth;
        var height = App.Configuration.WindowHeight;

        (X, Y) = alignment switch
        {
            EDirection.Top => ((width / 2f) - (Width / 2f), 0),
            EDirection.TopLeft => (0, 0),
            EDirection.TopRight => (width - Width, 0),
            EDirection.Bottom => ((width / 2f) - (Width / 2f), height - Height),
            EDirection.BottomLeft => (0, height - Height),
            EDirection.BottomRight => (width - Width, height - Height),
            EDirection.Left => (0, (height / 2f) - (Height / 2f)),
            EDirection.Right => (width - Width, (height / 2f) - (Height / 2f)),
            _ => ((width / 2f) - (Width / 2f), (height / 2f) - (Height / 2f)),
        };
        return this;
    }
    #endregion
};

public sealed record MouseDTO(EMouse Button, int X, int Y)
{
    public static implicit operator Vector2f(MouseDTO mouse) => new(mouse.X, mouse.Y);
}

public sealed record Logger(ELogger Guide, string Message);

public sealed record StatusDTO(string Name, int Level, int Hp, int MaxHp, int Mp, int MaxMp, int Exp, int MaxExp);
