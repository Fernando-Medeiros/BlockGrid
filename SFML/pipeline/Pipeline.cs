namespace SFMLGame.pipeline;

public sealed class Pipeline
{
    public string Folder => Global.TITLE;
    public string Path => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public void ConfigureFolders()
    {
        string mainFolder = $"{Path}/{Folder}";

        if (Directory.Exists(mainFolder) is false)
            Directory.CreateDirectory(mainFolder);

        foreach (var innerFolder in Enum.GetNames<EFolder>())
        {
            string currentFolder = $"{mainFolder}/{innerFolder}";

            if (Directory.Exists(currentFolder)) continue;

            Directory.CreateDirectory(currentFolder);
        }
    }
}
