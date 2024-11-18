namespace SFMLGame.core.interfaces;

public interface INode2D
{
    EBiome? Biome { get; }
    ETerrain? Terrain { get; }
    IBody2D? Body { get; }
    EOpacity Opacity { get; }
    Position2D Position2D { get; }
    IList<IObject2D> Objects { get; }
    void Dispose();
    void SetBody(IBody2D? body);
    void SetOpacity(EOpacity opacity);
    void Draw(RenderWindow window);
    INode2D? Get(params EDirection[] directions);
}