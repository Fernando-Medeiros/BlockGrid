namespace SFMLGame.pipeline;

public sealed class Pipeline
{
    public void ConfigureFolders()
    {
        string mainFolder = FileHandler.MainFolder;

        if (Directory.Exists(mainFolder) is false) Directory.CreateDirectory(mainFolder);

        foreach (var innerFolder in Enum.GetNames<EFolder>())
        {
            string currentFolder = $"{mainFolder}/{innerFolder}";

            if (Directory.Exists(currentFolder)) continue;

            Directory.CreateDirectory(currentFolder);
        }
    }

    public void ConfigureOptions()
    {
        var schema = FileHandler.DeserializeSchema<ConfigurationSchema>(EFolder.Options, "configuration");

        Global.Invoke(EEvent.SchemaChanged, schema);
    }
}
