using Godot;
using System;

public struct GameConfig
{
    public Vector2 fieldSize;
    public Vector2? ballPosition;
    public Vector2? ballDirection;
    public Vector2? paddlePosition;

    public GameConfig(
        Vector2 fieldSize,
        Vector2? ballPosition = null,
        Vector2? ballDirection = null,
        Vector2? paddlePosition = null
    )
    {
        this.fieldSize = fieldSize;
        this.ballPosition = ballPosition;
        this.ballDirection = ballDirection;
        this.paddlePosition = paddlePosition;
    }
}
