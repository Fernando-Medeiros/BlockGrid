namespace SFMLGame.core.widgets;

public class TextEntry : TextButton
{
    private void OnTextEntered(object? sender)
    {
        if ((char?)sender == '\b' && Text.Length > 0)
            Text = Text.Remove(Text.Length - 1, 1);
        else
            Text += $"{sender}";
    }

    public override void Event()
    {
        base.Event();
        Global.Subscribe(EEvent.TextEntered, OnTextEntered);
    }

    public override void Dispose()
    {
        base.Dispose();
        Global.UnSubscribe(EEvent.TextEntered, OnTextEntered);
    }
}
