namespace GameUI.Core.Sprites;

public sealed class PlayableSprite : ISprite
{
    public PlayableSprite(INode node)
    {
        Node = node;

        App.Subscribe(Event.KeyDown, MoveTo);
    }

    public Sprite Image { get; set; }
    public INode Node { get; set; }

    public void MoveTo(object? key)
    {
        INode? node = key switch
        {
            Key.W or Key.Up => Node.NodeNavigation.Top,
            Key.A or Key.Left => Node.NodeNavigation.Left,
            Key.D or Key.Right => Node.NodeNavigation.Right,
            Key.S or Key.Down => Node.NodeNavigation.Bottom,
            _ => null
        };

        if (node is null || node.Sprite != null) return;

        Node.Sprite = null;
        Node = node;
        Node.Sprite = this;
    }
}
