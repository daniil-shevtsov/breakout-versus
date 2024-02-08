using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class RectangleCircleIntersectionTest
{
    [TestCase]
    public async Task ShouldReturnZerosWhenNoIntersection()
    {
        var intersection = MyCollisionDetection.Intersection(
            rectSize: new Vector2(20, 10),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(100, 100)
        );
        AssertObject(intersection).IsEqual(Vector2.Zero);
    }

    [TestCase]
    public async Task ShouldReturnIntersectionWhenIntersectedBottomRight()
    {
        var intersection = MyCollisionDetection.Intersection(
            rectSize: new Vector2(20, 10),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(20, 10)
        );
        AssertObject(intersection).IsEqual(new Vector2(6f, 6f));
    }
}
