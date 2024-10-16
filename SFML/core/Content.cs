﻿using SFML.Audio;
using System.Xml.Serialization;

namespace SFMLGame.core;

public static class Content
{
    private static readonly Dictionary<EFont, Font> FontResources = [];
    private static readonly Dictionary<EIcon, Sprite> IconResources = [];
    private static readonly Dictionary<ESprite, Sprite> SpriteResources = [];
    private static readonly Dictionary<EPicture, Sprite> PictureResources = [];
    private static readonly Dictionary<ETerrain, Sprite> TerrainResources = [];
    private static readonly Dictionary<ESound, Sound> SoundResources = [];
    private static readonly Dictionary<EGraphic, Sprite> GraphicResources = [];

    #region Action
    public static Font GetResource(EFont font) => Load(font, FontResources);
    public static Sprite GetResource(EIcon icon) => Load(icon, IconResources);
    public static Sprite GetResource(ESprite sprite) => Load(sprite, SpriteResources);
    public static Sprite GetResource(ETerrain terrain) => Load(terrain, TerrainResources);
    public static Sound GetResource(ESound sound) => Load(sound, SoundResources);
    public static Sprite GetResource(EPicture picture) => Load(picture, PictureResources);
    public static Sprite GetResource(EGraphic graphic) => Load(graphic, GraphicResources);

    public static void PlayMusic()
    {
        App.CurrentMusic?.Stop();
        App.CurrentMusic?.Dispose();

        App.CurrentMusic = new($"./resources/music/{Enum.GetName(App.CurrentBiome)}.mp3".ToLower());

        App.CurrentMusic.Volume = App.CurrentMusicVolume;
        App.CurrentMusic.Loop = true;
        App.CurrentMusic.Play();
    }
    #endregion

    #region Resource
    private static Sprite Load<T>(T enumValue, Dictionary<T, Sprite> container) where T : Enum
    {
        if (container.TryGetValue(enumValue, out Sprite? value)) return value;

        Sprite resource = new(new Texture(Path(enumValue, "png")));
        container.Add(enumValue, resource);
        return resource;
    }

    private static Font Load<T>(T enumValue, Dictionary<T, Font> container) where T : Enum
    {
        if (container.TryGetValue(enumValue, out Font? value)) return value;

        Font resource = new(Path(enumValue, "ttf"));
        container.Add(enumValue, resource);
        return resource;
    }

    private static Sound Load<T>(T enumValue, Dictionary<T, Sound> container) where T : Enum
    {
        if (container.TryGetValue(enumValue, out Sound? value)) return value;

        Sound resource = new(new SoundBuffer(Path(enumValue, "ogg")));
        container.Add(enumValue, resource);
        return resource;
    }
    #endregion

    #region Region
    public static void SerializeSchema(RegionSchema schema)
    {
        string path = Path("region", $"{schema.Name}.xml");

        var serializer = new XmlSerializer(typeof(RegionSchema));

        using StreamWriter writer = new(path);

        serializer.Serialize(writer, schema);

        Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.General, $"Region {schema.Name} Saved"));
    }

    public static void DeserializeSchema(string fileName)
    {
        string path = Path("region", $"{fileName}.xml");

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

    #region IO
    private static string Path(string folder, string file)
    {
        string document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string path = $"{document}/{folder}";

        if (Directory.Exists(path) is false) Directory.CreateDirectory(path);

        return $"{path}/{file}";
    }

    private static string Path(Enum resource, string suffix)
    {
        Type type = resource.GetType();

        var folder = type.Name[1..];

        var name = Enum.GetName(type, resource);

        return $"./resources/{folder}/{name}.{suffix}".ToLower();
    }
    #endregion
}
