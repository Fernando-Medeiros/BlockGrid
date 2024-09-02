namespace SFMLGame.core.views;

// TODO :: Alterar o design da caixa de logs;
// TODO :: Adicionar maximo de 50 logs na lista;
// TODO :: Ajustar o tamanho da caixa para ser responsivo com o zoom da view;

public sealed class LoggerBoxShape : RectangleShape, IBoxShape
{
    private ELogger Guide { get; set; } = ELogger.General;
    private Dictionary<ELogger, List<string>> Loggers { get; } = [];

    public LoggerBoxShape()
    {
        foreach (var key in Enum.GetValues<ELogger>()) Loggers.Add(key, []);

        Size = new(200, 130);
        Position = new(10, 820);
        FillColor = new(222, 184, 135, 128);
    }

    public void ConfigureListeners(RenderWindow window)
    {
        window.MouseButtonPressed += (_, e) =>
        {
            if (GetGlobalBounds().Contains(e.X, e.Y))
            {
                Guide = Guide == 0 ? ELogger.Debug : (byte)Guide == 1 ? ELogger.General : ELogger.Dialog;
            }
        };

        Global.Subscribe(EEvent.Logger, (sender) =>
        {
            if (sender is Logger x) Loggers[x.Guide].Add(x.Message);
        });
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(this);

        int horizontal = 10;
        foreach (var guide in Enum.GetValues<ELogger>())
        {
            var text = new Text($"{guide}", Content.GetResource(Fonte.OpenSansSemibold), 12)
            {
                FillColor = guide == Guide ? Color.Green : Color.Black,
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
}
