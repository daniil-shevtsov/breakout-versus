using Godot;
using System;

public partial class Brick : StaticBody2D, Shaped
{
    public CollisionShape2D collisionShape = null;
    public RectangleShape2D rectangleShape = null;
    public ColorRect colorRect = null;
    public string brickId = "not initialized";
    public bool isEnabled = true;

    public RectangleShape2D shape
    {
        get => rectangleShape;
    }
    public Vector2 center
    {
        get => GlobalPosition;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        rectangleShape = (RectangleShape2D)collisionShape.Shape;
        colorRect = GetNode<ColorRect>("ColorRect");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void Toggle(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        if (isEnabled)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
