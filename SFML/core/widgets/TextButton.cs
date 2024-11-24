namespace SFMLGame.core.widgets;

public sealed class TextButton : BaseButton<Text>
{
    public string Text { get; set; } = string.Empty;

    public TextButton(object id, string text, Vector2f position) : base()
    {
        Id = id;
        Text = text;
        Position = position;

        Graphic = new Text()
        {
            Position = Position,
            DisplayedString = Text,
            OutlineThickness = Outline,
            Font = Content.GetResource<Font>(Font),
        };
    }

    public override void Render(RenderWindow window)
    {
        base.Render(window);

        Graphic.Position = Position;
        Graphic.CharacterSize = FontSize;
        Graphic.DisplayedString = Text;
        Graphic.OutlineThickness = Outline;
        Graphic.Font = Content.GetResource<Font>(Font);
        Graphic.OutlineColor = Factory.Color(OutlineColor);
        Graphic.FillColor = Focused ? Factory.Color(FocusedColor)
            : Selected ? Factory.Color(SelectedColor)
            : Factory.Color(Color);

        window.Draw(Graphic);
    }
}
