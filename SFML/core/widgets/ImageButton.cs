namespace SFMLGame.core.widgets;

public sealed class ImageButton : BaseButton<Sprite>
{
    public Enum Image { get; set; }

    public ImageButton(object id, Enum image, Vector2f position) : base()
    {
        Id = id;
        Image = image;
        Position = position;

        Graphic = new Sprite(copy: Content.GetResource<Sprite>(Image))
        {
            Position = Position,
        };
    }

    public override void Render(RenderWindow window)
    {
        base.Render(window);

        Graphic.Color = Factory.Color(EOpacity.Light);
        Graphic.Position = Position;
        window.Draw(Graphic);

        if (Focused || Selected)
        {
            var sprite = Content.GetResource<Sprite>(EGraphic.SelectedNode);
            sprite.Color = Factory.Color(EOpacity.Light);
            sprite.Position = Position;
            window.Draw(sprite);
        }
    }
}
