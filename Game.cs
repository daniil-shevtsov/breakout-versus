using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
    float paddleSpeed = 300f;
    float ballSpeed = 600;
    public List<Brick> bricks = new List<Brick>();

    private bool isBallStickedToPaddle = true;
    private Vector2 ballVelocity = Vector2.Zero;

    Paddle paddle = null;
    Ball ball = null;
    FieldArea fieldArea = null;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        ball = GetNode<Ball>("Ball");
        fieldArea = GetNode<FieldArea>("FieldArea");

        GD.Print(paddle);
        GD.Print(fieldArea);

        var scene = GetTree().CurrentScene;
        var brickResource = GD.Load<PackedScene>("res://brick.tscn");

        var dummyBrick = (Brick)brickResource.Instantiate();
        scene.CallDeferred("add_child", dummyBrick);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        var brickSize = dummyBrick.rectangleShape.Size;
        scene.CallDeferred("remove_child", dummyBrick);
        var fieldSize = fieldArea.rectangleShape.Size;

        var brickDistanceX = 4f;
        var brickDistanceY = 4f;
        int bricksToFitInRow = (int)Math.Floor(fieldSize.X / (brickDistanceX + brickSize.X));

        var brickCountX = bricksToFitInRow;
        var brickCountY = 4;

        var brickRowWidth =
            bricksToFitInRow * brickSize.X + (bricksToFitInRow - 1) * brickDistanceX;
        var extraSpace = fieldSize.X - brickRowWidth;

        var brickStartX = extraSpace / 2;
        var brickStartY = brickStartX;

        for (int i = 0; i < brickCountY; ++i)
        {
            for (int j = 0; j < brickCountX; ++j)
            {
                var brick = (Brick)brickResource.Instantiate();
                scene.CallDeferred("add_child", brick);
                bricks.Add(brick);

                brick.brickId = $"brick-({j},[i])";
                var brickPositionTopLeft = new Vector2(
                    brickStartX + j * brickDistanceX + j * brickSize.X,
                    brickStartY + i * brickDistanceY + i * brickSize.Y
                );
                var brickPositionCenter = new Vector2(
                    brickPositionTopLeft.X + brickSize.X / 2f,
                    brickPositionTopLeft.Y + brickSize.Y / 2f
                );
                brick.GlobalPosition = brickPositionCenter;
            }
        }

        paddle.GlobalPosition = new Vector2(
            fieldArea.rectangleShape.Size.X / 2f,
            fieldArea.rectangleShape.Size.Y * 0.9f
        );
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        ball.GlobalPosition = new Vector2(paddle.GlobalPosition.X, paddle.GlobalPosition.Y - 50f);
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
        if (Input.IsActionPressed("paddle_shoot_ball") && isBallStickedToPaddle)
        {
            isBallStickedToPaddle = false;
            var minAngle = -120;
            var maxAngle = -45;
            var randomAngle = new Random().NextDouble() * (maxAngle - minAngle) + minAngle;
            GD.Print($"generated angle {randomAngle}");
            var angle = Mathf.DegToRad(randomAngle);
            ballVelocity = new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));
        }

        var currentPosition = paddle.GlobalPosition;
        var movement = new Vector2((float)(paddleDirection * paddleSpeed * delta), 0f);
        var newPosition = currentPosition + movement;
        paddle.GlobalPosition = newPosition;

        if (isBallStickedToPaddle)
        {
            ball.GlobalPosition = new Vector2(
                paddle.GlobalPosition.X,
                paddle.GlobalPosition.Y - 50f
            );
        }
        else
        {
            var ballMovement = new Vector2(
                (float)(ballVelocity.X * ballSpeed * delta),
                (float)(ballVelocity.Y * ballSpeed * delta)
            );
            var newBallPosition = ball.GlobalPosition + ballMovement;

            if (newBallPosition.Y - ball.shape.Radius / 2 <= fieldArea.GlobalPosition.Y)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }
            else if (
                newBallPosition.Y + ball.shape.Radius / 2
                >= fieldArea.GlobalPosition.Y + fieldArea.rectangleShape.Size.Y
            )
            {
                isBallStickedToPaddle = true;
            }
            if (newBallPosition.X - ball.shape.Radius / 2 <= fieldArea.GlobalPosition.X)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            else if (
                newBallPosition.X + ball.shape.Radius / 2
                >= fieldArea.GlobalPosition.X + fieldArea.rectangleShape.Size.X
            )
            {
                ballVelocity.X = -ballVelocity.X;
            }

            ball.GlobalPosition = newBallPosition;

            // //TODO: This can be done much smarter
            // var ballMovement2 = new Vector2(
            //     (float)(ballVelocity.X * ballSpeed * delta),
            //     (float)(ballVelocity.Y * ballSpeed * delta)
            // );
            // ball.GlobalPosition = ball.GlobalPosition + ballMovement2;
        }
    }
}
