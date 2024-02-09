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
    public async Task TestCanLoadScene()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();
        runner.MaximizeView();
        runner.SetTimeFactor(0.25);
        game.isPaused = true;
        game.InitGame(
            new GameConfig(
                fieldSize: new Vector2(800, 320),
                ballPosition: new Vector2(400, 100),
                ballDirection: new Vector2(0, 1),
                paddlePosition: new Vector2(400 + 95.5f / 2 - 5, 120)
            )
        );
        await runner.SimulateFrames(3);
        game.isPaused = false;

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
}
