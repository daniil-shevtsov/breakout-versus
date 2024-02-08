using Godot;
using System;

public partial class Platformer : StaticBody2D, Shaped
{
    public RectangleShape2D _shape = null;
    public RectangleShape2D shape
    {
        get => _shape;
    }
    public Vector2 center
    {
        get => GlobalPosition;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
