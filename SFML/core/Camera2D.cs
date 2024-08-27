namespace SFMLGame.core;

public sealed class Camera2D(FloatRect rect) : View(rect)
{
    public void OnZoomChanged(object? sender, MouseWheelScrollEventArgs e)
    {
        if (e.Delta == 1)
        {
            if (Size.X <= rect.Width / 2) return;
            Zoom(0.9f);
        }
        if (e.Delta == -1)
        {
            if (Size.Y >= rect.Height) return;
            Zoom(1.1f);
        }
    }

    public void OnCenterChanged(object? sender)
    {
        if (Is.Not<Position2D>(sender)) return;

        var (_, _, posX, posY) = (Position2D)sender!;

        var (width, height) = (Size.X, Size.Y);

        float maxHeight = Global.MAX_ROW * Global.RECT;
        float maxWidth = Global.MAX_COLUMN * Global.RECT;

        float scrollX = posX - (width / 2);
        float scrollY = posY - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, maxWidth - width));
        scrollY = Math.Max(0, Math.Min(scrollY, maxHeight - height));

        Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));
    }
}
