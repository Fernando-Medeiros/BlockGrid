namespace SFMLGame.pipeline;

internal sealed partial class App
{
    public static void SubscribeGlobalListeners()
    {
        Window.TextEntered += (sender, e) =>
            Global.Invoke(EEvent.TextEntered, e.Unicode[0]);

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


        Global.Subscribe(EEvent.SchemaChanged, (sender) =>
        {
            if (sender is PlayerSchema playerSchema) Player = playerSchema;

            if (sender is WorldSchema worldSchema) World = worldSchema;

            if (sender is RegionSchema regionSchema) { Region = regionSchema; Content.PlayMusic(); }

            if (sender is ConfigurationSchema configurationSchema) Configuration = configurationSchema;
        });

        Global.Subscribe(EEvent.SceneChanged, (sender) =>
        {
            if (sender is EScene scene) CurrentScene = scene;
        });

        Global.Subscribe(EEvent.CameraChanged, (sender) =>
        {
            if (sender is Position2D position) CurrentPosition = position;
        });

        Global.Subscribe(EEvent.NodeChanged, (sender) =>
        {
            if (sender is INode2D node) SelectedNode = node;
        });

        Global.Subscribe(EEvent.SaveGameChanged, (sender) =>
        {
        });

        Global.Subscribe(EEvent.EndGameChanged, (sender) => Window.Close());
    }
}
