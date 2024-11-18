using System.Xml.Serialization;

namespace SFMLGame.pipeline;

public static class FileHandler
{
    public static string MainFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Global.TITLE);

    #region Schema
    public static void SerializeSchema<TSchema>(EFolder innerFolder, TSchema schema, string name) where TSchema : class
    {
        string path = $"{MainFolder}/{innerFolder}/{name}.xml";

        var serializer = new XmlSerializer(typeof(TSchema));

        using StreamWriter writer = new(path);

        serializer.Serialize(writer, schema);

        Global.Invoke(EEvent.LoggerChanged, new Logger(ELogger.General, $"Schema saved : {name}"));
    }

    public static TSchema DeserializeSchema<TSchema>(EFolder innerFolder, string name) where TSchema : class
    {
        string path = $"{MainFolder}/{innerFolder}/{name}.xml";

        TSchema schema = (TSchema)Activator.CreateInstance(typeof(TSchema))!;

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(TSchema));

            using StreamReader reader = new(path);

            schema = (TSchema)serializer.Deserialize(reader)!;

            Global.Invoke(EEvent.LoggerChanged, new Logger(ELogger.General, $"Schema loaded: {name}"));
        }

        return schema;
    }
    #endregion

    #region Resolve
    public static string ResourcePath(Enum resource, string suffix)
    {
        Type type = resource.GetType();

        var folder = type.Name[1..];

        var name = Enum.GetName(type, resource);

        return $"./resources/{folder}/{name}.{suffix}".ToLower();
    }
    #endregion
}
