namespace GameUI.Core;

public sealed class GameEnvironment
{
    public const byte VECTOR = 32;
    public const byte MAX_ROW = 24;
    public const byte MAX_COLUMN = 44;

    #region Property
    private TileTexture TileTexture { get; set; }
    #endregion

    #region Event
    public event EventHandler<EventArgs?>? OnTileTexture;
    public void Invoke(Event gameEvent, object? sender)
    {
        if (gameEvent is Event.TileTexture && sender is TileTexture texture)
            OnTileTexture?.Invoke(TileTexture = texture, default);
    }
    #endregion
}
