namespace SFMLGame.core.scenes.world;

public class WorldView(FloatRect viewRect)
    : View(viewRect), IGameObject
{
    private FloatRect ViewRect { get; } = viewRect;

    private static Position2D? Position2D { get; set; }
    private static IList<IList<INode2D>> Collection { get; } = [];

    #region Build 
    public void LoadContent()
    {
        for (byte row = 0; row < Global.MAX_ROW; row++)
        {
            Collection.Add([]);

            for (byte column = 0; column < Global.MAX_COLUMN; column++)
            {
                var position2D = new Position2D(row, column, column * Global.RECT, row * Global.RECT);

                Collection[row].Add(new Node2D(position2D));
            }
        }

        foreach (var nodeList in Collection)
            foreach (var node in nodeList)
            {
                var (row, column, _, _) = node.Position2D;

                node.Navigation[EDirection.Left] = Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.Right] = Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Top] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.TopLeft] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.TopRight] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Bottom] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.BottomLeft] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.BottomRight] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);
            }
    }

    public virtual void LoadEvents()
    {
        Global.Subscribe(EEvent.Scene, OnSceneChanged);
        Global.Subscribe(EEvent.Camera, OnCenterChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var nodeList in Collection)
            foreach (var node in nodeList)
            {
                if (node.Opacity is EOpacity.Dark)
                    continue;

                var (posX, posY, width, height) = (node.Position2D.X, node.Position2D.Y, Size.X / 2, Size.Y / 2);

                if (posX < Center.X - width || posX > Center.X + width)
                    continue;
                if (posY < Center.Y - height || posY > Center.Y + height)
                    continue;

                node.Draw(window);
            }
    }
    #endregion

    #region Event
    private void OnSceneChanged(object? sender)
    {
        if (AppState.CurrentScene != EScene.World) return;

        // TODO :: Remover após implementar a distribuição do pacote de região.
        // A cena deve ser salva em um enum no AppState e os pacotes distribuidos ao iniciar o jogo.
        Content.LoadScene();

        // O Player pode ser lançado para o node a partir do evento de envio dos dados da região.
        // O pacote será distribuido entre os nodes com Surface, Body2D.
        if (AppState.CurrentPlayer == null)
        {
            var node = Collection.ElementAt(21).ElementAt(21);
            AppState.CurrentPlayer = new PlayerBody2D(node);
            node.SetBody(AppState.CurrentPlayer);
        }
    }

    private void OnZoomChanged(object? sender)
    {
        if (AppState.CurrentScene != EScene.World) return;

        if (sender is Key.Z)
        {
            if (Size.X <= ViewRect.Width / 2) return;
            Zoom(0.9f);
            Global.Invoke(EEvent.Camera, Position2D);
        }

        if (sender is Key.X)
        {
            if (Size.Y >= ViewRect.Height) return;
            Zoom(1.1f);
            Global.Invoke(EEvent.Camera, Position2D);
        }
    }

    protected void OnCenterChanged(object? sender)
    {
        if (AppState.CurrentScene != EScene.World) return;

        if (sender is Position2D position2D) Position2D = position2D;

        if (Position2D != null)
        {
            var (row, column, posX, posY) = Position2D;

            var (width, height) = (Size.X, Size.Y);

            float scrollX = posX - (width / 2);
            float scrollY = posY - (height / 2);

            scrollX = Math.Max(0, Math.Min(scrollX, Global.WORLD_WIDTH - width));
            scrollY = Math.Max(0, Math.Min(scrollY, Global.WORLD_HEIGHT - height));

            Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));

            Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.Debug, $"R:{row} C:{column} X:{Center.X} Y:{Center.Y}"));
        }
    }
    #endregion
}

// TODO :: Otimizar a renderização e o uso do processador.
// Este objeto é redundante porque renderiza o world, porem o exibe em uma escala menor.
public sealed class WorldMapView : WorldView
{
    public WorldMapView(FloatRect rect) : base(rect)
    {
        Viewport = new(0.85f, 0, 0.15f, 0.15f);
    }

    public override void LoadEvents()
    {
        Global.Subscribe(EEvent.Camera, OnCenterChanged);
    }
}