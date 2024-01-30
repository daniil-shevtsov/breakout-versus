using Godot;
using System;
using System.Collections.Generic;
using static MyExtensions;

public partial class Game : Node2D
{
    float paddleSpeed = 700;
    float ballSpeed = 600;
    public List<Brick> bricks = new List<Brick>();

    private bool isBallStickedToPaddle = true;
    private Vector2 ballVelocity = Vector2.Zero;

    public int physicsProcessCount = 0;
    public int processCount = 0;

    public Paddle paddle = null;
    public Ball ball = null;
    public FieldArea fieldArea = null;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        paddle = GetNode<Paddle>("Paddle");
        ball = GetNode<Ball>("Ball");
        fieldArea = GetNode<FieldArea>("FieldArea");

        // var gameConfig = new GameConfig(new Vector2(1920, 1080));
        // InitGame(gameConfig);
        // await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        // var gameConfig2 = new GameConfig(new Vector2(800f, 600f));
        // InitGame(gameConfig2);
        // await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        var gameConfig = new GameConfig(new Vector2(800f, 600f));
        InitGame(gameConfig);
    }

    public async void InitGame(GameConfig gameConfig)
    {
        fieldArea.rectangleShape.Size = gameConfig.fieldSize;
        fieldArea.colorRect.Size = gameConfig.fieldSize;
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
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
        if (gameConfig.ballPosition != null && gameConfig.ballDirection != null)
        {
            ball.GlobalPosition = (Vector2)gameConfig.ballPosition;
            ballVelocity = (Vector2)gameConfig.ballDirection;
            isBallStickedToPaddle = false;
        }
        if (gameConfig.paddlePosition != null)
        {
            paddle.GlobalPosition = (Vector2)gameConfig.paddlePosition;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ++processCount;
        GD.Print($"Process {delta}");
    }

    public override void _PhysicsProcess(double delta)
    {
        ++physicsProcessCount;
        GD.Print($"PhysicsProcess {delta}");
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
        var currentBallPosition = ball.GlobalPosition;
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
            GD.Print($"ball speed: {ballSpeed} delta: {delta}");
            var ballMovement = new Vector2(
                (float)(ballVelocity.X * ballSpeed * delta),
                (float)(ballVelocity.Y * ballSpeed * delta)
            );
            var newBallPosition = ball.GlobalPosition + ballMovement;

            var oldCollidedWithPaddle =
                (ball.shape.Left(newBallPosition) >= paddle.shape.Left(paddle.GlobalPosition))
                && (ball.shape.Right(newBallPosition) <= paddle.shape.Right(paddle.GlobalPosition))
                && (ball.shape.Top(newBallPosition) >= paddle.shape.Top(paddle.GlobalPosition));
            var newCollidedWithPaddle =
                ball.Left() >= paddle.Left() && ball.Left() <= paddle.Right();

            var collidedWithPaddle = oldCollidedWithPaddle; //|| newCollidedWithPaddle;

            if (
                ball.shape.Bottom(newBallPosition)
                >= fieldArea.GlobalPosition.Y + fieldArea.rectangleShape.Size.Y
            )
            {
                isBallStickedToPaddle = true;
            }
            else if (ball.shape.Top(newBallPosition) <= fieldArea.GlobalPosition.Y)
            {
                newBallPosition = new Vector2(
                    newBallPosition.X,
                    fieldArea.GlobalPosition.Y + ball.shape.Radius / 2
                );
                ballVelocity.Y = -ballVelocity.Y;
            }
            else if (collidedWithPaddle)
            {
                //TODO: In reality need to use the angle of collision
                var zoneWidth = paddle.shape.Size.X * 0.33;
                if (
                    ball.shape.Right(newBallPosition)
                    <= paddle.shape.Left(paddle.GlobalPosition) + zoneWidth
                )
                {
                    ballVelocity.X -= ballSpeed * 0.25f;
                }
                else if (
                    ball.shape.Left(newBallPosition)
                    >= paddle.shape.Right(paddle.GlobalPosition) - zoneWidth
                )
                {
                    ballVelocity.X += ballSpeed * 0.25f;
                }
                else
                {
                    ballVelocity.Y = -ballVelocity.Y;
                    newBallPosition = new Vector2(
                        newBallPosition.X,
                        paddle.shape.Top(paddle.GlobalPosition)
                    );
                }
            }

            if (ball.shape.Left(newBallPosition) <= fieldArea.GlobalPosition.X)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            else if (
                ball.shape.Right(newBallPosition)
                >= fieldArea.GlobalPosition.X + fieldArea.rectangleShape.Size.X
            )
            {
                ballVelocity.X = -ballVelocity.X;
            }

            ball.GlobalPosition = newBallPosition;

            if (currentPosition != newPosition)
            {
                GD.Print($"new paddle position = {paddle.GlobalPosition}");
            }
            if (currentBallPosition != ball.GlobalPosition)
            {
                GD.Print($"new ball position = {ball.GlobalPosition} size = {ball.shape.Radius}");
                GD.Print(
                    $"ball top {ball.shape.Top(newBallPosition)} <=  field Y {fieldArea.GlobalPosition.Y} = {ball.shape.Top(newBallPosition) <= fieldArea.GlobalPosition.Y}"
                );
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Space)
            {
                GD.Print("Shoot ball pressed");
                //physicsProcessCount = 0;
                //processCount = 0;
                isBallStickedToPaddle = false;
                var minAngle = -120;
                var maxAngle = -45;
                var randomAngle = new Random().NextDouble() * (maxAngle - minAngle) + minAngle;

                var angle = Mathf.DegToRad(randomAngle);
                ballVelocity = new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));
                GD.Print($"new ball velocity {ballVelocity}");
            }
        }
    }
}
