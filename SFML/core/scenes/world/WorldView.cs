namespace SFMLGame.core.scenes.world;

public class WorldView(FloatRect viewRect)
    : View(viewRect), IGameObject
{
    private FloatRect ViewRect { get; } = viewRect;

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
                node.Position2D.Deconstruct(out var row, out var column, out _, out _);

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
        Global.Subscribe(EEvent.Region, OnRegionChanged);
        Global.Subscribe(EEvent.Camera, OnCameraChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnNodeSelected);
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
        if (App.CurrentScene != EScene.World) return;

        // TODO :: Remover após implementar a distribuição do pacote de região.
        // A cena deve ser salva em um enum no App e os pacotes distribuidos ao iniciar o jogo.
        Content.LoadScene();

        // O Player pode ser lançado para o node a partir do evento de envio dos dados da região.
        // O pacote será distribuido entre os nodes com Surface, Body2D.
        if (App.CurrentPlayer == null)
        {
            var node = Collection.ElementAt(21).ElementAt(21);
            App.CurrentPlayer = new PlayerBody2D(node);
            node.SetBody(App.CurrentPlayer);
        }
    }

    private void OnRegionChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is RegionDTO package)
        {
            foreach (var nodeList in Collection)
                foreach (var node in nodeList)
                    node.SetSurface(package.Surface[node.Position2D.Row][node.Position2D.Column]);
        }
    }

    private void OnNodeSelected(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is MouseDTO mouse)
        {
            var absolutePosition = App.MapCoords(mouse.X, mouse.Y, this);

            var posY = absolutePosition.Y - (Global.RECT / 2);
            var posX = absolutePosition.X - (Global.RECT / 2);

            int row = Math.Max(0, Math.Min(Convert.ToInt32(posY / Global.RECT), Global.MAX_ROW - 1));
            int column = Math.Max(0, Math.Min(Convert.ToInt32(posX / Global.RECT), Global.MAX_COLUMN - 1));

            App.SelectedNode = Collection[row][column];
        }
    }

    private void OnZoomChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is Key.Z)
        {
            if (Size.X <= ViewRect.Width / 2) return;
            Zoom(0.9f);
            OnCameraChanged(null);
        }

        if (sender is Key.X)
        {
            if (Size.Y >= ViewRect.Height) return;
            Zoom(1.1f);
            OnCameraChanged(null);
        }
    }

    protected void OnCameraChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        App.CurrentPosition.Deconstruct(out var row, out var column, out var posX, out var posY);

        var (width, height) = (Size.X, Size.Y);

        float scrollX = posX - (width / 2);
        float scrollY = posY - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, Global.WORLD_WIDTH - width));
        scrollY = Math.Max(0, Math.Min(scrollY, Global.WORLD_HEIGHT - height));

        Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));

#if DEBUG
        Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.Debug, $"R:{row} C:{column} X:{Center.X} Y:{Center.Y}"));
#endif
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
        Global.Subscribe(EEvent.Camera, OnCameraChanged);
    }
}