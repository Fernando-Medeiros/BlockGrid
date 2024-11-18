namespace SFMLGame.pipeline.schemas;

public sealed class ConfigurationSchema
{
    public ELanguage Language { get; set; } = ELanguage.EN;

    public byte Frame { get; set; } = (byte)EFrame.Minimum;

    public int WindowWidth { get; set; } = Global.WINDOW_WIDTH;

    public int WindowHeight { get; set; } = Global.WINDOW_HEIGHT;

    public byte MusicVolume { get; set; } = (byte)ESoundVolume.S4;

    public byte SoundVolume { get; set; } = (byte)ESoundVolume.S4;
}
