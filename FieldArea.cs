using Godot;
using System;

public partial class FieldArea : Area2D
{
    public RectangleShape2D rectangleShape = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rectangleShape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
