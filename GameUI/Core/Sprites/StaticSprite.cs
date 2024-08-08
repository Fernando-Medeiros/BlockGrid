namespace GameUI.Core.Sprites;

public sealed class StaticSprite : ISprite
{
    public StaticSprite(INode node)
    {
        Image = Sprite.Spider;

        Health = new HealthComponent();
        Movement = new MovementComponent();
    }

    #region Property
    public Sprite Image { get; set; }
    public IActionComponent? Action { get; }
    public IHealthComponent? Health { get; }
    public IMovementComponent? Movement { get; }
    #endregion
}
