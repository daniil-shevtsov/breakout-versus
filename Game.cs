using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static MyExtensions;

public partial class Game : Node2D
{
	float paddleSpeed = 700;
	float platformerSpeed = 700;
	float platformerFallingSpeed = 100;
	float jumpSpeed = 600;
	float ballSpeed = 600;
	public List<Brick> bricks = new List<Brick>();

	private bool isBallStickedToPaddle = true;
	public bool isPaused = false;
	public Vector2 ballVelocity = Vector2.Zero;
	public Vector2 platformerVelocity = Vector2.Zero;
	public Vector2 platformerAcceleration = Vector2.Zero;
	public bool isGrounded = false;

	public int physicsProcessCount = 0;
	public int processCount = 0;

	public Paddle paddle = null;
	public Platformer platformer = null;
	public Ball ball = null;
	public FieldArea fieldArea = null;

	private PackedScene brickResource = null;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		paddle = GetNode<Paddle>("Paddle");
		platformer = GetNode<Platformer>("Platformer");
		ball = GetNode<Ball>("Ball");
		fieldArea = GetNode<FieldArea>("FieldArea");

		var gameConfig = new GameConfig(new Vector2(800f, 600f));
		InitGame(gameConfig);
	}

	public void AddNewBrick(Vector2 position, string id)
	{
		var brick = (Brick)brickResource.Instantiate();
		var scene = GetTree().CurrentScene;
		scene.CallDeferred("add_child", brick);
		bricks.Add(brick);

		brick.brickId = id;
		brick.GlobalPosition = position;
		GD.Print($"Created {brick.brickId} at ${brick.center}");
	}

	public async void InitGame(GameConfig gameConfig)
	{
		fieldArea.rectangleShape.Size = gameConfig.fieldSize;
		fieldArea.colorRect.Size = gameConfig.fieldSize;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		var scene = GetTree().CurrentScene;
		brickResource = GD.Load<PackedScene>("res://brick.tscn");

		var dummyBrick = (Brick)brickResource.Instantiate();
		scene.CallDeferred("add_child", dummyBrick);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		var brickSize = dummyBrick.rectangleShape.Size;
		scene.CallDeferred("remove_child", dummyBrick);
		var fieldSize = fieldArea.rectangleShape.Size;

		var brickOffsetY = fieldArea.rectangleShape.Size.Y * 0.25f;

		var brickDistanceX = 4f;
		var brickDistanceY = 4f;
		int bricksToFitInRow = (int)Math.Floor(fieldSize.X / (brickDistanceX + brickSize.X));

		var brickCountX = bricksToFitInRow;
		var brickCountY = 4;

		var brickRowWidth =
			bricksToFitInRow * brickSize.X + (bricksToFitInRow - 1) * brickDistanceX;
		var extraSpace = fieldSize.X - brickRowWidth;

		var brickStartX = extraSpace / 2;
		var brickStartY = brickOffsetY + brickStartX;
		GD.Print($"Want to draw {brickCountX} * {brickCountY} bricks KEK");
		for (int i = 0; i < brickCountY; ++i)
		{
			for (int j = 0; j < brickCountX; ++j)
			{
				var brickId = $"brick-({j},{i})";
				var brickPositionTopLeft = new Vector2(
					brickStartX + j * brickDistanceX + j * brickSize.X,
					brickStartY + i * brickDistanceY + i * brickSize.Y
				);
				var brickPositionCenter = new Vector2(
					brickPositionTopLeft.X + brickSize.X / 2f,
					brickPositionTopLeft.Y + brickSize.Y / 2f
				);
				AddNewBrick(brickPositionCenter, brickId);
			}
		}

		paddle.GlobalPosition = new Vector2(
			fieldArea.rectangleShape.Size.X / 2f,
			fieldArea.rectangleShape.Size.Y * 0.9f
		);
		platformer.GlobalPosition = new Vector2(
			fieldArea.rectangleShape.Size.X / 2f,
			brickStartY - platformer.shape.Size.Y
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
		GD.Print($"paddle size: {paddle.shape.Size} ball radius: {ball.shape.Radius}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		++processCount;
	}

	public override void _PhysicsProcess(double delta)
	{
		++physicsProcessCount;
		if (!isPaused)
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

			var platformerDirection = 0f;
			if (Input.IsActionPressed("platformer_left"))
			{
				platformerDirection = -1.0f;
			}
			else if (Input.IsActionPressed("platformer_right"))
			{
				platformerDirection = 1.0f;
			}
			platformerVelocity.X = platformerDirection * platformerSpeed * (float)delta;

			if (isGrounded && Input.IsActionPressed("platformer_jump"))
			{
				platformerVelocity.Y = -jumpSpeed * (float)delta;
			}

			resolvePlatformerCollision(delta);

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

				var newCollidedWithPaddle =
					ball.shape.Left(newBallPosition) >= paddle.Left()
					&& ball.shape.Left(newBallPosition) <= paddle.Right()
					&& ball.shape.Bottom(newBallPosition) > paddle.Top();

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
				else if (newCollidedWithPaddle)
				{
					GD.Print("KEK collided with paddle");
					//TODO: In reality need to use the angle of collision
					var zoneWidth = paddle.shape.Size.X * 0.33;
					if (
						ball.shape.Right(newBallPosition)
						<= paddle.shape.Left(paddle.GlobalPosition) + zoneWidth
					)
					{
						GD.Print("KEK collided with right paddle zone");
						var randomAngle = -135;

						var angle = Mathf.DegToRad(randomAngle);
						ballVelocity = new Vector2(
							(float)Mathf.Cos(angle),
							(float)Mathf.Sin(angle)
						);
						newBallPosition = new Vector2(
							newBallPosition.X,
							paddle.shape.Top(paddle.GlobalPosition) - ball.shape.Radius
						);
						GD.Print($"KEK New ball velocity = {ballVelocity}");
						GD.Print(
							$"KEK paddle top {paddle.Top()} ball position = {ball.GlobalPosition} ball bottom = {ball.Bottom()}"
						);
					}
					else if (
						ball.shape.Left(newBallPosition)
						>= paddle.shape.Right(paddle.GlobalPosition) - zoneWidth
					)
					{
						GD.Print("KEK collided with right paddle zone");
						var randomAngle = -45;

						var angle = Mathf.DegToRad(randomAngle);
						ballVelocity = new Vector2(
							(float)Mathf.Cos(angle),
							(float)Mathf.Sin(angle)
						);
						newBallPosition = new Vector2(
							newBallPosition.X,
							paddle.shape.Top(paddle.GlobalPosition) - ball.shape.Radius
						);
						GD.Print($"KEK New ball velocity = {ballVelocity}");
						GD.Print(
							$"KEK paddle top {paddle.Top()} ball position = {ball.GlobalPosition} ball bottom = {ball.Bottom()}"
						);
					}
					else
					{
						GD.Print("KEK collided with middle paddle zone");
						ballVelocity.Y = -ballVelocity.Y;
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

				foreach (Brick brick in bricks)
				{
					if (brick.isEnabled && brick.Intersects(ball, newBallPosition))
					{
						GD.Print($"Collided with brick {brick.TopLeft()}");
						ballVelocity.Y = -ballVelocity.Y;
						brick.Toggle(false);
						break;
					}
				}

				ball.GlobalPosition = newBallPosition;
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Space && isBallStickedToPaddle)
			{
				GD.Print("Shoot ball pressed");
				isBallStickedToPaddle = false;
				var minAngle = -120;
				var maxAngle = -45;
				var randomAngle = new Random().NextDouble() * (maxAngle - minAngle) + minAngle;

				var angle = Mathf.DegToRad(randomAngle);
				ballVelocity = new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));
				GD.Print($"new ball velocity {ballVelocity}");
			}
		}

		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsReleased())
		{
			var position = eventMouseButton.Position;
			GD.Print("MOUSE Mouse Click/Unclick at: ", position);
			var clickedBrick = bricks.Find(
				brick =>
					MyCollisionDetection.IsIntersection(
						rectSize: brick.rectangleShape.Size,
						rectTopLeft: brick.rectangleShape.TopLeft(brick.GlobalPosition),
						circleRadius: 1f,
						circleCenter: position
					)
			);
			if (clickedBrick != null)
			{
				GD.Print($"MOUSE Toggle brick {clickedBrick.center}");
				clickedBrick.Toggle(!clickedBrick.isEnabled);
			}
			else
			{
				AddNewBrick(position, $"user-brick-{position}");
			}
		}
	}

	public void resolvePlatformerCollision(double delta)
	{
		var currentPlatformerPosition = platformer.GlobalPosition;
		platformerVelocity = platformerVelocity + platformerAcceleration * (float)delta;
		var newPlatformerPosition = currentPlatformerPosition + platformerVelocity;
		platformer.GlobalPosition = newPlatformerPosition;

		var intersectedBrick = bricks.Find(brick => brick.isEnabled && MyIntersection.BrickIntersection(brick, platformer) != Vector2.Zero);
		var groundBrick = intersectedBrick;
		Vector2 intersection;
		if (intersectedBrick != null)
		{
			intersection = MyIntersection.BrickIntersection(intersectedBrick, platformer);
			GD.Print($"KEK Intersection = {intersection}");
			newPlatformerPosition.Y = intersectedBrick.Top() - platformer.shape.Size.Y / 2;
			platformerAcceleration.Y = 0;
			GD.Print($"Set platformer to {newPlatformerPosition.Y}");
			isGrounded = true;
		}
		else
		{
			intersection = Vector2.Zero;
			isGrounded = false;
		}


		if (!isGrounded && platformerAcceleration.Y != platformerFallingSpeed)
		{
			GD.Print("Not grounded, so set falling velocity");
			platformerAcceleration.Y = platformerFallingSpeed;
		}
		else if (isGrounded)
		{
			GD.Print("GROUNDED");
			platformerAcceleration.Y = 0;
		}

		platformer.GlobalPosition = newPlatformerPosition;
	}
}
