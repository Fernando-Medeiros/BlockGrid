namespace MAUIGame.Core.Enums;

public enum Tile2D : byte
{
    House,
    Grass,
    Road,
    Desert,
    Water,
}

public enum TileTexture : byte
{
    Image,
    ASCII,
    Color,
}

public readonly struct TileAccess
{
    public static IReadOnlyList<Tile2D> BlockedTiles { get; } = [
        Tile2D.Water, Tile2D.House
        ];

    public static bool ItsBlocked(Tile2D tile) => BlockedTiles.Contains(tile);
}
