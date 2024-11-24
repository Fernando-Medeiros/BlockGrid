namespace SFMLGame.pipeline.schemas;

public sealed class ConfigurationSchema : IPrincipalSchema
{
    public string Token { get; set; } = "configuration";
    public ELanguage Language { get; set; } = ELanguage.EN;
    public byte Frame { get; set; } = (byte)EFrame.Minimum;
    public byte MusicVolume { get; set; } = (byte)EMusicVolume.S4;
    public byte SoundVolume { get; set; } = (byte)ESoundVolume.S4;
    public byte WindowMode { get; set; } = (byte)EWindowMode.Fullscreen;
    public (int Width, int Height) WindowResolution { get; set; } = Factory.Resolution(EWindowResolution.R_1024x768);
}
