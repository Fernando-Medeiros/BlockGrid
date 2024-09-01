using System.Text.Json;

namespace SFMLGame.core;

public static class Content
{
    private static bool _started;
    private static readonly Dictionary<Fonte, Font> FontResources = [];
    private static readonly Dictionary<Sprite2D, Sprite> SpriteResources = [];
    private static readonly Dictionary<Surface2D, Sprite> SurfaceResources = [];

    #region Action
    public static Font GetResource(Fonte font) => FontResources[font];
    public static Sprite GetResource(Sprite2D sprite) => SpriteResources[sprite];
    public static Sprite GetResource(Surface2D surface) => SurfaceResources[surface];

    public static void LoadResources()
    {
        if (_started) return;
        Load(FontResources);
        Load(SpriteResources);
        Load(SurfaceResources);
        _started = true;
    }
    #endregion

    #region Build
    private static void Load<T, C>(Dictionary<T, C> container) where T : Enum where C : class
    {
        var sufix = typeof(T) == typeof(Fonte) ? ".ttf" : ".png";

        foreach (T key in Enum.GetValues(typeof(T)))
        {
            var folder = typeof(T).Name;

            var fileName = Enum.GetName(typeof(T), key);

            var path = $"./resources/{folder}/{fileName}{sufix}".ToLower();

            if (typeof(C) == typeof(Font))
                container.Add(key, new Font(path) as C);

            if (typeof(C) == typeof(Sprite))
                container.Add(key, new Sprite(new Texture(path)) as C);
        }
    }

    public static async void LoadScene()
    {
        string filePath = "./resources/raw/TierE.json";

        if (File.Exists(filePath))
        {
            var jsonString = await File.ReadAllTextAsync(filePath);
            var scenePackage = JsonSerializer.Deserialize<ScenePackage>(jsonString);

            Global.Invoke(EEvent.LoadScene, scenePackage);
        }
    }
    #endregion
}
