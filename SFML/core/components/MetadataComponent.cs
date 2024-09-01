namespace SFMLGame.core.components;

public sealed class MetadataComponent : IMetadataComponent
{
    private bool flipped = false;
    public bool IsFlipped() => flipped;

    public void FlipTo(object? keyCode)
    {
        flipped = keyCode switch
        {
            Key.A or Key.Left => false,
            Key.D or Key.Right => true,
            _ => flipped,
        };
    }
}
