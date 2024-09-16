namespace SFMLGame.core.components;

public sealed class LightComponent : ILightComponent
{
    public void VisibilityTo(IBody2D body, EOpacity opacity)
    {
        EDirection[] directions = Factory.Resolve(body.Metadata?.GetDirection() ?? default);

        body.Node?.SetOpacity(opacity);

        EDirection[] collection;
        EDirection mainDirection = directions.First();

        static EDirection[] Repeat(EDirection direction, int level) =>
            Enumerable.Repeat(direction, level).ToArray();

        if (mainDirection is EDirection.Top or EDirection.Bottom or EDirection.Left or EDirection.Right)
        {
            foreach (var direction in directions)
                for (int row = 1; row <= 6; row++)
                {
                    collection = Repeat(direction, row);
                    body.Node?.Get(collection)?.SetOpacity(opacity);
                }

            foreach (var direction in directions.Skip(1).ToArray())
                for (int row = 1; row <= 6; row++)
                {
                    for (int column = 7; column >= Math.Max(1, row - 2); column--)
                    {
                        collection = [.. Repeat(direction, row), .. Repeat(mainDirection, Math.Max(1, column - row))];
                        body.Node?.Get(collection)?.SetOpacity(opacity);
                    }
                }
            return;
        }


        foreach (var direction in directions)
            for (int row = 1; row <= 5; row++)
            {
                collection = Repeat(direction, row);
                body.Node?.Get(collection)?.SetOpacity(opacity);

                if (direction == directions.Last()) continue;

                if (direction == directions[0] && row <= 4)
                    for (int repeat = 5; repeat >= 1; repeat--)
                    {
                        collection = [.. Repeat(direction, row), .. Repeat(directions.Last(), Math.Max(1, repeat - row))];
                        body.Node?.Get(collection)?.SetOpacity(opacity);
                    }

                if (direction == directions[1])
                    for (int repeat = 6; repeat >= -1; repeat--)
                    {
                        int _repeat = (repeat, row) switch
                        {
                            ( > 0, 1) => repeat,
                            ( > -1, 2) => repeat + 1,
                            ( > -1, 3) => repeat + 2,
                            _ => repeat + 2
                        };
                        collection = [.. Repeat(direction, row), .. Repeat(directions.Last(), _repeat)];
                        body.Node?.Get(collection)?.SetOpacity(opacity);
                    }
            }
    }
}
