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
        //await runner.AwaitMillis(2000);
        AssertObject(game.paddle.GlobalPosition).IsEqual(new Vector2(400, 540));
        AssertObject(game.ball.GlobalPosition).IsEqual(new Vector2(400, 490));
    }
}
