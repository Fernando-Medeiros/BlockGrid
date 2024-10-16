﻿namespace SFMLGame.core.scenes.main;

public sealed class MainScene : View, IView
{
    private IList<IView> Collection { get; } = [];

    public MainScene(FloatRect viewRect) : base(viewRect)
    {
        Global.Subscribe(EEvent.Scene, OnSceneChanged);
    }

    #region Initialize
    private void OnSceneChanged(object? sender)
    {
        if (sender is EScene.Main)
        {
            Build();
            Event();
            return;
        }

        Dispose();
    }
    #endregion

    #region Build
    public void Build()
    {
        Collection.Add(new BackgroundView());
        Collection.Add(new MainMenuHUD());
        Collection.Add(new OptionsHUD());

        foreach (var view in Collection) view.Build();
    }

    public void Event()
    {
        foreach (IView view in Collection)
            view.Event();

        foreach (var hud in Collection.OfType<IHud>())
            hud.OnClicked += OnHudChanged;
    }

    public void Render(RenderWindow window)
    {
        window.SetView(this);

        foreach (var view in Collection) view.Render(window);
    }
    #endregion

    #region Event
    private void OnHudChanged(object? sender)
    {
        if (sender is EMainMenu.Options)
        {
            foreach (IHud hud in Collection.Where(v => v is OptionsHUD or MainMenuHUD))
                hud.VisibilityChanged();
        }
    }
    #endregion

    #region Dispose
    public new void Dispose()
    {
        foreach (var view in Collection)
            view.Dispose();

        foreach (var hud in Collection.OfType<IHud>())
            hud.OnClicked -= OnHudChanged;

        Collection.Clear();

        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Forced);
    }
    #endregion
}
