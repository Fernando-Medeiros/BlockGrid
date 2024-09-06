namespace SFMLGame.core.interfaces;

public interface IGameObject
{
    void Draw(RenderWindow window);
    void LoadContent();
    void LoadEvents(RenderWindow window);
}
