using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
    float paddleSpeed = 300f;
    public List<Brick> bricks = new List<Brick>();

    Paddle paddle = null;
    FieldArea fieldArea = null;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        fieldArea = GetNode<FieldArea>("FieldArea");

        GD.Print(paddle);
        GD.Print(fieldArea);

        var scene = GetTree().CurrentScene;
        var brickResource = GD.Load<PackedScene>("res://brick.tscn");

        var brickCountX = 10;
        var brickCountY = 4;
        var brickStartX = fieldArea.rectangleShape.Size.X * 0.1f;
        var brickStartY = fieldArea.rectangleShape.Size.Y * 0.05f;
        var brickDistanceX = 0f;
        var brickDistanceY = 0f;
        for (int i = 0; i < brickCountY; ++i)
        {
            for (int j = 0; j < brickCountX; ++j)
            {
                var brick = (Brick)brickResource.Instantiate();
                scene.CallDeferred("add_child", brick);
                bricks.Add(brick);
                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

                brick.brickId = $"brick-({j},[i])";
                var brickSize = new Vector2(
                    brick.rectangleShape.Size.X,
                    brick.rectangleShape.Size.Y
                );
                var brickPosition = new Vector2(
                    brickStartX + j * brickDistanceX + j * brickSize.X,
                    brickStartY + i * brickDistanceY + i * brickSize.Y
                );
                brick.GlobalPosition = brickPosition;
            }
        }

        paddle.GlobalPosition = new Vector2(
            fieldArea.rectangleShape.Size.X / 2f,
            fieldArea.rectangleShape.Size.Y * 0.9f
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        var paddleDirection = 0f;
        if (Input.IsActionPressed("paddle_left"))
        {
            paddleDirection = -1.0f;
        }
        else if (Input.IsActionPressed("paddle_right"))
        {
            paddleDirection = 1.0f;
        }

        var currentPosition = paddle.GlobalPosition;
        var movement = new Vector2((float)(paddleDirection * paddleSpeed * delta), 0f);
        var newPosition = currentPosition + movement;
        paddle.GlobalPosition = newPosition;
    }
}
