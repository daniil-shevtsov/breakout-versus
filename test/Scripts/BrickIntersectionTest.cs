using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;
using static MyIntersection;
[TestSuite]
public partial class BrickIntersectionTest
{
    private GameConfig gameConfig = new GameConfig(new Vector2(800, 600));

    [TestCase]
    public async Task ShouldReturnZeroXForNotIntersectionRightBrickAndLeftPlatformer()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(4, 3));
        var platformer = new Rect(size: new Vector2(4, 6), center: new Vector2(-3, -3));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

    [TestCase]
    public async Task ShouldReturnZeroXForNotIntersectionLeftBrickAndRightPlatformer()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(-3, -3));
        var platformer = new Rect(size: new Vector2(4, 6), center: new Vector2(3, -3));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

    [TestCase]
    public async Task ShouldReturnXForIntersectionRightBrickAndLeftPlatformer()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(2, 0));
        var platformer = new Rect(size: new Vector2(4, 6), center: new Vector2(0, 0));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(2f);
    }

    [TestCase]
    public async Task ShouldReturnXForIntersectionLeftBrickAndRightPlatformer()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(-2, 0));
        var platformer = new Rect(size: new Vector2(4, 6), center: new Vector2(0, 0));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(2f);
    }

    [TestCase]
    public async Task ShouldReturnXForIntersectionPlatformerContainingBrick()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(0, 0));
        var platformer = new Rect(size: new Vector2(6, 6), center: new Vector2(0, 0));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(4f);
    }


    [TestCase]
    public async Task ShouldReturnXForIntersectionBrickContainingPlatformer()
    {
        var brick = new Rect(size: new Vector2(6, 4), center: new Vector2(0, 0));
        var platformer = new Rect(size: new Vector2(2, 2), center: new Vector2(0, 0));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(2f);
    }

    class Rect : Shaped
    {

        private Vector2 size;
        private RectangleShape2D _shape;
        private Vector2 _center;
        public Rect(Vector2 size, Vector2 center)
        {
            this.size = size;
            _center = center;
            _shape = new RectangleShape2D();
            _shape.Size = size;
        }
        public RectangleShape2D shape
        {
            get => _shape;
        }
        public Vector2 center
        {
            get => _center;
        }
    }
}