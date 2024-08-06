namespace GameUI.Core.Sprites;

public sealed class StaticSprite : ISprite
{
    public StaticSprite(INode node)
    {
        node.FadeTo(1);
        Image = Sprite.Spider;

        Movement = new MovementComponent();
    }

    #region Property
    public Sprite Image { get; set; }
    public IMovementComponent? Movement { get; set; }
    #endregion
}
