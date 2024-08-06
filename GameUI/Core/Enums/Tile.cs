namespace GameUI.Core.Enums;

public enum Tile : byte
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
    public static IReadOnlyList<Tile> BlockedTiles { get; } = [
        Tile.Water, Tile.House
        ];

    public static bool ItsBlocked(Tile tile) => BlockedTiles.Contains(tile);
}
