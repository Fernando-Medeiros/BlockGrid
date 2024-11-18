namespace SFMLGame.core.scenes.world;

public sealed class EnemyHUD : IView, IDisposable
{
    private Font? Font { get; set; }
    private Vector2f? Offset { get; set; }
    private StatusDTO? Data { get; set; }
    private RectangleShape? HpBar { get; set; }
    private RectangleShape? MpBar { get; set; }

    #region Build
    public void Event()
    {
        Global.Subscribe(EEvent.BasicStatus, OnBasicStatusChanged);
    }

    public void Build()
    {
        var (posY, space) = (5f, 5f);

        HpBar = new()
        {
            Size = new(300, 25),
            OutlineThickness = 1f,
            OutlineColor = Factory.Color(EColor.White),
            FillColor = Factory.Color(EColor.Tomate),
            Position = new((Global.WINDOW_WIDTH / 2) - (300 / 2), posY),
        };

        posY += HpBar.Size.Y + space;

        MpBar = new()
        {
            Size = new(150, 18),
            OutlineThickness = 1f,
            OutlineColor = Factory.Color(EColor.White),
            FillColor = Factory.Color(EColor.CornFlowerBlue),
            Position = new((Global.WINDOW_WIDTH / 2) - (150 / 2), posY),
        };

        Offset = new(space, 0);
        Data = new(string.Empty, 0, 0, 0, 0, 0, 0, 0);
        Font = core.Content.GetResource<Font>(EFont.OpenSansSemibold);
    }

    public void Render(RenderWindow window)
    {
        if (Data?.Hp <= 0) return;

        window.Draw(HpBar);
        window.Draw(MpBar);

        window.Draw(new Text($"HP: {Data?.Hp} / {Data?.MaxHp}", Font, 18)
        {
            FillColor = Factory.Color(EColor.White),
            Position = (Vector2f)(HpBar!.Position + Offset),
        });

        window.Draw(new Text($"MP: {Data?.Mp} / {Data?.MaxMp}", Font, 14)
        {
            FillColor = Factory.Color(EColor.White),
            Position = (Vector2f)(MpBar!.Position + Offset),
        });

        window.Draw(new Text(Data?.Name, Font, 14)
        {
            FillColor = Factory.Color(EColor.White),
            Position = MpBar.Position + new Vector2f(20, 18),
        });
    }
    #endregion

    #region Event
    private void OnBasicStatusChanged(object? sender)
    {
        if (sender is StatusDTO basicStatus)
            Data = basicStatus;
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.Unsubscribe(EEvent.BasicStatus, OnBasicStatusChanged);

        HpBar?.Dispose();
        MpBar?.Dispose();
        Font = null;
        Data = null;
        Offset = null;
        HpBar = null;
        MpBar = null;
    }
    #endregion
}
