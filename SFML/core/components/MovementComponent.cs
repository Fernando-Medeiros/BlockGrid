namespace SFMLGame.core.components;

public sealed class MovementComponent : IMovementComponent
{
    public void MoveTo(IBody2D body, object? keyCode)
    {
        var node = body.Metadata?.ResolveDirection(body, keyCode);

        if (Is.Null(node) || Is.Type<IBody2D>(node?.Body)) return;

        body.SetBody(null);
        body.SetNode(node);
        body.SetBody(body);

        var sound = Content.GetResource(ESound.StepLeaves0);
        sound.Volume = 10;
        sound.Play();
    }
}
