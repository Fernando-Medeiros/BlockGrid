namespace GameUI.Core.Sprites;

public sealed class PlayableSprite : ISprite
{
    public PlayableSprite(INode node)
    {
        node.FadeTo(1);
        Image = Sprite.Aracne;

        Action = new ActionComponent();
        Health = new HealthComponent();
        Movement = new MovementComponent();

        App.Subscribe(Event.KeyDown, (key) =>
        {
            Movement?.PushTo(ref node, ref key);
            Movement?.MoveTo(ref node, ref key);
            Action?.DamageTo(ref node, ref key);
        });
    }

    #region Property
    public Sprite Image { get; set; }
    public IActionComponent? Action { get; }
    public IHealthComponent? Health { get; }
    public IMovementComponent? Movement { get; }
    #endregion
}
