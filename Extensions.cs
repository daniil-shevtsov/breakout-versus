using Godot;

static class MyExtensions
{
    public static float Left(this RectangleShape2D shape, Vector2 center)
    {
        return center.X - shape.Size.X / 2;
    }

    public static float Right(this RectangleShape2D shape, Vector2 center)
    {
        return center.X + shape.Size.X / 2;
    }

    public static float Bottom(this RectangleShape2D shape, Vector2 center)
    {
        return center.Y + shape.Size.Y / 2;
    }

    public static float Top(this RectangleShape2D shape, Vector2 center)
    {
        return center.Y - shape.Size.Y / 2;
    }

    public static float Left(this CircleShape2D shape, Vector2 center)
    {
        return center.X - shape.Radius / 2;
    }

    public static float Right(this CircleShape2D shape, Vector2 center)
    {
        return center.X + shape.Radius / 2;
    }

    public static float Bottom(this CircleShape2D shape, Vector2 center)
    {
        return center.Y + shape.Radius / 2;
    }

    public static float Top(this CircleShape2D shape, Vector2 center)
    {
        return center.Y - shape.Radius / 2;
    }
}
