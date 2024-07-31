using System.Windows.Input;

namespace GameUI.ViewModels;

public sealed record Tier(IList<IList<ETile>> Matriz);

public sealed partial class MapViewModel : BaseViewModel
{
    public MapViewModel()
    {
        LoadCommand = new Command(LoadChanged);
        TextureCommand = new Command(TextureChanged);
    }

    #region Property
    public IList<ETile> Row01 { get; set; } = [];
    public IList<ETile> Row02 { get; set; } = [];
    public IList<ETile> Row03 { get; set; } = [];
    public IList<ETile> Row04 { get; set; } = [];
    public IList<ETile> Row05 { get; set; } = [];
    public IList<ETile> Row06 { get; set; } = [];
    public IList<ETile> Row07 { get; set; } = [];
    public IList<ETile> Row08 { get; set; } = [];
    public IList<ETile> Row09 { get; set; } = [];
    public IList<ETile> Row10 { get; set; } = [];
    public IList<ETile> Row11 { get; set; } = [];
    public IList<ETile> Row12 { get; set; } = [];
    public IList<ETile> Row13 { get; set; } = [];
    public IList<ETile> Row14 { get; set; } = [];
    public IList<ETile> Row15 { get; set; } = [];
    public IList<ETile> Row16 { get; set; } = [];
    public IList<ETile> Row17 { get; set; } = [];
    public IList<ETile> Row18 { get; set; } = [];
    public IList<ETile> Row19 { get; set; } = [];
    public IList<ETile> Row20 { get; set; } = [];
    public IList<ETile> Row21 { get; set; } = [];
    public IList<ETile> Row22 { get; set; } = [];
    public IList<ETile> Row23 { get; set; } = [];
    public IList<ETile> Row24 { get; set; } = [];
    #endregion

    #region Command
    public ICommand LoadCommand { get; }
    public ICommand TextureCommand { get; }
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

        Tier tier = JsonSerializer.Deserialize<Tier>(contents);
        
        Row01 = [.. tier.Matriz[0]]; Row02 = [.. tier.Matriz[1]]; Row03 = [.. tier.Matriz[2]];
        Row04 = [.. tier.Matriz[3]]; Row05 = [.. tier.Matriz[4]]; Row06 = [.. tier.Matriz[5]];
        Row07 = [.. tier.Matriz[6]]; Row08 = [.. tier.Matriz[7]]; Row09 = [.. tier.Matriz[8]];
        Row10 = [.. tier.Matriz[9]]; Row11 = [.. tier.Matriz[10]]; Row12 = [.. tier.Matriz[11]];
        Row13 = [.. tier.Matriz[12]]; Row14 = [.. tier.Matriz[13]]; Row15 = [.. tier.Matriz[14]];
        Row16 = [.. tier.Matriz[15]]; Row17 = [.. tier.Matriz[16]]; Row18 = [.. tier.Matriz[17]];
        Row19 = [.. tier.Matriz[18]]; Row20 = [.. tier.Matriz[19]]; Row21 = [.. tier.Matriz[20]];
        Row22 = [.. tier.Matriz[21]]; Row23 = [.. tier.Matriz[22]]; Row24 = [.. tier.Matriz[23]];

        OnPropertyChanged(nameof(Row01)); OnPropertyChanged(nameof(Row02)); OnPropertyChanged(nameof(Row03));
        OnPropertyChanged(nameof(Row04)); OnPropertyChanged(nameof(Row05)); OnPropertyChanged(nameof(Row06));
        OnPropertyChanged(nameof(Row07)); OnPropertyChanged(nameof(Row08)); OnPropertyChanged(nameof(Row09));
        OnPropertyChanged(nameof(Row10)); OnPropertyChanged(nameof(Row11)); OnPropertyChanged(nameof(Row12));
        OnPropertyChanged(nameof(Row13)); OnPropertyChanged(nameof(Row14)); OnPropertyChanged(nameof(Row15));
        OnPropertyChanged(nameof(Row16)); OnPropertyChanged(nameof(Row17)); OnPropertyChanged(nameof(Row18));
        OnPropertyChanged(nameof(Row19)); OnPropertyChanged(nameof(Row20)); OnPropertyChanged(nameof(Row21));
        OnPropertyChanged(nameof(Row22)); OnPropertyChanged(nameof(Row23)); OnPropertyChanged(nameof(Row24));

        NotBusy();
    }

    private void TextureChanged(object sender)
    {
        if (IsBusy()) return;
        TileCanvas.TileTexture = sender switch
        {
            "0" => ETileTexture.Color,
            "1" => ETileTexture.Image,
        };
        NotBusy();
    }
    #endregion
}
