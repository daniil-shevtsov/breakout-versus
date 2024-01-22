using Godot;
using System;
using GdUnit4;
using GdUnit4.Core;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class GameTest
{
    [TestCase]
    public async Task TestCanLoadScene()
    {
        var runner = ISceneRunner.Load("res://game_scene_root.tscn", true, true);
        var game = (Game)runner.Scene();

        await runner.SimulateFrames(1);

        AssertObject(game).IsNotNull();
    }
}
