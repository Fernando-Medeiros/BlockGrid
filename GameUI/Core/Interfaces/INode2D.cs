namespace GameUI.Core.Interfaces;

public interface INode2D
{
    Tile2D Tile { get; set; }
    IBody2D? Body { get; set; }
    IShader? Shader { get; set; }
    NodeCanva Canva { get; }
    NodeNavigation Navigate { get; }
    void Draw();
    void FadeTo(double opacity);
}