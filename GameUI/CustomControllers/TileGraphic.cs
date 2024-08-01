using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace GameUI.CustomControllers;

#region Structure
public class Global
{
    public const byte MAX_ROW = 24;
    public const byte MAX_COLUMN = 44;
    public const byte VECTOR = 32;
}

public enum ETileTexture : byte
{
    ASCII,
    Color,
    Image,
    Mono,
}

public enum ETile : byte
{
    Ground,
    Grass,
    Road,
    Desert,
    Water,
}

public interface ITile
{
    public byte Row { get; }
    public ETile Tile { get; set; }
    public bool Running { get; set; }
    public TileCanvas Canvas { get; }
    public void Draw();
    public Task Update();
    public void OnSelect(object? sender, TouchEventArgs e);
}
#endregion

public sealed class TileGraphic : GraphicsView, ITile
{
    public TileGraphic()
    {
        Row = Total;
        Node[Row].Add(this);

        if (Node[Row].Count >= Global.MAX_COLUMN) Total++;

        Drawable = Canvas;
        Opacity = 0.8;
        WidthRequest = Global.VECTOR;
        HeightRequest = Global.VECTOR;

        StartInteraction += OnSelect;

        Dispatcher.DispatchAsync(Update);
    }

    #region Linked
    private static byte Total;
    private static TileGraphic? SelectedTile;
    private static readonly IList<IList<TileGraphic>> Node = [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];
    #endregion

    #region Property
    public byte Row { get; }
    public bool Running { get; set; } = true;
    public TileCanvas Canvas { get; } = new();
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
    public void OnSelect(object? sender, TouchEventArgs e)
    {
        SelectedTile?.FadeTo(0.8);
        SelectedTile = sender as TileGraphic;
        SelectedTile?.FadeTo(1);
    }
    #endregion

    #region Action
    public async Task Update()
    {
        while (Running)
        {
            Draw();
            Invalidate();
            await Task.Delay(AuxFun.random.Next(3000));
        }
    }

    public void Draw()
    {
        if (Opacity <= 0.8)
            Opacity += Opacity == 0.8 ? -0.02 : 0.02;

        Canvas.Tile = Tile;
    }
    #endregion
}

public sealed class TileCanvas : IDrawable
{
    #region Linked
    private static readonly Dictionary<ETile, string> ASCIIResource = [];
    private static readonly Dictionary<ETile, Color> MonoResource = [];
    private static readonly Dictionary<ETile, Color> ColorResource = [];
    private static readonly Dictionary<ETile, IImage> ImageResource = [];
    public static ETileTexture Texture { get; set; }
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
                    string name = $"{Enum.GetName(tile)}.png".ToLower();

                    using var stream = await FileSystem.OpenAppPackageFileAsync(name);

                    var tileAscii = tile switch
                    {
                        ETile.Grass => ";;.;;;;.;;.",
                        ETile.Ground => "|||||||",
                        ETile.Road => "]]]]]]]]]",
                        ETile.Water => "~~~~~~~",
                        ETile.Desert => "::::::::",
                        _ => ".",
                    };
                    var tileMono = tile switch
                    {
                        ETile.Grass => Colors.Gray,
                        ETile.Ground => Colors.LightSlateGray,
                        ETile.Road => Colors.GhostWhite,
                        ETile.Water => Colors.AntiqueWhite,
                        ETile.Desert => Colors.LightGray,
                        _ => Colors.Transparent,
                    };
                    var tileColor = tile switch
                    {
                        ETile.Grass => Colors.DarkSeaGreen,
                        ETile.Ground => Colors.SandyBrown,
                        ETile.Road => Colors.LightGray,
                        ETile.Water => Colors.CornflowerBlue,
                        ETile.Desert => Colors.Beige,
                        _ => Colors.Transparent,
                    };

                    ASCIIResource.Add(tile, tileAscii);
                    MonoResource.Add(tile, tileMono);
                    ColorResource.Add(tile, tileColor);
                    ImageResource.Add(tile, PlatformImage.FromStream(stream));

                    stream.Close();
                }
            });
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (ImageResource.Count <= 0 || ColorResource.Count <= 0) return;

        if (Texture is ETileTexture.ASCII)
        {
            canvas.FontColor = ColorResource[Tile];
            canvas.DrawString(ASCIIResource[Tile], rect.Center.X, rect.Center.Y, HorizontalAlignment.Justified);
        }

        if (Texture is ETileTexture.Image)
        {
            canvas.DrawImage(ImageResource[Tile], rect.X, rect.Y, rect.Width, rect.Height);
        }

        if (Texture is ETileTexture.Color)
        {
            canvas.FillColor = ColorResource[Tile];
            canvas.FillRoundedRectangle(rect, 2f);
        }

        if (Texture is ETileTexture.Mono)
        {
            canvas.FillColor = MonoResource[Tile];
            canvas.FillRoundedRectangle(rect, 2f);
        }

        canvas.Antialias = true;
    }
}
