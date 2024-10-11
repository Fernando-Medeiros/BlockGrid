using SFML.Audio;

namespace SFMLGame;

internal sealed partial class App
{
    public static Music? CurrentSoundtrack { get; set; }

    public static IBody2D? CurrentPlayer { get; private set; }
    public static INode2D? SelectedNode { get; private set; }
    public static EScene CurrentScene { get; private set; } = EScene.Main;
    public static EBiome CurrentBiome { get; private set; } = EBiome.Forest;
    public static Position2D CurrentPosition { get; private set; } = Position2D.Empty;

    public static ELanguage CurrentLanguage { get; set; } = ELanguage.EN;
    public static byte CurrentFrame { get; set; } = (byte)EFrame.Minimum;
    public static int CurrentWidth { get; set; } = Global.WINDOW_WIDTH;
    public static int CurrentHeight { get; set; } = Global.WINDOW_HEIGHT;
    public static byte CurrentSoundVolume { get; set; } = (byte)EVolume.Maximum;
    public static byte CurrentSoundtrackVolume { get; set; } = (byte)EVolume.Maximum;
}
