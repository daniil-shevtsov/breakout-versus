using Godot;
using System;

public partial class Paddle : StaticBody2D
{
    CollisionShape2D collisionShape = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        GD.Print(collisionShape);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
