using System.Xml.Serialization;

namespace SFMLGame.core;

public static class Content
{
    private static bool _started;
    private static readonly Dictionary<EFont, Font> FontResources = [];
    private static readonly Dictionary<EIcon, Sprite> IconResources = [];
    private static readonly Dictionary<ESprite, Sprite> SpriteResources = [];
    private static readonly Dictionary<EPicture, Sprite> PictureResources = [];

    #region Action
    public static Font GetResource(EFont font) => FontResources[font];
    public static Sprite GetResource(EIcon icon) => IconResources[icon];
    public static Sprite GetResource(ESprite sprite) => SpriteResources[sprite];
    public static Sprite GetResource(EPicture picture) => PictureResources[picture];

    public static void LoadResources()
    {
        if (_started) return;
        Load(FontResources);
        Load(IconResources);
        Load(SpriteResources);
        Load(PictureResources);
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

    public static void SerializeSchema(RegionSchema schema, string fileName)
    {
        string path = $"./resources/raw/{fileName}.xml";

        var serializer = new XmlSerializer(typeof(RegionSchema));

        using StreamWriter writer = new(path);

        serializer.Serialize(writer, schema);

        Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.General, $"Region {schema.Name} Saved"));
    }

    public static void DeserializeSchema(string fileName)
    {
        string path = $"./resources/raw/{fileName}.xml";

        RegionSchema? schema = null;

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(RegionSchema));

            using StreamReader reader = new(path);

            schema = serializer.Deserialize(reader) as RegionSchema;
        }

        Global.Invoke(EEvent.Region, schema);

        Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.General, $"Region {schema?.Name} Loaded"));
    }
    #endregion
}
