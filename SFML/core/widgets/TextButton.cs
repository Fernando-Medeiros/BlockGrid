﻿namespace SFMLGame.core.widgets;

public sealed class TextButton : IButton, IDisposable
{
    private Text? Graphic { get; set; }
    private bool Focused { get; set; } = false;
    private bool Enabled { get; set; } = true;

    #region Property
    public required object Id { get; init; }
    public required string Text { get; set; }
    public required Vector2f Position { get; set; }

    public uint Size { get; set; } = 12;
    public float OutlineThickness { get; set; } = 1f;
    public EFont Font { get; set; } = EFont.Romulus;
    public EColor Color { get; set; } = EColor.White;
    public EColor OutlineColor { get; set; } = EColor.Black;
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    #endregion

    #region Action
    public void IsEnabled(bool value) => Enabled = value;
    #endregion

    #region Build
    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Graphic = new Text()
        {
            Position = Position,
            CharacterSize = Size,
            DisplayedString = Text,
            OutlineThickness = OutlineThickness,
            Font = Content.GetResource(Font),
            OutlineColor = Factory.Color(OutlineColor),
            FillColor = Focused ? Factory.Color(FocusedColor) : Factory.Color(Color),
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocus;

    private void OnButtonClicked(object? sender)
    {
        if (Enabled is false) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                OnClicked?.Invoke(Id);
                var sound = Content.GetResource(ESound.ButtonClicked);
                sound.Play();
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Enabled is false) return;

        if (sender is MouseDTO mouse)
        {
            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? false)
            {
                Focused = true;
                OnFocus?.Invoke(Id);
                return;
            }

            Focused = false;
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Graphic?.Dispose();
        Graphic = null;
        OnFocus = null;
        OnClicked = null;
        Global.UnSubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
