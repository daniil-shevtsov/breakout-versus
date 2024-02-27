using System;
using Godot;

static class MyIntersection
{
    public static Vector2 BrickIntersection(Shaped brick, Shaped platformer)
    {
        var isBrickLeft = brick.center < platformer.center;
        var isBrickInsidePlatformer = brick.Left() > platformer.Left() && brick.Right() < platformer.Right();
        var isPlatformerInsideBrick = platformer.Left() > brick.Left() && platformer.Right() < brick.Right();

        bool noOverlap = brick.Left() > platformer.Right() ||
                            platformer.Left() > brick.Right() ||
                            brick.Top() > platformer.Bottom() ||
                            platformer.Top() > brick.Bottom();

        if (!noOverlap)
        {
            float intersectionX;
            if (isBrickLeft)
            {
                intersectionX = platformer.Left() - brick.Right();
            }
            else
            {
                intersectionX = platformer.Right() - brick.Left();
            }

            if (isBrickInsidePlatformer)
            {
                intersectionX = brick.shape.Size.X;
            }
            else if (isPlatformerInsideBrick)
            {
                intersectionX = platformer.shape.Size.X;
            }

            return new Vector2(Mathf.Abs(intersectionX), 0f);
        }
        else
        {
            return Vector2.Zero;
        }


    }
}
