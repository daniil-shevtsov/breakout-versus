using Godot;
using System;

public partial class Game : Node2D
{
    float paddleSpeed = 300f;

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
            fieldArea.rectangleShape.Size.Y * 0.9f
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;
        if (Input.IsActionPressed("paddle_right"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("paddle_left"))
        {
            direction.X -= 1.0f;
        }

        var currentPosition = paddle.GlobalPosition;
        var movement = new Vector2((float)(direction.X * paddleSpeed * delta), 0f);
        var newPosition = currentPosition + movement;
        paddle.GlobalPosition = newPosition;
    }
}
