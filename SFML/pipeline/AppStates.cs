namespace SFMLGame.pipeline;

internal sealed partial class App
{
    public static EScene CurrentScene { get; private set; } = EScene.Main;

    public static Music? CurrentMusic { get; set; }
    public static INode2D? SelectedNode { get; private set; }
    public static Position2D CurrentPosition { get; private set; } = Position2D.Empty;

    #region Core
    public static WorldSchema World { get; set; } = new();
    public static PlayerSchema Player { get; set; } = new();
    public static RegionSchema Region { get; set; } = new();
    public static ConfigurationSchema Configuration { get; set; } = new();
    #endregion
}
