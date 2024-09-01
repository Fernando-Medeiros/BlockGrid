namespace SFMLGame.core.components;

public sealed class LightComponent : ILightComponent
{
    public void VisibilityTo(INode2D node, EOpacity opacity)
    {
        node.Get(EDirection.Top)?.SetOpacity(opacity);
        node.Get(EDirection.TopLeft)?.SetOpacity(opacity);
        node.Get(EDirection.TopRight)?.SetOpacity(opacity);
        node.Get(EDirection.Right)?.SetOpacity(opacity);
        node.SetOpacity(opacity);
        node.Get(EDirection.Left)?.SetOpacity(opacity);
        node.Get(EDirection.BottomLeft)?.SetOpacity(opacity);
        node.Get(EDirection.BottomRight)?.SetOpacity(opacity);
        node.Get(EDirection.Bottom)?.SetOpacity(opacity);
    }
}
