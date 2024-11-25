namespace SFMLGame.core.scenes.world;

public sealed class LoggerHUD : IView, IDisposable
{
    private ELogger SelectedGuide { get; set; } = ELogger.General;
    private Rect Rect { get; set; } = Rect.Empty;
    private IList<IButton> Buttons { get; } = [];
    private Dictionary<ELogger, List<string>> Loggers { get; } = [];

    #region Build
    public void Build()
    {
        Rect = new Rect()
            .WithSize(width: 200f, height: 160f)
            .WithPadding(vertical: 15f, horizontal: 15f)
            .WithAlignment(EDirection.BottomLeft);

        float posX = Rect.WidthLeft;

        foreach (var guide in Enum.GetValues<ELogger>())
        {
            Loggers.Add(guide, []);

            TextButton textButton = new(
                id: guide,
                text: guide.ToString(),
                position: new(posX, Rect.HeightTop))
            {
                FontSize = 20,
            };

            posX = textButton.GetPosition(EDirection.Right) + Rect.HorizontalPadding;

            Buttons.Add(textButton);
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
                Position = new Vector2f(Rect.WidthLeft, Rect.HeightTop + gap),
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

            foreach (var button in Buttons.OfType<TextButton>())
                button.Selected = button.Id.Equals(sender);
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
