namespace MAUIGame.Core;

public sealed class ResourceContainer
{
    private readonly Dictionary<Tile2D, GraphicsImage> TileResources = [];
    private readonly Dictionary<Sprite2D, GraphicsImage> SpriteResources = [];
    private readonly Dictionary<Shader2D, GraphicsImage> ShaderResources = [];

    #region Action
    public GraphicsImage GetResource(Tile2D tile) => TileResources[tile];
    public GraphicsImage GetResource(Sprite2D sprite) => SpriteResources[sprite];
    public GraphicsImage GetResource(Shader2D shader) => ShaderResources[shader];

    public async Task LoadResourcesAsync()
    {
        await Task.WhenAll([
            Load(TileResources),
            Load(SpriteResources),
            Load(ShaderResources),
        ]);

        App.Invoke(Event.LoadResource, true);
    }
    #endregion

    #region Build
    private static async Task Load<T>(Dictionary<T, GraphicsImage> container) where T : Enum
    {
        foreach (T key in Enum.GetValues(typeof(T)))
        {
            string name = $"{Enum.GetName(typeof(T), key)}.png".ToLower();

            using var stream = await FileSystem.OpenAppPackageFileAsync(name);

            container.Add(key, PlatformImage.FromStream(stream));
            stream.Close();
        }
    }
    #endregion
}
