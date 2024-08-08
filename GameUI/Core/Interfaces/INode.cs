namespace GameUI.Core.Interfaces;

public interface INode
{
    Tile Tile { get; set; }
    ISprite? Sprite { get; set; }
    IShader? Shader { get; set; }
    NodeCanva NodeCanva { get; }
    NodeNavigation NodeNavigation { get; }
    void ReDraw();
    void FadeTo(double opacity);
}