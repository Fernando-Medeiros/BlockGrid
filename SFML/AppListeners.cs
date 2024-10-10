namespace SFMLGame;

internal sealed partial class App
{
    private void LoadEvents()
    {
        Window.KeyPressed += (_, e) =>
           Global.Invoke(EEvent.KeyPressed, Enum.GetName(e.Code));

        Window.KeyReleased += (_, e) =>
            Global.Invoke(EEvent.KeyReleased, Enum.GetName(e.Code));

        Window.MouseMoved += (_, e) =>
            Global.Invoke(EEvent.MouseMoved, new MouseDTO(default, e.X, e.Y));

        Window.MouseWheelScrolled += (_, e) =>
           Global.Invoke(EEvent.MouseWheelScrolled, new MouseDTO(Enum.Parse<EMouse>($"{e.Delta}"), e.X, e.Y));

        Window.MouseButtonPressed += (_, e) =>
            Global.Invoke(EEvent.MouseButtonPressed, new MouseDTO(Enum.Parse<EMouse>(Enum.GetName(e.Button)), e.X, e.Y));

        Global.Subscribe(EEvent.Scene, (sender) =>
        {
            if (sender is EScene scene) CurrentScene = scene;
        });

        Global.Subscribe(EEvent.Camera, (sender) =>
        {
            if (sender is Position2D position) CurrentPosition = position;
        });

        Global.Subscribe(EEvent.Transport, (sender) =>
            {
                if (sender is IBody2D body) CurrentPlayer = body;
                if (sender is INode2D node) SelectedNode = node;
                if (sender is Position2D position) CurrentPosition = position;
                if (sender is EBiome biome) { CurrentBiome = biome; Content.PlayMusic(); }
            });

        Global.Subscribe(EEvent.EndGame, (sender) => Window.Close());
    }
}
