using Godot;

static class MyCollisionDetection
{
    public static Vector2 Intersection(
        Vector2 rectSize,
        Vector2 rectTopLeft,
        float circleRadius,
        Vector2 circleCenter
    )
    {
        var rectRightBottom = new Vector2(rectTopLeft.X + rectSize.X, rectTopLeft.Y + rectSize.Y);
        var intersection = Vector2.Zero;
        if (circleCenter == rectRightBottom)
        {
            intersection = new Vector2(circleRadius, circleRadius);
        }
        return intersection;
    }
}
