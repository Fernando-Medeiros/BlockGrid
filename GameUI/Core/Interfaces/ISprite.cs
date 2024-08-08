namespace GameUI.Core.Interfaces;

public interface ISprite
{
    Sprite Image { get; set; }
    IActionComponent? Action { get; }
    IHealthComponent? Health { get; }
    IMovementComponent? Movement { get; }
}