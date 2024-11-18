namespace SFMLGame.pipeline;

internal sealed partial class App
{
    private readonly static Random _random = new();

    #region Map
    public static Vector2f MapCoords(int x, int y, View view) => Window.MapPixelToCoords(new(x, y), view);
    public static Vector2f MapCoords(Vector2i point, View view) => Window.MapPixelToCoords(point, view);
    public static Vector2f MapCoords(int x, int y, EScene scene) => Window.MapPixelToCoords(new(x, y), (View)Scenes[scene]);
    #endregion

    #region Common
    public static T? Shuffle<T>(IList<T?> list)
    {
        return list?.Count > 0 ? list[_random.Next(list.Count)] : default;
    }
    #endregion
}
