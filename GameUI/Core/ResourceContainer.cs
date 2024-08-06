namespace GameUI.Core;

public sealed class ResourceContainer
{
    private readonly Dictionary<Tile, GraphicsImage> TileResources = [];
    private readonly Dictionary<Sprite, GraphicsImage> SpriteResources = [];
    private readonly Dictionary<Shader, GraphicsImage> ShaderResources = [];

    #region Action
    public GraphicsImage GetResource(Tile tile) => TileResources[tile];
    public GraphicsImage GetResource(Sprite sprite) => SpriteResources[sprite];
    public GraphicsImage GetResource(Shader shader) => ShaderResources[shader];

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
    private async Task Load<T>(Dictionary<T, GraphicsImage> container) where T : Enum
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
