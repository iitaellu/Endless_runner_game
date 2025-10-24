using Godot;
using System;

public partial class Collectable : Area2D
{
	private AnimatedSprite2D _animatedSprite2D;

	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	private CollisionShape2D _collisionShape2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		AddToGroup("obstacles");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExcited;
		BodyEntered += OnBodyEntered;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			var ui = GetNode<Ui>("/root/Main/UI");
			ui.AddPoints(10);
			QueueFree(); // disappear immediately
		}
	}

	public void getPoints()
	{
		QueueFree();

	}
	private void OnScreenExcited() => QueueFree();
}
