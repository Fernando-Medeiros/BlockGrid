﻿namespace SFMLGame.core.nodes;

public sealed class Node2D(Position2D position2D) : INode2D, IDisposable
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
        DrawDynamicBiome(window);
        DrawItems(window);
        DrawBody2D(window);
        DrawSelected(window);
    }
    #endregion

    #region Canva Layers
    private void DrawDynamicBiome(RenderWindow window)
    {
        if (Terrain is null) Terrain ??= Factory.Shuffle(App.CurrentBiome);

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
                Position = Position2D + new Vector2f(2, 2),
                Texture = sprite.Texture,
                Radius = Global.RECT / 2 - 2,
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

        var sprite = Content.GetResource(EGraphic.SelectedNode);
        sprite.Position = Position2D;
        window.Draw(sprite);
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Body = null;
        Terrain = null;
        GameItems.Clear();
        NavigationHandler = (_, _) => null;
    }
    #endregion
}
