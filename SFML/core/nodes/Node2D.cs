namespace SFMLGame.core.nodes;

public sealed class Node2D(Position2D position2D) : INode2D, IDisposable
{
    #region Property
    public EBiome? Biome { get; private set; }
    public ETerrain? Terrain { get; private set; }
    public IBody2D? Body { get; private set; }
    public EOpacity Opacity { get; private set; } = EOpacity.Opaque;
    public Position2D Position2D { get; init; } = position2D;

    // TODO :: Refatorar
    public IList<IObject2D> Objects { get; init; } = [];
    #endregion

    #region Static Common
    public static Func<EDirection, Position2D, INode2D?>? Navigation;
    #endregion

    #region Action
    public void SetBody(IBody2D? body) => Body = body;

    public void SetOpacity(EOpacity opacity) => Opacity = opacity;

    public INode2D? Get(params EDirection[] directions)
    {
        INode2D? node = this;
        foreach (var direction in directions)
            node = node is INode2D ? Navigation?.Invoke(direction, node.Position2D) : node;
        return node;
    }

    public void Draw(RenderWindow window)
    {
        DrawDynamicBiome(window);
        DrawItems(window);
        DrawBody2D(window);
        DrawSelected(window);
    }
    #endregion

    #region Canva Layers
    private void DrawDynamicBiome(RenderWindow window)
    {
        if (Biome != App.CurrentBiome)
        {
            Biome = App.CurrentBiome;
            Terrain = Factory.Shuffle(App.CurrentBiome);
        }

        var sprite = Content.GetResource((ETerrain)Terrain);
        sprite.Color = Factory.Color(Opacity);
        sprite.Position = Position2D;
        window.Draw(sprite);
    }

    private void DrawItems(RenderWindow window)
    {
        foreach (var gameItem in Objects)
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

        if (Opacity is EOpacity.Opaque or EOpacity.Regular && Body.Type != EBody.Player) return;

        var sprite = Content.GetResource((ESprite)Body.Sprite);
        sprite.Color = Factory.Color(Opacity);

        if (Body.Type != EBody.Static)
        {
            window.Draw(new CircleShape()
            {
                Position = Position2D + new Vector2f(2, 2),
                Texture = sprite.Texture,
                Radius = Global.RECT / 2 - 2,
                OutlineThickness = 1f,
                OutlineColor = Factory.Color(EColor.White),
            });
            return;
        }

        sprite.Position = Position2D;
        window.Draw(sprite);
    }

    private void DrawSelected(RenderWindow window)
    {
        if (App.SelectedNode != this) return;

        var sprite = Content.GetResource(EGraphic.SelectedNode);
        sprite.Position = Position2D;
        window.Draw(sprite);
    }
    #endregion

    #region Dispose
    public void Clear()
    {
        Body = null;
        Terrain = null;
        Objects.Clear();
        Opacity = EOpacity.Opaque;
    }

    public void Dispose()
    {
        Body?.Dispose();
        Clear();
    }
    #endregion
}
