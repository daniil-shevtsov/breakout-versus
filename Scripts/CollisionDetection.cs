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
        var rectBottom = rectTopLeft.Y + rectSize.Y;
        var rectRight = rectTopLeft.X + rectSize.X;
        var rectCenterX = rectTopLeft.X + rectSize.X / 2;
        var rectCenterY = rectTopLeft.Y + rectSize.Y / 2;
        var rectBottomRight = new Vector2(rectRight, rectBottom);
        var rectBottomCenter = new Vector2(rectCenterX, rectBottom);
        var rectCenter = new Vector2(rectCenterX, rectCenterY);

        var circleDiameter = circleRadius * 2;

        var intersection = Vector2.Zero;
        if (circleCenter == rectBottomRight)
        {
            intersection = new Vector2(circleRadius, circleRadius);
        }
        else if (circleCenter == rectBottomCenter)
        {
            intersection = new Vector2(circleDiameter, circleRadius);
        }
        else if (circleCenter == rectCenter)
        {
            intersection = new Vector2(circleDiameter, circleDiameter);
        }
        else if (
            new Vector2(circleCenter.X - circleRadius / 2, circleCenter.Y - circleRadius / 2)
            == rectBottomRight
        )
        {
            intersection = new Vector2(circleRadius / 2, circleRadius / 2);
        }
        return intersection;
    }
}
