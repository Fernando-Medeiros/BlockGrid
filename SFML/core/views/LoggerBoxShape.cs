namespace SFMLGame.core.views;

// TODO :: Alterar o design da caixa de logs;

public sealed class LoggerBoxShape : RectangleShape, IBoxShape
{
    private ELogger Guide { get; set; }
    private Dictionary<ELogger, List<string>> Loggers { get; } = [];

    public LoggerBoxShape()
    {
        foreach (var key in Enum.GetValues<ELogger>()) Loggers.Add(key, []);

        Size = new(200, 130);
        Position = new(05, 820);
        FillColor = new(222, 184, 135, Convert.ToByte(EOpacity.Regular));
    }

    #region Build
    public void ConfigureListeners(RenderWindow window)
    {
        window.MouseButtonPressed += OnGuideClicked;

        Global.Subscribe(EEvent.Logger, OnLoggerReceive);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(this);

        int horizontal = 10;
        foreach (var guide in Enum.GetValues<ELogger>())
        {
            var text = new Text($"{guide}", Content.GetResource(Fonte.OpenSansSemibold), 12)
            {
                FillColor = guide == Guide ? Color.Green : Color.White,
                Position = Position + new Vector2f(horizontal, 01),
            };
            horizontal += 67;
            window.Draw(text);
        }

        int vertical = 18;
        foreach (var logger in Loggers[Guide].Take(^10..))
        {
            var text = new Text(logger, Content.GetResource(Fonte.OpenSansRegular), 9)
            {
                FillColor = Color.White,
                Position = Position + new Vector2f(05, vertical),
            };
            vertical += 11;
            window.Draw(text);
        }
    }
    #endregion

    #region Event
    private void OnGuideClicked(object? sender, MouseButtonEventArgs e)
    {
        if (GetGlobalBounds().Contains(e.X, e.Y))
            Guide = (int)Guide switch
            {
                0 => ELogger.Debug,
                1 => ELogger.General,
                _ => ELogger.Dialog,
            };
    }

    private void OnLoggerReceive(object? sender)
    {
        if (sender is Logger x)
        {
            if (Loggers[x.Guide].Count >= 50)
                Loggers[x.Guide].RemoveRange(0, 25);

            Loggers[x.Guide].Add(x.Message);
        }
    }
    #endregion
}
