namespace SFMLGame.core;

public static class Content
{
    private static readonly Dictionary<EFont, Font> FontResources = [];
    private static readonly Dictionary<EIcon, Sprite> IconResources = [];
    private static readonly Dictionary<ERace, Sprite> RaceResources = [];
    private static readonly Dictionary<ESprite, Sprite> SpriteResources = [];
    private static readonly Dictionary<EPicture, Sprite> PictureResources = [];
    private static readonly Dictionary<ETerrain, Sprite> TerrainResources = [];
    private static readonly Dictionary<ESound, Sound> SoundResources = [];
    private static readonly Dictionary<EGraphic, Sprite> GraphicResources = [];
    private static readonly Dictionary<EAlignment, Sprite> AlignmentResources = [];
    private static readonly Dictionary<EProfession, Sprite> ProfessionResources = [];
    private static readonly Dictionary<EProficiency, Sprite> ProficiencyResources = [];

    #region Action
    public static T GetResource<T>(object? resourceEnum) where T : class
    {
        T? valueResult = null;

        if (resourceEnum is EFont font) valueResult = Load(font, FontResources) as T;
        if (resourceEnum is ESound sound) valueResult = Load(sound, SoundResources) as T;
        if (resourceEnum is EIcon icon) valueResult = Load(icon, IconResources) as T;
        if (resourceEnum is ERace race) valueResult = Load(race, RaceResources) as T;
        if (resourceEnum is ESprite sprite) valueResult = Load(sprite, SpriteResources) as T;
        if (resourceEnum is ETerrain terrain) valueResult = Load(terrain, TerrainResources) as T;
        if (resourceEnum is EPicture picture) valueResult = Load(picture, PictureResources) as T;
        if (resourceEnum is EGraphic graphic) valueResult = Load(graphic, GraphicResources) as T;
        if (resourceEnum is EAlignment alignment) valueResult = Load(alignment, AlignmentResources) as T;
        if (resourceEnum is EProfession profession) valueResult = Load(profession, ProfessionResources) as T;
        if (resourceEnum is EProficiency proficiency) valueResult = Load(proficiency, ProficiencyResources) as T;

        if (valueResult is null)
            throw new ArgumentException("Tipo de recurso desconhecido", nameof(resourceEnum));

        return valueResult;
    }

    public static void PlayMusic()
    {
        App.CurrentMusic?.Stop();
        App.CurrentMusic?.Dispose();

        string? name = Enum.GetName(App.Region.Biome);

        App.CurrentMusic = new($"./resources/music/{name}.mp3".ToLower())
        {
            Volume = App.Configuration.MusicVolume,
            Loop = true
        };
        App.CurrentMusic.Play();

        Global.Invoke(EEvent.LoggerChanged, new Logger(ELogger.General, $"Load music: {name}"));
    }
    #endregion

    #region Resource
    private static Sprite Load<TEnum>(TEnum enumValue, Dictionary<TEnum, Sprite> container) where TEnum : Enum
    {
        if (container.TryGetValue(enumValue, out Sprite? value)) return value;

        Sprite resource = new(new Texture(FileHandler.ResourcePath(enumValue, "png")));
        container.Add(enumValue, resource);
        return resource;
    }

    private static Font Load<TEnum>(TEnum enumValue, Dictionary<TEnum, Font> container) where TEnum : Enum
    {
        if (container.TryGetValue(enumValue, out Font? value)) return value;

        Font resource = new(FileHandler.ResourcePath(enumValue, "ttf"));
        container.Add(enumValue, resource);
        return resource;
    }

    private static Sound Load<TEnum>(TEnum enumValue, Dictionary<TEnum, Sound> container) where TEnum : Enum
    {
        if (container.TryGetValue(enumValue, out Sound? value)) return value;

        Sound resource = new(new SoundBuffer(FileHandler.ResourcePath(enumValue, "ogg")));
        container.Add(enumValue, resource);
        return resource;
    }
    #endregion
}
