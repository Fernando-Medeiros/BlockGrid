namespace SFMLGame.core.components;

public sealed class LightComponent : ILightComponent
{
    public void VisibilityTo(IBody2D body, EOpacity opacity)
    {
        EDirection[] directions = body.Metadata?.GetDirection() switch
        {
            EDirection.Top => [EDirection.Top, EDirection.Left, EDirection.Right],
            EDirection.Left => [EDirection.Left, EDirection.Bottom, EDirection.Top],
            EDirection.Right => [EDirection.Right, EDirection.Top, EDirection.Bottom],
            EDirection.Bottom => [EDirection.Bottom, EDirection.Left, EDirection.Right],
            EDirection.TopLeft => [EDirection.BottomLeft, EDirection.TopRight, EDirection.Left],
            EDirection.TopRight => [EDirection.BottomRight, EDirection.TopLeft, EDirection.Right],
            EDirection.BottomLeft => [EDirection.TopLeft, EDirection.BottomRight, EDirection.Left],
            EDirection.BottomRight => [EDirection.TopRight, EDirection.BottomLeft, EDirection.Right],
            _ => []
        };

        body.Node?.SetOpacity(opacity);

        EDirection[] collection;
        EDirection mainDirection = directions.First();

        static EDirection[] Repeat(EDirection direction, int level) =>
            Enumerable.Repeat(direction, level).ToArray();

        if (mainDirection is EDirection.Top or EDirection.Bottom or EDirection.Left or EDirection.Right)
        {
            foreach (var direction in directions)
                for (int level = 1; level <= 4; level++)
                {
                    collection = Repeat(direction, level);
                    body.Node?.Get(collection)?.SetOpacity(opacity);
                }

            foreach (var direction in directions.Skip(1).ToArray())
                for (int level = 1; level <= 3; level++)
                {
                    collection = [mainDirection, .. Repeat(direction, level)];
                    body.Node?.Get(collection)?.SetOpacity(opacity);

                    collection = [.. Repeat(mainDirection, level), direction];
                    body.Node?.Get(collection)?.SetOpacity(opacity);

                    if (level > 2) continue;

                    collection = [.. collection, direction];
                    body.Node?.Get(collection)?.SetOpacity(opacity);
                }
            return;
        }


        foreach (var direction in directions)
            for (int level = 1; level <= 3; level++)
            {
                collection = Repeat(direction, level);
                body.Node?.Get(collection)?.SetOpacity(opacity);

                if (direction == directions.Last()) continue;

                if (direction == directions[0] && level <= 2)
                    for (int repeat = 3; repeat > 1; repeat--)
                    {
                        collection = [.. Repeat(direction, level), .. Repeat(directions.Last(), repeat - level)];
                        body.Node?.Get(collection)?.SetOpacity(opacity);
                    }

                if (direction == directions[1])
                    for (int repeat = 4; repeat >= -1; repeat--)
                    {
                        int _repeat = (repeat, level) switch
                        {
                            ( > 0, 1) => repeat,
                            ( > -1, 2) => repeat + 1,
                            ( > -1, 3) => repeat + 2,
                            _ => repeat + 2
                        };
                        collection = [.. Repeat(direction, level), .. Repeat(directions.Last(), _repeat)];
                        body.Node?.Get(collection)?.SetOpacity(opacity);
                    }
            }
    }
}
