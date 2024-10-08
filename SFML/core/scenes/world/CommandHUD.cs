﻿namespace SFMLGame.core.scenes.world;

public sealed class CommandHUD : IGameObject, IDisposable
{
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;

    #region Build
    public void LoadContent()
    {
        Rect = new(X: Global.WINDOW_WIDTH - Global.RECT - 5, Y: Global.WINDOW_HEIGHT - 5, Width: 0f, Height: 0f);

        var (posY, space) = (Rect.Y, 5f);

        foreach (EIcon icon in Enum.GetValues<EIcon>())
        {
            posY -= Global.RECT + space;

            Buttons.Add(new ImageButton()
            {
                Id = icon,
                Image = icon,
                Position = new(Rect.X, posY),
            });
        }
    }

    public void LoadEvents()
    {
        foreach (IButton button in Buttons)
        {
            button.LoadEvents();
            button.OnClicked += OnButtonClicked;
        }
    }

    public void Draw(RenderWindow window)
    {
        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is EIcon.Exit)
        {
            Global.Invoke(EEvent.SaveGame, null);
            Global.Invoke(EEvent.Scene, EScene.Main);
        };

        if (sender is EIcon.ZoomIn) Global.Invoke(EEvent.KeyPressed, Key.Z);
        if (sender is EIcon.ZoomOut) Global.Invoke(EEvent.KeyPressed, Key.X);
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
    }
    #endregion
}
