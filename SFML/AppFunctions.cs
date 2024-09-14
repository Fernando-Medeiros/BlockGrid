namespace SFMLGame;

internal sealed partial class App
{
    #region
    public static Vector2f MapCoords(int x, int y, View view) => Window.MapPixelToCoords(new(x, y), view);
    public static Vector2f MapCoords(Vector2i point, View view) => Window.MapPixelToCoords(point, view);
    public static Vector2f MapCoords(int x, int y, EScene scene) => Window.MapPixelToCoords(new(x, y), (View)Scenes[scene]);
    #endregion
}
