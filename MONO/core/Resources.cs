using Microsoft.Xna.Framework.Content;

namespace MONOGame.Core;

public sealed class Resources
{
    private ContentManager? Content { get; set; }

    private readonly Dictionary<Fonte, SpriteFont> FontResources = [];
    private readonly Dictionary<Sprite2D, Texture2D> SpriteResources = [];
    private readonly Dictionary<Surface2D, Texture2D> SurfaceResources = [];

    #region Action
    public SpriteFont GetResource(Fonte font) => FontResources[font];
    public Texture2D GetResource(Sprite2D sprite) => SpriteResources[sprite];
    public Texture2D GetResource(Surface2D surface) => SurfaceResources[surface];

    public void SetContent(ContentManager content) => Content = content;

    public void LoadResources()
    {
        Load(FontResources);
        Load(SpriteResources);
        Load(SurfaceResources);
    }
    #endregion

    #region Build
    private void Load<T, C>(Dictionary<T, C> container) where T : Enum where C : class
    {
        foreach (T key in Enum.GetValues(typeof(T)))
        {
            var folder = typeof(T).Name;

            var fileName = Enum.GetName(typeof(T), key);

            var path = $"{folder}/{fileName}".ToLower();

            if (typeof(C) == typeof(SpriteFont))
                container.Add(key, Content?.Load<SpriteFont>(path) as C);

            if (typeof(C) == typeof(Texture2D))
                container.Add(key, Content?.Load<Texture2D>(path) as C);
        }
    }

    public async void LoadScene()
    {
        string filePath = "content/raw/TierE.json";

        if (File.Exists(filePath))
        {
            var jsonString = await File.ReadAllTextAsync(filePath);
            var scenePackage = JsonSerializer.Deserialize<ScenePackage>(jsonString);

            App.Global.Invoke(CoreEvent.LoadScene, scenePackage);
        }
    }
    #endregion
}
