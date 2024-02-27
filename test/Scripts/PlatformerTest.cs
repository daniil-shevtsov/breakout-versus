using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class PlatformerTest
{
    private GameConfig gameConfig = new GameConfig(new Vector2(800, 600));

    [TestCase]
    public async Task ShouldReturnZeroXForNotIntersectionLeftPlatformerAndRightBrink()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        await runner.SimulateFrames(3);

        var brickPosition = new Vector2(0,0);
        var platformerPosition = new Vector2(1000, 0);
        game.bricks[0].GlobalPosition = brickPosition;
        game.platformer.GlobalPosition = platformerPosition;

        var intersection = game.BrickIntersection(game.bricks[0], game.platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

        [TestCase]
    public async Task ShouldReturnZeroXForNotIntersectionRightPlatformerAndLeftBrink()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        await runner.SimulateFrames(3);

        var brickPosition = new Vector2(1000,0);
        var platformerPosition = new Vector2(0, 0);
        game.bricks[0].GlobalPosition = brickPosition;
        game.platformer.GlobalPosition = platformerPosition;

        var intersection = game.BrickIntersection(game.bricks[0], game.platformer);

        AssertFloat(intersection.X).IsEqual(0f);
    }

    [TestCase]
    public async Task ShouldStayGroundedWhenStandingOnABrick()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        runner.MaximizeView();
        runner.SetTimeFactor(0.25);
        await runner.SimulateFrames(3);

        var groundedPosition = new Vector2(
            game.platformer.GlobalPosition.X,
            game.bricks[0].Top() - game.platformer.shape.Size.Y / 2
        );
        game.platformer.GlobalPosition = groundedPosition;

        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.platformer.GlobalPosition).IsEqual(groundedPosition);

        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.platformer.GlobalPosition).IsEqual(groundedPosition);
    }

    [TestCase]
    public async Task ShouldFallWhenInAirUntilOnABrickAndThenGrounded()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        game.InitGame(gameConfig);
        runner.MaximizeView();
        runner.SetTimeFactor(0.25);
        await runner.SimulateFrames(3);

        var groundedPosition = new Vector2(
            game.platformer.GlobalPosition.X,
            game.bricks[0].Top() - game.platformer.shape.Size.Y / 2
        );
        var inTheAirPosition = new Vector2(groundedPosition.X, groundedPosition.Y - 20f);
        game.platformer.GlobalPosition = inTheAirPosition;
        GD.Print($"brick in test KEK {game.bricks[0].shape.Size} {game.bricks[0].colorRect.Size}");
        AssertObject(game.platformer.GlobalPosition).IsEqual(inTheAirPosition);

        await runner.AwaitPhysicsProcessCalls(6);
        AssertObject(game.platformer.GlobalPosition).IsEqual(groundedPosition);
        await runner.AwaitMillis(1000);
    }
}
