using Godot;

static class MyCollisionDetection
{
    public static bool IsIntersection(
        Vector2 rectSize,
        Vector2 rectTopLeft,
        float circleRadius,
        Vector2 circleCenter
    )
    {
        var rectTop = rectTopLeft.Y;
        var rectBottom = rectTopLeft.Y + rectSize.Y;
        var rectRight = rectTopLeft.X + rectSize.X;
        var rectLeft = rectTopLeft.X;
        var rectCenterX = rectTopLeft.X + rectSize.X / 2;
        var rectCenterY = rectTopLeft.Y + rectSize.Y / 2;
        var rectBottomRight = new Vector2(rectRight, rectBottom);
        var rectBottomCenter = new Vector2(rectCenterX, rectBottom);
        var rectCenter = new Vector2(rectCenterX, rectCenterY);

        var circleDiameter = circleRadius * 2;

        var closestRectanglePointToCircle = new Vector2(
            Mathf.Clamp(circleCenter.X, rectLeft, rectRight),
            Mathf.Clamp(circleCenter.Y, rectTop, rectBottom)
        );
        var distance = new Vector2(
            circleCenter.X - closestRectanglePointToCircle.X,
            circleCenter.Y - closestRectanglePointToCircle.Y
        );
        var isIntersection =
            (distance.X * distance.X + distance.Y * distance.Y) <= circleRadius * circleRadius;

        return isIntersection;
    }

    //TODO: Need to calculate correct dx dy of the intersection
    public static Vector2 Intersection(
        Vector2 rectSize,
        Vector2 rectTopLeft,
        float circleRadius,
        Vector2 circleCenter
    )
    {
        var rectTop = rectTopLeft.Y;
        var rectBottom = rectTopLeft.Y + rectSize.Y;
        var rectRight = rectTopLeft.X + rectSize.X;
        var rectLeft = rectTopLeft.X;
        var rectCenterX = rectTopLeft.X + rectSize.X / 2;
        var rectCenterY = rectTopLeft.Y + rectSize.Y / 2;
        var rectBottomRight = new Vector2(rectRight, rectBottom);
        var rectBottomCenter = new Vector2(rectCenterX, rectBottom);
        var rectCenter = new Vector2(rectCenterX, rectCenterY);

        var circleDiameter = circleRadius * 2;

        var closestRectanglePointToCircle = new Vector2(
            Mathf.Clamp(circleCenter.X, rectLeft, rectRight),
            Mathf.Clamp(circleCenter.Y, rectTop, rectBottom)
        );
        var distance = new Vector2(
            circleCenter.X - closestRectanglePointToCircle.X,
            circleCenter.Y - closestRectanglePointToCircle.Y
        );
        var isIntersection =
            (distance.X * distance.X + distance.Y * distance.Y) <= circleRadius * circleRadius;

        var intersection = Vector2.Zero;
        GD.Print(
            $"is intersection: {isIntersection}\nclosest: {closestRectanglePointToCircle}\ndistance: {distance}\n\n"
        );
        if (isIntersection)
        {
            intersection = Vector2.Zero;
        }
        else
        {
            intersection = Vector2.Zero;
        }

        return intersection;
    }
}
