namespace SFMLGame.core.scenes.main;

public sealed class CommandHUD : IGameObject
{
    private enum EOption : byte { NewGame, LoadGame, Options, Quit }

    private Font? Font { get; set; }
    private IList<(EOption, string, Vector2f)> Collection { get; } = [];

    #region Build
    public void LoadContent()
    {
        var (posX, posY, space) = (Global.WINDOW_WIDTH / 2, Global.WINDOW_HEIGHT / 3f, 60f);

        posY += space;
        Collection.Add((EOption.NewGame, nameof(EOption.NewGame), new(posX, posY)));

        posY += space;
        Collection.Add((EOption.LoadGame, nameof(EOption.LoadGame), new(posX, posY)));

        posY += space;
        Collection.Add((EOption.Options, nameof(EOption.Options), new(posX, posY)));

        posY += space;
        Collection.Add((EOption.Quit, nameof(EOption.Quit), new(posX, posY)));

        Font = Content.GetResource(EFont.Romulus);
    }

    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseButtonPressed, OnCommandClicked);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(new Text()
        {
            Font = Font,
            Position = new Vector2f(Global.WINDOW_WIDTH / 3, 25),
            CharacterSize = 100,
            OutlineThickness = 1f,
            FillColor = Colors.White,
            OutlineColor = Colors.Black,
            DisplayedString = Global.TITLE,
        });

        foreach (var (_, placeholder, position) in Collection)
        {
            window.Draw(new Text()
            {
                Font = Font,
                Position = position,
                CharacterSize = 35,
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
                    if (option is EOption.Quit) Global.Invoke(EEvent.EndGame, null);
                    if (option is EOption.NewGame) Global.Invoke(EEvent.Scene, EScene.World);
                }
            }
        }
    }
    #endregion
}
