namespace SFMLGame.core.views;

public sealed class EnemyBoxShape : IBoxShape
{
    private Font Font { get; }
    private Vector2f Offset { get; }
    private BasicStatus Data { get; set; }

    private RectangleShape HpBar { get; }
    private RectangleShape MpBar { get; }

    public EnemyBoxShape()
    {
        var (posY, space) = (5f, 5f);

        HpBar = new()
        {
            Size = new(300, 25),
            OutlineThickness = 1f,
            OutlineColor = Colors.White,
            FillColor = Colors.Tomate,
            Position = new(Global.WINDOW_WIDTH / 2 - (300 / 2), posY),
        };

        posY += HpBar.Size.Y + space;

        MpBar = new()
        {
            Size = new(150, 18),
            OutlineThickness = 1f,
            OutlineColor = Colors.White,
            FillColor = Colors.CornFlowerBlue,
            Position = new(Global.WINDOW_WIDTH / 2 - (150 / 2), posY),
        };

        Offset = new(space, 0);
        Data = new(string.Empty, 0, 0, 0, 0, 0, 0, 0);
        Font = Content.GetResource(Fonte.OpenSansSemibold);
    }

    #region Build
    public void ConfigureListeners(RenderWindow window)
    {
        Global.Subscribe(EEvent.BasicStatus, OnBasicStatusChanged);
    }

    public void Draw(RenderWindow window)
    {
        if (Data.Hp <= 0) return;

        window.Draw(HpBar);
        window.Draw(MpBar);

        window.Draw(new Text($"HP: {Data.Hp} / {Data.MaxHp}", Font, 18)
        {
            FillColor = Colors.White,
            Position = HpBar.Position + Offset,
        });

        window.Draw(new Text($"MP: {Data.Mp} / {Data.MaxMp}", Font, 14)
        {
            FillColor = Colors.White,
            Position = MpBar.Position + Offset,
        });

        window.Draw(new Text(Data.Name, Font, 14)
        {
            FillColor = Colors.White,
            Position = MpBar.Position + new Vector2f(20, 18),
        });
    }
    #endregion

    #region Event
    private void OnBasicStatusChanged(object? sender)
    {
        if (sender is BasicStatus basicStatus)
            Data = basicStatus;
    }
    #endregion
}
