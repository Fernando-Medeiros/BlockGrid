﻿namespace SFMLGame;

internal sealed partial class App
{
    public static IBody2D? CurrentPlayer { get; private set; }
    public static INode2D? SelectedNode { get; private set; }
    public static EBiome CurrentBiome { get; private set; } = EBiome.Forest;
    public static EScene CurrentScene { get; private set; } = EScene.Main;
    public static Position2D CurrentPosition { get; private set; } = Position2D.Empty;

    #region States
    private void OnCreate()
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
    }

    private void OnStart() { }

    private void OnResume()
    {
        Global.Subscribe(EEvent.Scene, (sender) =>
            { if (sender is EScene scene) CurrentScene = scene; });

        Global.Subscribe(EEvent.Camera, (sender) =>
            { if (sender is Position2D position) CurrentPosition = position; });

        Global.Subscribe(EEvent.Transport, (sender) =>
            {
                if (sender is IBody2D body) CurrentPlayer = body;
                if (sender is INode2D node) SelectedNode = node;
                if (sender is Position2D position) CurrentPosition = position;
                if (sender is EBiome biome) { CurrentBiome = biome; Content.PlayMusic(); }
            });
    }

    private void OnPause() { }

    private void OnDestroy()
    {
        Global.Subscribe(EEvent.EndGame, (sender) => Window.Close());
    }
    #endregion
}
