namespace SFMLGame.core;

public sealed record Position2D(byte Row, byte Column, float X, float Y);

public sealed record MouseDTO(EMouse Button, int X, int Y);
public sealed record RegionDTO(IList<IList<ESurface>> Surface);
public sealed record LoggerDTO(ELogger Guide, string Message);
public sealed record StatusDTO(string Name, int Level, int Hp, int MaxHp, int Mp, int MaxMp, int Exp, int MaxExp);
