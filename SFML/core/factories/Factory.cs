namespace SFMLGame.core.factories;

public static class Factory
{
    #region Color
    public static Color Color(EOpacity opacity) => opacity switch
    {
        EOpacity.Light => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Opaque => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Regular => new(255, 255, 255, Convert.ToByte(opacity)),
        _ => throw new Exception()
    };

    public static Color Color(EColor color) => color switch
    {
        EColor.Black => new(0, 0, 0),
        EColor.CornFlowerBlue => new(100, 149, 237),
        EColor.GoldRod => new(218, 165, 32),
        EColor.Tomate => new(255, 99, 71),
        EColor.Transparent => new(0, 0, 0, 0),
        EColor.White => new(255, 255, 255),
        EColor.DarkSeaGreen => new(0, 114, 119, 47),
        EColor.Gray => new(102, 102, 102),
        _ => throw new Exception()
    };
    #endregion

    #region Shuffle
    public static ESound Shuffle(ETerrain terrain) => terrain switch
    {
        ETerrain.BorealForestA or ETerrain.BorealForestB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.BorealForestC or ETerrain.BorealForestD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.DarkForestA or ETerrain.DarkForestB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.DarkForestC or ETerrain.DarkForestD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.DesertA or ETerrain.DesertB => App.Shuffle([ESound.StepSandA, ESound.StepSandB]),
        ETerrain.DesertC or ETerrain.DesertD => App.Shuffle([ESound.StepSandC, ESound.StepSandD]),
        ETerrain.ForestA or ETerrain.ForestB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.ForestC or ETerrain.ForestD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.GrassLandA or ETerrain.GrassLandB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.GrassLandC or ETerrain.GrassLandD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.HighlandA or ETerrain.HighlandB => App.Shuffle([ESound.StepLeavesA, ESound.StepDirtA]),
        ETerrain.HighlandC or ETerrain.HighlandD => App.Shuffle([ESound.StepLeavesA, ESound.StepDirtB]),
        ETerrain.MountainA or ETerrain.MountainB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.MountainC or ETerrain.MountainD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.SavannaA or ETerrain.SavannaB => App.Shuffle([ESound.StepLeavesA, ESound.StepDirtC]),
        ETerrain.SavannaC or ETerrain.SavannaD => App.Shuffle([ESound.StepLeavesA, ESound.StepDirtD]),
        ETerrain.SnowA or ETerrain.SnowB => App.Shuffle([ESound.StepSnowA, ESound.StepSnowB]),
        ETerrain.SnowC or ETerrain.SnowD => App.Shuffle([ESound.StepSnowC, ESound.StepSnowD]),
        ETerrain.SwampA or ETerrain.SwampB => App.Shuffle([ESound.StepMudA, ESound.StepMudB]),
        ETerrain.SwampC or ETerrain.SwampD => App.Shuffle([ESound.StepMudC, ESound.StepMudD]),
        ETerrain.TropicalForestA or ETerrain.TropicalForestB => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.TropicalForestC or ETerrain.TropicalForestD => App.Shuffle([ESound.StepLeavesA, ESound.StepLeavesB]),
        ETerrain.TundraA or ETerrain.TundraB => App.Shuffle([ESound.StepIceA, ESound.StepIceB]),
        ETerrain.TundraC or ETerrain.TundraD => App.Shuffle([ESound.StepIceC, ESound.StepIceD]),
        _ => throw new Exception()
    };

    public static ETerrain Shuffle(EBiome biome) => biome switch
    {
        EBiome.BorealForest => App.Shuffle([ETerrain.BorealForestA, ETerrain.BorealForestB, ETerrain.BorealForestC, ETerrain.BorealForestD]),
        EBiome.DarkForest => App.Shuffle([ETerrain.DarkForestA, ETerrain.DarkForestB, ETerrain.DarkForestC, ETerrain.DarkForestD]),
        EBiome.Desert => App.Shuffle([ETerrain.DesertA, ETerrain.DesertB, ETerrain.DesertC, ETerrain.DesertD]),
        EBiome.Forest => App.Shuffle([ETerrain.ForestA, ETerrain.ForestB, ETerrain.ForestC, ETerrain.ForestD]),
        EBiome.GrassLand => App.Shuffle([ETerrain.GrassLandA, ETerrain.GrassLandB, ETerrain.GrassLandC, ETerrain.GrassLandD]),
        EBiome.Highland => App.Shuffle([ETerrain.HighlandA, ETerrain.HighlandB, ETerrain.HighlandC, ETerrain.HighlandD]),
        EBiome.Mountain => App.Shuffle([ETerrain.MountainA, ETerrain.MountainB, ETerrain.MountainC, ETerrain.MountainD]),
        EBiome.Savanna => App.Shuffle([ETerrain.SavannaA, ETerrain.SavannaB, ETerrain.SavannaC, ETerrain.SavannaD,]),
        EBiome.Snow => App.Shuffle([ETerrain.SnowA, ETerrain.SnowB, ETerrain.SnowC, ETerrain.SnowD]),
        EBiome.Swamp => App.Shuffle([ETerrain.SwampA, ETerrain.SwampB, ETerrain.SwampC, ETerrain.SwampD]),
        EBiome.TropicalForest => App.Shuffle([ETerrain.TropicalForestA, ETerrain.TropicalForestB, ETerrain.TropicalForestC, ETerrain.TropicalForestD]),
        EBiome.Tundra => App.Shuffle([ETerrain.TundraA, ETerrain.TundraB, ETerrain.TundraC, ETerrain.TundraD]),
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

    public static IObject2D Build(enums.ESprite sprite) => sprite switch
    {
        enums.ESprite.Road => new Object2D() { Sprite = sprite },
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
