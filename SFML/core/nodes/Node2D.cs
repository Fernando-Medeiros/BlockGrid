namespace SFMLGame.core.nodes;

public sealed class Node2D(Position2D position2D) : INode2D
{
    #region Property
    public IBody2D? Body { get; private set; }
    public EOpacity Opacity { get; private set; }
    public Position2D Position2D { get; init; } = position2D;

    // TODO :: Refatorar
    private ESprite? Sprite { get; set; }
    public IList<IGameItem> GameItems { get; init; } = [];
    #endregion

    #region Static Common
    public static Func<EDirection, Position2D, INode2D?> NavigationHandler = (_, _) => null;

    private static readonly RectangleShape Terrain = new()
    {
        Size = new(Global.RECT, Global.RECT),
        FillColor = Factory.Get(App.RegionSurface),
    };
    #endregion

    #region Action
    public void SetBody(IBody2D? body) => Body = body;

    public void SetOpacity(EOpacity opacity) => Opacity = opacity;

    public INode2D? Get(params EDirection[] directions)
    {
        INode2D? node = this;
        foreach (var direction in directions)
            node = node is INode2D ? NavigationHandler(direction, node.Position2D) : node;
        return node;
    }

    public void Draw(RenderWindow window)
    {
        DrawTerrain(window);
        DrawItems(window);
        DrawBody2D(window);
        DrawSelected(window);
    }
    #endregion

    #region Canva Layers
    // TODO :: Salvar a configuração do terreno
    private void DrawTerrain(RenderWindow window)
    {
        Terrain.Position = Position2D;
        window.Draw(Terrain);

        if (GameItems.Count > 0) return;

        Sprite ??= App.Shuffle([ESprite.GrassA, ESprite.GrassB, ESprite.GrassC, ESprite.GrassD]);

        var sprite = Content.GetResource((ESprite)Sprite);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
        sprite.Position = Position2D;
        window.Draw(sprite);
    }

    private void DrawItems(RenderWindow window)
    {
        foreach (var gameItem in GameItems)
        {
            var sprite = Content.GetResource(gameItem.Sprite);
            sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
            sprite.Position = Position2D;
            window.Draw(sprite);
        }
    }

    private void DrawBody2D(RenderWindow window)
    {
        if (Body is null) return;

        if (Opacity is EOpacity.Regular && Body.Type != EBody.Player) return;

        var sprite = Content.GetResource((ESprite)Body.Sprite);

        if (Body.Type != EBody.Static)
        {
            window.Draw(new CircleShape()
            {
                Position = Position2D,
                Texture = sprite.Texture,
                Radius = Global.RECT / 2 - 1,
                OutlineThickness = 1f,
                OutlineColor = Colors.White,
            });
            return;
        }

        sprite.Position = Position2D;
        window.Draw(sprite);
    }

    private void DrawSelected(RenderWindow window)
    {
        if (App.SelectedNode != this) return;

        window.Draw(new RectangleShape()
        {
            Size = new(31, 31),
            Position = Position2D,
            FillColor = Colors.Transparent,
            OutlineThickness = 1f,
            OutlineColor = Colors.GoldRod,
        });
    }
    #endregion
}
