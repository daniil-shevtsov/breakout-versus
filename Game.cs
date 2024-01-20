using Godot;
using System;

public partial class Game : Node2D
{
    Paddle paddle = null;
    FieldArea fieldArea = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        fieldArea = GetNode<FieldArea>("FieldArea");

        GD.Print(paddle);
        GD.Print(fieldArea);

        paddle.GlobalPosition = new Vector2(
            fieldArea.rectangleShape.Size.X / 2f,
            fieldArea.rectangleShape.Size.Y / 2f
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
