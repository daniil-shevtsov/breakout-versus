using Godot;
using GdUnit4;
using GdUnit4.Core;
using System.Threading.Tasks;
using System;

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
