namespace SFMLGame.core.factories;

public static class Factory
{
    #region Color
    public static Color Color(EOpacity opacity) => opacity switch
    {
        EOpacity.Light => new(255, 255, 255, Convert.ToByte(opacity)),
        EOpacity.Regular => new(255, 255, 255, Convert.ToByte(opacity)),
        _ => throw new Exception()
    };
    #endregion

    #region Shuffle
    public static ESprite Shuffle(ESurface surface) => surface switch
    {
        ESurface.Desert => App.Shuffle([ESprite.SandA, ESprite.SandB]),
        ESurface.Soil => App.Shuffle([ESprite.GrassA, ESprite.GrassB, ESprite.GrassC, ESprite.GrassD]),
        _ => throw new Exception()
    };
    #endregion

    #region Build
    public static IBody2D Build(EBody body, INode2D node) => body switch
    {
        EBody.Npc => new NPCBody2D(node),
        EBody.Static => new StaticBody2D(node),
        EBody.Player => new PlayerBody2D(node),
        EBody.Enemy => new EnemyBody2D(node),
        _ => throw new Exception()
    };
    #endregion
}
