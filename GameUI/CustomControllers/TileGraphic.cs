using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace GameUI.CustomControllers;

public enum ETileTexture : byte
{
    Color,
    Image,
}

public enum ETile : byte
{
    None,
    Ground,
    Grass,
    Road,
    Desert,
    Water,
}

public interface ITile
{
    public ETile Tile { get; set; }
    public bool IsRunning { get; set; }
    public void LoadContents();
    public void LoadListerners();
    public void Draw();
    public Task Update();
    public void Dispose();
    public void OnLoaded(object sender, EventArgs e);
    public void OnUnloaded(object sender, EventArgs e);
}

public sealed class TileGraphic : GraphicsView, ITile, IDisposable
{
    #region Linked Tile
    private static TileGraphic? SelectedTile;
    #endregion

    #region CTO
    public TileGraphic()
    {
        LoadContents();
        LoadListerners();
    }

    ~TileGraphic() => Handler?.DisconnectHandler();
    #endregion

    #region Property
    public bool IsRunning { get; set; }

    private readonly TileCanvas Canvas = new();
    #endregion

    #region Bindable Property
    public ETile Tile
    {
        get { return (ETile)GetValue(TileProperty); }
        set { SetValue(TileProperty, value); }
    }

    internal static readonly BindableProperty TileProperty = BindableProperty.Create(
        nameof(Tile), typeof(ETile), typeof(TileGraphic), default(ETile),
        propertyChanged: (BindableObject tile, object old, object value) => tile.SetValue(TileProperty, (ETile)value));
    #endregion

    #region Event
    public void OnLoaded(object sender, EventArgs e) => IsRunning = true;
    public void OnUnloaded(object sender, EventArgs e) => IsRunning = false;
    public void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (SelectedTile is TileGraphic) SelectedTile.Scale = 1;

        SelectedTile = sender as TileGraphic;
        SelectedTile.Scale = 0.97;
    }
    #endregion

    #region Action
    public void LoadContents()
    {
        Drawable = Canvas;
        WidthRequest = 32;
        HeightRequest = 32;
    }

    public void LoadListerners()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        StartInteraction += OnStartInteraction;

        Dispatcher.DispatchAsync(Update);
    }

    public async Task Update()
    {
        while (Tile is ETile.None) await Task.Delay(2000);

        while (IsRunning)
        {
            Draw();
            Invalidate();
            await Task.Delay(AuxFun.random.Next(2000));
        }
    }

    public void Draw()
    {
        Opacity += Opacity >= 0.95 ? -0.05 : 0.05;
        Canvas.Tile = Tile;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}

public sealed class TileCanvas : IDrawable
{
    #region Linked
    private static readonly Dictionary<ETile, Color> TileColor = [];
    private static readonly Dictionary<ETile, IImage> TileImage = [];
    public static ETileTexture TileTexture { get; set; }
    #endregion

    #region Property
    public ETile Tile { get; set; }
    #endregion

    static TileCanvas()
    {
        Dispatcher
            .GetForCurrentThread()
            .DispatchAsync(async Task () =>
            {
                foreach (ETile tile in Enum.GetValues<ETile>())
                {
                    if (tile is ETile.None) continue;

                    string name = $"{Enum.GetName(tile)}.png".ToLower();

                    using var stream = await FileSystem.OpenAppPackageFileAsync(name);

                    var color = tile switch
                    {
                        ETile.Grass => Colors.DarkSeaGreen,
                        ETile.Ground => Colors.SandyBrown,
                        ETile.Road => Colors.LightGray,
                        ETile.Water => Colors.CornflowerBlue,
                        ETile.Desert => Colors.Beige,
                        _ => Colors.Transparent,
                    };

                    TileColor.Add(tile, color);
                    TileImage.Add(tile, PlatformImage.FromStream(stream));
                    stream.Close();
                }
            });
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Tile is ETile.None || TileImage.Count <= 0 || TileColor.Count <= 0) return;

        canvas.Antialias = true;

        if (TileTexture is ETileTexture.Image)
        {
            canvas.FillColor = Colors.Transparent;
            canvas.DrawImage(TileImage[Tile], 0, 0, 32, 32);
        }

        if (TileTexture is ETileTexture.Color)
        {
            canvas.FillColor = TileColor[Tile];
            canvas.FillRectangle(0, 0, 32, 32);
        }
    }
}
