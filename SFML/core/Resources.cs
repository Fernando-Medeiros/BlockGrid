using System.Text.Json;

namespace SFMLGame.Core;

public sealed class Resources
{
    private readonly Dictionary<Fonte, Font> FontResources = [];
    private readonly Dictionary<Sprite2D, Sprite> SpriteResources = [];
    private readonly Dictionary<Surface2D, Sprite> SurfaceResources = [];

    #region Action
    public Font GetResource(Fonte font) => FontResources[font];
    public Sprite GetResource(Sprite2D sprite) => SpriteResources[sprite];
    public Sprite GetResource(Surface2D surface) => SurfaceResources[surface];

    public void LoadResources()
    {
        Load(FontResources);
        Load(SpriteResources);
        Load(SurfaceResources);
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

    public async void LoadScene()
    {
        string filePath = "./resources/raw/TierE.json";

        if (File.Exists(filePath))
        {
            var jsonString = await File.ReadAllTextAsync(filePath);
            var scenePackage = JsonSerializer.Deserialize<ScenePackage>(jsonString);

            App.Global.Invoke(CoreEvent.LoadScene, scenePackage);
        }
    }
    #endregion
}
