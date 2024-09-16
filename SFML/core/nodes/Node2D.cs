namespace SFMLGame.core.nodes;

public sealed class Node2D(Position2D position2D) : INode2D
{
    #region Property
    private ESprite? Terrain { get; set; }
    public IBody2D? Body { get; private set; }
    public EOpacity Opacity { get; private set; }
    public Position2D Position2D { get; init; } = position2D;

    // TODO :: Refatorar
    public IList<IGameItem> GameItems { get; init; } = [];
    #endregion

    #region Static Common
    public static Func<EDirection, Position2D, INode2D?> NavigationHandler = (_, _) => null;
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
        DrawDynamicTerrain(window);
        DrawItems(window);
        DrawBody2D(window);
        DrawSelected(window);
    }
    #endregion

    #region Canva Layers
    private void DrawDynamicTerrain(RenderWindow window)
    {
        if (Terrain is null) Terrain ??= Factory.Shuffle(App.RegionSurface);

        var sprite = Content.GetResource((ESprite)Terrain);
        sprite.Color = Factory.Color(Opacity);
        sprite.Position = Position2D;
        window.Draw(sprite);
    }

    private void DrawItems(RenderWindow window)
    {
        foreach (var gameItem in GameItems)
        {
            var sprite = Content.GetResource(gameItem.Sprite);
            sprite.Color = Factory.Color(Opacity);
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
