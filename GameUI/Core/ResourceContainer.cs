namespace GameUI.Core;

public sealed class ResourceContainer
{
    private readonly Dictionary<ETile, string> ASCIIResource = [];
    private readonly Dictionary<ETile, GraphicsColor> MonoResource = [];
    private readonly Dictionary<ETile, GraphicsColor> ColorResource = [];
    private readonly Dictionary<ETile, GraphicsImage> ImageResource = [];

    public bool Available { get; private set; }

    public object GetResource(ETileTexture texture, ETile tile)
    {
        return texture switch
        {
            ETileTexture.Mono => MonoResource[tile],
            ETileTexture.ASCII => ASCIIResource[tile],
            ETileTexture.Color => ColorResource[tile],
            ETileTexture.Image => ImageResource[tile],
            _ => throw new ArgumentException(nameof(GetResource))
        };
    }

    public async Task LoadResourcesAsync()
    {
        foreach (ETile tile in Enum.GetValues<ETile>())
        {
            string name = $"{Enum.GetName(tile)}.png".ToLower();

            using var stream = await FileSystem.OpenAppPackageFileAsync(name);

            var tileAscii = tile switch
            {
                ETile.Grass => ";;.;;;;.;;.",
                ETile.Ground => "|||||||",
                ETile.Road => "]]]]]]]]]",
                ETile.Water => "~~~~~~~",
                ETile.Desert => "::::::::",
                _ => ".",
            };
            var tileMono = tile switch
            {
                ETile.Grass => Colors.Gray,
                ETile.Ground => Colors.LightSlateGray,
                ETile.Road => Colors.GhostWhite,
                ETile.Water => Colors.AntiqueWhite,
                ETile.Desert => Colors.LightGray,
                _ => Colors.Transparent,
            };
            var tileColor = tile switch
            {
                ETile.Grass => Colors.DarkSeaGreen,
                ETile.Ground => Colors.SandyBrown,
                ETile.Road => Colors.LightGray,
                ETile.Water => Colors.CornflowerBlue,
                ETile.Desert => Colors.Beige,
                _ => Colors.Transparent,
            };

            ASCIIResource.Add(tile, tileAscii);
            MonoResource.Add(tile, tileMono);
            ColorResource.Add(tile, tileColor);
            ImageResource.Add(tile, PlatformImage.FromStream(stream));

            stream.Close();
        }
        Available = true;
    }
}
