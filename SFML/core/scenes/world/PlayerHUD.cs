namespace SFMLGame.core.scenes.world;

public sealed class PlayerHUD : IGameObject
{
    private Font? Font { get; set; }
    private Vector2f? Offset { get; set; }
    private BasicStatus? Data { get; set; }
    private RectangleShape? HpBar { get; set; }
    private RectangleShape? MpBar { get; set; }
    private RectangleShape? ExpBar { get; set; }

    #region Build
    public void LoadEvents(RenderWindow window)
    {
        Global.Subscribe(EEvent.BasicStatus, OnBasicStatusChanged);
    }

    public void LoadContent()
    {
        var (posX, posY, space) = (5f, 5f, 5f);

        HpBar = new()
        {
            Size = new(250, 25),
            OutlineThickness = 1f,
            OutlineColor = Colors.White,
            FillColor = Colors.Tomate,
            Position = new(posX, posY),
        };

        posY += HpBar.Size.Y + space;

        MpBar = new()
        {
            Size = new(150, 18),
            OutlineThickness = 1f,
            OutlineColor = Colors.White,
            FillColor = Colors.CornFlowerBlue,
            Position = new(posX, posY),
        };

        posY += MpBar.Size.Y + space;

        ExpBar = new()
        {
            Size = new(150, 18),
            OutlineThickness = 1f,
            OutlineColor = Colors.White,
            FillColor = Colors.GoldRod,
            Position = new(posX, posY),
        };

        Offset = new(space, 0);
        Data = new(string.Empty, 0, 0, 0, 0, 0, 0, 0);
        Font = Content.GetResource(Fonte.OpenSansSemibold);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(HpBar);
        window.Draw(MpBar);
        window.Draw(ExpBar);

        window.Draw(new Text($"HP: {Data?.Hp} / {Data?.MaxHp}", Font, 18)
        {
            FillColor = Colors.White,
            Position = (Vector2f)(HpBar!.Position + Offset),
        });

        window.Draw(new Text($"MP: {Data?.Mp} / {Data?.MaxMp}", Font, 14)
        {
            FillColor = Colors.White,
            Position = (Vector2f)(MpBar!.Position + Offset),
        });

        window.Draw(new Text($"EX: {Data?.Exp} / {Data?.MaxExp}", Font, 14)
        {
            FillColor = Colors.White,
            Position = (Vector2f)(ExpBar!.Position + Offset),
        });
    }
    #endregion

    #region Event
    private void OnBasicStatusChanged(object? sender)
    {
    }
    #endregion
}
