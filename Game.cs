using Godot;
using System;

public partial class Game : Node2D
{
    Paddle paddle = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        GD.Print(paddle);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
