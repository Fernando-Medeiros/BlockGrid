namespace MAUIGame.Core;

public sealed record Position2D(byte Row, byte Column);
public sealed record ScenePackage(IList<IList<Tile2D>> Surface);
