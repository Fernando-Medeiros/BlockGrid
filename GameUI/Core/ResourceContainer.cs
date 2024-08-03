namespace GameUI.Core;

public sealed class ResourceContainer
{
    private readonly Dictionary<Tile, string> ASCIIResource = [];
    private readonly Dictionary<Tile, GraphicsColor> ColorResource = [];
    private readonly Dictionary<Tile, GraphicsImage> TileImageResource = [];
    private readonly Dictionary<Sprite, GraphicsImage> SpriteImageResource = [];
    private readonly Dictionary<Shader, GraphicsImage> ShaderImageResource = [];

    #region Action
    public object GetResource(Sprite sprite) => SpriteImageResource[sprite];
    public object GetResource(Shader shader) => ShaderImageResource[shader];
    public object GetResource(TileTexture texture, Tile tile) => texture switch
    {
        TileTexture.ASCII => ASCIIResource[tile],
        TileTexture.Color => ColorResource[tile],
        TileTexture.Image => TileImageResource[tile],
        _ => GetResource(texture, tile)
    };
    public async Task LoadResourcesAsync()
    {
        await Task.WhenAll([LoadSprites(), LoadShaders(), LoadTiles()]);

        Invoke(Event.LoadResource, true);
    }
    #endregion

    #region Event
    public event EventHandler<EventArgs?>? OnLoadResource;
    public void Invoke(Event @event, object? args)
    {
        if (@event is Event.LoadResource)
            OnLoadResource?.Invoke(args, default);
    }
    #endregion

    #region Load
    private async Task LoadSprites()
    {
        foreach (var sprite in Enum.GetValues<Sprite>())
        {
            string name = $"{Enum.GetName(sprite)}.png".ToLower();

            using var stream = await FileSystem.OpenAppPackageFileAsync(name);

            SpriteImageResource.Add(sprite, PlatformImage.FromStream(stream));
            stream.Close();
        }
    }

    private async Task LoadShaders()
    {
        foreach (var shader in Enum.GetValues<Shader>())
        {
            string name = $"{Enum.GetName(shader)}.png".ToLower();

            using var stream = await FileSystem.OpenAppPackageFileAsync(name);

            ShaderImageResource.Add(shader, PlatformImage.FromStream(stream));
            stream.Close();
        }
    }

    private async Task LoadTiles()
    {
        foreach (Tile tile in Enum.GetValues<Tile>())
        {
            string name = $"{Enum.GetName(tile)}.png".ToLower();

            using var stream = await FileSystem.OpenAppPackageFileAsync(name);

            var ascii = tile switch
            {
                Tile.Grass => ";;.;;;;.;;.",
                Tile.House => "|||||||",
                Tile.Road => "]]]]]]]]]",
                Tile.Water => "~~~~~~~",
                Tile.Desert => "::::::::",
                _ => ".",
            };
            var color = tile switch
            {
                Tile.Grass => Colors.DarkSeaGreen,
                Tile.House => Colors.SandyBrown,
                Tile.Road => Colors.LightGray,
                Tile.Water => Colors.CornflowerBlue,
                Tile.Desert => Colors.Beige,
                _ => Colors.Transparent,
            };

            ASCIIResource.Add(tile, ascii);
            ColorResource.Add(tile, color);
            TileImageResource.Add(tile, PlatformImage.FromStream(stream));
            stream.Close();
        }
    }
    #endregion
}
