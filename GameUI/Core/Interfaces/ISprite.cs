namespace GameUI.Core.Interfaces;

public interface ISprite
{
    public Sprite Image { get; set; }
    public IMovementComponent? Movement { get; set; }
}