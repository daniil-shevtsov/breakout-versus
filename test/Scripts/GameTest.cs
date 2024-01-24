using Godot;
using System;
using GdUnit4;
using GdUnit4.Core;
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
        GD.Print("TestPaddleShootingBallOnSpace start");
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        game.InitGame(gameConfig);
        GD.Print("Test after init");
        await runner.SimulateFrames(3);
        GD.Print("Test after simulate frames");

        AssertObject(game.paddle.GlobalPosition).IsEqual(new Vector2(400, 540));
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 490));

        runner.SimulateKeyPressed(Key.Space);
        GD.Print("TestPaddleShootingBallOnSpace before AwaitPhysicsProcessCalls");
        await game.ToSignal(game.GetTree(), SceneTree.SignalName.PhysicsFrame);
        await runner.AwaitMillis(1);

        GD.Print("TestPaddleShootingBallOnSpace before assert at end");
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 480));
        GD.Print("TestPaddleShootingBallOnSpace end");
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

        //await runner.AwaitPhysicsProcessCalls(3);
        await runner.AwaitPhysicsProcessCalls(3);

        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 28));
    }
}
