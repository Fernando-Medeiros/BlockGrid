namespace SFMLGame.core.scenes.world;

public sealed class WorldMapHUD : IGameObject, IDisposable
{
    private IGameObject? GameObject { get; set; }

    #region Build
    public void LoadContent()
    {
        GameObject = new WorldMapView(new FloatRect(0, 0, Global.WORLD_WIDTH / 2f, Global.WORLD_HEIGHT / 2f));
    }

    public void LoadEvents() => GameObject?.LoadEvents();

    public void Draw(RenderWindow window) => GameObject?.Draw(window);
    #endregion

    #region Event
    #endregion

    #region Dispose
    public void Dispose()
    {
        GameObject?.Dispose();
        GameObject = null;
    }
    #endregion
}
