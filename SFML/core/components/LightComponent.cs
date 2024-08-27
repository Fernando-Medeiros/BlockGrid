namespace SFMLGame.Core.Components;

public sealed class LightComponent : ILightComponent
{
    public void VisibilityTo(INode2D node, Opacity opacity)
    {
        node.Get(Direction.Top)?.SetOpacity(opacity);
        node.Get(Direction.TopLeft)?.SetOpacity(opacity);
        node.Get(Direction.TopRight)?.SetOpacity(opacity);
        node.Get(Direction.Right)?.SetOpacity(opacity);
        node.SetOpacity(opacity);
        node.Get(Direction.Left)?.SetOpacity(opacity);
        node.Get(Direction.BottomLeft)?.SetOpacity(opacity);
        node.Get(Direction.BottomRight)?.SetOpacity(opacity);
        node.Get(Direction.Bottom)?.SetOpacity(opacity);
    }
}
