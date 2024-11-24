namespace SFMLGame.core.widgets;

public sealed class TextEntry : BaseButton<RectangleShape>
{
    public TextEntry(
        object id,
        string placeholder,
        Vector2f position,
        Vector2f borderSize)
    {
        Id = id;
        Position = position;
        BorderSize = borderSize;
        Placeholder = placeholder;

        EntryGraphic = new Text();
        Graphic = new RectangleShape();
    }

    #region SFML Property
    private Text EntryGraphic { get; set; }
    #endregion

    #region Control Property
    public bool LengthReached { get; private set; }
    #endregion

    #region Required Property
    public string Text { get; set; } = string.Empty;
    public string Placeholder { get; set; }
    public Vector2f BorderSize { get; set; }
    #endregion

    #region Custom Property
    public int Padding { get; } = 5;
    public EColor LengthReachedColor { get; set; } = EColor.Tomate;
    #endregion

    public override void Event()
    {
        base.Event();
        Global.Subscribe(EEvent.TextEntered, OnTextEntered);
    }

    public override void Render(RenderWindow window)
    {
        base.Render(window);

        Graphic.Size = BorderSize;
        Graphic.Position = Position;
        Graphic.OutlineThickness = Outline;
        Graphic.FillColor = Factory.Color(EColor.Transparent);
        Graphic.OutlineColor = Focused ? Factory.Color(FocusedColor)
            : LengthReached ? Factory.Color(LengthReachedColor)
            : Factory.Color(Color);

        EntryGraphic.CharacterSize = FontSize;
        EntryGraphic.OutlineThickness = Outline;
        EntryGraphic.Font = Content.GetResource<Font>(Font);
        EntryGraphic.Position = Position + new Vector2f(Padding, -Padding);
        EntryGraphic.OutlineColor = Factory.Color(OutlineColor);
        EntryGraphic.DisplayedString = Text.Length == 0 ? Placeholder : Text;
        EntryGraphic.FillColor = Focused ? Factory.Color(FocusedColor)
            : Selected ? Factory.Color(SelectedColor)
            : Factory.Color(Color);

        window.Draw(EntryGraphic);
        window.Draw(Graphic);
    }

    protected override void OnTextEntered(object? sender)
    {
        if (Disposed || Enabled is false || Selected is false) return;

        LengthReached = EntryGraphic?.GetLocalBounds().Width >= Graphic?.GetGlobalBounds().Width - (Padding * 4);

        string temp = $"{sender}";

        if (temp is Key.CEscape)
            Selected = false;

        else if (temp is Key.CBackspace)
        {
            Text = Text.Length > 0 ? Text.Remove(Text.Length - 1, 1) : Text;
            LengthReached = false;
        }

        else if (LengthReached)
            return;

        else if (temp is Key.CTab || temp is " ")
            Text += " ";

        else if (temp.Length > 0)
            Text += temp;

        base.OnTextEntered(sender);
    }

    public override void Dispose()
    {
        base.Dispose();
        EntryGraphic?.Dispose();
        Global.Unsubscribe(EEvent.TextEntered, OnTextEntered);
    }
}
