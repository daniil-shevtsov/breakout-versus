using Godot;
using GdUnit4;
using GdUnit4.Core;
using System.Threading.Tasks;
using System;

static class MyExtensions
{
    public static bool Intersects(this Brick brick, Ball ball, Vector2 newBallPosition)
    {
        return MyCollisionDetection.IsIntersection(
            rectSize: brick.rectangleShape.Size,
            rectTopLeft: brick.rectangleShape.TopLeft(brick.GlobalPosition),
            circleRadius: ball.shape.Radius,
            circleCenter: newBallPosition
        );
    }

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

    public static Vector2 TopLeft(this RectangleShape2D shape, Vector2 center)
    {
        return new Vector2(shape.Left(center), shape.Top(center));
    }

    public static float Top(this Ball ball)
    {
        return ball.shape.Top(ball.GlobalPosition);
    }

    public static float Left(this Ball ball)
    {
        return ball.shape.Left(ball.GlobalPosition);
    }

    public static float Right(this Ball ball)
    {
        return ball.shape.Right(ball.GlobalPosition);
    }

    public static float Bottom(this Ball ball)
    {
        return ball.shape.Bottom(ball.GlobalPosition);
    }

    public static Vector2 Center(this Ball ball)
    {
        return ball.GlobalPosition;
    }

    public static float Top(this Shaped paddle)
    {
        return paddle.shape.Top(paddle.center);
    }

    public static float Left(this Shaped paddle)
    {
        return paddle.shape.Left(paddle.center);
    }

    public static float Right(this Shaped paddle)
    {
        return paddle.shape.Right(paddle.center);
    }

    public static float Bottom(this Shaped paddle)
    {
        return paddle.shape.Bottom(paddle.center);
    }

    public static Vector2 TopCenter(this Ball ball)
    {
        return new Vector2(ball.GlobalPosition.X, ball.Top());
    }

    public static Vector2 BottomCenter(this Ball ball)
    {
        return new Vector2(ball.GlobalPosition.X, ball.Bottom());
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

    public static Vector2 TopLeft(this Brick brick)
    {
        return brick.rectangleShape.TopLeft(brick.GlobalPosition);
    }

    public static async Task AwaitPhysicsProcessCalls(this ISceneRunner runner, uint n)
    {
        var scene = runner.Scene();
        for (int i = 0; i < n; ++i)
        {
            await scene.ToSignal(scene.GetTree(), SceneTree.SignalName.PhysicsFrame);
            await runner.AwaitMillis(1);
        }
    }
}
