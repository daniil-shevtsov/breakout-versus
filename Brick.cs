using Godot;
using System;

public partial class Brick : StaticBody2D
{
    public CollisionShape2D collisionShape = null;
    public RectangleShape2D rectangleShape = null;
    public ColorRect colorRect = null;
    public string brickId = "not initialized";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        rectangleShape = (RectangleShape2D)collisionShape.Shape;
        colorRect = GetNode<ColorRect>("ColorRect");

        GD.Print(collisionShape);
        GD.Print(rectangleShape);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
