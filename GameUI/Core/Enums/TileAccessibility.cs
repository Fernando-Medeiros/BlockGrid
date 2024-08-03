namespace GameUI.Core.Enums;

[Flags]
public enum TileAccessibility : byte
{
    Block = Tile.Water | Tile.House,
}