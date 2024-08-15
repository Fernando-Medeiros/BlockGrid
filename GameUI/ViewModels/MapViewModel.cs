namespace GameUI.ViewModels;

public sealed partial class MapViewModel : BaseViewModel
{
    public MapViewModel()
    {
        LoadCommand = new Command(LoadChanged);

        App.Subscribe(Event.Camera, CameraChanged);
    }

    #region Property
    public ScrollView? _scrollView { get; set; }
    #endregion

    #region Command
    public ICommand LoadCommand { get; }
    #endregion

    #region Action
    private void CameraChanged(object? sender)
    {
        if (Is.Not<Position2D>(sender)) return;

        var (row, column) = (Position2D)sender!;

        double scrollY = (row * GameEnvironment.VECTOR) - (_scrollView!.Height / 2);
        double scrollX = (column * GameEnvironment.VECTOR) - (_scrollView!.Width / 2);

        _scrollView.ScrollToAsync(scrollX, scrollY, false);
    }

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
