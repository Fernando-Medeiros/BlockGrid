namespace GameUI.Core.Interfaces;

public interface IMovementComponent
{
    public void PushTo(ref INode node, object? key);
    public void MoveTo(ref INode node, object? key);
}
