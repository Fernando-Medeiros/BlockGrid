namespace SFMLGame.core.widgets;

public sealed class Button : BaseButton<RectangleShape>
{
    public Button(
      object id,
      string text,
      Vector2f position,
      Vector2f borderSize)
    {
        Id = id;
        Text = text;
        Position = position;
        BorderSize = borderSize;

        EntryGraphic = new Text();
        Graphic = new RectangleShape();
    }

    #region SFML Property
    private Text EntryGraphic { get; set; }
    #endregion

    #region Required Property
    public string Text { get; set; }
    public Vector2f BorderSize { get; set; }
    #endregion

    #region Custom Property
    public int Padding { get; } = 5;
    public EColor DisabledColor { get; set; } = EColor.Gray;
    #endregion

    public override void Render(RenderWindow window)
    {
        base.Render(window);

        // Border
        Graphic.Size = BorderSize;
        Graphic.Position = Position;
        Graphic.OutlineThickness = Outline;
        Graphic.OutlineColor = Factory.Color(OutlineColor);
        Graphic.FillColor = Enabled is false ? Factory.Color(DisabledColor)
            : Focused ? Factory.Color(FocusedColor) : Factory.Color(BackgroundColor);

        // Text
        EntryGraphic.CharacterSize = FontSize;
        EntryGraphic.DisplayedString = Text;
        EntryGraphic.OutlineThickness = Outline;
        EntryGraphic.Font = Content.GetResource<Font>(Font);
        EntryGraphic.OutlineColor = Factory.Color(OutlineColor);
        EntryGraphic.Position = Position + new Vector2f(BorderSize.X / 3f, -Padding);
        EntryGraphic.FillColor = Enabled is false ? Factory.Color(DisabledColor) : Factory.Color(Color);

        window.Draw(Graphic);
        window.Draw(EntryGraphic);
    }

    public override void Dispose()
    {
        base.Dispose();
        EntryGraphic?.Dispose();
    }
}
