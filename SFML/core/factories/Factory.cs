namespace SFMLGame.core.factories;

public static class Factory
{
    public static Color Get(ESurface? surface) => surface switch
    {
        ESurface.Soil => Colors.Soil,
        ESurface.Desert => Colors.Desert,
        _ => throw new Exception()
    };

    public static IBody2D Get(EBody? body, INode2D node) => body switch
    {
        EBody.Npc => new NPCBody2D(node),
        EBody.Static => new StaticBody2D(node),
        EBody.Player => new PlayerBody2D(node),
        EBody.Enemy => new EnemyBody2D(node),
        _ => throw new Exception()
    };
}
