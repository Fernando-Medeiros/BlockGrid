﻿namespace SFMLGame.core.widgets;

public sealed class TextButton : IButton, IDisposable
{
    private Text? Graphic { get; set; }

    #region Control Property
    public bool Activated { get; private set; } = true;
    public bool Focused { get; set; }
    public bool Selected { get; set; }
    #endregion

    #region Required Property
    public required object Id { get; init; }
    public required string Text { get; set; }
    public required Vector2f Position { get; set; }
    #endregion

    #region Custom Property
    public uint Size { get; set; } = 12;
    public float Outline { get; set; } = 1f;

    public EFont Font { get; set; } = EFont.Romulus;
    public EColor Color { get; set; } = EColor.White;
    public EColor OutlineColor { get; set; } = EColor.Black;
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    public EColor SelectedColor { get; set; } = EColor.CornFlowerBlue;
    #endregion

    #region Action
    public bool Equal(object? value) => Id.Equals(value);
    public void SetActivated(bool value) => Activated = value;
    #endregion

    #region Build
    public void Event()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public void Render(RenderWindow window)
    {
        window.Draw(Graphic = new Text()
        {
            Position = Position,
            CharacterSize = Size,
            DisplayedString = Text,
            OutlineThickness = Outline,
            Font = Content.GetResource<Font>(Font),
            OutlineColor = Factory.Color(OutlineColor),
            FillColor = Focused ? Factory.Color(FocusedColor)
                : Selected ? Factory.Color(SelectedColor)
                : Factory.Color(Color),
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnChanged;
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocused;

    private void OnButtonClicked(object? sender)
    {
        if (Activated is false) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                Selected = !Selected;
                OnClicked?.Invoke(Id);

                var sound = Content.GetResource<Sound>(ESound.ButtonClicked);
                sound.Volume = App.Configuration.SoundVolume;
                sound.Play();
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Activated is false) return;

        if (sender is MouseDTO mouse)
        {
            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? false)
            {
                Focused = true;
                OnFocused?.Invoke(Id);
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
        OnClicked = null;
        OnFocused = null;
        OnChanged = null;
        Global.Unsubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Unsubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
