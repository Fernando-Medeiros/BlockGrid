namespace SFMLGame.core.components;

public sealed class StatusComponent : IStatusComponent
{
    private IBody2D Body { get; }
    private int Level { get; set; }
    private int Hp { get; set; }
    private int Mp { get; set; }
    private int Exp { get; set; }
    private int MaxHp { get; set; }
    private int MaxMp { get; set; }
    private int MaxExp { get; set; }

    public StatusComponent(IBody2D body)
    {
        Level = 1;
        Hp = 10;
        Mp = 10;
        MaxHp = 10;
        MaxMp = 10;
        MaxExp = 10;
        Body = body;
    }

    public void ReceiveDamage(int damage)
    {
        if (damage <= 0) return;

        Hp -= damage;

        Global.Invoke(EEvent.BasicStatus, new BasicStatus($"{Body.Sprite} Level {Level}", Level, Hp, MaxHp, Mp, MaxMp, Exp, MaxExp));

        Global.Invoke(EEvent.Logger, new Logger(ELogger.General, $"Attack :: {Body.Sprite} take {damage} damage!"));

        if (Hp <= 0)
        {
            Global.Invoke(EEvent.Logger, new Logger(ELogger.General, $"Defeat :: {Body.Sprite} level {Level} is dead!"));
            Body.Dispose();
        }
    }
}
