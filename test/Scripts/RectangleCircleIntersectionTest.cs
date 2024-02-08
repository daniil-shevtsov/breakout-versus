using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class RectangleCircleIntersectionTest
{
    [TestCase]
    public async Task ShouldNotIntersectWhenShapesFarFromEachOther()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(20, 10),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(100, 100)
        );
        AssertBool(intersection).IsFalse();
    }

    [TestCase]
    public async Task ShouldIntersectionWhenIntersectedBottomRight()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(20, 10),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(20, 10)
        );
        AssertBool(intersection).IsTrue();
    }

    [TestCase]
    public async Task ShouldIntersectWhenIntersectedBottomCenter()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(20, 10),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(10, 10)
        );
        AssertBool(intersection).IsTrue();
    }

    [TestCase]
    public async Task ShouldIntersectWhenRectangleContainsWholeCircle()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(40, 40),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(20, 20)
        );
        AssertBool(intersection).IsTrue();
    }

    [TestCase]
    public async Task ShouldIntersectWhenRectangleIntersectsQuarterOfCircle()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(80, 40),
            rectTopLeft: new Vector2(0, 0),
            circleRadius: 6f,
            circleCenter: new Vector2(83, 43)
        );
        AssertBool(intersection).IsTrue();
    }

    [TestCase]
    public async Task ShouldIntersectWhenCircleContainsWholeRectangleAndCentersSame()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(24, 40),
            rectTopLeft: new Vector2(148, 120),
            circleRadius: 128,
            circleCenter: new Vector2(160, 160)
        );
        AssertBool(intersection).IsTrue();
    }

    [TestCase]
    public async Task ShouldIntersectWhenCircleContainsWholeRectangleAndCentersDifferent()
    {
        var intersection = MyCollisionDetection.IsIntersection(
            rectSize: new Vector2(24, 40),
            rectTopLeft: new Vector2(158, 142),
            circleRadius: 128,
            circleCenter: new Vector2(160, 160)
        );
        AssertBool(intersection).IsTrue();
    }

    //TODO: Uncomment when there is logic to calculate dx dy of the intersection
    // [TestCase]
    // public async Task ShouldReturnZerosWhenNoIntersection()
    // {
    //     var intersection = MyCollisionDetection.Intersection(
    //         rectSize: new Vector2(20, 10),
    //         rectTopLeft: new Vector2(0, 0),
    //         circleRadius: 6f,
    //         circleCenter: new Vector2(100, 100)
    //     );
    //     AssertObject(intersection).IsEqual(Vector2.Zero);
    // }

    // [TestCase]
    // public async Task ShouldReturnIntersectionWhenIntersectedBottomRight()
    // {
    //     var intersection = MyCollisionDetection.Intersection(
    //         rectSize: new Vector2(20, 10),
    //         rectTopLeft: new Vector2(0, 0),
    //         circleRadius: 6f,
    //         circleCenter: new Vector2(20, 10)
    //     );
    //     AssertObject(intersection).IsEqual(new Vector2(6f, 6f));
    // }

    // [TestCase]
    // public async Task ShouldReturnIntersectionWhenIntersectedBottomCenter()
    // {
    //     var intersection = MyCollisionDetection.Intersection(
    //         rectSize: new Vector2(20, 10),
    //         rectTopLeft: new Vector2(0, 0),
    //         circleRadius: 6f,
    //         circleCenter: new Vector2(10, 10)
    //     );
    //     AssertObject(intersection).IsEqual(new Vector2(12f, 6f));
    // }

    // [TestCase]
    // public async Task ShouldReturnIntersectionWhenRectangleContainsWholeCircle()
    // {
    //     var intersection = MyCollisionDetection.Intersection(
    //         rectSize: new Vector2(40, 40),
    //         rectTopLeft: new Vector2(0, 0),
    //         circleRadius: 6f,
    //         circleCenter: new Vector2(20, 20)
    //     );
    //     AssertObject(intersection).IsEqual(new Vector2(12f, 12f));
    // }

    // [TestCase]
    // public async Task ShouldReturnIntersectionWhenRectangleIntersectsQuarterOfCircle()
    // {
    //     var intersection = MyCollisionDetection.Intersection(
    //         rectSize: new Vector2(80, 40),
    //         rectTopLeft: new Vector2(0, 0),
    //         circleRadius: 6f,
    //         circleCenter: new Vector2(83, 43)
    //     );
    //     AssertObject(intersection).IsEqual(new Vector2(3f, 3f));
    // }
}
