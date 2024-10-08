﻿namespace MAUIGame.Core.Entities;

public sealed class EnemyBody2D : IBody2D, IDisposable
{
    public EnemyBody2D(INode2D node)
    {
        Node = node;
        Sprite = Sprite2D.Spider;

        Health = new HealthComponent();
        Movement = new MovementComponent();
    }

    #region Property
    public INode2D? Node { get; private set; }
    public Sprite2D? Sprite { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IHealthComponent? Health { get; private set; }
    public IMovementComponent? Movement { get; private set; }
    #endregion

    #region Action
    public void Execute(object? key)
    {
    }

    public void SetSprite(Sprite2D? x) => Sprite = x;
    public void SetNode(INode2D? x) => Node = x;
    public void SetBody(IBody2D? x) { if (Node is not null) Node.Body = x; }
    #endregion

    public void Dispose()
    {
        Action = null;
        Health = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);
    }
}
