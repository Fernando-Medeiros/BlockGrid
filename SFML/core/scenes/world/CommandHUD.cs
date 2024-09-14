namespace SFMLGame.core.scenes.world;

public sealed class CommandHUD : IGameObject
{
    private Font? Font { get; set; }
    private IList<(EIcon, string, Vector2f)> Collection { get; } = [];

    #region Build
    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseButtonPressed, OnCommandClicked);
    }

    public void LoadContent()
    {
        var (posX, posY, space) = ((float)(Global.WINDOW_WIDTH - Global.RECT), (float)Global.WINDOW_HEIGHT, 5f);
        posX -= space;

        posY -= Global.RECT + space;
        Collection.Add((EIcon.Exit, Key.Escape[..3], new(posX, posY)));

        posY -= Global.RECT + space;
        Collection.Add((EIcon.ZoomOut, Key.X, new(posX, posY)));

        posY -= Global.RECT + space;
        Collection.Add((EIcon.ZoomIn, Key.Z, new(posX, posY)));

        Font = Content.GetResource(EFont.OpenSansSemibold);
    }

    public void Draw(RenderWindow window)
    {
        foreach (var (icon, command, position) in Collection)
        {
            var sprite = Content.GetResource(icon);
            sprite.Color = Colors.GoldRod;
            sprite.Position = position;
            window.Draw(sprite);

            window.Draw(new Text(command, Font, 14)
            {
                OutlineThickness = 1f,
                FillColor = Colors.White,
                OutlineColor = Colors.Black,
                Position = position,
            });
        }
    }
    #endregion

    #region Event
    private void OnCommandClicked(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            foreach (var (icon, _, pos) in Collection)
            {
                if (mouse.X >= pos.X && mouse.X <= pos.X + Global.RECT &&
                    mouse.Y >= pos.Y && mouse.Y <= pos.Y + Global.RECT)
                {
                    if (icon == EIcon.Exit) Global.Invoke(EEvent.Scene, EScene.Main);
                    if (icon == EIcon.ZoomIn) Global.Invoke(EEvent.KeyPressed, Key.Z);
                    if (icon == EIcon.ZoomOut) Global.Invoke(EEvent.KeyPressed, Key.X);
                }
            }
        }
    }
    #endregion
}
