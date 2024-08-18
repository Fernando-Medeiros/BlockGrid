namespace SFMLGame.Core;

public sealed record Position2D(byte Row, byte Column, float PosX, float PosY);
public sealed record ScenePackage(IList<IList<Surface2D>> Surface);
