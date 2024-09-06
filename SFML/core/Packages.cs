namespace SFMLGame.core;

public sealed record Position2D(byte Row, byte Column, float X, float Y);
public sealed record BasicStatus(string Name, int Level, int Hp, int MaxHp, int Mp, int MaxMp, int Exp, int MaxExp);
public sealed record Logger(ELogger Guide, string Message);
public sealed record ScenePackage(IList<IList<Surface2D>> Surface);
