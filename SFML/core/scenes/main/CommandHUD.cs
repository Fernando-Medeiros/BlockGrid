﻿namespace SFMLGame.core.scenes.main;

public sealed class CommandHUD : IGameObject
{
    private enum EOption : byte { Start, Exit }

    private Font? Font { get; set; }
    private IList<(EOption, string, Vector2f)> Collection { get; } = [];

    #region Build
    public void LoadContent()
    {
        var (posX, posY, space) = (Global.WINDOW_WIDTH / 2f, Global.WINDOW_HEIGHT / 3f, 60f);

        posY += space;
        Collection.Add((EOption.Start, nameof(EOption.Start), new(posX, posY)));

        posY += space;
        Collection.Add((EOption.Exit, nameof(EOption.Exit), new(posX, posY)));

        Font = Content.GetResource(EFont.OpenSansSemibold);
    }

    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseButtonPressed, OnCommandClicked);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Content.GetResource(EPicture.MainBackground));

        foreach (var (_, placeholder, position) in Collection)
        {
            window.Draw(new Text()
            {
                Font = Font,
                Position = position,
                CharacterSize = 28,
                OutlineThickness = 1f,
                FillColor = Colors.White,
                OutlineColor = Colors.Black,
                DisplayedString = placeholder,
            });
        }
    }
    #endregion

    #region Event
    private void OnCommandClicked(object? sender)
    {
        if (App.CurrentScene != EScene.Main) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            foreach (var (option, _, pos) in Collection)
            {
                if (mouse.X >= pos.X && mouse.X <= pos.X + Global.RECT &&
                    mouse.Y >= pos.Y && mouse.Y <= pos.Y + Global.RECT)
                {
                    if (option is EOption.Exit) Global.Invoke(EEvent.EndGame, null);
                    if (option is EOption.Start) Global.Invoke(EEvent.Scene, EScene.World);
                }
            }
        }
    }
    #endregion
}
