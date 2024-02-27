using Godot;

static class MyIntersection
{
    public static Vector2 BrickIntersection(Shaped brick, Shaped platformer)
    {
        var intersectionX = platformer.Right() - brick.Left();
        if (Mathf.Abs(platformer.center.X - brick.center.X) < 30)
        {
            return new Vector2(intersectionX, 0f);
        }
        else
        {
            return Vector2.Zero;
        }

    }
}
