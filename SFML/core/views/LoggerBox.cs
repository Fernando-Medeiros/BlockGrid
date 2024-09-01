namespace SFMLGame.core.views;

public sealed class LoggerBox : RectangleShape
{
    Vector2i _position = new(10, 820);
    ELogger _guide = ELogger.General;

    readonly Dictionary<ELogger, List<string>> _loggers = [];

    public LoggerBox()
    {
        foreach (var key in Enum.GetValues<ELogger>())
            _loggers.Add(key, []);

        Size = new Vector2f(200, 130);
        FillColor = new Color(222, 184, 135, 128);
    }

    public void ConfigureListeners(RenderWindow window)
    {
        window.MouseButtonPressed += (sender, e) =>
        {
            if (e.Button == Mouse.Button.Left)
            {
                var (posX, posY, width, height) = (_position.X, _position.Y, Size.X, Size.Y);
                
                if (posX < e.X - width || posX > e.X + width)
                    return;
                if (posY < e.Y - height || posY > e.Y + height)
                    return;
                _guide = _guide == 0 ? ELogger.Debug : (byte)_guide == 1 ? ELogger.General : ELogger.Dialog;
            }
        };

        Global.Subscribe(EEvent.Logger, (sender) =>
        {
            if (sender is Logger x) _loggers[x.Guide].Add(x.Message);
        });
    }

    public void Draw(RenderWindow window)
    {
        Position = window.MapPixelToCoords(_position);
        window.Draw(this);

        int horizontal = 10;
        foreach (var guide in Enum.GetValues<ELogger>())
        {
            var text = new Text($"{guide}", Content.GetResource(Fonte.OpenSansSemibold), 12)
            {
                FillColor =  guide == _guide ? Color.Green : Color.Black,
                Position = Position + new Vector2f(horizontal, 01),
            };
            horizontal += 67;
            window.Draw(text);
        }

        int vertical = 18;
        foreach (var logger in _loggers[_guide].Take(^10..))
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
}
