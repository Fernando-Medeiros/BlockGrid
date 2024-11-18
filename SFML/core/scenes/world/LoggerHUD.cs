namespace SFMLGame.core.scenes.world;

public sealed class LoggerHUD : IView, IDisposable
{
    private IList<IButton> Buttons { get; } = [];
    private ELogger SelectedGuide { get; set; } = ELogger.General;
    private Dictionary<ELogger, List<string>> Loggers { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;

    #region Build
    public void Build()
    {
        Rect = new(x: 5f, y: Global.WINDOW_HEIGHT - 160f, width: 200f, height: 160f);

        var (posX, space) = (Rect.X, 70f);

        foreach (var guide in Enum.GetValues<ELogger>())
        {
            Loggers.Add(guide, []);

            Buttons.Add(new TextButton()
            {
                Id = guide,
                Size = 20,
                Font = EFont.Romulus,
                Position = new(posX, Rect.Y),
                Text = Enum.GetName(guide)!,
            });
            posX += space;
        }
    }

    public void Event()
    {
        foreach (IButton button in Buttons)
        {
            button.Event();
            button.OnClicked += OnButtonClicked;
        }

        Global.Subscribe(EEvent.LoggerChanged, OnLoggerReceive);
    }

    public void Render(RenderWindow window)
    {
        foreach (IButton button in Buttons) button.Render(window);

        int gap = 24;
        foreach (var logger in Loggers[SelectedGuide].Take(^10..))
        {
            window.Draw(new Text(logger, Content.GetResource<Font>(EFont.OpenSansRegular), 12)
            {
                FillColor = Factory.Color(EColor.White),
                Position = new Vector2f(Rect.X, Rect.Y + gap),
            });
            gap += 11;
        }
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is ELogger guide)
        {
            SelectedGuide = guide;

            foreach (TextButton button in Buttons)
            {
                if (button.Id == sender)
                {
                    button.Color = EColor.CornFlowerBlue;
                    continue;
                }
                button.Color = EColor.White;
            }
        }
    }

    private void OnLoggerReceive(object? sender)
    {
        if (sender is Logger dto)
        {
            if (Loggers[dto.Guide].Count >= 50)
                Loggers[dto.Guide].RemoveRange(0, 25);

            Loggers[dto.Guide].Add(dto.Message);
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.Unsubscribe(EEvent.LoggerChanged, OnLoggerReceive);

        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
        Loggers.Clear();
    }
    #endregion
}
