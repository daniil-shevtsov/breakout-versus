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
    public async Task ShouldReturnZeroXForNotIntersectionLeftPlatformerAndRightBrick()
    {
        var brick = new Rect(size: new Vector2(40, 20), center: new Vector2(520, 530));
        var platformer = new Rect(size: new Vector2(50, 80), center: new Vector2(110, 120));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

    [TestCase]
    public async Task ShouldReturnZeroXForNotIntersectionRightPlatformerAndLeftBrick()
    {
        var brick = new Rect(size: new Vector2(40, 20), center: new Vector2(520, 530));
        var platformer = new Rect(size: new Vector2(50, 80), center: new Vector2(110, 120));

        var intersection = BrickIntersection(brick, platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

    [TestCase]
    public async Task ShouldReturnXForIntersectionLeftPlatformerAndRightBrick()
    {
        var brick = new Rect(size: new Vector2(4, 2), center: new Vector2(2, 0));
        var platformer = new Rect(size: new Vector2(4, 6), center: new Vector2(0, 0));

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