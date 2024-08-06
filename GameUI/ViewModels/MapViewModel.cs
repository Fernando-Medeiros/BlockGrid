namespace GameUI.ViewModels;

public sealed partial class MapViewModel : BaseViewModel
{
    public MapViewModel()
    {
        LoadCommand = new Command(LoadChanged);
    }

    #region Property
    #endregion

    #region Command
    public ICommand LoadCommand { get; }
    #endregion

    #region Action
    private async void LoadChanged(object sender)
    {
        if (IsBusy()) return;

        using var stream = await FileSystem.OpenAppPackageFileAsync($"Tier{sender}.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        stream.Close();
        reader.Close();

        var scenePackage = JsonSerializer.Deserialize<ScenePackage>(contents);

        App.Invoke(Event.LoadScene, scenePackage);

        NotBusy();
    }
    #endregion
}
