namespace MAUIGame.Core.Components;

public sealed class MovementComponent : IMovementComponent
{
    public void PushTo(IBody2D body, object? key)
    {
        var node = body.Node?.Navigate.GetNode(key);

        if (Is.Null(node) || Is.Null(node?.Body)) return;

        node?.Body?.Movement?.MoveTo(node.Body, key);
    }

    public void MoveTo(IBody2D body, object? key)
    {
        var node = body.Node?.Navigate.GetNode(key);

        if (Is.Null(node) || Is.Type<IBody2D>(node?.Body) || Is.Blocked(node?.Tile)) return;

        node?.Navigate?.VisibilityTo(0.2);

        MoveBy(body, node);

        node?.FadeTo(1.0);

        node?.Navigate?.VisibilityTo(1.0);
    }

    public void MoveBy(IBody2D body, INode2D? node)
    {
        if (Is.Null(node) || Is.Type<IBody2D>(node?.Body) || Is.Blocked(node?.Tile)) return;

        body.SetBody(null);
        body.SetNode(node);
        body.SetBody(body);
    }
}
