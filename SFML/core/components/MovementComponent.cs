namespace SFMLGame.core.components;

public sealed class MovementComponent : IMovementComponent
{
    public void MoveTo(IBody2D body, object? keyCode)
    {
        var node = body.Metadata?.ResolveDirection(body, keyCode);

        if (node is null || node.Body is not null) return;

        body.SetBody(null);
        body.SetNode(node);
        body.SetBody(body);

        var sound = Content.GetResource(Factory.Shuffle((ETerrain)node.Terrain));
        sound.Volume = App.CurrentSoundVolume;
        sound.Play();
    }
}
