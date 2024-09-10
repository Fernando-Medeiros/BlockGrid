using System.Text.Json;

namespace SFMLGame.core;

public static class Content
{
    private static bool _started;
    private static readonly Dictionary<EFont, Font> FontResources = [];
    private static readonly Dictionary<EIcon, Sprite> IconResources = [];
    private static readonly Dictionary<ESprite, Sprite> SpriteResources = [];
    private static readonly Dictionary<EPicture, Sprite> PictureResources = [];
    private static readonly Dictionary<ESurface, Sprite> SurfaceResources = [];

    #region Action
    public static Font GetResource(EFont font) => FontResources[font];
    public static Sprite GetResource(EIcon icon) => IconResources[icon];
    public static Sprite GetResource(ESprite sprite) => SpriteResources[sprite];
    public static Sprite GetResource(EPicture picture) => PictureResources[picture];
    public static Sprite GetResource(ESurface surface) => SurfaceResources[surface];

    public static void LoadResources()
    {
        if (_started) return;
        Load(FontResources);
        Load(IconResources);
        Load(SpriteResources);
        Load(PictureResources);
        Load(SurfaceResources);
        _started = true;
    }
    #endregion

    #region Build
    private static void Load<T, C>(Dictionary<T, C> container) where T : Enum where C : class
    {
        var suffix = typeof(T) == typeof(EFont) ? ".ttf" : ".png";

        foreach (T key in Enum.GetValues(typeof(T)))
        {
            var folder = typeof(T).Name[1..];

            var fileName = Enum.GetName(typeof(T), key);

            var path = $"./resources/{folder}/{fileName}{suffix}".ToLower();

            if (typeof(T) == typeof(EFont))
            {
                container.Add(key, new Font(path) as C);
                continue;
            }
            container.Add(key, new Sprite(new Texture(path)) as C);
        }
    }

    // TODO :: Trabalhar no Mapa
    static string map = "TierE";
    public static async void LoadScene()
    {
        string filePath = $"./resources/raw/{map}.json";

        map = map == "TierE" ? "TierF" : "TierE";

        if (File.Exists(filePath))
        {
            var jsonString = await File.ReadAllTextAsync(filePath);
            var scenePackage = JsonSerializer.Deserialize<RegionDTO>(jsonString);

            Global.Invoke(EEvent.Region, scenePackage);
        }
    }
    #endregion
}
