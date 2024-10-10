namespace SFMLGame;

internal sealed partial class App
{
    public static IBody2D? CurrentPlayer { get; private set; }
    public static INode2D? SelectedNode { get; private set; }
    public static EScene CurrentScene { get; private set; } = EScene.Main;
    public static EBiome CurrentBiome { get; private set; } = EBiome.Forest;
    public static Position2D CurrentPosition { get; private set; } = Position2D.Empty;

    public static byte CurrentFPS { get; set; } = 30;
    public static int CurrentWidth { get; set; } = Global.WINDOW_WIDTH;
    public static int CurrentHeight { get; set; } = Global.WINDOW_HEIGHT;
    public static byte CurrentMusicVolume { get; set; } = Global.MAX_VOLUME;
    public static byte CurrentSoundVolume { get; set; } = Global.MAX_VOLUME;
}
