namespace SFMLGame.core.factories;

public static class Factory
{
    #region Color
    public static Color Color(EOpacity opacity) => opacity switch
    {
        EOpacity.Dark => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Light => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Opaque => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Regular => new(255, 255, 255, Convert.ToByte(opacity)),
        _ => throw new Exception()
    };
    #endregion

    #region Shuffle
    public static ESprite Shuffle(EBiome biome) => biome switch
    {
        EBiome.BorealForest => App.Shuffle([ESprite.BorealForestA, ESprite.BorealForestB, ESprite.BorealForestC, ESprite.BorealForestD]),
        EBiome.DarkForest => App.Shuffle([ESprite.DarkForestA, ESprite.DarkForestB, ESprite.DarkForestC, ESprite.DarkForestD]),
        EBiome.Desert => App.Shuffle([ESprite.DesertA, ESprite.DesertB, ESprite.DesertC, ESprite.DesertD]),
        EBiome.Forest => App.Shuffle([ESprite.ForestA, ESprite.ForestB, ESprite.ForestC, ESprite.ForestD]),
        EBiome.GrassLand => App.Shuffle([ESprite.GrassLandA, ESprite.GrassLandB, ESprite.GrassLandC, ESprite.GrassLandD]),
        EBiome.Highland => App.Shuffle([ESprite.HighlandA, ESprite.HighlandB, ESprite.HighlandC, ESprite.HighlandD]),
        EBiome.Mountain => App.Shuffle([ESprite.MountainA, ESprite.MountainB, ESprite.MountainC, ESprite.MountainD]),
        EBiome.Savanna => App.Shuffle([ESprite.SavannaA, ESprite.SavannaB, ESprite.SavannaC, ESprite.SavannaD,]),
        EBiome.Snow => App.Shuffle([ESprite.SnowA, ESprite.SnowB, ESprite.SnowC, ESprite.SnowD]),
        EBiome.Swamp => App.Shuffle([ESprite.SwampA, ESprite.SwampB, ESprite.SwampC, ESprite.SwampD]),
        EBiome.TropicalForest => App.Shuffle([ESprite.TropicalForestA, ESprite.TropicalForestB, ESprite.TropicalForestC, ESprite.TropicalForestD]),
        EBiome.Tundra => App.Shuffle([ESprite.TundraA, ESprite.TundraB, ESprite.TundraC, ESprite.TundraD]),
        _ => throw new Exception()
    };
    #endregion

    #region Build
    public static IBody2D Build(EBody body, INode2D node) => body switch
    {
        EBody.Npc => new NPCBody2D(node),
        EBody.Static => new StaticBody2D(node),
        EBody.Player => new PlayerBody2D(node),
        EBody.Enemy => new EnemyBody2D(node),
        _ => throw new Exception()
    };
    #endregion

    #region Shorten
    public static EDirection Resolve(EDirection direction, int offset) => (direction, offset) switch
    {
        (EDirection.Top, -1) => EDirection.TopLeft,
        (EDirection.Left, -1) => EDirection.BottomLeft,
        (EDirection.Right, -1) => EDirection.TopRight,
        (EDirection.Bottom, -1) => EDirection.BottomRight,
        (EDirection.TopLeft, -1) => EDirection.Left,
        (EDirection.TopRight, -1) => EDirection.Top,
        (EDirection.BottomLeft, -1) => EDirection.Bottom,
        (EDirection.BottomRight, -1) => EDirection.Right,
        (EDirection.Top, 1) => EDirection.TopRight,
        (EDirection.Left, 1) => EDirection.TopLeft,
        (EDirection.Right, 1) => EDirection.BottomRight,
        (EDirection.Bottom, 1) => EDirection.BottomLeft,
        (EDirection.TopLeft, 1) => EDirection.Top,
        (EDirection.TopRight, 1) => EDirection.Right,
        (EDirection.BottomLeft, 1) => EDirection.Left,
        (EDirection.BottomRight, 1) => EDirection.Bottom,
        _ => throw new Exception()
    };

    public static EDirection[] Resolve(EDirection direction) => direction switch
    {
        EDirection.Top => [EDirection.Top, EDirection.Left, EDirection.Right],
        EDirection.Left => [EDirection.Left, EDirection.Bottom, EDirection.Top],
        EDirection.Right => [EDirection.Right, EDirection.Top, EDirection.Bottom],
        EDirection.Bottom => [EDirection.Bottom, EDirection.Left, EDirection.Right],
        EDirection.TopLeft => [EDirection.BottomLeft, EDirection.TopRight, EDirection.Left],
        EDirection.TopRight => [EDirection.BottomRight, EDirection.TopLeft, EDirection.Right],
        EDirection.BottomLeft => [EDirection.TopLeft, EDirection.BottomRight, EDirection.Left],
        EDirection.BottomRight => [EDirection.TopRight, EDirection.BottomLeft, EDirection.Right],
        _ => throw new Exception()
    };

    public static EDirection Resolve(EDirection direction, object? keyCode) => (direction, keyCode) switch
    {
        (EDirection.Top, Key.W) => EDirection.Top,
        (EDirection.Top, Key.A) => EDirection.Left,
        (EDirection.Top, Key.D) => EDirection.Right,
        (EDirection.Top, Key.S) => EDirection.Bottom,
        (EDirection.Bottom, Key.W) => EDirection.Bottom,
        (EDirection.Bottom, Key.A) => EDirection.Left,
        (EDirection.Bottom, Key.D) => EDirection.Right,
        (EDirection.Bottom, Key.S) => EDirection.Top,
        (EDirection.Left, Key.W) => EDirection.Left,
        (EDirection.Left, Key.A) => EDirection.Bottom,
        (EDirection.Left, Key.D) => EDirection.Top,
        (EDirection.Left, Key.S) => EDirection.Right,
        (EDirection.Right, Key.W) => EDirection.Right,
        (EDirection.Right, Key.A) => EDirection.Top,
        (EDirection.Right, Key.D) => EDirection.Bottom,
        (EDirection.Right, Key.S) => EDirection.Left,
        (EDirection.TopRight, Key.W) => EDirection.TopRight,
        (EDirection.TopRight, Key.A) => EDirection.TopLeft,
        (EDirection.TopRight, Key.D) => EDirection.BottomRight,
        (EDirection.TopRight, Key.S) => EDirection.BottomLeft,
        (EDirection.TopLeft, Key.W) => EDirection.TopLeft,
        (EDirection.TopLeft, Key.A) => EDirection.BottomLeft,
        (EDirection.TopLeft, Key.D) => EDirection.TopRight,
        (EDirection.TopLeft, Key.S) => EDirection.BottomRight,
        (EDirection.BottomRight, Key.W) => EDirection.BottomRight,
        (EDirection.BottomRight, Key.A) => EDirection.TopRight,
        (EDirection.BottomRight, Key.D) => EDirection.BottomLeft,
        (EDirection.BottomRight, Key.S) => EDirection.TopLeft,
        (EDirection.BottomLeft, Key.W) => EDirection.BottomLeft,
        (EDirection.BottomLeft, Key.A) => EDirection.TopLeft,
        (EDirection.BottomLeft, Key.D) => EDirection.BottomRight,
        (EDirection.BottomLeft, Key.S) => EDirection.TopRight,
        _ => throw new Exception()
    };
    #endregion
}
