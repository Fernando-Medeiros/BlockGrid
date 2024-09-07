namespace SFMLGame.core.scenes.world;

public sealed class WorldMapHUD : IGameObject
{
    private WorldView? World { get; set; }

    #region Build
    public void LoadEvents(RenderWindow window) => World?.LoadEvents(window);

    public void LoadContent()
    {
        World = new(new FloatRect(0, 0, 3000, 3000))
        {
            Viewport = new(0.85f, 0, 0.15f, 0.15f)
        };
    }

    public void Draw(RenderWindow window) => World?.Draw(window);
    #endregion

    #region Event
    #endregion
}
