using System.Windows.Input;

namespace GameUI.ViewModels;

public sealed record Tier(IList<IList<ETile>> Matriz);

public sealed partial class MapViewModel : BaseViewModel
{
    public MapViewModel()
    {
        LoadCommand = new Command(Load);
    }

    #region Property
    public ObservableCollection<ETile> Row01 { get; set; } = [];
    public ObservableCollection<ETile> Row02 { get; set; } = [];
    public ObservableCollection<ETile> Row03 { get; set; } = [];
    public ObservableCollection<ETile> Row04 { get; set; } = [];
    public ObservableCollection<ETile> Row05 { get; set; } = [];
    public ObservableCollection<ETile> Row06 { get; set; } = [];
    public ObservableCollection<ETile> Row07 { get; set; } = [];
    public ObservableCollection<ETile> Row08 { get; set; } = [];
    public ObservableCollection<ETile> Row09 { get; set; } = [];
    public ObservableCollection<ETile> Row10 { get; set; } = [];
    public ObservableCollection<ETile> Row11 { get; set; } = [];
    public ObservableCollection<ETile> Row12 { get; set; } = [];
    public ObservableCollection<ETile> Row13 { get; set; } = [];
    public ObservableCollection<ETile> Row14 { get; set; } = [];
    public ObservableCollection<ETile> Row15 { get; set; } = [];
    public ObservableCollection<ETile> Row16 { get; set; } = [];
    #endregion

    #region Command
    public ICommand LoadCommand { get; }
    #endregion

    #region Action
    private async void Load(object sender)
    {
        if (IsBusy()) return;

        using var stream = await FileSystem.OpenAppPackageFileAsync($"Tier{sender}.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        stream.Close();
        reader.Close();

        Tier tier = JsonSerializer.Deserialize<Tier>(contents);

        Row01.Clear(); Row02.Clear(); Row03.Clear(); Row04.Clear(); Row05.Clear(); Row06.Clear();
        Row07.Clear(); Row08.Clear(); Row09.Clear(); Row10.Clear(); Row11.Clear(); Row12.Clear();
        Row13.Clear(); Row14.Clear(); Row15.Clear(); Row16.Clear();

        Row01 = [.. tier.Matriz[0]];
        Row02 = [.. tier.Matriz[1]];
        Row03 = [.. tier.Matriz[2]];
        Row04 = [.. tier.Matriz[3]];
        Row05 = [.. tier.Matriz[4]];
        Row06 = [.. tier.Matriz[5]];
        Row07 = [.. tier.Matriz[6]];
        Row08 = [.. tier.Matriz[7]];
        Row09 = [.. tier.Matriz[8]];
        Row10 = [.. tier.Matriz[9]];
        Row11 = [.. tier.Matriz[10]];
        Row12 = [.. tier.Matriz[11]];
        Row13 = [.. tier.Matriz[12]];
        Row14 = [.. tier.Matriz[13]];
        Row15 = [.. tier.Matriz[14]];
        Row16 = [.. tier.Matriz[15]];

        OnPropertyChanged(nameof(Row01));
        OnPropertyChanged(nameof(Row02));
        OnPropertyChanged(nameof(Row03));
        OnPropertyChanged(nameof(Row04));
        OnPropertyChanged(nameof(Row05));
        OnPropertyChanged(nameof(Row06));
        OnPropertyChanged(nameof(Row07));
        OnPropertyChanged(nameof(Row08));
        OnPropertyChanged(nameof(Row09));
        OnPropertyChanged(nameof(Row10));
        OnPropertyChanged(nameof(Row11));
        OnPropertyChanged(nameof(Row12));
        OnPropertyChanged(nameof(Row13));
        OnPropertyChanged(nameof(Row14));
        OnPropertyChanged(nameof(Row15));
        OnPropertyChanged(nameof(Row16));

        NotBusy();
    }
    #endregion
}
