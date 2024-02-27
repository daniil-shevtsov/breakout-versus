using System;
using Godot;

static class MyIntersection
{
    public static Vector2 BrickIntersection(Shaped brick, Shaped platformer)
    {
        var isBrickLeft = brick.center < platformer.center;
        bool isNotIntersected;
        if (isBrickLeft)
        {
            isNotIntersected = brick.Right() < platformer.Left();
        }
        else
        {
            isNotIntersected = platformer.Right() < brick.Left();
        }
        float intersectionX;
        if (isBrickLeft)
        {
            intersectionX = platformer.Left() - brick.Right();
        }
        else
        {
            intersectionX = platformer.Right() - brick.Left();
        }

        if (isNotIntersected)
        {
            intersectionX = 0f;
        }

        return new Vector2(Mathf.Abs(intersectionX), 0f);
    }
}
