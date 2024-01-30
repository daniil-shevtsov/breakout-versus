using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class GameTest
{
    private GameConfig gameConfig = new GameConfig(new Vector2(800, 600));

    [TestCase]
    public async Task TestCanLoadScene()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();

        await runner.SimulateFrames(1);

        AssertObject(game).IsNotNull();
    }

    [TestCase]
    public async Task TestPaddleAndBallInitialPosition()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        await runner.SimulateFrames(1);
        var game = (Game)runner.Scene();

        await runner.SimulateFrames(1);
        game.InitGame(gameConfig);
        await runner.SimulateFrames(1);

        AssertObject(game.paddle.GlobalPosition).IsEqual(new Vector2(400, 540));
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 490));
    }

    [TestCase]
    public async Task TestPaddleShootingBallOnSpace()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        game.InitGame(gameConfig);
        await runner.SimulateFrames(3);

        AssertObject(game.paddle.GlobalPosition).IsEqual(new Vector2(400, 540));
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 490));

        runner.SimulateKeyPressed(Key.Space);
        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 480));
    }

    [TestCase]
    public async Task TestBallCollidingWithFieldTop()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        game.InitGame(new GameConfig(new Vector2(800, 120)));
        await runner.SimulateFrames(3);

        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 58));

        runner.SimulateKeyPressed(Key.Space);

        await runner.AwaitPhysicsProcessCalls(5);

        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 8));
        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.ball.TopCenter()).IsEqual(new Vector2(400, 0));
        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.ball.TopCenter()).IsEqual(new Vector2(400, 10));
    }

    [TestCase]
    public async Task TestBallCollidingWithCenterOfPaddle()
    {
        // paddle size: (95.5, 20) ball radius: 10
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        runner.MaximizeView();
        runner.SetTimeFactor(0.25);
        var game = (Game)runner.Scene();
        game.isPaused = true;
        game.InitGame(
            gameConfig: new GameConfig(
                fieldSize: new Vector2(800, 320),
                ballPosition: new Vector2(400, 100),
                ballDirection: new Vector2(0, 1),
                paddlePosition: new Vector2(400, 120)
            )
        );
        await runner.SimulateFrames(3);
        game.isPaused = false;
        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 100));
        await runner.AwaitPhysicsProcessCalls(1);
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 90));
    }

    [TestCase]
    public async Task TestBallCollidingWithRightPaddleEdge()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        runner.MaximizeView();
        runner.SetTimeFactor(0.25);
        var game = (Game)runner.Scene();
        game.InitGame(
            new GameConfig(
                new Vector2(800, 320),
                new Vector2(400, 144),
                new Vector2(0, 1),
                new Vector2(400 - 95.5f / 2 + 4, 250)
            )
        );
        await runner.AwaitMillis(5000);
    }
}
