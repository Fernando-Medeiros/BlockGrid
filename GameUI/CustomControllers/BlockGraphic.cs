using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace GameUI.CustomControllers;

public enum ETile : byte
{
    None,
    Ground,
    Grass,
    Road,
    Desert,
    Water,
}

public interface IBlock
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

public sealed class BlockGraphic : GraphicsView, IBlock, IDisposable
{
    #region Linked Blocks
    private static BlockGraphic? SelectedBlock;
    #endregion

    #region CTO
    public BlockGraphic()
    {
        LoadContents();
        LoadListerners();
    }

    ~BlockGraphic() => Handler?.DisconnectHandler();
    #endregion

    #region Properties
    public bool IsRunning { get; set; }

    private readonly Drawable drawable = new();
    #endregion

    #region Bindable Property
    public ETile Tile
    {
        get { return (ETile)GetValue(TileProperty); }
        set { SetValue(TileProperty, value); }
    }

    internal static readonly BindableProperty TileProperty = BindableProperty.Create(
        nameof(Tile), typeof(ETile), typeof(BlockGraphic), default(ETile),
        propertyChanged: (BindableObject block, object old, object value) => block.SetValue(TileProperty, (ETile)value));
    #endregion

    #region Events
    public void OnLoaded(object sender, EventArgs e) => IsRunning = true;
    public void OnUnloaded(object sender, EventArgs e) => IsRunning = false;
    public void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (SelectedBlock != null) SelectedBlock.Scale = 1;

        SelectedBlock = sender as BlockGraphic;
        SelectedBlock.Scale = 0.97;
    }
    #endregion

    #region Actions
    public void LoadContents()
    {
        Drawable = drawable;
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
        drawable.Tile = Tile;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}

public sealed class Drawable : IDrawable
{
    private static readonly Dictionary<ETile, IImage> Images = [];

    public ETile Tile { get; set; }

    static Drawable()
    {
        Dispatcher
            .GetForCurrentThread()
            .DispatchAsync(async Task () =>
            {
                foreach (ETile tile in Enum.GetValues<ETile>())
                {
                    if (tile is ETile.None) continue;

                    string fileName = $"{Enum.GetName(tile)}.png".ToLower();

                    using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);

                    Images[tile] = (PlatformImage.FromStream(stream));
                    stream.Close();
                }
            });
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Tile is ETile.None || Images.Count <= 0) return;

        canvas.Antialias = true;
        canvas.DrawImage(Images[Tile], 0, 0, 32, 32);
    }
}
