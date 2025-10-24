using Godot;
using System;

public partial class End : StaticBody2D
{

public Sprite2D Sprite2D;
public CollisionShape2D CollisionShape2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		AddToGroup("obstacles");
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
		CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
