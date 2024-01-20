using Godot;
using System;

public partial class Game : Node2D
{
    Paddle paddle = null;
    Area2D fieldArea = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        fieldArea = GetNode<Area2D>("FieldArea");

        GD.Print(paddle);
        GD.Print(fieldArea);

        paddle.GlobalPosition = new Vector2(
            ((RectangleShape2D)fieldArea.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.X
                / 2f,
            ((RectangleShape2D)fieldArea.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.Y
                / 2f
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
