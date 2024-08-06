namespace GameUI.Core.Sprites;

public sealed class PlayableSprite : ISprite
{
    public PlayableSprite(INode node)
    {
        node.FadeTo(1);
        Image = Sprite.Aracne;

        Movement = new MovementComponent();

        App.Subscribe(Event.KeyDown, (key) =>
        {
            Movement?.PushTo(ref node, key);
            Movement?.MoveTo(ref node, key);
        });
    }

    #region Property
    public Sprite Image { get; set; }
    public IMovementComponent? Movement { get; set; }
    #endregion
}
